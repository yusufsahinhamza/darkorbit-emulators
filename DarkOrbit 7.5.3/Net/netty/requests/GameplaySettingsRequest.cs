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
        public const short ID = 27274;

        public Boolean autoBoost = false;
        public Boolean autoRefinement = false;
        public Boolean quickslotStopAttack = false;
        public Boolean doubleclickAttack = false;
        public Boolean autoChangeAmmo = false;
        public Boolean autoStart = false;
        public Boolean autoBuyGreenBootyKeys = false;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.autoBoost = param1.readBoolean();
            this.autoRefinement = param1.readBoolean();
            this.quickslotStopAttack = param1.readBoolean();
            this.doubleclickAttack = param1.readBoolean();
            this.autoChangeAmmo = param1.readBoolean();
            this.autoStart = param1.readBoolean();
            this.autoBuyGreenBootyKeys = param1.readBoolean();
        }
    }
}
