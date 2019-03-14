using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class SlotBarConfigSetRequest
    {
        public const short ID = 18681;

        public int FromIndex;
        public String ItemId;
        public String FromSlotBarId;
        public int ToIndex;
        public String ToSlotBarId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            FromIndex = parser.readInt();
            FromIndex = (int)(((uint)FromIndex) >> 11 | ((uint)FromIndex << 21));
            ToSlotBarId = parser.readUTF();
            ToIndex = parser.readInt();
            ToIndex = (int)(((uint)ToIndex) >> 2 | ((uint)ToIndex << 30));
            ItemId = parser.readUTF();
            FromSlotBarId = parser.readUTF();
        }
    }
}
