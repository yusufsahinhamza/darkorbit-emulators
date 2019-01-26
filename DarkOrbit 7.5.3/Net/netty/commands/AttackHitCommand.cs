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
        public const short ID = 27342;

        public static byte[] write(AttackTypeModule attackType, int attackerId, int targetId, int targetHitpoints, int targetShield, int targetNanoHull, int damage, bool skilled)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(attackType.write());
            param1.writeInt(attackerId);
            param1.writeInt(targetId);
            param1.writeInt(targetHitpoints);
            param1.writeInt(targetShield);
            param1.writeInt(targetNanoHull);
            param1.writeInt(damage);
            param1.writeBoolean(skilled);
            return param1.ToByteArray();
        }
    }
}
