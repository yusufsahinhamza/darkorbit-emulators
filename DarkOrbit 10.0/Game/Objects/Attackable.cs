using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Objects.Stations;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects
{
    abstract class Attackable : Tick
    {
        public int TickId { get; set; }

        public int Id { get; }

        public abstract string Name { get; set; }

        public abstract Clan Clan { get; set; }

        public abstract Position Position { get; set; }

        public abstract Spacemap Spacemap { get; set; }

        public abstract int FactionId { get; set; }

        public abstract int CurrentHitPoints { get; set; }

        public abstract int MaxHitPoints { get; set; }

        public abstract int CurrentNanoHull { get; set; }

        public abstract int MaxNanoHull { get; set; }

        public abstract int CurrentShieldPoints { get; set; }

        public abstract int MaxShieldPoints { get; set; }

        public abstract double ShieldAbsorption { get; set; }

        public abstract double ShieldPenetration { get; set; }

        public DateTime LastCombatTime { get; set; }

        public virtual int AttackRange => 700;

        public virtual int RenderRange => 2000;

        public bool Invisible { get; set; }

        public bool Destroyed = false;

        public Character MainAttacker { get; set; }
        public ConcurrentDictionary<int, Attacker> Attackers = new ConcurrentDictionary<int, Attacker>();

        public ConcurrentDictionary<int, Character> InRangeCharacters = new ConcurrentDictionary<int, Character>();
        public ConcurrentDictionary<int, VisualModifierCommand> VisualModifiers = new ConcurrentDictionary<int, VisualModifierCommand>();

        public Attackable Selected { get; set; }

        protected Attackable(int id)
        {
            Id = id;
            Invisible = false;

            if (Clan == null || !GameManager.Clans.ContainsKey(Clan.Id))
                Clan = GameManager.GetClan(0);
        }

        public abstract void Tick();

        public bool InRange(Attackable attackable, int range = 2000)
        {
            if (attackable == null || attackable.Spacemap.Id != Spacemap.Id) return false;
            if (attackable is Character character)
            {
                if (character == null || character.Destroyed) return false;
                if (this is Player thisPlayer && thisPlayer.Storage.Duel != null && thisPlayer.Storage.Duel?.GetOpponent(thisPlayer) != attackable) return false;
            }
            if (range == -1 || attackable.Spacemap.Options.RangeDisabled) return true;
            return attackable.Id != Id && Position.DistanceTo(attackable.Position) <= range;
        }

        public void Deselection(bool emp = false)
        {
            Selected = null;

            if (this is Player player)
            {
                player.DisableAttack(player.Settings.InGameSettings.selectedLaser);
                player.Group?.UpdateTarget(player, new List<command_i3O> { new GroupPlayerTargetModule(new GroupPlayerShipModule(GroupPlayerShipModule.NONE), "", new GroupPlayerInformationsModule(0, 0, 0, 0, 0, 0)) });

                if (emp)
                {
                    string empMessagePacket = "0|A|STM|msg_own_targeting_harmed";
                    player.SendPacket(empMessagePacket);
                    player.SendCommand(ShipDeselectionCommand.write());
                    player.SendPacket("0|UI|MM|NOISE");
                }
            }
        }

        public void UpdateStatus()
        {
            if (CurrentHitPoints > MaxHitPoints) CurrentHitPoints = MaxHitPoints;
            if (CurrentHitPoints < 0) CurrentHitPoints = 0;
            if (CurrentShieldPoints > MaxShieldPoints) CurrentShieldPoints = MaxShieldPoints;
            if (CurrentShieldPoints < 0) CurrentShieldPoints = 0;

            if (this is Player player)
            {
                player.SendCommand(AttributeHitpointUpdateCommand.write(CurrentHitPoints, MaxHitPoints, CurrentNanoHull, MaxNanoHull));
                player.SendCommand(AttributeShieldUpdateCommand.write(player.CurrentShieldPoints, player.MaxShieldPoints));
                player.SendCommand(SetSpeedCommand.write(player.Speed, player.Speed));
                player.Group?.UpdateTarget(player, new List<command_i3O> { new GroupPlayerInformationsModule(player.CurrentHitPoints, player.MaxHitPoints, player.CurrentShieldPoints, player.MaxShieldPoints, player.CurrentNanoHull, player.MaxNanoHull) });
            }
            else if (this is Pet pet)
            {
                var owner = pet.Owner;
                owner.SendCommand(PetHitpointsUpdateCommand.write(pet.CurrentHitPoints, pet.MaxHitPoints, false));
                owner.SendCommand(PetShieldUpdateCommand.write(pet.CurrentShieldPoints, pet.MaxShieldPoints));
            }
            else if (this is BattleStation battleStation)
            {
                var module = battleStation.EquippedStationModule[battleStation.Clan.Id].Where(x => x.Module.type == StationModuleModule.HULL).FirstOrDefault();
                module.Module.maxHitpoints = MaxHitPoints;
                module.Module.currentHitpoints = CurrentHitPoints;
                module.Module.maxShield = MaxShieldPoints;
                module.Module.currentShield = CurrentShieldPoints;
            }
            else if (this is Satellite satellite)
            {
                satellite.Module.maxHitpoints = MaxHitPoints;
                satellite.Module.currentHitpoints = CurrentHitPoints;
                satellite.Module.maxShield = MaxShieldPoints;
                satellite.Module.currentShield = CurrentShieldPoints;
            }

            foreach (var otherCharacter in Spacemap.Characters.Values)
            {
                if (otherCharacter is Player otherPlayer && otherPlayer.Selected == this)
                {
                    if (this is Character character)
                        otherPlayer.SendCommand(ShipSelectionCommand.write(Id, character.Ship.Id, CurrentShieldPoints, MaxShieldPoints, CurrentHitPoints, MaxHitPoints, CurrentNanoHull, MaxNanoHull, false));
                    else if (this is Activatable activatable)
                    {
                        otherPlayer.SendCommand(AssetInfoCommand.write(
                            activatable.Id,
                            activatable.GetAssetType(),
                            activatable is Satellite ? (activatable as Satellite).DesignId : 0,
                            0,
                            activatable.CurrentHitPoints,
                            activatable.MaxHitPoints,
                            activatable.MaxShieldPoints > 0 ? true : false,
                            activatable.CurrentShieldPoints,
                            activatable.MaxShieldPoints
                            ));
                    }
                }
            }
        }

        public void SendPacketToInRangePlayers(string packet)
        {
            foreach (var character in InRangeCharacters.Values)
                if (character is Player player)
                    player.SendPacket(packet);
        }

        public void SendCommandToInRangePlayers(byte[] command)
        {
            foreach (var character in InRangeCharacters.Values)
                if (character is Player player)
                    player.SendCommand(command);
        }

        public void Destroy(Character destroyer, DestructionType destructionType)
        {
            if (this is Spaceball || Destroyed) return;

            if (MainAttacker != null && MainAttacker is Player)
            {
                destroyer = MainAttacker;
                destructionType = DestructionType.PLAYER;
            }

            Destroyed = true;

            var destroyCommand = ShipDestroyedCommand.write(Id, 0);

            if (this is Activatable)
                GameManager.SendCommandToMap(Spacemap.Id, destroyCommand);
            else
                SendCommandToInRangePlayers(destroyCommand);

            if (this is Player thisPlayer)
            {
                if (EventManager.JackpotBattle.InActiveEvent(thisPlayer))
                    GameManager.SendPacketToMap(EventManager.JackpotBattle.Spacemap.Id, $"0|A|STM|msg_jackpot_players_left|%COUNT%|{(EventManager.JackpotBattle.Spacemap.Characters.Count - 1)}"); //remove aşşağıda olduğu için böyle olması lazım sanırım

                if (destroyer is Player destroyerPlayer && destroyerPlayer.Storage.KilledPlayerIds.Where(x => x == Id).Count() <= 13)
                    destroyerPlayer.Storage.KilledPlayerIds.Add(Id);

                thisPlayer.SkillManager.DisableAllSkills();
                thisPlayer.Pet.Deactivate(true);
                thisPlayer.SendCommand(destroyCommand);
                thisPlayer.DisableAttack(thisPlayer.Settings.InGameSettings.selectedLaser);
                thisPlayer.CurrentInRangePortalId = -1;
                thisPlayer.Storage.InRangeAssets.Clear();
                thisPlayer.KillScreen(destroyer, destructionType);
            }

            if (this is BattleStation battleStation)
            {
                foreach (var module in battleStation.EquippedStationModule[battleStation.Clan.Id])
                {
                    module.Destroy(destroyer, destructionType);

                    Activatable activatable;
                    Spacemap.Activatables.TryRemove(module.Id, out activatable);
                    GameManager.SendCommandToMap(Spacemap.Id, AssetRemoveCommand.write(module.GetAssetType(), module.Id));
                }

                battleStation.VisualModifiers.Clear();
                battleStation.EquippedStationModule[battleStation.Clan.Id].Clear();
                battleStation.Clan = GameManager.GetClan(0);
                battleStation.InBuildingState = false;
                battleStation.FactionId = 0;
                battleStation.BuildTimeInMinutes = 0;

                Program.TickManager.RemoveTick(battleStation);

                foreach (var entry in Spacemap.Characters.Values)
                {
                    if (entry is Player player)
                    {
                        short relationType = entry.Clan.Id != 0 && Clan.Id != 0 ? Clan.GetRelation(entry.Clan) : (short)0;
                        player.SendCommand(battleStation.GetAssetCreateCommand(relationType));
                    }
                }
            }

            if (this is Satellite satellite)
            {
                if (!satellite.BattleStation.Destroyed)
                {
                    satellite.Module.type = StationModuleModule.NONE;
                    satellite.Module.currentHitpoints = 0;
                    satellite.Module.currentShield = 0;
                    satellite.DesignId = 0;

                    GameManager.SendCommandToMap(Spacemap.Id, satellite.GetAssetCreateCommand(0));
                }
            }

            if (destructionType == DestructionType.PLAYER)
            {
                var destroyerPlayer = destroyer as Player;
                destroyerPlayer.Deselection();

                if (!(this is Activatable))
                {
                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                        mySqlClient.ExecuteNonQuery($"INSERT INTO log_player_kills (killer_id, target_id) VALUES ({destroyerPlayer.Id}, {Id})");

                    var count = destroyerPlayer.Storage.KilledPlayerIds.Where(x => x == Id).Count();

                    if (count > 13 && destroyerPlayer.Storage.Duel == null && destroyerPlayer.Storage.Uba == null)
                        destroyerPlayer.SendPacket($"0|A|STM|pusher_info_no_reward|%NAME%|{Name}");

                    if (destroyerPlayer.Storage.Duel == null && destroyerPlayer.Storage.Uba == null)
                    {
                        if (count < 13)
                        {
                            int experience = destroyerPlayer.Ship.GetExperienceBoost((this as Character).Ship.Rewards.Experience);
                            int honor = destroyerPlayer.GetHonorBoost(destroyerPlayer.Ship.GetHonorBoost((this as Character).Ship.Rewards.Honor));
                            int uridium = (this as Character).Ship.Rewards.Uridium;
                            var changeType = ChangeType.INCREASE;

                            short relationType = destroyerPlayer.Clan.Id != 0 && Clan.Id != 0 ? Clan.GetRelation(destroyerPlayer.Clan) : (short)0;
                            if ((destroyerPlayer.FactionId == FactionId && relationType != ClanRelationModule.AT_WAR && (this is Player player && !(EventManager.JackpotBattle.InActiveEvent(player))) || (this is Pet thisPet && destroyerPlayer.Pet == thisPet)))
                                changeType = ChangeType.DECREASE;

                            destroyerPlayer.ChangeData(DataType.EXPERIENCE, experience);
                            destroyerPlayer.ChangeData(DataType.HONOR, honor, changeType);
                            destroyerPlayer.ChangeData(DataType.URIDIUM, uridium, changeType);
                        }
                    }

                    new CargoBox(AssetTypeModule.BOXTYPE_FROM_SHIP, Position, Spacemap, false, false, destroyerPlayer);
                }
            }

            if (this is Character character)
                Spacemap.RemoveCharacter(character);

            CurrentHitPoints = 0;
            Deselection();
            InRangeCharacters.Clear();
            VisualModifiers.Clear();

            if (this is Pet pet)
                pet.Deactivate(true, true);
        }

        public void AddVisualModifier(VisualModifierCommand visualModifier)
        {
            VisualModifiers.TryAdd(visualModifier.modifier, visualModifier);

            if (this is Character)
                SendCommandToInRangePlayers(visualModifier.writeCommand());
            else if (this is Activatable)
                GameManager.SendCommandToMap(Spacemap.Id, visualModifier.writeCommand());

            if (this is Player player)
            {
                player.SendCommand(visualModifier.writeCommand());

                switch (visualModifier.modifier)
                {
                    case VisualModifierCommand.INVINCIBILITY:
                        player.Storage.invincibilityEffect = true;
                        player.Storage.invincibilityEffectTime = DateTime.Now;
                        break;
                    case VisualModifierCommand.MIRRORED_CONTROLS:
                        player.Storage.mirroredControlEffect = true;
                        player.Storage.mirroredControlEffectTime = DateTime.Now;
                        break;
                    case VisualModifierCommand.WIZARD_ATTACK:
                        player.Storage.wizardEffect = true;
                        player.Storage.wizardEffectTime = DateTime.Now;
                        break;
                }
            }
        }

        public void RemoveVisualModifier(int modifier)
        {
            var visualModifier = VisualModifiers.FirstOrDefault(x => x.Value.modifier == modifier).Value;

            if (visualModifier != null)
            {
                VisualModifiers.TryRemove(visualModifier.modifier, out visualModifier);

                if (this is Character character)
                    SendCommandToInRangePlayers(new VisualModifierCommand(visualModifier.userId, visualModifier.modifier, 0, character.Ship.LootId, 0, false).writeCommand());
                else if (this is Activatable)
                    GameManager.SendCommandToMap(Spacemap.Id, new VisualModifierCommand(visualModifier.userId, visualModifier.modifier, 0, "", 0, false).writeCommand());

                if (this is Player player)
                    player.SendCommand(new VisualModifierCommand(visualModifier.userId, visualModifier.modifier, 0, player.Ship.LootId, 0, false).writeCommand());
            }
        }
    }
}
