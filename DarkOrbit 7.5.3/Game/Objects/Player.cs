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
using Ow.Game.Objects.Activatables;
using Ow.Game.Objects.Players;
using Ow.Net.netty;
using static Ow.Game.Objects.Players.Managers.PlayerSettings;

namespace Ow.Game.Objects
{
    class Player : Character
    {
        public int RankId { get; set; }
        public bool Premium { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public PlayerSettings Settings { get; set; }
        public EquipmentBase Equipment { get; set; }
        public DataBase Data { get; set; }

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

        public bool underPLD8 = false;
        public DateTime underPLD8Time = new DateTime();

        public Pet Pet { get; set; }
        public AttackManager AttackManager { get; set; }
        public SettingsManager SettingsManager { get; set; }
        public DroneManager DroneManager { get; set; }
        public CpuManager CpuManager { get; set; }
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
            DroneManager = new DroneManager(this);
            AttackManager = new AttackManager(this);
            TechManager = new TechManager(this);
            SkillManager = new SkillManager(this);
            MoveManager = new MoveManager(this);
            SettingsManager = new SettingsManager(this);
            CpuManager = new CpuManager(this);
            BoosterManager = new BoosterManager(this);
            Pet = new Pet(this);
        }

        public new void Tick()
        {
            CheckHitpointsRepair();
            CheckShieldPointsRepair();
            CheckRadiation();
            MoveManager.Tick();
            AttackManager.LaserAttack();
            AttackManager.RocketLauncher.Tick();
            Logout();

            CheckUnderEffects();
            DroneManager.Tick();
            TechManager.Tick();
            SkillManager.Tick();

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
            if (LastCombatTime.AddSeconds(10) >= DateTime.Now || lastHpRepairTime.AddSeconds(1) >= DateTime.Now || CurrentHitPoints == MaxHitPoints) return;

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
            if (underDCR_250 && underDCR_250Time.AddMilliseconds(TimeManager.DCR_250_DURATION) < DateTime.Now)
                DeactiveDCR_250();
            if (underPLD8 && underPLD8Time.AddMilliseconds(TimeManager.PLD8_DURATION) < DateTime.Now)
                DeactivePLD8();
            if (underSLM_01 && underSLM_01Time.AddMilliseconds(TimeManager.SLM_01_DURATION) < DateTime.Now)
                DeactiveSLM_01();
            if (invincibilityEffect && invincibilityEffectTime.AddMilliseconds(TimeManager.INVINCIBILITY_DURATION) < DateTime.Now)
                DeactiveInvincibilityEffect();
            if (mirroredControlEffect && mirroredControlEffectTime.AddMilliseconds(TimeManager.MIRRORED_CONTROL_DURATION) < DateTime.Now)
                DeactiveMirroredControlEffect();
            if (wizardEffect && wizardEffectTime.AddMilliseconds(TimeManager.WIZARD_DURATION) < DateTime.Now)
                DeactiveWizardEffect();
        }

        public void DeactivePLD8()
        {
            underPLD8 = false;
            SendPacket("0|n|MAL|REM|" + Id + "");
            SendPacketToInRangePlayers("0|n|MAL|REM|" + Id + "");
        }

