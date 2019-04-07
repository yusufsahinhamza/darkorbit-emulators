using Ow.Game.Clans;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using Ow.Game.Events;
using Ow.Game.Objects.Stations;
using Ow.Game.Objects.Players;
using Ow.Net.netty;

namespace Ow.Game.Objects
{
    class Player : Character
    {
        public int RankId { get; set; }
        public bool Premium { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }

        public int CurrentInRangePortalId = -1;
        public int CurrentShieldConfig1 { get; set; }
        public int CurrentShieldConfig2 { get; set; }
        public int CurrentConfig { get; set; }

        public Settings Settings = new Settings();
        public EquipmentBase Equipment { get; set; }
        public DataBase Data { get; set; }
        public Group Group { get; set; }
        public Pet Pet { get; set; }
        public Storage Storage { get; set; }
        public AttackManager AttackManager { get; set; }
        public SettingsManager SettingsManager { get; set; }
        public DroneManager DroneManager { get; set; }
        public CpuManager CpuManager { get; set; }
        public TechManager TechManager { get; set; }
        public SkillManager SkillManager { get; set; }
        public BoosterManager BoosterManager { get; set; }

        public Player(int id, string name, Clan clan, int factionId, Position position, Spacemap spacemap, int rankId, int shipId)
                     : base(id, name, factionId, GameManager.GetShip(shipId), position, spacemap, clan)
        {
            Name = name;
            Clan = clan;
            FactionId = factionId;
            Spacemap = spacemap;
            RankId = rankId;

            QueryManager.LoadUser(this);
            InitiateManagers();
        }

        public void InitiateManagers()
        {
            Storage = new Storage(this);
            DroneManager = new DroneManager(this);
            AttackManager = new AttackManager(this);
            TechManager = new TechManager(this);
            SkillManager = new SkillManager(this);
            SettingsManager = new SettingsManager(this);
            CpuManager = new CpuManager(this);
            BoosterManager = new BoosterManager(this);
            Pet = new Pet(this);
        }

        public override void Tick()
        {
            Movement.ActualPosition(this);
            CheckHitpointsRepair();
            CheckShieldPointsRepair();
            CheckRadiation();
            AttackManager.LaserAttack();
            AttackManager.RocketLauncher.Tick();
            Logout();

            Storage.Tick();
            DroneManager.Tick();
            TechManager.Tick();
            SkillManager.Tick();

            /*
            if (MoveManager.Moving && Collecting)
            {
                Collecting = false;
                SendPacket("0|" + ServerCommands.SET_ATTRIBUTE + "|" + ServerCommands.ASSEMBLE_COLLECTION_BEAM_CANCELLED + "|0|" + ID + "|" + -1);
            }
            */
        }

        public DateTime lastHpRepairTime = new DateTime();
        private void CheckHitpointsRepair()
        {
            if (LastCombatTime.AddSeconds(10) >= DateTime.Now || lastHpRepairTime.AddSeconds(1) >= DateTime.Now) return;

            if (CurrentHitPoints == MaxHitPoints || AttackingOrUnderAttack())
            {
                if (Storage.RepairBotActivated)
                    RepairBot(false);
                return;
            }

            RepairBot(true);

            int repairHitpoints = MaxHitPoints / 35;
            Heal(repairHitpoints);

            lastHpRepairTime = DateTime.Now;
        }

        public DateTime lastShieldRepairTime = new DateTime();
        private void CheckShieldPointsRepair()
        {
            if (LastCombatTime.AddSeconds(10) >= DateTime.Now || lastShieldRepairTime.AddSeconds(1) >= DateTime.Now ||
                CurrentShieldPoints == MaxShieldPoints || Settings.InGameSettings.selectedFormation == DroneManager.MOTH_FORMATION
                || Settings.InGameSettings.selectedFormation == DroneManager.WHEEL_FORMATION) return;

            int repairShield = MaxShieldPoints / 25;
            CurrentShieldPoints += repairShield;
            UpdateStatus();

            lastShieldRepairTime = DateTime.Now;
        }

        public DateTime lastRadiationDamageTime = new DateTime();
        public void CheckRadiation()
        {
            if (Storage.Jumping || !Storage.IsInRadiationZone || Storage.invincibilityEffectTime.AddSeconds(5) >= DateTime.Now || lastRadiationDamageTime.AddSeconds(1) >= DateTime.Now) return;

            AttackManager.Damage(this, this, DamageType.RADIATION, 20000, true, true, false);
            lastRadiationDamageTime = DateTime.Now;
        }

        public void SetSpeedBoost(int speed)
        {
            Storage.SpeedBoost = speed;
            SendCommand(SetSpeedCommand.write(Speed, Speed));
        }

