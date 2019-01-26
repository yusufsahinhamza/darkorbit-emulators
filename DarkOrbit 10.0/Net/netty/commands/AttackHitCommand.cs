using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttackHitCommand
    {
        public const short ID = 3758;

        public static byte[] write(AttackTypeModule attackTypeValue, int attackerId, int targetId, int targetHitpoints, int targetShield, int targetNanoHull, int damage, bool skilled)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(targetNanoHull << 2 | targetNanoHull >> 30);
            param1.writeInt(attackerId >> 13 | attackerId << 19);
            param1.writeBoolean(skilled);
            param1.write(attackTypeValue.write());
            param1.writeInt(targetId << 15 | targetId >> 17);
            param1.writeInt(damage >> 2 | damage << 30);
            param1.writeInt(targetShield >> 15 | targetShield << 17);
            param1.writeInt(targetHitpoints << 9 | targetHitpoints >> 23);
            return param1.ToByteArray();
        }
    }
}
