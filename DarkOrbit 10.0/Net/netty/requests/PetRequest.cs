using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class PetRequest
    {
        public const short ID = 645;

        public const short LAUNCH = 0;
        public const short DEACTIVATE = 1;
        public const short TOGGLE_ACTIVATION = 2;
        public const short HOTKEY_GUARD_MODE = 3;
        public const short REPAIR_DESTROYED_PET = 4;
        public const short HOTKEY_REPAIR_SHIP = 5;

        public short petRequestType;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            petRequestType = parser.readShort();
        }
    }
}
