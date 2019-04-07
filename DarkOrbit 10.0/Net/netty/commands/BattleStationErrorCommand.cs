using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationErrorCommand
    {
        public const short ID = 20259;

        public const short UNSPECIFIED = 0;

        public const short NO_CLAN = 1;

        public const short STATION_ALREADY_BUILDING = 2;

        public const short ITEM_HITPOINTS_ZERO = 3;

        public const short ITEM_ALREADY_EQUIPPED_IN_ANOTHER_ASTEROID = 4;

        public const short CONCURRENT_EQUIP = 5;

        public const short REPLACE_RIGHT_MISSING = 6;

        public const short ITEM_NOT_OWNED = 7;

        public const short OUT_OF_RANGE = 8;

        public const short EQUIP_OF_SAME_PLAYER_RUNNING = 9;

        public const short SATELLITE_ONLY = 10;

        public const short HUB_ONLY = 11;

        public const short ITEM_NOT_IN_STATION = 12;

        public const short MAX_NUMBER_OF_MODULE_TYPE_EXCEEDED = 13;

        public const short DEFLECTOR_NO_RIGHTS = 14;

        public const short DEFLECTOR_ALREADY_OFF = 15;

        public const short REPAIR_NO_MODULE = 16;

        public const short REPAIR_NO_MONEY = 17;

        public const short REPAIR_ALREADY_RUNNING = 18;

        public static byte[] write(short type)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(type);
            param1.writeShort(-1134);
            param1.writeShort(-7171);
            return param1.ToByteArray();
        }
    }
}
