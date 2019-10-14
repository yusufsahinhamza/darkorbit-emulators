using Newtonsoft.Json;
using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations
{
    public class SatelliteBase
    {
        public int OwnerId { get; set; }
        public int ItemId { get; set; }
        public int SlotId { get; set; }
        public int DesignId { get; set; }
        public short Type { get; set; }
        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentShieldPoints { get; set; }
        public int MaxShieldPoints { get; set; }
        public int InstallationSecondsLeft { get; set; }
        public bool Installed { get; set; }

        public SatelliteBase(int ownerId, int itemId, int slotId, int designId, short type, int currentHp, int maxHp, int currentShd, int maxShd, int installationSecondsLeft, bool installed)
        {
            OwnerId = ownerId;
            ItemId = itemId;
            SlotId = slotId;
            DesignId = designId;
            Type = type;
            CurrentHitPoints = currentHp;
            MaxHitPoints = maxHp;
            CurrentShieldPoints = currentShd;
            MaxShieldPoints = maxShd;
            InstallationSecondsLeft = installationSecondsLeft;
            Installed = installed;
        }
    }

    class Satellite : Activatable
    {
        public int DesignId { get; set; }
        public BattleStation BattleStation { get; set; }
        public int OwnerId { get; set; }

        public bool EmergencyRepairActive = false;
        public bool Installed = false;
        public int InstallationSecondsLeft = 0;

        public int ItemId { get; set; }
        public int SlotId { get; set; }
        public short Type { get; set; }

        public Satellite(BattleStation battleStation, int ownerId, string name, int designId, int itemId, int slotId, short type, Position position) : base(battleStation.Spacemap, battleStation.FactionId, position, battleStation.Clan, AssetTypeModule.SATELLITE)
        {
            ShieldAbsorption = 0.8;
            BattleStation = battleStation;
            OwnerId = ownerId;
            Name = name;
            DesignId = designId;
            ItemId = itemId;
            SlotId = slotId;
            Type = type;

            MaxHitPoints = 100000;
            CurrentHitPoints = MaxHitPoints;
            CurrentShieldPoints = 100000;
            MaxShieldPoints = 100000;

            Program.TickManager.AddTick(this);
        }

        public DateTime installationTime = new DateTime();
        public new void Tick()
        {
            if (!Installed)
            {
                var player = GameManager.GetPlayerById(OwnerId);

                if (InstallationSecondsLeft > 0)
                {
                    if (BattleStation.AssetTypeId == AssetTypeModule.ASTEROID)
                    {
                        if (player == null || player.Position.DistanceTo(BattleStation.Position) > 700)
                            Remove(false, true, true);
                    }

                    if (installationTime.AddSeconds(1) < DateTime.Now)
                    {
                        InstallationSecondsLeft--;
                        installationTime = DateTime.Now;
                    }
                }
                else if (InstallationSecondsLeft <= 0)
                {
                    Installed = true;

                    if (BattleStation.AssetTypeId == AssetTypeModule.BATTLESTATION)
                        RemoveVisualModifier(VisualModifierCommand.BATTLESTATION_INSTALLING);

                    if (player != null)
                        BattleStation.Click(player.GameSession);

                    QueryManager.BattleStations.Modules(BattleStation);
                }
            }
            else if (Installed)
            {
                if (BattleStation.AssetTypeId == AssetTypeModule.BATTLESTATION)
                {
                    if (Type != StationModuleModule.DEFLECTOR && Type != StationModuleModule.HULL && Type != StationModuleModule.NONE
                        && Type != StationModuleModule.DAMAGE_BOOSTER && Type != StationModuleModule.EXPERIENCE_BOOSTER 
                        && Type != StationModuleModule.HONOR_BOOSTER && Type != StationModuleModule.REPAIR)
                    {
                        foreach (var character in Spacemap.Characters.Values)
                        {
                            if (character is Player || character is Pet)
                                Attack(character);
                        }
                    }
                    else if (Type == StationModuleModule.REPAIR)
                        RepairModules();
                }
            }
        }

        public DateTime repairTime = new DateTime();
        public void RepairModules()
        {
            //TODO check
            if (!Destroyed && repairTime.AddSeconds(1) < DateTime.Now)
            {
                foreach (var module in BattleStation.EquippedStationModule[Clan.Id])
                {
                    if (module.LastCombatTime.AddSeconds(10) >= DateTime.Now) return;
                    if (module.CurrentHitPoints >= module.MaxHitPoints) return;

                    module.Heal(7500);
                }

                repairTime = DateTime.Now;
            }
        }

        public DateTime lastAttackTime = new DateTime();
        public void Attack(Attackable target, double shieldPenetration = 0)
        {
            var missProbability = Type == StationModuleModule.LASER_LOW_RANGE ? 0.1 : Type == StationModuleModule.LASER_MID_RANGE ? 0.3 : Type == StationModuleModule.LASER_HIGH_RANGE ? 0.4 : Type == StationModuleModule.ROCKET_LOW_ACCURACY ? 0.5 : Type == StationModuleModule.ROCKET_MID_ACCURACY ? 0.3 : 1.00;

            var damage = AttackManager.RandomizeDamage((Type == StationModuleModule.LASER_LOW_RANGE ? 59850 : Type == StationModuleModule.LASER_MID_RANGE ? 48450 : Type == StationModuleModule.LASER_HIGH_RANGE ? 28500 : Type == StationModuleModule.ROCKET_LOW_ACCURACY ? 85500 : Type == StationModuleModule.ROCKET_MID_ACCURACY ? 71250 : 0), missProbability);
            damage = 1000; //for test

            var damageType = (Type == StationModuleModule.LASER_LOW_RANGE || Type == StationModuleModule.LASER_MID_RANGE || Type == StationModuleModule.LASER_HIGH_RANGE) ? DamageType.LASER : (Type == StationModuleModule.ROCKET_LOW_ACCURACY || Type == StationModuleModule.ROCKET_MID_ACCURACY) ? DamageType.ROCKET : DamageType.LASER;

            var cooldown = (Type == StationModuleModule.ROCKET_LOW_ACCURACY || Type == StationModuleModule.ROCKET_MID_ACCURACY) ? 2 : 1;

            if (target.Position.DistanceTo(Position) < GetRange())
            {
                if (!TargetDefinition(target)) return;

                if (lastAttackTime.AddSeconds(cooldown) < DateTime.Now)
                {
                    int damageShd = 0, damageHp = 0;

                    double shieldAbsorb = System.Math.Abs(target.ShieldAbsorption - shieldPenetration);

                    if (shieldAbsorb > 1)
                        shieldAbsorb = 1;

                    if ((target.CurrentShieldPoints - damage) >= 0)
                    {
                        damageShd = (int)(damage * shieldAbsorb);
                        damageHp = damage - damageShd;
                    }
                    else
                    {
                        int newDamage = damage - target.CurrentShieldPoints;
                        damageShd = target.CurrentShieldPoints;
                        damageHp = (int)(newDamage + (damageShd * shieldAbsorb));
                    }

                    if ((target.CurrentHitPoints - damageHp) < 0)
                    {
                        damageHp = target.CurrentHitPoints;
                    }

                    if (target is Player && !(target as Player).Attackable())
                    {
                        damage = 0;
                        damageShd = 0;
                        damageHp = 0;
                    }

                    if (damageType == DamageType.LASER)
                    {
                        if (target is Player && (target as Player).Storage.Sentinel)
                            damageShd -= Maths.GetPercentage(damageShd, 30);

                        if (target is Player && (target as Player).Storage.Diminisher)
                            if ((target as Player).Storage.UnderDiminisherEntity == this)
                                damageShd += Maths.GetPercentage(damage, 30);

                        var laserRunCommand = AttackLaserRunCommand.write(Id, target.Id, 0, false, false);
                        SendCommandToInRangeCharacters(laserRunCommand);
                    }
                    else if (damageType == DamageType.ROCKET)
                    {
                        var rocketRunPacket = $"0|v|{Id}|{target.Id}|H|" + 1 + "|0|1";
                        SendPacketToInRangeCharacters(rocketRunPacket);
                    }

                    if (damage == 0)
                    {
                        SendCommandToInRangeCharacters(AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 1), target);

                        if (target is Player)
                            (target as Player).SendCommand(AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 0));
                    }
                    else
                    {
                        var attackHitCommand =
                                AttackHitCommand.write(new AttackTypeModule((short)damageType), Id,
                                                     target.Id, target.CurrentHitPoints,
                                                     target.CurrentShieldPoints, target.CurrentNanoHull,
                                                     damage > damageShd ? damage : damageShd, false);

                        SendCommandToInRangeCharacters(attackHitCommand);
                    }

                    if (damageHp >= target.CurrentHitPoints || target.CurrentHitPoints <= 0)
                        target.Destroy(this, DestructionType.MISC);
                    else
                    {
                        if (target.CurrentNanoHull > 0)
                        {
                            if (target.CurrentNanoHull - damageHp < 0)
                            {
                                var nanoDamage = damageHp - target.CurrentNanoHull;
                                target.CurrentNanoHull = 0;
                                target.CurrentHitPoints -= nanoDamage;
                            }
                            else
                                target.CurrentNanoHull -= damageHp;
                        }
                        else
                            target.CurrentHitPoints -= damageHp;
                    }

                    target.CurrentShieldPoints -= damageShd;
                    target.LastCombatTime = DateTime.Now;

                    target.UpdateStatus();

                    lastAttackTime = DateTime.Now;
                }
            }
        }

        public override void Click(GameSession gameSession) { }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(GetAssetType(), Name,
                                          FactionId, Clan.Tag, Id, DesignId, 0,
                                          Position.X, Position.Y, Clan.Id, false, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          VisualModifiers.Values.ToList());
        }

        public void Remove(bool deleteModule = false, bool removeList = true, bool closeUI = false)
        {
            var player = GameManager.GetPlayerById(OwnerId);

            if (player != null)
            {
                var module = player.Storage.BattleStationModules.Where(x => x.Id == ItemId).FirstOrDefault();

                if (module != null)
                {
                    if (deleteModule)
                        player.Storage.BattleStationModules.Remove(module);
                    else
                    {
                        BattleStation.EquippedStationModule[player.Clan.Id].Remove(this);

                        if (removeList)
                        {
                            if (BattleStation.EquippedStationModule[player.Clan.Id].Count == 0)
                                BattleStation.EquippedStationModule.Remove(player.Clan.Id);
                        }

                        module.InUse = false;
                    }

                    if (closeUI)
                        player.SendCommand(OutOfBattleStationRangeCommand.write(BattleStation.Id));

                    QueryManager.SavePlayer.Modules(player);
                    QueryManager.BattleStations.Modules(BattleStation);
                }
            }

            Program.TickManager.RemoveTick(this);
        }

        public int GetRange()
        {
            return Type == StationModuleModule.LASER_LOW_RANGE ? 590 : Type == StationModuleModule.LASER_MID_RANGE ? 650 : Type == StationModuleModule.LASER_HIGH_RANGE ? 720 : Type == StationModuleModule.ROCKET_LOW_ACCURACY ? 900 : Type == StationModuleModule.ROCKET_MID_ACCURACY ? 780 : 0;
        }

        public static string GetName(short type)
        {
            return type == StationModuleModule.REPAIR ? "REPM-1" : type == StationModuleModule.LASER_HIGH_RANGE ? "LTM-HR" : type == StationModuleModule.LASER_MID_RANGE ? "LTM-MR" : type == StationModuleModule.LASER_LOW_RANGE ? "LTM-LR" : type == StationModuleModule.ROCKET_LOW_ACCURACY ? "RAM-LA" : type == StationModuleModule.ROCKET_MID_ACCURACY ? "RAM-MA" : type == StationModuleModule.HONOR_BOOSTER ? "HONM-1" : type == StationModuleModule.DAMAGE_BOOSTER ? "DMGM-1" : type == StationModuleModule.EXPERIENCE_BOOSTER ? "XPM-1" : "";
        }

        public static Position GetPosition(Position center, int slotId)
        {
            return slotId == 9 ? new Position(center.X - 171, center.Y - 236) : slotId == 2 ? new Position(center.X + 170, center.Y - 235) : slotId == 3 ? new Position(center.X + 412, center.Y - 98) : slotId == 4 ? new Position(center.X + 412, center.Y + 97) : slotId == 5 ? new Position(center.X + 170, center.Y + 236) : slotId == 6 ? new Position(center.X - 171, center.Y + 235) : slotId == 7 ? new Position(center.X - 413, center.Y + 97) : slotId == 8 ? new Position(center.X - 413, center.Y - 98) : center;
        }
    }
}
