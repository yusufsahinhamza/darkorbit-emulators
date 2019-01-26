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
        public const short ID = 6417;

        public int positionX;
        public int targetX;
        public int targetY;
        public int positionY;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.positionX = param1.readInt();
            this.targetY = param1.readInt();
            this.targetX = param1.readInt();
            this.positionY = param1.readInt();
        }
    }
}
