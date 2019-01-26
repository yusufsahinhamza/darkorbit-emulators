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

namespace Ow.Game.Objects
{
    class Player : Character
    {
        public int Uridium { get; set; }
        public int Credits { get; set; }
        public int Experience { get; set; }
        public int Honor { get; set; }
        public int RankId { get; set; }
        public bool Premium { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public PlayerSettings Settings { get; set; }

        public bool UbaMatchmakingAccepted = false;
        public Player UbaOpponent = null;

        public Group Group { get; set; }
        public Dictionary<int, Group> GroupInvites = new Dictionary<int, Group>();

        public ConcurrentDictionary<int, Activatable> InRangeAssets = new ConcurrentDictionary<int, Activatable>();
        public Dictionary<int, Player> DuelInvites = new Dictionary<int, Player>();
        public Duel Duel { get; set; }
        public int CurrentInRangePortalId = -1;
        public int CurrentShieldConfig1 { get; set; }
        public int CurrentShieldConfig2 { get; set; }
        public int CurrentConfig { get; set; }
        public int SpeedBoost = 0;
        public bool IsInDemilitarizedZone = false;
        public bool IsInRadiationZone = false;
        public bool IsInEquipZone = false;
        public bool AutoRocket = false;
        public bool AutoRocketLauncher = false;
        public bool RepairBotActivated = false;
        public bool ShieldSkillActivated = false;
        public bool PrecisionTargeter = false;
        public bool EnergyLeech = false;
        public bool Jumping = false;
        public bool GodMode = false;
        public bool GroupInitialized { get; set; }

        public bool Sentinel = false;
        public bool Spectrum = false;

        public bool Diminisher = false;
        public Player UnderDiminisherPlayer { get; set; }
        public bool Venom = false;
        public Player UnderVenomPlayer { get; set; }

        public bool underR_IC3 = false;
        public DateTime underR_IC3Time = new DateTime();

        public bool underDCR_250 = false;
        public DateTime underDCR_250Time = new DateTime();

        public bool underSLM_01 = false;
        public DateTime underSLM_01Time = new DateTime();

        public bool invincibilityEffect = false;
        public DateTime invincibilityEffectTime = new DateTime();

        public bool mirroredControlEffect = false;
        public DateTime mirroredControlEffectTime = new DateTime();

        public bool wizardEffect = false;
        public DateTime wizardEffectTime = new DateTime();

        public Pet Pet { get; set; }
        public AttackManager AttackManager { get; set; }
        public SettingsManager SettingsManager { get; set; }
        public DroneManager DroneManager { get; set; }
        public CpuManager CpuManager { get; set; }
        public EquipmentManager EquipmentManager { get; set; }
        public TechManager TechManager { get; set; }
        public SkillManager SkillManager { get; set; }
        public MoveManager MoveManager { get; set; }
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
            AttackManager = new AttackManager(this);
            TechManager = new TechManager(this);
            SkillManager = new SkillManager(this);
            MoveManager = new MoveManager(this);
            SettingsManager = new SettingsManager(this);
            DroneManager = new DroneManager(this);
            CpuManager = new CpuManager(this);
            EquipmentManager = new EquipmentManager(this);
            BoosterManager = new BoosterManager(this);
            Pet = new Pet(this);
        }

        public new void Tick()
        {
            CheckHitpointsRepair();
            CheckShieldPointsRepair();
            CheckRadiation();
            CheckUnderEffects();

            AttackManager.LaserAttack();
            AttackManager.RocketLauncher.Tick();
            DroneManager.Tick();
            MoveManager.Tick();
            TechManager.Tick();
            SkillManager.Tick();

            Logout();
            // UpdateCurrentCooldowns();
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
            if (AttackingOrUnderAttack() || lastHpRepairTime.AddSeconds(1) >= DateTime.Now || CurrentHitPoints == MaxHitPoints) return;

            int repairHitpoints = MaxHitPoints / 35;
            Heal(repairHitpoints);

            lastHpRepairTime = DateTime.Now;
        }

