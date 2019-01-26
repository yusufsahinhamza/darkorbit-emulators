using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class GameplaySettingsRequest
    {
        public const short ID = 25300;

        public bool DoubleClickAttackEnabled = false;
        public bool AutoChangeAmmo = false;
        public bool AutoStartEnabled = false;
        public bool AutoRefinement = false;
        public bool AutoBoost = false;
        public bool AutoBuyBootyKeys = false;
        public bool QuickSlotStopAttack = false;
        public bool ShowBattlerayNotifications = false;
        public bool varE3N = false;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            AutoStartEnabled = parser.readBoolean();
            DoubleClickAttackEnabled = parser.readBoolean();
            varE3N = parser.readBoolean();
            AutoBuyBootyKeys = parser.readBoolean();
            parser.readShort();
            AutoBoost = parser.readBoolean();
            QuickSlotStopAttack = parser.readBoolean();
            AutoChangeAmmo = parser.readBoolean();
            ShowBattlerayNotifications = parser.readBoolean();
            AutoRefinement = parser.readBoolean();
        }
    }
}
