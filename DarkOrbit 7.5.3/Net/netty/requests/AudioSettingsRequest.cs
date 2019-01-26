using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class AudioSettingsRequest
    {
        public const short ID = 7694;

        public bool sound = false;
        public bool music = false;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.sound = param1.readBoolean();
            this.music = param1.readBoolean();
        }
    }
}