        public void DeactiveDCR_250()
        {
            underDCR_250 = false;
            SendPacket("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendPacketToInRangePlayers("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendCommand(AttributeShipSpeedUpdateCommand.write(Speed));
        }

        public void DeactiveSLM_01()
        {
            underSLM_01 = false;
            SendPacket("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendPacketToInRangePlayers("0|n|fx|end|SABOTEUR_DEBUFF|" + Id + "");
            SendCommand(AttributeShipSpeedUpdateCommand.write(Speed));
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
            if (Jumping || !IsInRadiationZone || invincibilityEffectTime.AddSeconds(5) >= DateTime.Now || lastRadiationDamageTime.AddSeconds(1) >= DateTime.Now) return;

            AttackManager.Damage(this, this, DamageType.RADIATION, 20000, true, true, false);
            lastRadiationDamageTime = DateTime.Now;
        }

        public void SetSpeedBoost(int speed)
        {
            SpeedBoost = speed;
            SendCommand(AttributeShipSpeedUpdateCommand.write(Speed));
        }

        public void RepairBot(bool activated)
        {
            RepairBotActivated = activated;
            SendPacket(GetBeaconPacket());
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
                var value = CurrentConfig == 1 ? Equipment.Config1Speed : Equipment.Config2Speed;

                switch (SettingsManager.SelectedFormation)
                {
                    case DroneManager.CRAB_FORMATION:
                        value -= Maths.GetPercentage(value, 20);
                        break;
                    case DroneManager.BAT_FORMATION:
                        value -= Maths.GetPercentage(value, 15);
                        break;
                }

                if (underDCR_250)
                    value -= Maths.GetPercentage(value, 30);

                if (underSLM_01)
                    value -= Maths.GetPercentage(value, 50);

                value += SpeedBoost;

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
                //value += Maths.GetPercentage(value, BoosterManager.GetPercentage(BoostedAttributeType.SHIELD));
                //portaldan atlayınca can yükseliyor booster hatası

                switch (SettingsManager.SelectedFormation)
                {
                    case DroneManager.HEART_FORMATION:
                    case DroneManager.TURTLE_FORMATION:
                        value += Maths.GetPercentage(value, 10);
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
                var value = CurrentConfig == 1 ? Equipment.Config1Damage : Equipment.Config2Damage;
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
            DroneManager.UpdateDrones();
            UpdateStatus();
        }

        public string GetBeaconPacket()
        {
            return $"0|{ServerCommands.BEACON}|{Position.X}|{Position.Y}|{Convert.ToInt32(IsInDemilitarizedZone)}|{Convert.ToInt32(RepairBotActivated)}|0|{Convert.ToInt32(IsInRadiationZone)}|{Convert.ToInt32(CurrentInRangePortalId != -1)}|100";
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
                Ship.Id,
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
                VisualModifiers.Values.ToList());
        }

        public byte[] GetShipInitializationCommand()
        {
            return ShipInitializationCommand.write(
                Id,
                Name,
                Ship.Id,
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
                0,
                0,
                3,
                true,
                Data.experience,
                Data.honor,
                Level,
                Data.credits,
                Data.uridium,
                0,
                RankId,
                GetClanTag(),
                100,
                false,
                Invisible,
                VisualModifiers.Values.ToList());
        }

        public bool Attackable()
        {
            if (AttackManager.IshCooldown.AddMilliseconds(TimeManager.ISH_DURATION) > DateTime.Now || invincibilityEffect || GodMode)
                return false;
            else
                return true;
        }

        public void SendCooldown(string itemId, int time)
        {
            SendPacket("0|A|CLD|" + itemId + "|" + time / 1000 + "");
        }

        public void UpdateCurrentCooldowns()
        {
            int smbCooldown = (int)(TimeManager.SMB_COOLDOWN - (DateTime.Now - AttackManager.SmbCooldown).TotalMilliseconds);
            int ishCooldown = (int)(TimeManager.ISH_COOLDOWN - (DateTime.Now - AttackManager.IshCooldown).TotalMilliseconds);
            int empCooldown = (int)(TimeManager.EMP_COOLDOWN - (DateTime.Now - AttackManager.EmpCooldown).TotalMilliseconds);
            int mineCooldown = (int)(TimeManager.MINE_COOLDOWN - (DateTime.Now - AttackManager.mineCooldown).TotalMilliseconds);
            int dcrCooldown = (int)(TimeManager.DCR_250_COOLDOWN - (DateTime.Now - AttackManager.dcr_250Cooldown).TotalMilliseconds);
            int pldCooldown = (int)(TimeManager.PLD8_COOLDOWN - (DateTime.Now - AttackManager.pld8Cooldown).TotalMilliseconds);
            /*
            int energyLeechCooldown = (int)(TimeManager.ENERGY_LEECH_COOLDOWN - (DateTime.Now - TechManager.EnergyLeech.cooldown).TotalMilliseconds);
            int chainImpulseCooldown = (int)(TimeManager.CHAIN_IMPULSE_COOLDOWN - (DateTime.Now - TechManager.ChainImpulse.cooldown).TotalMilliseconds);
            int precisionTargeterCooldown = (int)(TimeManager.PRECISION_TARGETER_COOLDOWN - (DateTime.Now - TechManager.PrecisionTargeter.cooldown).TotalMilliseconds);
            int backupShieldsCooldown = (int)(TimeManager.BACKUP_SHIELD_COOLDOWN - (DateTime.Now - TechManager.BackupShields.cooldown).TotalMilliseconds);
            int battleRepairBotCooldown = (int)(TimeManager.BATTLE_REPAIR_BOT_COOLDOWN - (DateTime.Now - TechManager.BattleRepairBot.cooldown).TotalMilliseconds);
            */
            int sentinelCooldown = (int)(TimeManager.SENTINEL_COOLDOWN - (DateTime.Now - SkillManager.Sentinel.cooldown).TotalMilliseconds);
            int diminisherCooldown = (int)(TimeManager.DIMINISHER_COOLDOWN - (DateTime.Now - SkillManager.Diminisher.cooldown).TotalMilliseconds);
            int venomCooldown = (int)(TimeManager.VENOM_COOLDOWN - (DateTime.Now - SkillManager.Venom.cooldown).TotalMilliseconds);
            int spectrumCooldown = (int)(TimeManager.SPECTRUM_COOLDOWN - (DateTime.Now - SkillManager.Spectrum.cooldown).TotalMilliseconds);
            int solaceCooldown = (int)(TimeManager.SOLACE_COOLDOWN - (DateTime.Now - SkillManager.Solace.cooldown).TotalMilliseconds);

            if (empCooldown >= 0) Settings.CurrentCooldowns.smbCooldown = smbCooldown;
            if (ishCooldown >= 0) Settings.CurrentCooldowns.ishCooldown = ishCooldown;
            if (empCooldown >= 0) Settings.CurrentCooldowns.empCooldown = empCooldown;
            if (mineCooldown >= 0) Settings.CurrentCooldowns.mineCooldown = mineCooldown;
            if (dcrCooldown >= 0) Settings.CurrentCooldowns.dcrCooldown = dcrCooldown;
            if (pldCooldown >= 0) Settings.CurrentCooldowns.pldCooldown = pldCooldown;
            /*
            if (energyLeechCooldown >= 0)  Settings.CurrentCooldowns.energyLeechCooldown = energyLeechCooldown;
            if (chainImpulseCooldown >= 0)  Settings.CurrentCooldowns.chainImpulseCooldown = chainImpulseCooldown;
            if (precisionTargeterCooldown >= 0)  Settings.CurrentCooldowns.precisionTargeterCooldown = precisionTargeterCooldown;
            if (backupShieldsCooldown >= 0)  Settings.CurrentCooldowns.backupShieldsCooldown = backupShieldsCooldown;
            if (battleRepairBotCooldown >= 0)  Settings.CurrentCooldowns.battleRepairBotCooldown = battleRepairBotCooldown;
            */
            if (sentinelCooldown >= 0) Settings.CurrentCooldowns.sentinelCooldown = sentinelCooldown;
            if (diminisherCooldown >= 0) Settings.CurrentCooldowns.diminisherCooldown = diminisherCooldown;
            if (venomCooldown >= 0) Settings.CurrentCooldowns.venomCooldown = venomCooldown;
            if (spectrumCooldown >= 0) Settings.CurrentCooldowns.spectrumCooldown = spectrumCooldown;
            if (solaceCooldown >= 0) Settings.CurrentCooldowns.solaceCooldown = solaceCooldown;
            QueryManager.SavePlayer.Settings(this);
        }

        public void SetCurrentCooldowns()
        {
            AttackManager.SmbCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.SMB_COOLDOWN - Settings.CurrentCooldowns.smbCooldown));
            AttackManager.IshCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.ISH_COOLDOWN - Settings.CurrentCooldowns.ishCooldown));
            AttackManager.EmpCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.EMP_COOLDOWN - Settings.CurrentCooldowns.empCooldown));
            AttackManager.mineCooldown = DateTime.Now.AddMilliseconds(-(TimeManager.MINE_COOLDOWN - Settings.CurrentCooldowns.mineCooldown));
            AttackManager.dcr_250Cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.DCR_250_COOLDOWN - Settings.CurrentCooldowns.dcrCooldown));
            AttackManager.pld8Cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.PLD8_COOLDOWN - Settings.CurrentCooldowns.pldCooldown));
            /*
            TechManager.EnergyLeech.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.ENERGY_LEECH_COOLDOWN - Settings.CurrentCooldowns.energyLeechCooldown));
            TechManager.ChainImpulse.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.CHAIN_IMPULSE_COOLDOWN - Settings.CurrentCooldowns.chainImpulseCooldown));
            TechManager.PrecisionTargeter.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.PRECISION_TARGETER_COOLDOWN - Settings.CurrentCooldowns.precisionTargeterCooldown));
            TechManager.BackupShields.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.BACKUP_SHIELD_COOLDOWN - Settings.CurrentCooldowns.backupShieldsCooldown));
            TechManager.BattleRepairBot.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.BATTLE_REPAIR_BOT_COOLDOWN - Settings.CurrentCooldowns.battleRepairBotCooldown));
            */
            SkillManager.Sentinel.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.SENTINEL_COOLDOWN - Settings.CurrentCooldowns.sentinelCooldown));
            SkillManager.Diminisher.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.DIMINISHER_COOLDOWN - Settings.CurrentCooldowns.diminisherCooldown));
            SkillManager.Venom.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.VENOM_COOLDOWN - Settings.CurrentCooldowns.venomCooldown));
            SkillManager.Spectrum.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.SPECTRUM_COOLDOWN - Settings.CurrentCooldowns.spectrumCooldown));
            SkillManager.Solace.cooldown = DateTime.Now.AddMilliseconds(-(TimeManager.SOLACE_COOLDOWN - Settings.CurrentCooldowns.solaceCooldown));
        }

        public void SendCurrentCooldowns()
        {
            var smbCooldown = (DateTime.Now - AttackManager.SmbCooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.SMARTBOMB_COOLDOWN, (int)(TimeManager.SMB_COOLDOWN - smbCooldown));

            var ishCooldown = (DateTime.Now - AttackManager.IshCooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.INSTASHIELD_COOLDOWN, (int)(TimeManager.ISH_COOLDOWN - ishCooldown));

            var empCooldown = (DateTime.Now - AttackManager.EmpCooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.EMP_COOLDOWN, (int)(TimeManager.EMP_COOLDOWN - empCooldown));

            var mineCooldown = (DateTime.Now - AttackManager.mineCooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.MINE_COOLDOWN, (int)(TimeManager.MINE_COOLDOWN - mineCooldown));

            var dcrCooldown = (DateTime.Now - AttackManager.dcr_250Cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.DCR_ROCKET, (int)(TimeManager.DCR_250_COOLDOWN - dcrCooldown));

            var pldCooldown = (DateTime.Now - AttackManager.pld8Cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.PLASMA_DISCONNECT_COOLDOWN, (int)(TimeManager.PLD8_COOLDOWN - pldCooldown));

            /*
            var energyLeechCooldown = (DateTime.Now - TechManager.EnergyLeech.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.TECH_ENERGY_LEECH, (int)(TimeManager.ENERGY_LEECH_COOLDOWN - energyLeechCooldown));

            var chainImpulseCooldown = (DateTime.Now - TechManager.ChainImpulse.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.TECH_ELECTRIC_CHAIN_IMPULSE, (int)(TimeManager.CHAIN_IMPULSE_COOLDOWN - chainImpulseCooldown));

            var precisionTargeterCooldown = (DateTime.Now - TechManager.PrecisionTargeter.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.TECH_ROCKET_PROBABILITY_MAXIMIZER, (int)(TimeManager.PRECISION_TARGETER_COOLDOWN - precisionTargeterCooldown));

            var backupShieldsCooldown = (DateTime.Now - TechManager.BackupShields.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.TECH_SHIELD_BACK_UP, (int)(TimeManager.BACKUP_SHIELD_COOLDOWN - backupShieldsCooldown));

            var battleRepairBotCooldown = (DateTime.Now - TechManager.BattleRepairBot.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.TECH_BATTLE_REP_BOT, (int)(TimeManager.BATTLE_REPAIR_BOT_COOLDOWN - battleRepairBotCooldown));
            */

            var sentinelCooldown = (DateTime.Now - SkillManager.Sentinel.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.SKILL_SENTINEL, (int)(TimeManager.SENTINEL_COOLDOWN - sentinelCooldown));

            var diminisherCooldown = (DateTime.Now - SkillManager.Diminisher.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.SKILL_DIMINISHER, (int)(TimeManager.DIMINISHER_COOLDOWN - diminisherCooldown));

            var venomCooldown = (DateTime.Now - SkillManager.Venom.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.SKILL_VENOM, (int)(TimeManager.VENOM_COOLDOWN - venomCooldown));

            var spectrumCooldown = (DateTime.Now - SkillManager.Spectrum.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.SKILL_SPECTRUM, (int)(TimeManager.SPECTRUM_COOLDOWN - spectrumCooldown));

            var solaceCooldown = (DateTime.Now - SkillManager.Solace.cooldown).TotalMilliseconds;
            SendCooldown(ServerCommands.SKILL_SOLACE, (int)(TimeManager.SOLACE_COOLDOWN - solaceCooldown));
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


        public void ChangeShip(int shipId)
        {
            var player = this;
            if (player.Jumping) return;

            player.Jumping = true;

            var pet = player.Pet.Activated;
            player.Pet.Deactivate(true);

            player.Ship = GameManager.GetShip(shipId);
            player.CurrentInRangePortalId = -1;
            player.Selected = null;
            player.DisableAttack(player.SettingsManager.SelectedLaser);
            player.Spacemap.RemoveCharacter(player);
            player.InRangeAssets.Clear();
            player.InRangeCharacters.Clear();

            player.Spacemap.AddAndInitPlayer(player);
            player.Jumping = false;

            if (pet)
                player.Pet.Activate();
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

        public void KillScreen(Character killerPlayer, DestructionType destructionType, bool killedLogin = false)
        {
            var killScreenOptionModules = new List<KillScreenOptionModule>();
            var basicRepair =
                   new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_REPAIR),
                                              new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()));
            var portalRepair =
                  new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.AT_JUMPGATE_REPAIR),
                                             new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()));
            var deathLocationRepair =
                  new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR),
                                             new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                             new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()));
            var fullRepair =
                   new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_FULL_REPAIR),
                                              new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()),
                                              new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new List<MessageWildcardReplacementModule>()));
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
            Settings.InGameSettings.inEquipZone = basicRepair || fullRepair ? true : false;
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

            AddVisualModifier(new VisualModifierCommand(Id, VisualModifierCommand.INVINCIBILITY, 0, true));

            Spacemap.AddAndInitPlayer(this);

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

        public void EnableAttack(int itemId)
        {
            AttackManager.Attacking = true;
        }

        public void DisableAttack(int itemId)
        {
            AttackManager.Attacking = false;
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
                if (!Program.TickManager.Exists(this)) return;
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
                if (!Program.TickManager.Exists(this)) return;
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
                //SendPacket("0|l|");
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

        public GameSession GetGameSession()
        {
            return GameManager.GetGameSession(Id);
        }
    }
}