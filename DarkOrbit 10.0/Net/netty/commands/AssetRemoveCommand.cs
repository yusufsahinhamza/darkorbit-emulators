using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AssetRemoveCommand
    {
        public const short ID = 3397;

        public static byte[] write(AssetTypeModule assetType, int uid)
        {
            var param1 = new ByteArray(ID);
            param1.write(assetType.write());
            param1.writeShort(-17782);
            param1.writeInt(uid << 15 | uid >> 17);
            return param1.ToByteArray();
        }
    }
}
