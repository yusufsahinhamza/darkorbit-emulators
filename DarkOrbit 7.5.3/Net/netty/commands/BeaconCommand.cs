using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BeaconCommand
    {
        public const short ID = 21794;

        public static byte[] write(int posx, int posy, Boolean in_peace_area, Boolean repair_robot_on, Boolean in_trade_area,
            Boolean in_radiation_zone, Boolean in_jump_area, int fast_repair_area_and_voucher_count)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(posx);
            param1.writeInt(posy);
            param1.writeBoolean(in_peace_area);
            param1.writeBoolean(repair_robot_on);
            param1.writeBoolean(in_trade_area);
            param1.writeBoolean(in_radiation_zone);
            param1.writeBoolean(in_jump_area);
            param1.writeInt(fast_repair_area_and_voucher_count);
            return param1.ToByteArray();
        }
    }
}
