using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class MoveRequest
    {
        public const short ID = 17063;

        public int targetY;
        public int positionX;
        public int targetX;
        public int positionY;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            targetY = parser.readInt();
            targetY = (int)(((uint)targetY << 8) | ((uint)targetY >> 24));
            positionX = parser.readInt();
            positionX = (int)(((uint)positionX << 15) | ((uint)positionX >> 17));
            parser.readShort();
            targetX = parser.readInt();
            targetX = (int)(((uint)targetX >> 4) | ((uint)targetX << 28));
            positionY = parser.readInt();
            positionY = (int)(((uint)positionY << 5) | ((uint)positionY >> 27));
        }
    }
}
