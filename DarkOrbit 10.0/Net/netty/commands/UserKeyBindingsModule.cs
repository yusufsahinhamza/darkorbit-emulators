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

        public const short JUMP = 0;
        public const short CHANGE_CONFIG = 1;
        public const short ACTIVATE_LASER = 2;
        public const short LAUNCH_ROCKET = 3;
        public const short PET_ACTIVATE = 4;
        public const short PET_GUARD_MODE = 5;
        public const short LOGOUT = 6;
        public const short QUICKSLOT = 7;
        public const short QUICKSLOT_PREMIUM = 8;
        public const short TOGGLE_WINDOWS = 9;
        public const short PERFORMANCE_MONITORING = 10;
        public const short ZOOM_IN = 11;
        public const short ZOOM_OUT = 12;
        public const short PET_REPAIR_SHIP = 13;

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

        public UserKeyBindingsModule() { }

        public void read(ByteParser parser)
        {
            actionType = parser.readShort();
            charCode = parser.readShort();
            parser.readShort();
            int i = 0;
            int length = parser.readInt();
            while (i < length)
            {
                int keyCode = parser.readInt();
                keyCode = (int)(((uint)keyCode << 5) | ((uint)keyCode >> 27));
                this.keyCodes.Add(keyCode);
                i++;
            }
            parameter = parser.readInt();
            parameter = (int)(((uint)parameter >> 13) | ((uint)parameter << 19));
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
