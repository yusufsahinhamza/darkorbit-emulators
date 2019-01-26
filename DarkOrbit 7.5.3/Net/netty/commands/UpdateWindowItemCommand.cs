using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UpdateWindowItemCommand
    {
        public const short ID = 14317;

        public static byte[] write(bool maximized, int height, bool visible, int varMl, int varq32, ClientUITooltipsCommand toolTip, string varJG, int width, ClientUITooltipsCommand helpText, string itemId)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(11362);
            param1.write(toolTip.write());
            param1.writeUTF(itemId);
            param1.writeBoolean(visible);
            param1.writeInt(height >> 14 | height << 18);
            param1.writeBoolean(maximized);
            param1.writeInt(width >> 2 | width << 30);
            param1.writeUTF(varJG);
            param1.writeShort(3358);
            param1.writeInt(varMl << 14 | varMl >> 18);
            param1.write(helpText.write());
            param1.writeInt(varq32 >> 12 | varq32 << 20);
            return param1.ToByteArray();
        }
    }
}
