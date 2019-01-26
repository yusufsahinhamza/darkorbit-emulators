using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class POIDesignModule
    {
        public static short ID = 20767;

        public const short NONE = 0;
        public const short ASTEROIDS = 1;
        public const short ASTEROIDS_BLUE = 2;
        public const short ASTEROIDS_MIXED_WITH_SCRAP = 3;
        public const short SCRAP = 4;
        public const short NEBULA = 5;
        public const short SIMPLE = 6;
        public const short SECTOR_CONTROL_HOME_ZONE = 7;
        public const short SECTOR_CONTROL_SECTOR_ZONE = 8;

        public short designValue = 0;

        public POIDesignModule(short designValue)
        {
            this.designValue = designValue;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-19291);
            param1.writeShort(this.designValue);
            param1.writeShort(26576);
            return param1.Message.ToArray();
        }
    }
}
