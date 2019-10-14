using Ow.Game.Events;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Mines;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Objects.Players.Skills;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players
{
    public class ModuleBase
    {
        public int Id { get; set; }
        public short Type { get; set; }
        public bool InUse { get; set; }

        public ModuleBase(int id, short type, bool inUse)
        {
            Id = id;
            Type = type;
            InUse = inUse;
        }
    }

    class Storage
    {
        public Player Player { get; set; }

        public ConcurrentDictionary<int, Object> InRangeObjects = new ConcurrentDictionary<int, Object>();
        public Dictionary<int, Group> GroupInvites = new Dictionary<int, Group>();
        public ConcurrentDictionary<int, Activatable> InRangeAssets = new ConcurrentDictionary<int, Activatable>();
        public ConcurrentDictionary<int, Player> DuelInvites = new ConcurrentDictionary<int, Player>();
        public List<int> KilledPlayerIds = new List<int>();
        public Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
        public List<ModuleBase> BattleStationModules = new List<ModuleBase>();

        public bool UbaMatchmakingAccepted = false;
        public Duel Duel { get; set; }
        public Uba Uba = null;

        public DateTime KillscreenPortalRepairTime = new DateTime();
        public DateTime KillscreenDeathLocationRepairTime = new DateTime();

        public int SpeedBoost = 0;
        public bool OnBlockedMinePosition = false;
        public bool IsInDemilitarizedZone = false;
        public bool IsInRadiationZone = false;
        public bool IsInEquipZone = false;
        public bool AtHangar = false;

        public bool AutoRocket = false;
        public bool AutoRocketLauncher = false;
        public bool RepairBotActivated = false;
        public bool ShieldSkillActivated = false;
        public bool PrecisionTargeter = false;
        public bool EnergyLeech = false;

        public bool Jumping = false;
        public bool GodMode = false;
        public bool GroupInitialized { get; set; }

        public bool Lightning = false;
        public bool Sentinel = false;
        public bool Spectrum = false;

        public bool Diminisher = false;
        public Attackable UnderDiminisherEntity { get; set; }

        public bool Venom = false;
        public Attackable UnderVenomEntity { get; set; }

        public bool underR_IC3 = false;
        public DateTime underR_IC3Time = new DateTime();

        public bool underDCR_250 = false;
        public DateTime underDCR_250Time = new DateTime();

        public bool underSLM_01 = false;
        public DateTime underSLM_01Time = new DateTime();

        public DateTime invincibilityEffectTime = new DateTime();

        public bool mirroredControlEffect = false;
        public DateTime mirroredControlEffectTime = new DateTime();

        public bool wizardEffect = false;
        public DateTime wizardEffectTime = new DateTime();

        public bool underPLD8 = false;
        public DateTime underPLD8Time = new DateTime();

        public bool underDrawFire = false;
        public DateTime underDrawFireTime = new DateTime();

        public Storage(Player player) { Player = player; }

        public void Tick()
        {
            if (underR_IC3 && underR_IC3Time.AddMilliseconds(TimeManager.R_IC3_DURATION) < DateTime.Now)
                DeactiveR_RIC3();
            if (underDCR_250 && underDCR_250Time.AddMilliseconds(TimeManager.DCR_250_DURATION) < DateTime.Now)
                DeactiveDCR_250();
            if (underPLD8 && underPLD8Time.AddMilliseconds(TimeManager.PLD8_DURATION) < DateTime.Now)
                DeactivePLD8();
            if (underSLM_01 && underSLM_01Time.AddMilliseconds(TimeManager.SLM_01_DURATION) < DateTime.Now)
                DeactiveSLM_01();
            if (Player.Invincible && invincibilityEffectTime.AddMilliseconds(TimeManager.INVINCIBILITY_DURATION) < DateTime.Now)
                DeactiveInvincibilityEffect();
            if (mirroredControlEffect && mirroredControlEffectTime.AddMilliseconds(TimeManager.MIRRORED_CONTROL_DURATION) < DateTime.Now)
                DeactiveMirroredControlEffect();
            if (wizardEffect && wizardEffectTime.AddMilliseconds(TimeManager.WIZARD_DURATION) < DateTime.Now)
                DeactiveWizardEffect();
            if (underDrawFire && underDrawFireTime.AddMilliseconds(TimeManager.CITADEL_DRAWFIRE_DURATION) < DateTime.Now)
                DeactiveDrawFireEffect();
        }

        public void DeactivePLD8()
        {
            if (underPLD8)
            {
                underPLD8 = false;
                Player.SendPacket("0|n|MAL|REM|" + Player.Id + "");
                Player.SendPacketToInRangePlayers("0|n|MAL|REM|" + Player.Id + "");
            }
        }
        public void DeactiveR_RIC3()
        {
            if (underR_IC3)
            {
                underR_IC3 = false;
                Player.SendPacket("0|n|fx|end|ICY_CUBE|" + Player.Id + "");
                Player.SendPacketToInRangePlayers("0|n|fx|end|ICY_CUBE|" + Player.Id + "");
                Player.SendCommand(SetSpeedCommand.write(Player.Speed, Player.Speed));
            }
        }

        public void DeactiveDCR_250()
        {
            if (underDCR_250)
            {
                underDCR_250 = false;

                if (underDCR_250Time < underSLM_01Time || !underSLM_01)
                {
                    Player.SendPacket("0|n|fx|end|SABOTEUR_DEBUFF|" + Player.Id + "");
                    Player.SendPacketToInRangePlayers("0|n|fx|end|SABOTEUR_DEBUFF|" + Player.Id + "");
                }

                Player.SendCommand(SetSpeedCommand.write(Player.Speed, Player.Speed));
            }
        }

        public void DeactiveSLM_01()
        {
            if (underSLM_01)
            {
                underSLM_01 = false;

                if (underSLM_01Time < underDCR_250Time || !underDCR_250)
                {
                    Player.SendPacket("0|n|fx|end|SABOTEUR_DEBUFF|" + Player.Id + "");
                    Player.SendPacketToInRangePlayers("0|n|fx|end|SABOTEUR_DEBUFF|" + Player.Id + "");
                }

                Player.SendCommand(SetSpeedCommand.write(Player.Speed, Player.Speed));
            }
        }

        public void DeactiveInvincibilityEffect()
        {
            if (Player.Invincible)
            {
                Player.Invincible = false;
                Player.RemoveVisualModifier(VisualModifierCommand.INVINCIBILITY);
            }
        }

        public void DeactiveMirroredControlEffect()
        {
            if (mirroredControlEffect)
            {
                mirroredControlEffect = false;
                Player.RemoveVisualModifier(VisualModifierCommand.MIRRORED_CONTROLS);
            }
        }

        public void DeactiveWizardEffect()
        {
            if (wizardEffect)
            {
                wizardEffect = false;
                Player.RemoveVisualModifier(VisualModifierCommand.WIZARD_ATTACK);
            }
        }

        public void DeactiveDrawFireEffect()
        {
            if (underDrawFire)
            {
                underDrawFire = false;
                Player.RemoveVisualModifier(VisualModifierCommand.DRAW_FIRE_TARGET);
            }
        }
    }
}
