using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class AssetHandleClickRequest
    {
        public const short ID = 15665;

        public int AssetId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            AssetId = parser.readInt();
            AssetId = (int)(((uint)AssetId) << 15 | ((uint)AssetId >> 17));
        }
    }
}
