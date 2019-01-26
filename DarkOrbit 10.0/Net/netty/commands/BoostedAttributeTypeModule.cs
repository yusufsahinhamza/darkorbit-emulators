using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BoostedAttributeTypeModule
    {
        public const short ID = 19265;

        public const short EP = 0;
        public const short HONOUR = 1;
        public const short DAMAGE = 2;
        public const short SHIELD = 3;
        public const short REPAIR = 4;
        public const short SHIELDRECHARGE = 5;
        public const short RESOURCE = 6;
        public const short MAXHP = 7;
        public const short ABILITY_COOLDOWN = 8;
        public const short BONUSBOXES = 9;
        public const short QUESTREWARD = 10;

        public short typeValue = 0;

        public BoostedAttributeTypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.typeValue);
            param1.writeShort(2096);
            return param1.Message.ToArray();
        }
    }
}
