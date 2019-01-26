using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupInvitationBehaviorModule
    {
        public const short ID = 19280;

        public const short ON = 1;    
        public const short OFF = 0;

        public short mode = 0;

        public GroupInvitationBehaviorModule(short mode)
        {
            this.mode = mode;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(22289);
            param1.writeShort(this.mode);
            return param1.Message.ToArray();
        }
    }
}
