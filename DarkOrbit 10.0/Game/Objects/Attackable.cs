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
using Ow.Utils;
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
        public bool Invincible { get; set; }

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
            Invincible = false;

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

                if (this is Player player)
                {                 
                    if (Duel.InDuel(player) && player.Storage.Duel?.GetOpponent(player) != attackable)
                        return false;
                }
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

            foreach (var otherCharacter in Spacemap?.Characters.Values)
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

        public void SendPacketToInRangeCharacters(string packet)
        {
            foreach (var character in Spacemap.Characters.Values)
                if (character is Player player && character.Position.DistanceTo(Position) < RenderRange)
                    player.SendPacket(packet);
        }

        public void SendCommandToInRangeCharacters(byte[] command, Attackable expectPlayer = null)
        {
            foreach (var character in Spacemap.Characters.Values)
                if (character is Player player && player != expectPlayer && character.Position.DistanceTo(Position) < RenderRange)
                    player.SendCommand(command);
        }

        public void Destroy(Attackable destroyer, DestructionType destructionType)
        {
            if (this is Spaceball || Destroyed) return;

            if (MainAttacker != null && MainAttacker is Player)
            {
                destroyer = MainAttacker;
                destructionType = DestructionType.PLAYER;
            }

            Destroyed = true;

            var destroyCommand = ShipDestroyedCommand.write(Id, 0);

            if (this is Activatable && (this is Satellite && (this as Satellite).BattleStation.AssetTypeId == AssetTypeModule.BATTLESTATION))
                GameManager.SendCommandToMap(Spacemap.Id, destroyCommand);
            else if (this is Character)
                SendCommandToInRangePlayers(destroyCommand);

            if (this is Player player)
            {
                if (EventManager.JackpotBattle.InEvent(player))
                    GameManager.SendPacketToMap(EventManager.JackpotBattle.Spacemap.Id, $"0|A|STM|msg_jackpot_players_left|%COUNT%|{(EventManager.JackpotBattle.Spacemap.Characters.Count - 1)}");

                if (Duel.InDuel(player))
                    Duel.RemovePlayer(player);

                if (destroyer is Player && (destroyer as Player).Storage.KilledPlayerIds.Where(x => x == player.Id).Count() <= 13)
                    (destroyer as Player).Storage.KilledPlayerIds.Add(player.Id);

                player.SkillManager.DisableAllSkills();
                player.Pet.Deactivate(true);
                player.SendCommand(destroyCommand);
                player.DisableAttack(player.Settings.InGameSettings.selectedLaser);
                player.CurrentInRangePortalId = -1;
                player.Storage.InRangeAssets.Clear();
                player.KillScreen(destroyer, destructionType);
            }
            else if (this is BattleStation battleStation)
            {
                foreach (var module in battleStation.EquippedStationModule[battleStation.Clan.Id])
                    module.Destroy(destroyer, destructionType);

                battleStation.VisualModifiers.Clear();
                battleStation.EquippedStationModule.Remove(battleStation.Clan.Id);
                battleStation.Clan = GameManager.GetClan(0);
                battleStation.InBuildingState = false;
                battleStation.FactionId = 0;
                battleStation.BuildTimeInMinutes = 0;
                battleStation.AssetTypeId = AssetTypeModule.ASTEROID;

                Program.TickManager.RemoveTick(battleStation);

                GameManager.SendCommandToMap(Spacemap.Id, battleStation.GetAssetCreateCommand(0));
                QueryManager.BattleStations.BattleStation(battleStation);
                QueryManager.BattleStations.Modules(battleStation);
            }
            else if (this is Satellite satellite)
            {
                satellite.Remove(true);
                satellite.Type = StationModuleModule.NONE;
                satellite.CurrentHitPoints = 0;
                satellite.CurrentShieldPoints = 0;
                satellite.DesignId = 0;
                Program.TickManager.RemoveTick(satellite);

                if (satellite.BattleStation.Destroyed)
                {
                    var activatable = satellite as Activatable;
                    satellite.Spacemap.Activatables.TryRemove(satellite.Id, out activatable);
                    GameManager.SendCommandToMap(Spacemap.Id, AssetRemoveCommand.write(satellite.GetAssetType(), satellite.Id));
                }
                else if (satellite.BattleStation.AssetTypeId == AssetTypeModule.BATTLESTATION)
                    GameManager.SendCommandToMap(Spacemap.Id, satellite.GetAssetCreateCommand(0));

                QueryManager.BattleStations.Modules(satellite.BattleStation);
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

                    if (count < 13 && !Duel.InDuel(destroyerPlayer))
                    {
                        int experience = destroyerPlayer.Ship.GetExperienceBoost((this as Character).Ship.Rewards.Experience);
                        experience += Maths.GetPercentage(experience, destroyerPlayer.BoosterManager.GetPercentage(BoostedAttributeType.EP));

                        int honor = destroyerPlayer.GetHonorBoost(destroyerPlayer.Ship.GetHonorBoost((this as Character).Ship.Rewards.Honor));
                        honor += Maths.GetPercentage(honor, destroyerPlayer.BoosterManager.GetPercentage(BoostedAttributeType.HONOUR));

                        int uridium = (this as Character).Ship.Rewards.Uridium;
                        var changeType = ChangeType.INCREASE;

                        if (destroyerPlayer.TargetDefinition(this))
                            changeType = ChangeType.DECREASE;

                        destroyerPlayer.ChangeData(DataType.EXPERIENCE, experience);
                        destroyerPlayer.ChangeData(DataType.HONOR, honor, changeType);
                        destroyerPlayer.ChangeData(DataType.URIDIUM, uridium, changeType);
                    }
                    else if (count > 13 && !Duel.InDuel(destroyerPlayer))
                        destroyerPlayer.SendPacket($"0|A|STM|pusher_info_no_reward|%NAME%|{Name}");

                    if (this is Player) //TODO CHECK FOR NPC AND ETC...
                        new CargoBox(Position, Spacemap, false, false, destroyerPlayer);
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

        public DateTime outOfRangeCooldown = new DateTime();
        public DateTime inAttackCooldown = new DateTime();
        public DateTime peaceAreaCooldown = new DateTime();
        public bool TargetDefinition(Attackable target, bool sendMessage = true, bool isPlayerRocketAttack = false)
        {
            if (target == null) return false;

            short relationType = Clan.Id != 0 && target.Clan.Id != 0 ? Clan.GetRelation(target.Clan) : (short)0;

            if (this is Player player)
            {
                if (relationType != ClanRelationModule.AT_WAR)
                {
                    if ((target is Player || target is Pet) &&
                        !(target is Pet pet && pet == player.Pet) &&
                        (target.FactionId == FactionId || target.Clan.Id == Clan.Id) &&
                        (!EventManager.JackpotBattle.InEvent(player) &&
                        !Duel.InDuel(player)))
                    {
                        player.DisableAttack(player.Settings.InGameSettings.selectedLaser);

                        if (sendMessage)
                            player.SendPacket("0|A|STD|You can't attack members of your own company!");

                        return false;
                    }
                    else if (target.FactionId == FactionId || target.Clan.Id == Clan.Id || relationType == ClanRelationModule.ALLIED || relationType == ClanRelationModule.NON_AGGRESSION_PACT)
                        return false;
                }

                if (target is Player targetPlayer && targetPlayer.Group != null)
                {
                    if (player.Group == targetPlayer.Group)
                    {
                        player.DisableAttack(player.Settings.InGameSettings.selectedLaser);

                        if (sendMessage)
                            player.SendPacket("0|A|STD|You can't attack members of your group!");

                        return false;
                    }
                }

                if (target is Player && (target as Player).Storage.IsInDemilitarizedZone || target is Pet && (target as Pet).Owner.Storage.IsInDemilitarizedZone || (Duel.InDuel(player) && player.Storage.Duel.PeaceArea))
                {
                    player.DisableAttack(player.Settings.InGameSettings.selectedLaser);

                    if (peaceAreaCooldown.AddSeconds(10) < DateTime.Now)
                    {
                        if (sendMessage)
                        {
                            player.SendPacket("0|A|STM|peacearea");

                            if (target is Player)
                                (target as Player).SendPacket("0|A|STM|peacearea");

                            peaceAreaCooldown = DateTime.Now;
                        }
                    }
                    return false;
                }

                if (inAttackCooldown.AddSeconds(10) < DateTime.Now)
                {
                    if (sendMessage)
                    {
                        player.SendPacket("0|A|STM|oppoatt|%!|" + (target is Player && EventManager.JackpotBattle.InEvent(target as Player) ? EventManager.JackpotBattle.Name : target.Name));
                        inAttackCooldown = DateTime.Now;
                    }
                }
            }
            else if (this is Satellite)
            {
                if (relationType != ClanRelationModule.AT_WAR && (target.FactionId == FactionId || target.Clan.Id == Clan.Id || relationType == ClanRelationModule.ALLIED || relationType == ClanRelationModule.NON_AGGRESSION_PACT))
                    return false;
            }

            var range = this is Player ? (isPlayerRocketAttack ? (this as Player).AttackManager.GetRocketRange() : AttackRange) : this is Satellite ? (this as Satellite).GetRange() : AttackRange;

            if (Position.DistanceTo(target.Position) > range)
            {
                if (outOfRangeCooldown.AddSeconds(5) < DateTime.Now)
                {
                    if (sendMessage)
                    {
                        if (this is Player && !isPlayerRocketAttack)
                            (this as Player).SendPacket("0|A|STM|outofrange");

                        if (target is Player)
                            (target as Player).SendPacket("0|A|STM|attescape");

                        outOfRangeCooldown = DateTime.Now;
                    }
                }
                return false;
            }
            return true;
        }

        public void Heal(int amount, int healerId = 0, HealType healType = HealType.HEALTH)
        {
            if (amount < 0)
                return;

            switch (healType)
            {
                case HealType.HEALTH:
                    if (CurrentHitPoints + amount > MaxHitPoints)
                        amount = MaxHitPoints - CurrentHitPoints;
                    CurrentHitPoints += amount;
                    break;
                case HealType.SHIELD:
                    if (CurrentShieldPoints + amount > MaxShieldPoints)
                        amount = MaxShieldPoints - CurrentShieldPoints;
                    CurrentShieldPoints += amount;
                    break;
            }

            var healPacket = "0|A|HL|" + healerId + "|" + Id + "|" + (healType == HealType.HEALTH ? "HPT" : "SHD") + "|" + CurrentHitPoints + "|" + amount;

            if (this is Player player)
            {
                if (!Invisible)
                {
                    foreach (var otherPlayers in InRangeCharacters.Values)
                        if (otherPlayers.Selected == this)
                            if (otherPlayers is Player)
                                (otherPlayers as Player).SendPacket(healPacket);
                }

                player.SendPacket(healPacket);
            }
            else if (this is Activatable)
            {
                foreach (var character in Spacemap.Characters.Values)
                    if (character.Selected == this && character is Player && character.Position.DistanceTo(Position) < RenderRange)
                        (character as Player).SendPacket(healPacket);
            }

            UpdateStatus();
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
                        player.Invincible = true;
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
