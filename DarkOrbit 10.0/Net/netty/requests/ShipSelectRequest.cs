using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class ShipSelectRequest
    {
        public const short ID = 18091;

        public int targetID, name_95, name_13, posX, posY;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            name_13 = parser.readInt();
            name_13 = name_13 << 5 | name_13 >> 27;
            posX = parser.readInt();
            posX = posX << 12 | posX >> 20;
            posY = parser.readInt();
            posY = posY >> 14 | posY << 18;
            targetID = parser.readInt();
            targetID = (int)(((uint)targetID) >> 14 | ((uint)targetID << 18));
            name_95 = parser.readInt();
            name_95 = name_95 >> 7 | name_95 << 25;
        }
    }
}
