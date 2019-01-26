using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerHadesGateModule : command_i3O
    {
        public const short ID = 32090;

        public int wave = 0;    
        public Boolean active = false;

        public GroupPlayerHadesGateModule(bool active, int wave)
        {
            this.active = active;
            this.wave = wave;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeInt(this.wave >> 8 | this.wave << 24);
            param1.writeBoolean(this.active);
            return param1.Message.ToArray();
        }
    }
}
