using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class ShipWarpModule
    {
        public const short ID = 26497;

        public int shipId = 0;
        public int shipTypeId = 0;
        public String shipDesignName = "";
        public int uridiumPrice = 0;
        public int voucherPrice = 0;
        public int hangarId = 0;
        public String hangarName = "";

        public ShipWarpModule(int shipId, int shipTypeId, string shipDesignName, int uridiumPrice, int voucherPrice, int hangarId, string hangarName)
        {
            this.shipId = shipId;
            this.shipTypeId = shipTypeId;
            this.shipDesignName = shipDesignName;
            this.uridiumPrice = uridiumPrice;
            this.voucherPrice = voucherPrice;
            this.hangarId = hangarId;
            this.hangarName = hangarName;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(shipId);
            param1.writeInt(shipTypeId);
            param1.writeUTF(shipDesignName);
            param1.writeInt(uridiumPrice);
            param1.writeInt(voucherPrice);
            param1.writeInt(hangarId);
            param1.writeUTF(hangarName);
            return param1.Message.ToArray();
        }
    }
}
