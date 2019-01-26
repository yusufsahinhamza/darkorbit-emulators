using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UserKeyBindingsModule
    {
        public const short ID = 423;

        public short actionType = 0;
        public int charCode = 0;
        public List<int> keyCodes = new List<int>();
        public int parameter = 0;

        public UserKeyBindingsModule(short param1, List<int> param2, int param3, int param4)
        {
            actionType = param1;
            keyCodes = param2;
            parameter = param3;
            charCode = param4;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.actionType);
            param1.writeShort((short)this.charCode);
            param1.writeShort(-20216);
            param1.writeInt(this.keyCodes.Count);
            foreach(var i in this.keyCodes)
            {
                param1.writeInt(i >> 5 | i << 27);
            }
            param1.writeInt(this.parameter << 13 | this.parameter >> 19);
            return param1.Message.ToArray();
        }
    }
}
