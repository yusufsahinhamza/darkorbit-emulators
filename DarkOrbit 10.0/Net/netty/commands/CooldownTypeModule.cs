using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class CooldownTypeModule
    {
        public static short ID = 7786;

        public static short short_378 = 20;
        public static short ROCKET_PROBABILITY_MAXIMIZER = 15;
        public static short short_317 = 24;
        public static short short_1124 = 26;
        public static short SPEED_LEECH = 17;
        public static short short_755 = 2;
        public static short short_1220 = 31;
        public static short short_2642 = 32;
        public static short NONE = 0;
        public static short short_1587 = 5;
        public static short short_1048 = 41;
        public static short short_1736 = 23;
        public static short BATTLE_REPAIR_BOT = 18;
        public static short short_2172 = 38;
        public static short SINGULARITY = 34;
        public static short short_918 = 22;
        public static short ENERGY_CHAIN_IMPULSE = 14;
        public static short short_1085 = 19;
        public static short short_899 = 8;
        public static short short_1699 = 6;
        public static short RAPID_SALVO_BLAST = 9;
        public static short short_888 = 2;
        public static short short_1952 = 40;
        public static short ENERGY_LEECH_ARRAY = 13;
        public static short MINE = 1;
        public static short short_2204 = 37;
        public static short short_2419 = 43;
        public static short SHIELD_BACKUP = 16;
        public static short short_138 = 30;
        public static short PLASMA = 7;
        public static short short_1789 = 10;
        public static short short_2342 = 42;
        public static short short_2738 = 25;
        public static short short_1815 = 33;
        public static short short_987 = 36;
        public static short ROCKET = 4;
        public static short short_798 = 12;
        public static short short_255 = 21;
        public static short short_2047 = 28;
        public static short ROCKET_LAUNCHER = 11;
        public static short short_1554 = 39;
        public static short short_1428 = 27;
        public static short SPEED_BUFF = 35;
        public static short short_1439 = 3;

        public short var_1413 = 0;

        public CooldownTypeModule(short param1)
        {
            this.var_1413 = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.var_1413);
            param1.writeShort(5387);
            return param1.Message.ToArray();
        }
    }
}
