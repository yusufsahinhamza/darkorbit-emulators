using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttackMissedCommand
    {
        public const short ID = 9360;

        public static byte[] write(AttackTypeModule attackType, int targetUserId, int skillColorId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(skillColorId >> 10 | skillColorId << 22);
            param1.write(attackType.write());
            param1.writeInt(targetUserId >> 4 | targetUserId << 28);
            return param1.ToByteArray();
        }
    }
}
