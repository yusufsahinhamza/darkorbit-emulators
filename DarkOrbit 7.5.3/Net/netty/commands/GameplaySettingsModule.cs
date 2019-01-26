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
        public const short ID = 2278;

        public Boolean notSet = false;

        public Boolean autoBoost = false;

        public Boolean autoRefinement = false;

        public Boolean quickslotStopAttack = false;

        public Boolean doubleclickAttack = false;

        public Boolean autoChangeAmmo = false;

        public Boolean autoStart = false;

        public Boolean autoBuyGreenBootyKeys = false;

        public GameplaySettingsModule(Boolean notSet, Boolean autoBoost, Boolean autoRefinement, Boolean quickslotStopAttack, Boolean doubleclickAttack, Boolean autoChangeAmmo, Boolean autoStart, Boolean autoBuyGreenBootyKeys)
        {
            this.notSet = notSet;
            this.autoBoost = autoBoost;
            this.autoRefinement = autoRefinement;
            this.quickslotStopAttack = quickslotStopAttack;
            this.doubleclickAttack = doubleclickAttack;
            this.autoChangeAmmo = autoChangeAmmo;
            this.autoStart = autoStart;
            this.autoBuyGreenBootyKeys = autoBuyGreenBootyKeys;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.notSet);
            param1.writeBoolean(this.autoBoost);
            param1.writeBoolean(this.autoRefinement);
            param1.writeBoolean(this.quickslotStopAttack);
            param1.writeBoolean(this.doubleclickAttack);
            param1.writeBoolean(this.autoChangeAmmo);
            param1.writeBoolean(this.autoStart);
            param1.writeBoolean(this.autoBuyGreenBootyKeys);
            return param1.Message.ToArray();
        }
    }
}
