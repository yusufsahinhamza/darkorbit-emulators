using Ow.Game;
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
using System.Threading.Tasks;
using Ow.Managers.MySQLManager;
using Newtonsoft.Json;

namespace Ow.Game.Objects
{
    class Player : Character
    {
        public string PetName { get; set; }
        public int RankId { get; set; }
        public int WarRank { get; set; }
        public bool Premium { get; set; }
        public string Title { get; set; }

        public int Level
        {
            get
            {
                short lvl = 1;
                long expNext = 10000;

                while (Data.experience >= expNext)
                {
                    expNext *= 2;
                    lvl++;
                }
                
                return lvl;
            }
            set
            {
                Level = value;
            }
        }

        public int CurrentInRangePortalId = -1;
        public int CurrentShieldConfig1 { get; set; }
        public int CurrentShieldConfig2 { get; set; }
        public int CurrentConfig { get; set; }

        public SettingsBase Settings = new SettingsBase();
        public DestructionsBase Destructions { get; set; }
        public EquipmentBase Equipment { get; set; }
        public DataBase Data { get; set; }
        public SkillTreeBase SkillTree = new SkillTreeBase();
        public Group Group { get; set; }
        public Pet Pet { get; set; }
        public AttackManager AttackManager { get; set; }
        public SettingsManager SettingsManager { get; set; }
        public DroneManager DroneManager { get; set; }
        public CpuManager CpuManager { get; set; }
        public TechManager TechManager { get; set; }
        public SkillManager SkillManager { get; set; }
        public BoosterManager BoosterManager { get; set; }

        public Player(int id, string name, Clan clan, int factionId, int rankId, int warRank, Ship ship)
                     : base(id, name, factionId, ship, new Position(0, 0), null, clan)
        {
            Name = name;
            Clan = clan;
            FactionId = factionId;
            RankId = rankId;
            WarRank = warRank;
            InitiateManagers();

            MaxNanoHull = ship.BaseHitpoints;
        }

        public void InitiateManagers()
        {
            DroneManager = new DroneManager(this);
            AttackManager = new AttackManager(this);
            TechManager = new TechManager(this);
            SkillManager = new SkillManager(this);
            SettingsManager = new SettingsManager(this);
            CpuManager = new CpuManager(this);
            BoosterManager = new BoosterManager(this);
        }

        public override void Tick()
        {
            Movement.ActualPosition(this);
            CheckHitpointsRepair();
            CheckShieldPointsRepair();
            CheckRadiation();
            AttackManager.LaserAttack();
            AttackManager.RocketLauncher.Tick();
            RefreshAttackers();
            Logout();

            Storage.Tick();
            DroneManager.Tick();
            TechManager.Tick();
            SkillManager.Tick();
            BoosterManager.Tick();
        }

        public DateTime lastHpRepairTime = new DateTime();
        private void CheckHitpointsRepair()
        {
            if (CurrentHitPoints >= MaxHitPoints || AttackingOrUnderAttack())
            {
                if (Storage.RepairBotActivated)
                    RepairBot(false);
                return;
            }

            if (lastHpRepairTime.AddSeconds(1) >= DateTime.Now) return;

            if (!Storage.RepairBotActivated)
                RepairBot(true);

            int repairHitpoints = MaxHitPoints / 40;
            repairHitpoints += Maths.GetPercentage(repairHitpoints, BoosterManager.GetPercentage(BoostedAttributeType.REPAIR));
            repairHitpoints += Maths.GetPercentage(repairHitpoints, GetSkillPercentage("Engineering"));

            Heal(repairHitpoints);

            lastHpRepairTime = DateTime.Now;
        }

        public DateTime lastShieldRepairTime = new DateTime();
        private void CheckShieldPointsRepair()
        {
            if (LastCombatTime.AddSeconds(10) >= DateTime.Now || lastShieldRepairTime.AddSeconds(1) >= DateTime.Now ||
                CurrentShieldPoints >= MaxShieldPoints || Settings.InGameSettings.selectedFormation == DroneManager.MOTH_FORMATION
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
                var value = CurrentConfig == 1 ? Equipment.Configs.Config1Speed : Equipment.Configs.Config2Speed;

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.DOME_FORMATION:
                        value -= Maths.GetPercentage(value, 50);
                        break;
                    case DroneManager.CRAB_FORMATION:
                        value -= Maths.GetPercentage(value, 15);
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

                if (Storage.Lightning)
                    value += Maths.GetPercentage(value, 30);

                value += Storage.SpeedBoost;

                return value;
            }
        }