        public DateTime lastShieldRepairTime = new DateTime();
        private void CheckShieldPointsRepair()
        {
            if (LastCombatTime.AddSeconds(10) >= DateTime.Now || lastShieldRepairTime.AddSeconds(1) >= DateTime.Now ||
                CurrentShieldPoints == MaxShieldPoints || SettingsManager.SelectedFormation == DroneManager.MOTH_FORMATION) return;

            int repairShield = MaxShieldPoints / 25;
            CurrentShieldPoints += repairShield;
            UpdateStatus();

            lastShieldRepairTime = DateTime.Now;
        }

        private DateTime LastInviteClean = new DateTime();
        public void CleanInvites()
        {
            if (LastInviteClean.AddSeconds(30) < DateTime.Now)
            {
                foreach (var invite in GroupInvites.ToList())
                {
                    if (GameManager.GetGameSession(invite.Key) == null ||
                        invite.Value == null)
                    {
                        GroupInvites.Remove(invite.Key);
                        //var gHandler = new GroupSystemHandler();
                        //gHandler.Error(Player.GetGameSession(), ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NOT_AVAILABLE);
                        //gHandler.DeleteInvitation(World.StorageManager.GetGameSession(invite.Key)?.Player, Player);
                    }
                }
                LastInviteClean = DateTime.Now;
            }
        }

        public void CheckUnderEffects()
        {
            if (underR_IC3 && underR_IC3Time.AddMilliseconds(TimeManager.R_IC3_DURATION) < DateTime.Now)
                DeactiveR_RIC3();
            if (underDCR_250 && underDCR_250Time.AddMilliseconds(TimeManager.DCR_250_DURATION) < DateTime.Now)
                DeactiveDCR_250();
            if (underSLM_01 && underSLM_01Time.AddMilliseconds(TimeManager.SLM_01_DURATION) < DateTime.Now)
                DeactiveSLM_01();
            if (invincibilityEffect && invincibilityEffectTime.AddMilliseconds(TimeManager.INVINCIBILITY_DURATION) < DateTime.Now)
                DeactiveInvincibilityEffect();
            if (mirroredControlEffect && mirroredControlEffectTime.AddMilliseconds(TimeManager.MIRRORED_CONTROL_DURATION) < DateTime.Now)
                DeactiveMirroredControlEffect();
            if (wizardEffect && wizardEffectTime.AddMilliseconds(TimeManager.WIZARD_DURATION) < DateTime.Now)
                DeactiveWizardEffect();
        }

        public void DeactiveR_RIC3()
        {
            underR_IC3 = false;
            SendPacket("0|n|fx|end|ICY_CUBE|" + Id + "");
            SendPacketToInRangePlayers("0|n|fx|end|ICY_CUBE|" + Id + "");
            SendCommand(SetSpeedCommand.write(Speed, Speed));
        }

