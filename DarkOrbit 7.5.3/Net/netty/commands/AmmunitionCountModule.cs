using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AmmunitionCountModule
    {
        public const short ID = 14644;

        public AmmunitionTypeModule type;
        public double amount;

        public AmmunitionCountModule(AmmunitionTypeModule type, double amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(type.write());
            param1.writeDouble(this.amount);
            return param1.Message.ToArray();
        }
    }
}