        public override int MaxHitPoints
        {
            get
            {
                var value = CurrentConfig == 1 ? Equipment.Configs.Config1Hitpoints : Equipment.Configs.Config2Hitpoints;
                value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.MAXHP));

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

        public double RocketSpeed
        {
            get
            {
                var value = Premium ? 1.0 : 3.0;

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.DOME_FORMATION:
                        value -= 0.25;
                        break;
                    case DroneManager.RING_FORMATION:
                        value += 0.25;
                        break;
                }

                return value;
            }
        }

        public double RocketLauncherSpeed
        {
            get
            {
                var value = 1.0;

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.DOME_FORMATION:
                        value -= 0.25;
                        break;
                    case DroneManager.STAR_FORMATION:
                        value += 0.33;
                        break;
                    case DroneManager.RING_FORMATION:
                        value += 0.25;
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
                var value = CurrentConfig == 1 ? Equipment.Configs.Config1Shield : Equipment.Configs.Config2Shield;
                value += Maths.GetPercentage(value, 40);
                value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.SHIELD));
                value += Maths.GetPercentage(value, GetSkillPercentage("Shield Engineering"));

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.TURTLE_FORMATION:
                        value += Maths.GetPercentage(value, 10);
                        break;
                    case DroneManager.RING_FORMATION:
                        value += Maths.GetPercentage(value, 85);
                        break;
                    case DroneManager.DRILL_FORMATION:
                        value -= Maths.GetPercentage(value, 25);
                        break;
                    case DroneManager.DOME_FORMATION:
                        value += Maths.GetPercentage(value, 30);
                        break;
                    case DroneManager.HEART_FORMATION:
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
                        return 0.13; // 0.2
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
                var value = CurrentConfig == 1 ? Equipment.Configs.Config1Damage : Equipment.Configs.Config2Damage;
                value += Maths.GetPercentage(value, 60); //seprom
                value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.DAMAGE));

                switch (SettingsManager.Player.Settings.InGameSettings.selectedFormation)
                {
                    case DroneManager.DOME_FORMATION:
                        value -= Maths.GetPercentage(value, 50);
                        break;
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
                        value += Maths.GetPercentage(value, 20);
                        break;
                    case DroneManager.WHEEL_FORMATION:
                        value -= Maths.GetPercentage(value, 20);
                        break;
                }

                value = Ship.GetLaserDamageBoost(value, FactionId, (Selected != null ? Selected.FactionId : 0));

                value += Storage.DamageBoost;

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
                value += Maths.GetPercentage(value, GetSkillPercentage("Rocket Fusion"));

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
                    case DroneManager.DOUBLE_ARROW_FORMATION:
                        value += Maths.GetPercentage(value, 30);
                        break;
                    case DroneManager.CHEVRON_FORMATION:
                        value += Maths.GetPercentage(value, 65);
                        break;
                }
                return value;
            }
        }

        public double RocketMissProbability
        {
            get
            {
                var value = 0.1;
                value -= Maths.GetDoublePercentage(value, GetSkillPercentage("Heat-seeking Missiles"));

                if (Storage.PrecisionTargeter)
                    value = 0;

                return value;
            }
        }

        public bool UpdateActivatable(Activatable pEntity, bool pInRange)
        {
            if (Storage.InRangeAssets.ContainsKey(pEntity.Id))
            {
                if (!pInRange)
                {
                    if (pEntity is Portal portal && portal.Working)
                        CurrentInRangePortalId = -1;
                    Storage.InRangeAssets.TryRemove(pEntity.Id, out pEntity);
                    return true;
                }
            }
            else
            {
                if (pInRange)
                {
                    if (pEntity is Portal portal && portal.Working)
                        CurrentInRangePortalId = pEntity.Id;
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

        public void SetTitle(string title, bool permanent = false)
        {
            Title = title;
            var packet = Title != "" ? $"0|n|t|{Id}|1|{Title}" : $"0|n|trm|{Id}";
            SendPacket(packet);
            SendPacketToInRangePlayers(packet);

            if (permanent)
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET title = '{Title}' WHERE userId = {Id}");
        }

        public byte[] GetBeaconCommand()
        {
            return BeaconCommand.write(1, 1, 1, 1, Storage.IsInDemilitarizedZone, Storage.RepairBotActivated, (SkillTree.engineering == 5),
                         "equipment_extra_repbot_rep-4", Storage.IsInRadiationZone);
        }

        public byte[] GetShipCreateCommand(Player otherPlayer, short relationType)
        {
            return ShipCreateCommand.write(
                Id,
                Ship.LootId,
                3,
                !EventManager.JackpotBattle.InEvent(this) ? Clan.Tag : "",
                !EventManager.JackpotBattle.InEvent(this) ? (otherPlayer.RankId == 21 ? $"{Name} - {Id}" : Name) : EventManager.JackpotBattle.Name,
                Position.X,
                Position.Y,
                FactionId,
                !EventManager.JackpotBattle.InEvent(this) ? Clan.Id : 0,
                RankId,
                false,
                new ClanRelationModule(!EventManager.JackpotBattle.InEvent(this) ? relationType : ClanRelationModule.NONE),
                GetRingsCount(),
                false,
                false,
                Invisible,
                !EventManager.JackpotBattle.InEvent(this) ? relationType : ClanRelationModule.NONE,
                !EventManager.JackpotBattle.InEvent(this) ? relationType : ClanRelationModule.NONE,
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
                CurrentNanoHull,
                MaxNanoHull,
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
                GetRingsCount(),
                true,
                Invisible,
                true,
                VisualModifiers.Values.ToList());
        }

        public int GetRingsCount()
        {
            return WarRank == 1 ? 100 : WarRank == 2 ? 63 : WarRank == 3 ? 31 : WarRank == 4 ? 15 : WarRank == 5 ? 7 : WarRank == 6 ? 3 : WarRank == 7 ? 1 : 0;
        }

        public bool Attackable()
        {
            return (AttackManager.IshCooldown.AddMilliseconds(TimeManager.ISH_DURATION) > DateTime.Now || Invincible || Storage.GodMode) ? false : true;
        }

        public void SendCooldown(string itemId, int time, bool countdown = false)
        {
            SendCommand(UpdateMenuItemCooldownGroupTimerCommand.write(
            SettingsManager.GetCooldownType(itemId),
            new ClientUISlotBarCategoryItemTimerStateModule( countdown ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168), time, time));
        }

        public void UpdateCurrentCooldowns()
        {
            Settings.Cooldowns[AmmunitionManager.SMB_01] = AttackManager.SmbCooldown.ToString("yyyy-MM-dd HH:mm:ss");
            Settings.Cooldowns[AmmunitionManager.ISH_01] = AttackManager.IshCooldown.ToString("yyyy-MM-dd HH:mm:ss");
            Settings.Cooldowns[AmmunitionManager.EMP_01] = AttackManager.EmpCooldown.ToString("yyyy-MM-dd HH:mm:ss");
            Settings.Cooldowns["ammunition_mine"] = AttackManager.mineCooldown.ToString("yyyy-MM-dd HH:mm:ss");
            Settings.Cooldowns[AmmunitionManager.DCR_250] = AttackManager.dcr_250Cooldown.ToString("yyyy-MM-dd HH:mm:ss");
            Settings.Cooldowns[AmmunitionManager.PLD_8] = AttackManager.pld8Cooldown.ToString("yyyy-MM-dd HH:mm:ss");
            Settings.Cooldowns[AmmunitionManager.R_IC3] = AttackManager.r_ic3Cooldown.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var skill in Storage.Skills.Values)
                Settings.Cooldowns[skill.LootId] = Storage.Skills[skill.LootId].cooldown.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void SetCurrentCooldowns()
        {
            if (Settings.Cooldowns[AmmunitionManager.SMB_01] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[AmmunitionManager.SMB_01]))).TotalSeconds;
                AttackManager.SmbCooldown = DateTime.Now.AddSeconds(-seconds);
            }

            if (Settings.Cooldowns[AmmunitionManager.ISH_01] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[AmmunitionManager.ISH_01]))).TotalSeconds;
                AttackManager.IshCooldown = DateTime.Now.AddSeconds(-seconds);
            }

            if (Settings.Cooldowns[AmmunitionManager.EMP_01] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[AmmunitionManager.EMP_01]))).TotalSeconds;
                AttackManager.EmpCooldown = DateTime.Now.AddSeconds(-seconds);
            }

            if (Settings.Cooldowns["ammunition_mine"] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns["ammunition_mine"]))).TotalSeconds;
                AttackManager.mineCooldown = DateTime.Now.AddSeconds(-seconds);
            }

            if (Settings.Cooldowns[AmmunitionManager.DCR_250] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[AmmunitionManager.DCR_250]))).TotalSeconds;
                AttackManager.dcr_250Cooldown = DateTime.Now.AddSeconds(-seconds);
            }

            if (Settings.Cooldowns[AmmunitionManager.PLD_8] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[AmmunitionManager.PLD_8]))).TotalSeconds;
                AttackManager.pld8Cooldown = DateTime.Now.AddSeconds(-seconds);
            }

            if (Settings.Cooldowns[AmmunitionManager.R_IC3] != "")
            {
                var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[AmmunitionManager.R_IC3]))).TotalSeconds;
                AttackManager.r_ic3Cooldown = DateTime.Now.AddSeconds(-seconds);
            }

            foreach (var skill in Storage.Skills.Values)
            {
                if (Settings.Cooldowns.ContainsKey(skill.LootId))
                {
                    var seconds = (int)(DateTime.Now.Subtract(DateTime.Parse(Settings.Cooldowns[skill.LootId]))).TotalSeconds;
                    skill.cooldown = DateTime.Now.AddSeconds(-seconds);
                }
            }
        }

        public void SelectEntity(int entityId)
        {
            if (AttackManager.Attacking)
                DisableAttack(SettingsManager.Player.Settings.InGameSettings.selectedLaser);

            try
            {
                if (InRangeCharacters.ContainsKey(entityId))
                {
                    var character = InRangeCharacters.Values.Where(x => x.Id == entityId).FirstOrDefault();

                    if (character != null && !character.Destroyed)
                    {
                        if (character is Player player && (player.AttackManager.EmpCooldown.AddMilliseconds(TimeManager.EMP_DURATION) > DateTime.Now)) return;
                        Selected = character;

                        SendCommand(ShipSelectionCommand.write(
                            character.Id,
                            character.Ship.Id,
                            character.CurrentShieldPoints,
                            character.MaxShieldPoints,
                            character.CurrentHitPoints,
                            character.MaxHitPoints,
                            character.CurrentNanoHull,
                            character.MaxNanoHull,
                            character is Player ? true : false));
                    }
                }
                else if (Storage.InRangeAssets.ContainsKey(entityId))
                {
                    var asset = Storage.InRangeAssets.Values.Where(x => x.Id == entityId).FirstOrDefault();

                    if (asset != null && (asset is BattleStation || asset is Satellite) && !asset.Destroyed)
                    {
                        Selected = asset;

                        SendCommand(AssetInfoCommand.write(
                            asset.Id,
                            asset.GetAssetType(),
                            asset is Satellite satellite ? satellite.DesignId : 0,
                            3,
                            asset.CurrentHitPoints,
                            asset.MaxHitPoints,
                            asset.MaxShieldPoints > 0 ? true : false,
                            asset.CurrentShieldPoints,
                            asset.MaxShieldPoints
                            ));
                    }
                }

                if (Selected != null)
                {
                    Group?.UpdateTarget(this, new List<command_i3O> { new GroupPlayerTargetModule(new GroupPlayerShipModule(Selected is Player player ? player.Ship.GroupShipId : GroupPlayerShipModule.WRECK), Selected.Name, new GroupPlayerInformationsModule(Selected.CurrentHitPoints, Selected.MaxHitPoints, Selected.CurrentShieldPoints, Selected.MaxShieldPoints, Selected.CurrentNanoHull, Selected.MaxNanoHull)) });
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("SelectEntity void exception " + e, "Player.cs");
                Logger.Log("error_log", $"- [Player.cs] SelectEntity void exception: {e}");
            }
        }

        public void ChangeShip(int shipId)
        {
            SkillManager.DisableAllSkills();
            Ship = GameManager.GetShip(shipId);
            QueryManager.SetEquipment(this);
            SkillManager.InitiateSkills(true);

            LastCombatTime = DateTime.Now.AddSeconds(-999);
            Spacemap.RemoveCharacter(this);
            CurrentInRangePortalId = -1;
            Deselection();
            Storage.InRangeAssets.Clear();
            InRangeCharacters.Clear();

            Spacemap.AddAndInitPlayer(this);
            UpdateStatus();
        }

        public async void Jump(int mapId, Position targetPosition)
        {
            Storage.Jumping = true;
            await Task.Delay(Portal.JUMP_DELAY);

            LastCombatTime = DateTime.Now.AddSeconds(-999);
            Spacemap.RemoveCharacter(this);
            CurrentInRangePortalId = -1;
            Deselection();
            Storage.InRangeAssets.Clear();
            InRangeCharacters.Clear();
            SetPosition(targetPosition);

            Spacemap = GameManager.GetSpacemap(mapId);

            Spacemap.AddAndInitPlayer(this);
            Storage.Jumping = false;
        }

        public void KillScreen(Attackable killerEntity, DestructionType destructionType, bool killedLogin = false)
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
                    KillScreenPostCommand.write(killerEntity != null ? killerEntity.Name : "", "http://localhost/indexInternal.es?action=internalDock",
                                              "MISC", new DestructionTypeModule((short)destructionType),
                                              killScreenOptionModules);

            SendCommand(killScreenPostCommand);
        }

        public void Respawn(bool basicRepair = false, bool deathLocation = false, bool atNearestPortal = false, bool fullRepair = false)
        {
            LastCombatTime = DateTime.Now.AddSeconds(-999);

            AddVisualModifier(VisualModifierCommand.INVINCIBILITY, 0, "", 0, true);

            Storage.IsInDemilitarizedZone = basicRepair || fullRepair ? true : false;
            Storage.IsInEquipZone = basicRepair || fullRepair ? true : false;
            Storage.IsInRadiationZone = false;

            if (atNearestPortal)
                SetPosition(GetNearestPortalPosition());
            else if (deathLocation)
                CurrentHitPoints = Maths.GetPercentage(MaxHitPoints, 10);
            else
            {
                CurrentHitPoints = Maths.GetPercentage(MaxHitPoints, 1);
                SetPosition(GetBasePosition());
            }

            if (basicRepair || fullRepair)
                Spacemap = GameManager.GetSpacemap(GetBaseMapId());

            if (fullRepair)
            {
                CurrentHitPoints = MaxHitPoints;
                CurrentShieldConfig1 = MaxShieldPoints;
                CurrentShieldConfig2 = MaxShieldPoints;
            }

            Spacemap.AddAndInitPlayer(this, Destroyed);

            Group?.UpdateTarget(this, new List<command_i3O> { new GroupPlayerDisconnectedModule(false) });

            Destroyed = false;
        }

        public int GetBaseMapId()
        {
            return FactionId == 1 ? 13 : FactionId == 2 ? 14 : 15;
        }

        public Position GetBasePosition()
        {
            return FactionId == 1 ? Position.MMOPosition : FactionId == 2 ? Position.EICPosition : Position.VRUPosition;
        }

        public void ChangeData(DataType dataType, int amount, ChangeType changeType = ChangeType.INCREASE)
        {
            if (amount == 0) return;
            amount = Convert.ToInt32(amount);

            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = mySqlClient.ExecuteQueryRow($"SELECT data FROM player_accounts WHERE userId = {Id}");
                Data = JsonConvert.DeserializeObject<DataBase>(result["data"].ToString());
            }

            switch (dataType)
            {
                case DataType.URIDIUM:
                    Data.uridium = (changeType == ChangeType.INCREASE ? (Data.uridium + amount) : (Data.uridium - amount));
                    if (Data.uridium < 0) Data.uridium = 0;
                    SendPacket($"0|LM|ST|URI|{(changeType == ChangeType.DECREASE ? "-" : "")}{amount}|{Data.uridium}");
                    break;
                case DataType.CREDITS:
                    Data.credits = (changeType == ChangeType.INCREASE ? (Data.credits + amount) : (Data.credits - amount));
                    if (Data.credits < 0) Data.credits = 0;
                    SendPacket($"0|LM|ST|CRE|{(changeType == ChangeType.DECREASE ? "-" : "")}{amount}|{Data.credits}");
                    break;
                case DataType.HONOR:
                    Data.honor = (changeType == ChangeType.INCREASE ? (Data.honor + amount) : (Data.honor - amount));
                    if (Data.honor < 0) Data.honor = 0;
                    SendPacket($"0|LM|ST|HON|{(changeType == ChangeType.DECREASE ? "-" : "")}{amount}|{Data.honor}");
                    break;
                case DataType.EXPERIENCE:
                    Data.experience = (changeType == ChangeType.INCREASE ? (Data.experience + amount) : (Data.experience - amount));
                    if (Data.experience < 0) Data.experience = 0;
                    SendPacket($"0|LM|ST|EP|{(changeType == ChangeType.DECREASE ? "-" : "")}{amount}|{Data.experience}|{Level}");
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
            if (LastAttackTime(combatSecond)) return true;
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
        }

        public void DisableAttack(string itemId)
        {
            AttackManager.Attacking = false;
            SendCommand(RemoveMenuItemHighlightCommand.write(new class_h2P(class_h2P.ITEMS_CONTROL), itemId, new class_K18(class_K18.ACTIVE)));
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
                if (!Program.TickManager.Exists(this)) return;
                if (gameSession.Client.Socket == null || !gameSession.Client.Socket.IsBound || !gameSession.Client.Socket.Connected) return;

                gameSession.Client.Send(LegacyModule.write(packet));
            }
            catch (Exception e)
            {
                Out.WriteLine("SendPacket void exception " + e, "Player.cs");
                Logger.Log("error_log", $"- [Player.cs] SendPacket void exception: {e}");
            }
        }

        public void SendCommand(byte[] command)
        {
            try
            {
                var gameSession = GameManager.GetGameSession(Id);

                if (gameSession == null) return;
                if (!Program.TickManager.Exists(this)) return;
                if (gameSession.Client.Socket == null || !gameSession.Client.Socket.IsBound || !gameSession.Client.Socket.Connected) return;

                gameSession.Client.Send(command);
            }
            catch (Exception e)
            {
                Out.WriteLine("SendCommand void exception " + e, "Player.cs");
                Logger.Log("error_log", $"- [Player.cs] SendCommand void exception: {e}");
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

            if (!Storage.IsInDemilitarizedZone && (AttackingOrUnderAttack() || Moving || Spacemap.Options.LogoutBlocked))
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

        public int GetSkillPercentage(string skillName)
        {
            int value = 0;

            var detonation1 = SkillTree.detonation1;
            var detonation2 = SkillTree.detonation2;
            var shieldEngineering = SkillTree.shieldEngineering;
            var engineering = SkillTree.engineering;
            var heatseekingMissiles = SkillTree.heatseekingMissiles;
            var rocketFusion = SkillTree.rocketFusion;
            var cruelty1 = SkillTree.cruelty1;
            var cruelty2 = SkillTree.cruelty2;
            var explosives = SkillTree.explosives;
            var luck1 = SkillTree.luck1;
            var luck2 = SkillTree.luck2;

            if (skillName == "Shield Engineering")
            {
                value += shieldEngineering == 1 ? 4 : shieldEngineering == 2 ? 8 : shieldEngineering == 3 ? 12 : shieldEngineering == 4 ? 18 : shieldEngineering == 5 ? 25 : 0;
            }
            else if (skillName == "Engineering")
            {
                value += engineering == 1 ? 5 : engineering == 2 ? 10 : engineering == 3 ? 15 : engineering == 4 ? 20 : engineering == 5 ? 30 : 0;
            }
            else if (skillName == "Detonation")
            {
                value += detonation2 >= 1 ? (detonation2 == 1 ? 21 : detonation2 == 2 ? 28 : detonation2 == 3 ? 50 : 0) : (detonation1 == 1 ? 7 : detonation1 == 2 ? 14 : 0);
            }
            else if (skillName == "Heat-seeking Missiles")
            {
                value += heatseekingMissiles == 1 ? 1 : heatseekingMissiles == 2 ? 2 : heatseekingMissiles == 3 ? 4 : heatseekingMissiles == 4 ? 6 : heatseekingMissiles == 5 ? 10 : 0;
            }
            else if (skillName == "Rocket Fusion")
            {
                value += rocketFusion == 1 ? 2 : rocketFusion == 2 ? 4 : rocketFusion == 3 ? 6 : rocketFusion == 4 ? 8 : rocketFusion == 5 ? 15 : 0;
            }
            else if (skillName == "Cruelty")
            {
                value += cruelty2 >= 1 ? (cruelty2 == 1 ? 12 : cruelty2 == 2 ? 18 : cruelty2 == 3 ? 25 : 0) : (cruelty1 == 1 ? 4 : cruelty1 == 2 ? 8 : 0);
            }
            else if (skillName == "Explosives")
            {
                value += explosives == 1 ? 4 : explosives == 2 ? 8 : explosives == 3 ? 12 : explosives == 4 ? 18 : explosives == 5 ? 25 : 0;
            }
            else if (skillName == "Luck")
            {
                value += luck2 >= 1 ? (luck2 == 1 ? 6 : luck2 == 2 ? 8 : luck2 == 3 ? 12 : 0) : (luck1 == 1 ? 2 : luck1 == 2 ? 4 : 0);
            }

            return value;
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