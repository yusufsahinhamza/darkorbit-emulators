using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupInitializationCommand
    {
        public const short ID = 2496;

        public static byte[] write(int userId, int varf38, int leaderId, List<GroupPlayerModule> varR3, GroupInvitationBehaviorModule groupInvitationBehaviorModule)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(varf38 << 12 | varf38 >> 20);
            param1.writeInt(varR3.Count);
            foreach(var _loc2_ in varR3)
            {
                param1.write(_loc2_.write());
            }
            param1.writeInt(userId >> 16 | userId << 16);
            param1.write(groupInvitationBehaviorModule.write());
            param1.writeInt(leaderId >> 15 | leaderId << 17);
            return param1.ToByteArray();
        }
    }
}