        public void RepairBot(bool activated)
        {
            Storage.RepairBotActivated = activated;
            SendCommand(GetBeaconCommand());
        }

        public void SetShieldSkillActivated(bool pShieldSkillActivated)
        {
            Storage.ShieldSkillActivated = pShieldSkillActivated;

            if (pShieldSkillActivated)
                SendCommand(AttributeSkillShieldUpdateCommand.write(1, 1, 0));
            else
                SendCommand(AttributeSkillShieldUpdateCommand.write(0, 0, 0));
        }

        public override int Speed
        {
            get
            {
                var value = CurrentConfig == 1 ? Equipment.Config1Speed : Equipment.Config2Speed;

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.CRAB_FORMATION:
                        value -= Maths.GetPercentage(value, 20);
                        break;
                    case DroneManager.BAT_FORMATION:
                        value -= Maths.GetPercentage(value, 15);
                        break;
                    case DroneManager.RING_FORMATION:
                        value -= Maths.GetPercentage(value, 5);
                        break;
                    case DroneManager.DRILL_FORMATION:
                        value -= Maths.GetPercentage(value, 5);
                        break;
                    case DroneManager.WHEEL_FORMATION:
                        value += Maths.GetPercentage(value, 5);
                        break;
                }

                if (Storage.underDCR_250)
                    value -= Maths.GetPercentage(value, 30);

                if (Storage.underSLM_01)
                    value -= Maths.GetPercentage(value, 50);

                if (Storage.underR_IC3)
                    value -= value;

                value += Storage.SpeedBoost;

                return value;
            }
        }

