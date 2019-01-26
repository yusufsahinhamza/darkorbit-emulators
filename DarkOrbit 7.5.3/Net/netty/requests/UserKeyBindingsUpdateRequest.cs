using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class UserKeyBindingsUpdateRequest
    {
        public const short ID = 23508;

        public bool remove = false;
        public List<UserKeyBindingsModule> changedKeyBindings = new List<UserKeyBindingsModule>();

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);

            int _loc2_ = 0;
            int _loc3_ = param1.readInt();
            while (_loc2_ < _loc3_)
            {
                param1.readShort();
                var _loc4_ = new UserKeyBindingsModule();
                _loc4_.readCommand(param1);
                this.changedKeyBindings.Add(_loc4_);
                _loc2_++;
            }
            this.remove = param1.readBoolean();
        }
    }
}
