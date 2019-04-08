using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class Uba047Module : command_NQ
    {
        public const short ID = 7717;

        public int timer = 0;
        public UbaM1tModule var44b { get; set; }

        public Uba047Module(int timer, UbaM1tModule param2)
        {
            this.timer = timer;
            this.var44b = param2;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeInt(this.timer << 10 | this.timer >> 22);
            param1.writeShort(-29785);
            param1.write(var44b.write());
            return param1.Message.ToArray();
        }
    }
}
