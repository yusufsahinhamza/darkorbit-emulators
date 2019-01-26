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
        public const short ID = 24;

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
        public List<int> keyCodes = new List<int>();
        public int parameter = 0;
        public int charCode = 0;

        public UserKeyBindingsModule() { }

        public UserKeyBindingsModule(short actionType, List<int> keyCodes, int parameter, int charCode)
        {
            this.actionType = actionType;
            this.keyCodes = keyCodes;
            this.parameter = parameter;
            this.charCode = charCode;
        }

        public void readCommand(ByteParser param1)
        {
            this.actionType = param1.readShort();
            int _loc2_ = 0;
            int _loc3_ = param1.readInt();
            while (_loc2_ < _loc3_)
            {
                this.keyCodes.Add(param1.readInt());
                _loc2_++;
            }
            this.parameter = param1.readInt();
            this.charCode = param1.readShort();
        }


        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.actionType);
            param1.writeInt(this.keyCodes.Count);
            foreach(var i in this.keyCodes)
            {
                param1.writeInt(i);
            }
            param1.writeInt(this.parameter);
            param1.writeShort((short)this.charCode);
            return param1.Message.ToArray();
        }
    }
}
