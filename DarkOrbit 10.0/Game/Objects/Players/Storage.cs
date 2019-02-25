using Ow.Game.Events;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Mines;
using Ow.Game.Objects.Stations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players
{
    class Storage
    {
        public Player Player { get; set; }

        public ConcurrentDictionary<string, Collectable> InRangeCollectables = new ConcurrentDictionary<string, Collectable>();
        public ConcurrentDictionary<string, Mine> InRangeMines = new ConcurrentDictionary<string, Mine>();
        public Dictionary<int, Group> GroupInvites = new Dictionary<int, Group>();
        public ConcurrentDictionary<int, Activatable> InRangeAssets = new ConcurrentDictionary<int, Activatable>();
        public Dictionary<int, Player> DuelInvites = new Dictionary<int, Player>();

        public bool UbaMatchmakingAccepted = false;
        public Player UbaOpponent = null;
        public Duel Duel { get; set; }

        public DateTime KillscreenPortalRepairTime = new DateTime();
        public DateTime KillscreenDeathLocationRepairTime = new DateTime();

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

        public bool underPLD8 = false;
        public DateTime underPLD8Time = new DateTime();

        public Storage(Player player) { Player = player; }
    }
}
