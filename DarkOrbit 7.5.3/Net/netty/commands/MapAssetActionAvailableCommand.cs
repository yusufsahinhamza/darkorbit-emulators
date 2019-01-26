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

        public static short ID = 18101;

        public static byte[] write(int mapAssetId, short state)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(mapAssetId);
            param1.writeShort(state);
            return param1.ToByteArray();
        }
    }
}