        public void DeactiveDCR_250()
        {
            underDCR_250 = false;
            SendPacket("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendPacketToInRangePlayers("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendCommand(SetSpeedCommand.write(Speed, Speed));
        }

        public void DeactiveSLM_01()
        {
            underSLM_01 = false;
            SendPacket("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendPacketToInRangePlayers("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendCommand(SetSpeedCommand.write(Speed, Speed));
        }

        public void DeactiveInvincibilityEffect()
        {
            invincibilityEffect = false;
            RemoveVisualModifier(VisualModifierCommand.INVINCIBILITY);
        }

        public void DeactiveMirroredControlEffect()
        {
            mirroredControlEffect = false;
            RemoveVisualModifier(VisualModifierCommand.MIRRORED_CONTROLS);
        }

        public void DeactiveWizardEffect()
        {
            wizardEffect = false;
            RemoveVisualModifier(VisualModifierCommand.WIZARD_ATTACK);
        }

        public DateTime lastRadiationDamageTime = new DateTime();
        public void CheckRadiation()
        {
            if (!Jumping && IsInRadiationZone && invincibilityEffectTime.AddSeconds(5) < DateTime.Now)
            {
                if (lastRadiationDamageTime.AddSeconds(1) < DateTime.Now)
                {
                    int damage = 20000;

                    AttackManager.Damage(this, this, DamageType.RADIATION, damage, true, true, false);

                    lastRadiationDamageTime = DateTime.Now;
                }
            }
        }

        public void SetSpeedBoost(int speed)
        {
            SpeedBoost = speed;
            SendCommand(SetSpeedCommand.write(Speed, Speed));
        }

        public void RepairBot(bool activated)
        {
            RepairBotActivated = activated;
            SendCommand(GetBeaconCommand());
        }

        public void SetShieldSkillActivated(bool pShieldSkillActivated)
        {
            ShieldSkillActivated = pShieldSkillActivated;

            if (pShieldSkillActivated)
                SendCommand(AttributeSkillShieldUpdateCommand.write(1, 1, 0));
            else
                SendCommand(AttributeSkillShieldUpdateCommand.write(0, 0, 0));
        }

        public override int Speed
        {
            get
            {
                var value = 540;

                switch (SettingsManager.SelectedFormation)
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

                if (underDCR_250)
                    value -= Maths.GetPercentage(value, 30);

                if (underSLM_01)
                    value -= Maths.GetPercentage(value, 50);

                if (underR_IC3)
                    value -= value;

                value += SpeedBoost;

                return value;
            }
        }

        public override int MaxHitPoints
        {
            get
            {
                var value = Ship.BaseHitpoints;
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.MAXHP));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.SelectedFormation)
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

                switch (SettingsManager.SelectedFormation)
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
                var value = CurrentConfig == 1 ? EquipmentManager.shieldCfg1 : EquipmentManager.shieldCfg2;
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.SHIELD));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.SelectedFormation)
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
                switch (SettingsManager.SelectedFormation)
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
                switch (SettingsManager.SelectedFormation)
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
                var value = CurrentConfig == 1 ? EquipmentManager.damageCfg1 : EquipmentManager.damageCfg2;
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.DAMAGE));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.SelectedFormation)
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
            switch (SettingsManager.SelectedFormation)
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
                switch (SettingsManager.SelectedFormation)
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
            if (InRangeAssets.ContainsKey(pEntity.Id))
            {
                if (pInRange)
                {
                    return false;
                }
                else
                {
                    if (pEntity is Portal)
                    {
                        CurrentInRangePortalId = -1;
                    }
                    InRangeAssets.TryRemove(pEntity.Id, out pEntity);
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
                    InRangeAssets.TryAdd(pEntity.Id, pEntity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public DateTime ConfigCooldown = new DateTime();
        public void ChangeConfiguration(string LootID)
        {
            if (ConfigCooldown.AddSeconds(5) < DateTime.Now || GodMode)
            {
                SendPacket("0|S|CFG|" + LootID);
                SetCurrentConfiguration(Convert.ToInt32(LootID));
                ConfigCooldown = DateTime.Now;
                QueryManager.SavePlayer.Settings(this);
            }
            else
            {
                SendPacket("0|A|STM|config_change_failed_time");
            }
        }

        public void SetCurrentConfiguration(int pCurrentConfiguration)
        {
            CurrentConfig = Convert.ToInt32(pCurrentConfiguration);
            Settings.InGameSettings.currentConfig = Convert.ToInt32(pCurrentConfiguration);

            String drones = GetDronesPacket();
            SendPacket(drones);
            SendPacketToInRangePlayers(drones);

            UpdateStatus();
        }

        public byte[] GetBeaconCommand()
        {
            return BeaconCommand.write(1, 1, 1, 1, IsInDemilitarizedZone, RepairBotActivated, false,
                         "equipment_extra_repbot_rep-4", IsInRadiationZone);
        }

        public string GetClanTag()
        {
            if (Clan != null)
                return Clan.Tag;
            else
                return "";
        }

        public int GetClanId()
        {
            if (Clan != null)
                return Clan.Id;
            else
                return 0;
        }

        public byte[] GetShipCreateCommand(bool fromAdmin, short relationType, bool sameClan, bool jackpotBattle = false)
        {
            return ShipCreateCommand.write(
                Id,
                Ship.LootId,
                3,
                jackpotBattle ? "" : GetClanTag(),
                jackpotBattle ? EventManager.JackpotBattle.Name : (Name + (fromAdmin ? " (" + Id + ")" : "")),
                Position.X,
                Position.Y,
                FactionId,
                GetClanId(),
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
                GetClanId(),
                3,
                Premium,
                Experience,
                Honor,
                (short)Level,
                Credits,
                Uridium,
                0,
                RankId,
                GetClanTag(),
                100,
                true,
                Invisible,
                true,
                VisualModifiers.Values.ToList());
        }

        public bool Attackable()
        {
            if (AttackManager.IshCooldown.AddMilliseconds(TimeManager.ISH_DURATION) > DateTime.Now || invincibilityEffect || GodMode)
                return false;
            else
                return true;
        }

        public void SendCooldown(string itemId, int time, bool countdown = false)
        {
            SendCommand(UpdateMenuItemCooldownGroupTimerCommand.write(
            SettingsManager.GetCooldownType(itemId),
            new ClientUISlotBarCategoryItemTimerStateModule(
                        countdown ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168),
            time,
            time));
        }

        public void UpdateCurrentCooldowns()
        {
            int empCooldown = (int)(TimeManager.EMP_COOLDOWN - (DateTime.Now - AttackManager.EmpCooldown).TotalMilliseconds);

            if (empCooldown >= 0)
            {
                Settings.CurrentCooldowns.empCooldown = empCooldown;
                QueryManager.SavePlayer.Settings(this);
            }
        }

        public void SetCurrentCooldowns()
        {
            AttackManager.EmpCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.EMP_COOLDOWN - Settings.CurrentCooldowns.empCooldown));
        }

        public void SendCurrentCooldowns()
        {
            var ishCooldown = (DateTime.Now - AttackManager.IshCooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.ISH_01, (int)(TimeManager.ISH_COOLDOWN - ishCooldown));

            var smbCooldown = (DateTime.Now - AttackManager.SmbCooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.SMB_01, (int)(TimeManager.SMB_COOLDOWN - smbCooldown));

            var empCooldown = (DateTime.Now - AttackManager.EmpCooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.EMP_01, (int)(TimeManager.EMP_COOLDOWN - empCooldown));

            var rsbCooldown = (DateTime.Now - AttackManager.lastRSBAttackTime).TotalMilliseconds;
            SendCooldown(AmmunitionManager.RSB_75, (int)(3000 - rsbCooldown));

            var formationCooldown = (DateTime.Now - DroneManager.formationCooldown).TotalMilliseconds;
            SendCooldown(DroneManager.DEFAULT_FORMATION, (int)(TimeManager.FORMATION_COOLDOWN - formationCooldown));

            var shieldTechCooldown = (DateTime.Now - TechManager.BackupShields.cooldown).TotalMilliseconds;
            SendCooldown(TechManager.TECH_BACKUP_SHIELDS, (int)(TimeManager.BACKUP_SHIELD_COOLDOWN - shieldTechCooldown));

            var repairTechCooldown = (DateTime.Now - TechManager.BattleRepairBot.cooldown).TotalMilliseconds;
            SendCooldown(TechManager.TECH_BATTLE_REPAIR_BOT, (int)((TechManager.BattleRepairBot.Active ? TimeManager.BATTLE_REPAIR_BOT_DURATION : TimeManager.BATTLE_REPAIR_BOT_COOLDOWN) - repairTechCooldown), TechManager.BattleRepairBot.Active ? true : false);

            var chainTechCooldown = (DateTime.Now - TechManager.ChainImpulse.cooldown).TotalMilliseconds;
            SendCooldown(TechManager.TECH_CHAIN_IMPULSE, (int)(TimeManager.CHAIN_IMPULSE_COOLDOWN - chainTechCooldown));

            var energyTechCooldown = (DateTime.Now - TechManager.EnergyLeech.cooldown).TotalMilliseconds;
            SendCooldown(TechManager.TECH_ENERGY_LEECH, (int)((TechManager.EnergyLeech.Active ? TimeManager.ENERGY_LEECH_DURATION : TimeManager.ENERGY_LEECH_COOLDOWN) - energyTechCooldown), TechManager.EnergyLeech.Active ? true : false);

            var precisionTechCooldown = (DateTime.Now - TechManager.PrecisionTargeter.cooldown).TotalMilliseconds;
            SendCooldown(TechManager.TECH_PRECISION_TARGETER, (int)((TechManager.PrecisionTargeter.Active ? TimeManager.PRECISION_TARGETER_DURATION : TimeManager.PRECISION_TARGETER_COOLDOWN) - precisionTechCooldown), TechManager.PrecisionTargeter.Active ? true : false);

            var sentinelSkillCooldown = (DateTime.Now - SkillManager.Sentinel.cooldown).TotalMilliseconds;
            SendCooldown(SkillManager.SENTINEL, (int)((SkillManager.Sentinel.Active ? TimeManager.SENTINEL_DURATION : TimeManager.SENTINEL_COOLDOWN) - sentinelSkillCooldown), SkillManager.Sentinel.Active ? true : false);

            var diminisherSkillCooldown = (DateTime.Now - SkillManager.Diminisher.cooldown).TotalMilliseconds;
            SendCooldown(SkillManager.DIMINISHER, (int)((SkillManager.Diminisher.Active ? TimeManager.DIMINISHER_DURATION : TimeManager.DIMINISHER_COOLDOWN) - diminisherSkillCooldown), SkillManager.Diminisher.Active ? true : false);

            var spectrumSkillCooldown = (DateTime.Now - SkillManager.Spectrum.cooldown).TotalMilliseconds;
            SendCooldown(SkillManager.SPECTRUM, (int)((SkillManager.Spectrum.Active ? TimeManager.SPECTRUM_DURATION : TimeManager.SPECTRUM_COOLDOWN) - spectrumSkillCooldown), SkillManager.Spectrum.Active ? true : false);

            var venomSkillCooldown = (DateTime.Now - SkillManager.Venom.cooldown).TotalMilliseconds;
            SendCooldown(SkillManager.VENOM, (int)((SkillManager.Venom.Active ? TimeManager.VENOM_DURATION : TimeManager.VENOM_COOLDOWN) - venomSkillCooldown), SkillManager.Venom.Active ? true : false);

            var solaceSkillCooldown = (DateTime.Now - SkillManager.Solace.cooldown).TotalMilliseconds;
            SendCooldown(SkillManager.SOLACE, (int)(TimeManager.SOLACE_COOLDOWN - solaceSkillCooldown));

            var r_ic3Cooldown = (DateTime.Now - AttackManager.r_ic3Cooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.R_IC3, (int)(TimeManager.R_IC3_COOLDOWN - r_ic3Cooldown));

            var dcr_250Cooldown = (DateTime.Now - AttackManager.dcr_250Cooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.DCR_250, (int)(TimeManager.DCR_250_COOLDOWN - dcr_250Cooldown));

            var wiz_xCooldown = (DateTime.Now - AttackManager.wiz_xCooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.WIZ_X, (int)(TimeManager.WIZARD_COOLDOWN - wiz_xCooldown));

            var mineCooldown = (DateTime.Now - SettingsManager.mineCooldown).TotalMilliseconds;
            SendCooldown(AmmunitionManager.SLM_01, (int)(TimeManager.MINE_COOLDOWN - mineCooldown));
        }

        public void SelectShip(int targetId)
        {
            if (AttackManager.Attacking)
                DisableAttack(SettingsManager.SelectedLaser);

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
                }
            }
            catch (Exception e)
            {
                Out.WriteLine(e.StackTrace);
            }
        }

        public void Jump(int mapId, Position targetPosition)
        {
            var player = this;

            player.Jumping = true;

            var pet = player.Pet.Activated;
            player.Pet.Deactivate(true);

            player.CurrentInRangePortalId = -1;
            player.Selected = null;
            player.DisableAttack(player.SettingsManager.SelectedLaser);
            player.Spacemap.RemoveCharacter(player);
            player.InRangeAssets.Clear();
            player.InRangeCharacters.Clear();
            player.SetPosition(targetPosition);

            var targetSpacemap = GameManager.GetSpacemap(mapId);
            player.Spacemap = targetSpacemap;

            player.Spacemap.AddAndInitPlayer(player);
            player.Jumping = false;

            if (pet)
                player.Pet.Activate();
        }

        public String GetDronesPacket()
        {
            var DronePacket = "3|6|0|4|6|0|2|6|0|2|6|0|2|6|0|2|6|0|2|6|0|2|6|0|2|6|0|2|6|0";
            var drones = "0|n|d|" + Id + "|" + DronePacket;
            return drones;
        }

        public void KillScreen(Character killerPlayer, DestructionType destructionType, bool killedLogin = false)
        {
            var killScreenOptionModules = new List<KillScreenOptionModule>();
            var basicRepair =
                   new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_REPAIR),
                                              new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()));
            var portalRepair =
                  new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.AT_JUMPGATE_REPAIR),
                                             new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()));
            var deathLocationRepair =
                  new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR),
                                             new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                 new ClientUITooltipTextFormatModule(
                                                                                         ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                 new List<MessageWildcardReplacementModule>()));
            var fullRepair =
                   new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_FULL_REPAIR),
                                              new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                  new ClientUITooltipTextFormatModule(
                                                                                          ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                  new List<MessageWildcardReplacementModule>()));
            killScreenOptionModules.Add(basicRepair);
            if (!killedLogin)
            {
                if (Spacemap.Activatables.FirstOrDefault(x => x.Value is Portal).Value is Portal)
                    killScreenOptionModules.Add(portalRepair);

                if (Spacemap.Id != EventManager.JackpotBattle.Spacemap.Id && Spacemap.Id != 121)
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
            IsInDemilitarizedZone = basicRepair || fullRepair ? true : false;
            IsInEquipZone = basicRepair || fullRepair ? true : false;
            IsInRadiationZone = false;

            if (atNearestPortal)
                SetPosition(GetNearestPortalPosition());
            else if (deathLocation)
                CurrentHitPoints = Maths.GetPercentage(MaxHitPoints, 1);
            else
                MoveManager.SetPosition();

            if (basicRepair || fullRepair)
            {
                var mapId = FactionId == 1 ? 13 : FactionId == 2 ? 14 : 15;
                Spacemap = GameManager.GetSpacemap(mapId);
            }

            if (fullRepair)
            {
                CurrentHitPoints = MaxHitPoints;
                CurrentShieldConfig1 = MaxShieldPoints;
                CurrentShieldConfig2 = MaxShieldPoints;
            }

            Spacemap.AddAndInitPlayer(this);

            AddVisualModifier(new VisualModifierCommand(Id, VisualModifierCommand.INVINCIBILITY, 0, "", 0, true));

            Destroyed = false;
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

        public void SendPacket(string packet)
        {
            try
            {
                var gameSession = GameManager.GetGameSession(Id);
                if (gameSession == null) return;
                if (!Spacemap.Characters.ContainsKey(Id)) return;
                //if (!Program.TickManager.Ticks.Contains(this) && !Program.TickManager.ToBeAdded.Contains(this)) return;
                if (!gameSession.Client.Socket.IsBound) return;
                if (!gameSession.Client.Socket.Connected) return;

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
                //if (!Program.TickManager.Ticks.Contains(this) && !Program.TickManager.ToBeAdded.Contains(this)) return;
                if (!gameSession.Client.Socket.IsBound) return;
                if (!gameSession.Client.Socket.Connected) return;

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

            if (AttackingOrUnderAttack() || Moving || Spacemap.Id == EventManager.JackpotBattle.Spacemap.Id)
            {
                AbortLogout();
                return;
            }

            if (LogoutStartTime.AddSeconds((Premium || RankId == 21) ? 5 : 10) < DateTime.Now)
            {
                SendPacket("0|l|" + Id);
                var gameSession = GetGameSession();
                gameSession.Disconnect(GameSession.DisconnectionType.NORMAL);
                LoggingOut = false;
            }

        }

        public void AbortLogout()
        {
            LoggingOut = false;
            SendPacket("0|t");
        }

        public short GetGroupShipId()
        {
            switch (Ship.Id)
            {
                case Ship.GOLIATH_SPECTRUM:
                    return GroupPlayerShipModule.SPECTRUM;
                case Ship.GOLIATH_SENTINEL:
                    return GroupPlayerShipModule.SENTINEL;
                case Ship.GOLIATH_DIMINISHER:
                    return GroupPlayerShipModule.DIMINISHER;
                case Ship.GOLIATH_SOLACE:
                    return GroupPlayerShipModule.SOLACE;
                case Ship.GOLIATH_VENOM:
                    return GroupPlayerShipModule.VENOM;
                default:
                    return GroupPlayerShipModule.NONE;
            }
        }

        public GameSession GetGameSession()
        {
            return GameManager.GetGameSession(Id);
        }
    }
}