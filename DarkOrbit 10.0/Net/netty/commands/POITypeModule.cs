using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class POITypeModule
    {
        public static short ID = 29441;

        public const short GENERIC = 0;
        public const short FACTORIZED = 1;
        public const short TRIGGER = 2;
        public const short DAMAGE = 3;
        public const short HEALING = 4;
        public const short NO_ACCESS = 5;
        public const short FACTION_NO_ACCESS = 6;
        public const short ENTER_LEAVE = 7;
        public const short RADIATION = 8;
        public const short CAGE = 9;
        public const short MINE_FIELD = 10;
        public const short BUFF_ZONE = 11;
        public const short SECTOR_CONTROL_HOME_ZONE = 12;
        public const short SECTOR_CONTROL_SECTOR_ZONE = 13;

        public short typeValue = 0;

        public POITypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(22758);
            param1.writeShort(2628);
            param1.writeShort(this.typeValue);
            return param1.Message.ToArray();
        }
    }
}
