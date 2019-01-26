using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MapAssetActionAvailableCommand
    {
        public static short OFF = 1;
        public static short ON = 0;

        public static short ID = 30787;

        public static byte[] write(int assetId, short state, bool activatable, ClientUITooltipsCommand toolTip, class_h45 param5)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(param5.write());
            param1.writeShort(state);
            param1.writeInt(assetId << 6 | assetId >> 26);
            param1.writeBoolean(activatable);
            param1.write(toolTip.write());
            param1.writeShort(-10810);
            param1.writeShort(19301);
            return param1.ToByteArray();
        }
    }
}
