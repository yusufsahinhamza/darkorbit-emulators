using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttackTypeModule
    {
        public const short ID = 32205;

        public const short ROCKET = 0;
        public const short LASER = 1;
        public const short MINE = 2;
        public const short RADIATION = 3;
        public const short PLASMA = 4;
        public const short ECI = 5;
        public const short SL = 6;
        public const short CID = 7;
        public const short SINGULARITY = 8;
        public const short KAMIKAZE = 9;
        public const short REPAIR = 10;
        public const short DECELERATION = 11;
        public const short SHIELD_ABSORBER_ROCKET_CREDITS = 12;
        public const short SHIELD_ABSORBER_ROCKET_URIDIUM = 13;
        public const short STICKY_BOMB = 14;
        public const short varQ3U = 15;

        public static short typeValue = 0;

        public AttackTypeModule(short param1)
        {
            typeValue = param1;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(typeValue);
            param1.writeShort(-6647);
            return param1.Message.ToArray();
        }
    }
}