        public override int MaxHitPoints
        {
            get
            {
                var value = CurrentConfig == 1 ? Equipment.Config1Hitpoints : Equipment.Config2Hitpoints;
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.MAXHP));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.CHEVRON_FORMATION:
                        value -= Maths.GetPercentage(value, 20);
                        break;
                    case DroneManager.DIAMOND_FORMATION:
                        value -= Maths.GetPercentage(value, 30);
                        break;
                    case DroneManager.MOTH_FORMATION:
                    case DroneManager.HEART_FORMATION:
                        value += Maths.GetPercentage(value, 20);
                        break;
                }
                value = Ship.GetHitPointsBoost(value);
                return value;
            }
        }

        public int RocketSpeed
        {
            get
            {
                var value = Premium ? 1 : 3;

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.RING_FORMATION:
                        value += Maths.GetPercentage(value, 25);
                        break;
                }

                return value;
            }
        }

        public override int CurrentShieldPoints
        {
            get
            {
                var value = CurrentConfig == 1 ? CurrentShieldConfig1 : CurrentShieldConfig2;
                return value;
            }
            set
            {
                if (CurrentConfig == 1)
                    CurrentShieldConfig1 = value;
                else
                    CurrentShieldConfig2 = value;
            }
        }

        public override int MaxShieldPoints
        {
            get
            {
                var value = CurrentConfig == 1 ? Equipment.Config1Shield : Equipment.Config2Shield;
                value += Maths.GetPercentage(value, 40);
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.SHIELD));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.HEART_FORMATION:
                    case DroneManager.TURTLE_FORMATION:
                        value += Maths.GetPercentage(value, 10);
                        break;
                    case DroneManager.RING_FORMATION:
                        value += Maths.GetPercentage(value, 120);
                        break;
                    case DroneManager.DRILL_FORMATION:
                        value -= Maths.GetPercentage(value, 25);
                        break;
                    case DroneManager.DOUBLE_ARROW_FORMATION:
                        value -= Maths.GetPercentage(value, 20);
                        break;
                }
                value = Ship.GetShieldPointsBoost(value);
                return value;
            }
        }

        public override double ShieldAbsorption
        {
            get
            {
                var value = 0.8;
                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.CRAB_FORMATION:
                        value += 0.2;
                        break;
                    case DroneManager.BARRAGE_FORMATION:
                        value -= 0.15;
                        break;
                    case DroneManager.DRILL_FORMATION:
                        value -= 0.05;
                        break;
                }
                return value;
            }
        }

        public override double ShieldPenetration
        {
            get
            {
                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.MOTH_FORMATION:
                        return 0.2;
                    case DroneManager.DOUBLE_ARROW_FORMATION:
                        return 0.1;
                    case DroneManager.PINCER_FORMATION:
                        return -0.1;
                    default:
                        return 0;
                }
            }
        }

        public override int Damage
        {
            get
            {
                var value = CurrentConfig == 1 ? Equipment.Config1Damage : Equipment.Config2Damage;
                value += Maths.GetPercentage(value, 60);
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.DAMAGE));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.TURTLE_FORMATION:
                        value -= Maths.GetPercentage(value, (int)7.5);
                        break;
                    case DroneManager.ARROW_FORMATION:
                        value -= Maths.GetPercentage(value, 3);
                        break;
                    case DroneManager.PINCER_FORMATION:
                        value += Maths.GetPercentage(value, 3);
                        break;
                    case DroneManager.HEART_FORMATION:
                        value -= Maths.GetPercentage(value, 5);
                        break;
                    case DroneManager.RING_FORMATION:
                        value -= Maths.GetPercentage(value, 25);
                        break;
                    case DroneManager.DRILL_FORMATION:
                        value += Maths.GetPercentage(value, 25);
                        break;
                    case DroneManager.WHEEL_FORMATION:
                        value -= Maths.GetPercentage(value, 20);
                        break;
                }
                value = Ship.GetLaserDamageBoost(value);
                return value;
            }
        }

        public int GetHonorBoost(int honor)
        {
            switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
            {
                case DroneManager.PINCER_FORMATION:
                    return honor += Maths.GetPercentage(honor, 5);
                default:
                    return honor;
            }
        }

        public override int RocketDamage
        {
            get
            {
                var value = AttackManager.GetRocketDamage();
                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.TURTLE_FORMATION:
                        value -= Maths.GetPercentage(value, (int)7.5);
                        break;
                    case DroneManager.ARROW_FORMATION:
                        value += Maths.GetPercentage(value, 20);
                        break;
                    case DroneManager.STAR_FORMATION:
                        value += Maths.GetPercentage(value, 25);
                        break;
                    case DroneManager.CHEVRON_FORMATION:
                        value += Maths.GetPercentage(value, 50);
                        break;
                }
                return value;
            }
        }

        public bool UpdateActivatable(Activatable pEntity, bool pInRange)
        {
            if (Storage.InRangeAssets.ContainsKey(pEntity.Id))
            {
                if (!pInRange)
                {
                    if (pEntity is Portal)
                    {
                        CurrentInRangePortalId = -1;
                    }
                    Storage.InRangeAssets.TryRemove(pEntity.Id, out pEntity);
                    return true;
                }
            }
            else
            {
                if (pInRange)
                {
                    if (pEntity is Portal)
                    {
                        CurrentInRangePortalId = pEntity.Id;
                    }
                    Storage.InRangeAssets.TryAdd(pEntity.Id, pEntity);
                    return true;
                }
            }
            return false;
        }

        public DateTime ConfigCooldown = new DateTime();
        public void ChangeConfiguration(string LootID)
        {
            if (ConfigCooldown.AddSeconds(5) < DateTime.Now || Storage.GodMode)
            {
                SendPacket("0|S|CFG|" + LootID);
                SetCurrentConfiguration(Convert.ToInt32(LootID));
                ConfigCooldown = DateTime.Now;
            }
            else
            {
                SendPacket("0|A|STM|config_change_failed_time");
            }
        }

        public void SetCurrentConfiguration(int pCurrentConfiguration)
        {
            CurrentConfig = Convert.ToInt32(pCurrentConfiguration);
            Settings.InGameSettings.currentConfig = CurrentConfig;
            DroneManager.UpdateDrones();
            UpdateStatus();
        }

        public byte[] GetBeaconCommand()
        {
            return BeaconCommand.write(1, 1, 1, 1, Storage.IsInDemilitarizedZone, Storage.RepairBotActivated, false,
                         "equipment_extra_repbot_rep-4", Storage.IsInRadiationZone);
        }

        public byte[] GetShipCreateCommand(bool fromAdmin, short relationType, bool sameClan, bool jackpotBattle = false)
        {
            return ShipCreateCommand.write(
                Id,
                Ship.LootId,
                3,
                jackpotBattle ? "" : Clan.Tag,
                jackpotBattle ? EventManager.JackpotBattle.Name : (Name + (fromAdmin ? " (" + Id + ")" : "")),
                Position.X,
                Position.Y,
                FactionId,
                Clan.Id,
                RankId,
                RankId == 21 ? true : false,
                new ClanRelationModule((sameClan && Spacemap.Id != EventManager.JackpotBattle.Spacemap.Id) ? ClanRelationModule.ALLIED : relationType),
                100,
                false,
                false,
                false,
                (sameClan && Spacemap.Id != EventManager.JackpotBattle.Spacemap.Id) ? ClanRelationModule.NON_AGGRESSION_PACT : ClanRelationModule.NONE,
                (sameClan && Spacemap.Id != EventManager.JackpotBattle.Spacemap.Id) ? ClanRelationModule.ALLIED : relationType,
                VisualModifiers.Values.ToList(),
                new class_11d(class_11d.DEFAULT));
        }

        public byte[] GetShipInitializationCommand()
        {
            return ShipInitializationCommand.write(
                Id,
                Name,
                Ship.LootId,
                Speed,
                CurrentShieldPoints,
                MaxShieldPoints,
                CurrentHitPoints,
                MaxHitPoints,
                0,
                0,
                0,
                0,
                Position.X,
                Position.Y,
                Spacemap.Id,
                FactionId,
                Clan.Id,
                3,
                Premium,
                Data.experience,
                Data.honor,
                (short)Level,
                Data.credits,
                Data.uridium,
                0,
                RankId,
                Clan.Tag,
                100,
                true,
                Invisible,
                true,
                VisualModifiers.Values.ToList());
        }

        public bool Attackable()
        {
            return (AttackManager.IshCooldown.AddMilliseconds(TimeManager.ISH_DURATION) > DateTime.Now || Storage.invincibilityEffect || Storage.GodMode) ? false : true;
        }

        public void SendCooldown(string itemId, int time, bool countdown = false)
        {
            SendCommand(UpdateMenuItemCooldownGroupTimerCommand.write(
            SettingsManager.GetCooldownType(itemId),
            new ClientUISlotBarCategoryItemTimerStateModule( countdown ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168), time, time));
        }

        public void UpdateCurrentCooldowns()
        {
            int smbCooldown = (int)(TimeManager.SMB_COOLDOWN - (DateTime.Now - AttackManager.SmbCooldown).TotalMilliseconds);
            int ishCooldown = (int)(TimeManager.ISH_COOLDOWN - (DateTime.Now - AttackManager.IshCooldown).TotalMilliseconds);
            int empCooldown = (int)(TimeManager.EMP_COOLDOWN - (DateTime.Now - AttackManager.EmpCooldown).TotalMilliseconds);
            int mineCooldown = (int)(TimeManager.MINE_COOLDOWN - (DateTime.Now - AttackManager.mineCooldown).TotalMilliseconds);
            int dcrCooldown = (int)(TimeManager.DCR_250_COOLDOWN - (DateTime.Now - AttackManager.dcr_250Cooldown).TotalMilliseconds);
            int pldCooldown = (int)(TimeManager.PLD8_COOLDOWN - (DateTime.Now - AttackManager.pld8Cooldown).TotalMilliseconds);
            int r_ic3Cooldown = (int)(TimeManager.R_IC3_COOLDOWN - (DateTime.Now - AttackManager.r_ic3Cooldown).TotalMilliseconds);

            if (smbCooldown >= 0) Settings.Cooldowns[AmmunitionManager.SMB_01] = smbCooldown;
            if (ishCooldown >= 0) Settings.Cooldowns[AmmunitionManager.ISH_01] = ishCooldown;
            if (empCooldown >= 0) Settings.Cooldowns[AmmunitionManager.EMP_01] = empCooldown;
            if (mineCooldown >= 0) Settings.Cooldowns[AmmunitionManager.ACM_01] = mineCooldown;
            if (dcrCooldown >= 0) Settings.Cooldowns[AmmunitionManager.DCR_250] = dcrCooldown;
            if (pldCooldown >= 0) Settings.Cooldowns[AmmunitionManager.PLD_8] = pldCooldown;
            if (r_ic3Cooldown >= 0) Settings.Cooldowns[AmmunitionManager.R_IC3] = r_ic3Cooldown;

            /*
            int energyLeechCooldown = (int)(TimeManager.ENERGY_LEECH_COOLDOWN - (DateTime.Now - TechManager.EnergyLeech.cooldown).TotalMilliseconds);
            int chainImpulseCooldown = (int)(TimeManager.CHAIN_IMPULSE_COOLDOWN - (DateTime.Now - TechManager.ChainImpulse.cooldown).TotalMilliseconds);
            int precisionTargeterCooldown = (int)(TimeManager.PRECISION_TARGETER_COOLDOWN - (DateTime.Now - TechManager.PrecisionTargeter.cooldown).TotalMilliseconds);
            int backupShieldsCooldown = (int)(TimeManager.BACKUP_SHIELD_COOLDOWN - (DateTime.Now - TechManager.BackupShields.cooldown).TotalMilliseconds);
            int battleRepairBotCooldown = (int)(TimeManager.BATTLE_REPAIR_BOT_COOLDOWN - (DateTime.Now - TechManager.BattleRepairBot.cooldown).TotalMilliseconds);
            */

            /*
            if (energyLeechCooldown >= 0)  Settings.CurrentCooldowns.energyLeechCooldown = energyLeechCooldown;
            if (chainImpulseCooldown >= 0)  Settings.CurrentCooldowns.chainImpulseCooldown = chainImpulseCooldown;
            if (precisionTargeterCooldown >= 0)  Settings.CurrentCooldowns.precisionTargeterCooldown = precisionTargeterCooldown;
            if (backupShieldsCooldown >= 0)  Settings.CurrentCooldowns.backupShieldsCooldown = backupShieldsCooldown;
            if (battleRepairBotCooldown >= 0)  Settings.CurrentCooldowns.battleRepairBotCooldown = battleRepairBotCooldown;
            */

            foreach (var skill in Storage.Skills.Values)
            {
                int cooldown = (int)(skill.Cooldown - (DateTime.Now - skill.cooldown).TotalMilliseconds);
                if (cooldown >= 0) Settings.Cooldowns[skill.LootId] = cooldown;
            }

            QueryManager.SavePlayer.Settings(this, "cooldowns", Settings.Cooldowns);
        }

        public void SetCurrentCooldowns()
        {
            if (Settings.Cooldowns[AmmunitionManager.SMB_01] > 0) AttackManager.SmbCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.SMB_COOLDOWN - Settings.Cooldowns[AmmunitionManager.SMB_01]));
            if (Settings.Cooldowns[AmmunitionManager.ISH_01] > 0) AttackManager.IshCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.ISH_COOLDOWN - Settings.Cooldowns[AmmunitionManager.ISH_01]));
            if (Settings.Cooldowns[AmmunitionManager.EMP_01] > 0) AttackManager.EmpCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.EMP_COOLDOWN - Settings.Cooldowns[AmmunitionManager.EMP_01]));
            if (Settings.Cooldowns[AmmunitionManager.ACM_01] > 0) AttackManager.mineCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.MINE_COOLDOWN - Settings.Cooldowns[AmmunitionManager.ACM_01]));
            if (Settings.Cooldowns[AmmunitionManager.DCR_250] > 0) AttackManager.dcr_250Cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.DCR_250_COOLDOWN - Settings.Cooldowns[AmmunitionManager.DCR_250]));
            if (Settings.Cooldowns[AmmunitionManager.PLD_8] > 0) AttackManager.pld8Cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.PLD8_COOLDOWN - Settings.Cooldowns[AmmunitionManager.PLD_8]));
            if (Settings.Cooldowns[AmmunitionManager.R_IC3] > 0) AttackManager.r_ic3Cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.R_IC3_COOLDOWN - Settings.Cooldowns[AmmunitionManager.R_IC3]));
            /*
            TechManager.EnergyLeech.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.ENERGY_LEECH_COOLDOWN - Settings.CurrentCooldowns.energyLeechCooldown));
            TechManager.ChainImpulse.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.CHAIN_IMPULSE_COOLDOWN - Settings.CurrentCooldowns.chainImpulseCooldown));
            TechManager.PrecisionTargeter.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.PRECISION_TARGETER_COOLDOWN - Settings.CurrentCooldowns.precisionTargeterCooldown));
            TechManager.BackupShields.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.BACKUP_SHIELD_COOLDOWN - Settings.CurrentCooldowns.backupShieldsCooldown));
            TechManager.BattleRepairBot.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.BATTLE_REPAIR_BOT_COOLDOWN - Settings.CurrentCooldowns.battleRepairBotCooldown));
            */

            foreach (var skill in Storage.Skills.Values)
            {
                if (Settings.Cooldowns[skill.LootId] > 0) skill.cooldown = DateTime.Now.AddMilliseconds(-(skill.Cooldown - Settings.Cooldowns[skill.LootId]));
            }
        }

        public void SelectShip(int targetId)
        {
            if (AttackManager.Attacking)
                DisableAttack(SettingsManager.Player.Settings.InGameSettings.selectedLaser);

            try
            {
                foreach (var entry in InRangeCharacters.Values.Where(entry => entry.Id == targetId))
                {
                    if (entry is Player && ((entry as Player).AttackManager.EmpCooldown.AddMilliseconds(TimeManager.EMP_DURATION) > DateTime.Now)) return;
                    Selected = entry;

                    SendCommand(ShipSelectionCommand.write(
                        entry.Id,
                        entry.Ship.Id,
                        entry.CurrentShieldPoints,
                        entry.MaxShieldPoints,
                        entry.CurrentHitPoints,
                        entry.MaxHitPoints,
                        0,
                        0,
                        entry is Player ? true : false));

                    Group?.UpdatePlayer(this, new List<command_i3O> { new GroupPlayerTargetModule(new GroupPlayerShipModule(entry.Ship.GroupShipId), entry.Name, new GroupPlayerInformationsModule(entry.CurrentHitPoints, entry.MaxHitPoints, entry.CurrentShieldPoints, entry.MaxShieldPoints, entry.CurrentNanoHull, entry.MaxNanoHull)) });
                }
            }
            catch (Exception e)
            {
                Out.WriteLine(e.StackTrace);
            }
        }

        public void ChangeShip(int shipId)
        {
            var player = this;
            if (player.Storage.Jumping) return;

            player.Storage.Jumping = true;

            var pet = player.Pet.Activated;
            var gearId = player.Pet.GearId;
            player.Pet.Deactivate(true);

            player.SkillManager.DisableAllSkills();
            player.Ship = GameManager.GetShip(shipId);
            player.SkillManager.InitiateSkills();
            player.CurrentInRangePortalId = -1;
            player.Deselection();
            player.Spacemap.RemoveCharacter(player);
            player.Storage.InRangeAssets.Clear();
            player.InRangeCharacters.Clear();

            player.Spacemap.AddAndInitPlayer(player);
            player.Storage.Jumping = false;

            if (pet)
            {
                player.Pet.Activate();
                player.Pet.SwitchGear(gearId);
            }
        }

        public void Jump(int mapId, Position targetPosition)
        {
            var player = this;

            player.Storage.Jumping = true;

            var pet = player.Pet.Activated;
            var gearId = player.Pet.GearId;
            player.Pet.Deactivate(true);

            player.CurrentInRangePortalId = -1;
            player.Deselection();
            player.Spacemap.RemoveCharacter(player);
            player.Storage.InRangeAssets.Clear();
            player.InRangeCharacters.Clear();
            player.SetPosition(targetPosition);

            var targetSpacemap = GameManager.GetSpacemap(mapId);
            player.Spacemap = targetSpacemap;

            player.Spacemap.AddAndInitPlayer(player);
            player.Storage.Jumping = false;

            if (pet)
            {
                player.Pet.Activate();
                player.Pet.SwitchGear(gearId);
            }
        }

        public void KillScreen(Character killerPlayer, DestructionType destructionType, bool killedLogin = false)
        {
            var killScreenOptionModules = new List<KillScreenOptionModule>();
            var basicRepair =
                   new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_REPAIR),
                                              new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()));
            var portalRepairTime = (int)(15 - ((DateTime.Now - Storage.KillscreenPortalRepairTime).TotalSeconds));
            var portalRepairPrice = 200;
            var portalRepair =
                  new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.AT_JUMPGATE_REPAIR),
                                             new PriceModule(PriceModule.URIDIUM, portalRepairPrice), Data.uridium >= portalRepairPrice, portalRepairTime,
                                             new MessageLocalizedWildcardCommand("desc_killscreen_repair_gate", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule> { new MessageWildcardReplacementModule("%COUNT%", portalRepairPrice.ToString(), new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED)) }),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand(Data.uridium >= portalRepairPrice ? "btn_killscreen_repair_for_uri" : "btn_killscreen_payment", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule> { new MessageWildcardReplacementModule("%COUNT%", portalRepairPrice.ToString(), new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED)) }));
            var deathLocationRepairTime = (int)(30 - ((DateTime.Now - Storage.KillscreenDeathLocationRepairTime).TotalSeconds));
            var deathLocationRepairPrice = 300;
            var deathLocationRepair =
                  new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR),
                                             new PriceModule(PriceModule.URIDIUM, deathLocationRepairPrice), Data.uridium >= deathLocationRepairPrice, deathLocationRepairTime,
                                             new MessageLocalizedWildcardCommand("desc_killscreen_repair_location", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule> { new MessageWildcardReplacementModule("%COUNT%", deathLocationRepairPrice.ToString(), new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED)) }),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand(Data.uridium >= deathLocationRepairPrice ? "btn_killscreen_repair_for_uri" : "btn_killscreen_payment", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule> { new MessageWildcardReplacementModule("%COUNT%", deathLocationRepairPrice.ToString(), new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED)) }));
            var fullRepair =
                   new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_FULL_REPAIR),
                                              new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), new List<MessageWildcardReplacementModule>()));
            killScreenOptionModules.Add(basicRepair);

            if (!killedLogin)
            {
                if (Spacemap.Activatables.FirstOrDefault(x => x.Value is Portal).Value is Portal portal && portal.Working && Data.uridium >= portalRepairPrice)
                    killScreenOptionModules.Add(portalRepair);

                if (Spacemap.Options.DeathLocationRepair && Data.uridium >= deathLocationRepairPrice)
                    killScreenOptionModules.Add(deathLocationRepair);

                //killScreenOptionModules.Add(fullRepair);
            }

            var killScreenPostCommand =
                    KillScreenPostCommand.write(killerPlayer != null ? killerPlayer.Name : "", "http://localhost/indexInternal.es?action=internalDock",
                                              "MISC", new DestructionTypeModule((short)destructionType),
                                              killScreenOptionModules);

            SendCommand(killScreenPostCommand);
        }

        public void Respawn(bool basicRepair = false, bool deathLocation = false, bool atNearestPortal = false, bool fullRepair = false)
        {
            AddVisualModifier(new VisualModifierCommand(Id, VisualModifierCommand.INVINCIBILITY, 0, "", 0, true));

            Storage.IsInDemilitarizedZone = basicRepair || fullRepair ? true : false;
            Settings.InGameSettings.inEquipZone = basicRepair || fullRepair ? true : false;
            Storage.IsInRadiationZone = false;

            if (atNearestPortal)
                SetPosition(GetNearestPortalPosition());
            else if (deathLocation)
                CurrentHitPoints = Maths.GetPercentage(MaxHitPoints, 10);
            else
            {
                CurrentHitPoints = Maths.GetPercentage(MaxHitPoints, 1);
                SetPosition(FactionId == 1 ? Position.MMOPosition : FactionId == 2 ? Position.EICPosition : Position.VRUPosition);
            }

            if (basicRepair || fullRepair)
                Spacemap = GameManager.GetSpacemap(GetBaseMapId());

            if (fullRepair)
            {
                CurrentHitPoints = MaxHitPoints;
                CurrentShieldConfig1 = MaxShieldPoints;
                CurrentShieldConfig2 = MaxShieldPoints;
            }

            Spacemap.AddAndInitPlayer(this);

            Destroyed = false;
        }

        public int GetBaseMapId()
        {
            //return FactionId == 1 ? 13 : FactionId == 2 ? 14 : 15;
            return 16;
        }

        public void ChangeData(DataType dataType, int amount, ChangeType changeType = ChangeType.INCREASE)
        {
            amount = Convert.ToInt32(amount);
            switch (dataType)
            {
                case DataType.URIDIUM:
                    Data.uridium = (changeType == ChangeType.INCREASE ? (Data.uridium + amount) : (Data.uridium - amount));
                    if (Data.uridium < 0) Data.uridium = 0;
                    SendPacket("0|LM|ST|URI|" + (changeType == ChangeType.DECREASE ? "-" : "") + "" + amount + "|" + Data.uridium);
                    break;
                case DataType.CREDITS:
                    Data.credits = (changeType == ChangeType.INCREASE ? (Data.credits + amount) : (Data.credits - amount));
                    if (Data.credits < 0) Data.credits = 0;
                    SendPacket("0|LM|ST|CRE|" + (changeType == ChangeType.DECREASE ? "-" : "") + "" + amount + "|" + Data.credits);
                    break;
                case DataType.HONOR:
                    Data.honor = (changeType == ChangeType.INCREASE ? (Data.honor + amount) : (Data.honor - amount));
                    SendPacket("0|LM|ST|HON|" + (changeType == ChangeType.DECREASE ? "-" : "") + "" + amount + "|" + Data.honor);
                    break;
                case DataType.EXPERIENCE:
                    Data.experience = (changeType == ChangeType.INCREASE ? (Data.experience + amount) : (Data.experience - amount));
                    if (Data.experience < 0) Data.experience = 0;
                    SendPacket("0|LM|ST|EP|" + (changeType == ChangeType.DECREASE ? "-" : "") + "" + amount + "|" + Data.experience + "|" + Level);
                    CheckNextLevel(Data.experience);
                    break;
                case DataType.JACKPOT:
                    break;
            }
            QueryManager.SavePlayer.Information(this);
        }

        public void CheckNextLevel(long experience)
        {
            short lvl = 1;
            long expNext = 10000;

            while (experience >= expNext)
            {
                expNext *= 2;
                lvl++;
            }

            if (lvl > Level)
            {
                SendPacket($"0|{ServerCommands.SET_ATTRIBUTE}|{ServerCommands.LEVEL_UPDATE}|{lvl}|{expNext - experience}");
                var levelUpCommand = LevelUpCommand.write(Id, lvl);
                SendCommand(levelUpCommand);
                SendCommandToInRangePlayers(levelUpCommand);
                Level = lvl;
                QueryManager.SavePlayer.Information(this);
            }
        }

        public bool AttackingOrUnderAttack(int combatSecond = 10)
        {
            if (LastCombatTime.AddSeconds(combatSecond) > DateTime.Now) return true;
            if (AttackManager.Attacking) return true;
            return false;
        }

        public bool LastAttackTime(int combatSecond = 10)
        {
            if (AttackManager.lastAttackTime.AddSeconds(combatSecond) > DateTime.Now) return true;
            if (AttackManager.lastRSBAttackTime.AddSeconds(combatSecond) > DateTime.Now) return true;
            if (AttackManager.lastRocketAttack.AddSeconds(combatSecond) > DateTime.Now) return true;
            return false;
        }

        public void EnableAttack(string itemId)
        {
            AttackManager.Attacking = true;
            SendCommand(AddMenuItemHighlightCommand.write(new class_h2P(class_h2P.ITEMS_CONTROL), itemId, new class_K18(class_K18.ACTIVE), new class_I1W(false, 0)));
            Group?.UpdatePlayer(this, new List<command_i3O> { new GroupPlayerAttackingModule(true) });
        }

        public void DisableAttack(string itemId)
        {
            AttackManager.Attacking = false;
            SendCommand(RemoveMenuItemHighlightCommand.write(new class_h2P(class_h2P.ITEMS_CONTROL), itemId, new class_K18(class_K18.ACTIVE)));
            Group?.UpdatePlayer(this, new List<command_i3O> { new GroupPlayerAttackingModule(false) });
        }

        public Position GetNearestPortalPosition()
        {
            var activatablesOrdered = Spacemap.Activatables.Values.OrderBy(x => x.Position.DistanceTo(Position));
            var nearestPortal = activatablesOrdered.FirstOrDefault(x => x is Portal);

            return nearestPortal.Position;
        }

        public void SaveSettings()
        {
            QueryManager.SavePlayer.Settings(this, "audio", Settings.Audio);
            QueryManager.SavePlayer.Settings(this, "quality", Settings.Quality);
            QueryManager.SavePlayer.Settings(this, "classY2T", Settings.ClassY2T);
            QueryManager.SavePlayer.Settings(this, "display", Settings.Display);
            QueryManager.SavePlayer.Settings(this, "gameplay", Settings.Gameplay);
            QueryManager.SavePlayer.Settings(this, "window", Settings.Window);
            QueryManager.SavePlayer.Settings(this, "boundKeys", Settings.BoundKeys);
            QueryManager.SavePlayer.Settings(this, "inGameSettings", Settings.InGameSettings);
            QueryManager.SavePlayer.Settings(this, "cooldowns", Settings.Cooldowns);
            QueryManager.SavePlayer.Settings(this, "slotbarItems", Settings.SlotBarItems);
            QueryManager.SavePlayer.Settings(this, "premiumSlotbarItems", Settings.PremiumSlotBarItems);
            QueryManager.SavePlayer.Settings(this, "proActionBarItems", Settings.ProActionBarItems);
        }

        public void SendPacket(string packet)
        {
            try
            {
                var gameSession = GameManager.GetGameSession(Id);
                if (gameSession == null) return;
                if (!Spacemap.Characters.ContainsKey(Id)) return;
                if (!Program.TickManager.Exists(this)) return;
                if (gameSession.Client.Socket == null) return;
                if (!gameSession.Client.Socket.IsBound) return;
                if (!gameSession.Client.Socket.Connected) return;

                //Console.WriteLine("PACKET SENDED => " + Name);
                gameSession.Client.Send(LegacyModule.write(packet));
            }
            catch (Exception e)
            {
                Out.WriteLine("SendPacket Problem: " + e);
            }
        }

        public void SendCommand(byte[] command)
        {
            try
            {
                var gameSession = GameManager.GetGameSession(Id);
                if (gameSession == null) return;
                if (!Spacemap.Characters.ContainsKey(Id)) return;
                if (!Program.TickManager.Exists(this)) return;
                if (gameSession.Client.Socket == null) return;
                if (!gameSession.Client.Socket.IsBound) return;
                if (!gameSession.Client.Socket.Connected) return;

                //Console.WriteLine("COMMAND SENDED => " + Name);
                gameSession.Client.Send(command);
            }
            catch (Exception e)
            {
                Out.WriteLine("SendCommand Problem: " + e);
            }
        }

        public bool LoggingOut = false;
        private DateTime LogoutStartTime = new DateTime();

        public void Logout(bool start = false)
        {
            if (start)
            {
                LoggingOut = true;
                LogoutStartTime = DateTime.Now;
                return;
            }

            if (!LoggingOut) return;

            if (AttackingOrUnderAttack() || Moving || Spacemap.Options.LogoutBlocked)
            {
                AbortLogout();
                return;
            }

            if (LogoutStartTime.AddSeconds((Premium || RankId == 21) ? 5 : 10) < DateTime.Now)
            {
                SendPacket("0|l|" + Id);
                GameSession.Disconnect(GameSession.DisconnectionType.NORMAL);
                LoggingOut = false;
            }

        }

        public void AbortLogout()
        {
            LoggingOut = false;
            SendPacket("0|t");
        }

        public GameSession GameSession
        {
            get
            {
                return GameManager.GetGameSession(Id);
            }
        }

        public override byte[] GetShipCreateCommand() { return null; }
    }
}