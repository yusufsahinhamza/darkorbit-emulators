using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AmmunitionTypeModule
    {
        public const short ID = 15629;

        public const short ROCKET = 0;

        public const short TORPEDO = 1;

        public const short WIZARD = 2;

        public const short PRANK = 3;

        public const short BATTERY = 4;

        public const short FIREWORK = 5;

        public const short FIREWORK_1 = 6;

        public const short FIREWORK_2 = 7;

        public const short FIREWORK_3 = 8;

        public const short MINE = 9;

        public const short MINE_EMP = 10;

        public const short MINE_SAB = 11;

        public const short MINE_DD = 12;

        public const short MINE_SL = 13;

        public const short SMARTBOMB = 14;

        public const short INSTANT_SHIELD = 15;

        public const short PLASMA = 16;

        public const short EMP = 17;

        public const short LASER_AMMO = 18;

        public const short ROCKET_AMMO = 19;

        public const short RSB = 20;

        public const short HELLSTORM = 21;

        public const short UBER_ROCKET = 22;

        public const short ECO_ROCKET = 23;

        public const short SAR01 = 24;

        public const short SAR02 = 25;

        public const short X1 = 26;

        public const short X2 = 27;

        public const short X3 = 28;

        public const short X4 = 29;

        public const short SAB = 30;

        public const short CBO = 31;

        public const short R310 = 32;

        public const short PLT2026 = 33;

        public const short PLT2021 = 34;

        public const short PLT3030 = 35;

        public const short BDR1211 = 36;

        public const short DECELERATION = 37;

        public const short CBR = 38;

        public const short HITAC_LASER = 39;

        public const short JOB100 = 40;

        public const short BDR1212 = 41;

        public short typeValue;

        public AmmunitionTypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(this.typeValue);
            return param1.Message.ToArray();
        }
    }
}
