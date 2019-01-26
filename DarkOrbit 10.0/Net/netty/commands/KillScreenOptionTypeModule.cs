using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class KillScreenOptionTypeModule
    {
        public const short ID = 11653;

        public const short FREE_PHOENIX = 0;
        public const short BASIC_REPAIR = 1;
        public const short AT_JUMPGATE_REPAIR = 2;
        public const short AT_DEATHLOCATION_REPAIR = 3;
        public const short AT_SECTOR_CONTROL_SPAWNPOINT = 4;
        public const short EXIT_SECTOR_CONTROL = 5;
        public const short BASIC_FULL_REPAIR = 6;
        public const short short_1942 = 7;
        public const short short_564 = 8;
        public const short const_2728 = 9;

        public short typeValue = 0;

        public KillScreenOptionTypeModule(short pRepairTypeValue)
        {
            this.typeValue = pRepairTypeValue;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.typeValue);
            param1.writeShort(20436);
            return param1.Message.ToArray();
        }
    }
}
