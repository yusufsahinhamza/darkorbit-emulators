using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GameplaySettingsModule
    {
        public const short ID = 20988;

        public bool NotSet = false;
        public bool AutoRefinement = false;
        public bool QuickSlotStopAttack = false;
        public bool AutoBoost = false;
        public bool AutoBuyBootyKeys = false;
        public bool DoubleClickAttackEnabled = false;
        public bool AutoChangeAmmo = false;
        public bool AutoStartEnabled = false;
        public bool ShowBattlerayNotifications = false;
        public bool varE3N = false;

        public GameplaySettingsModule(bool param1, bool param2, bool param3, bool param4, bool param5,
                                      bool param6, bool param7, bool param8, bool param9, bool param10  )
        {
            this.NotSet = param1;
            this.AutoBoost = param2;
            this.AutoRefinement = param3;
            this.QuickSlotStopAttack = param4;
            this.DoubleClickAttackEnabled = param5;
            this.AutoChangeAmmo = param6;
            this.AutoStartEnabled = param7;
            this.AutoBuyBootyKeys = param8;
            this.ShowBattlerayNotifications = param9;
            this.varE3N = param10;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.NotSet);
            param1.writeBoolean(this.AutoRefinement);
            param1.writeBoolean(this.AutoChangeAmmo);
            param1.writeBoolean(this.varE3N);
            param1.writeBoolean(this.AutoBuyBootyKeys);
            param1.writeBoolean(this.AutoStartEnabled);
            param1.writeBoolean(this.QuickSlotStopAttack);
            param1.writeBoolean(this.ShowBattlerayNotifications);
            param1.writeBoolean(this.DoubleClickAttackEnabled);
            param1.writeShort(-15038);
            param1.writeBoolean(this.AutoBoost);
            return param1.Message.ToArray();
        }
    }
}
