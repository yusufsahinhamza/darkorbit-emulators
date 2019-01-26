using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BoosterTypeModule
    {
        public const short ID = 26950;

        public const short DMG_B01 = 0;
        public const short DMG_B02 = 1;
        public const short EP_B01 = 2;
        public const short EP_B02 = 3;
        public const short EP50 = 4;
        public const short HON_B01 = 5;
        public const short HON_B02 = 6;
        public const short HON50 = 7;
        public const short HP_B01 = 8;
        public const short HP_B02 = 9;
        public const short REP_B01 = 10;
        public const short REP_B02 = 11;
        public const short REP_S01 = 12;
        public const short RES_B01 = 13;
        public const short RES_B02 = 14;
        public const short SHD_B01 = 15;
        public const short SHD_B02 = 16;
        public const short SREG_B01 = 17;
        public const short SREG_B02 = 18;
        public const short BB_01 = 19;
        public const short QR_01 = 20;
        public const short CD_B01 = 21;
        public const short CD_B02 = 22;
        public const short KAPPA_B01 = 23;
        public const short HONM_1 = 24;
        public const short XPM_1 = 25;
        public const short DMGM_1 = 26;

        public short typeValue = 0;

        public BoosterTypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(16615);
            param1.writeShort(4539);
            param1.writeShort(this.typeValue);
            return param1.Message.ToArray();
        }
    }
}
