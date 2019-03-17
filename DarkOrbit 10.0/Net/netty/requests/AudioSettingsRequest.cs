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
        public const short ID = 32621;

        public int voice = 0;
        public int music = 0;
        public bool playCombatMusic = false;
        public int sound = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            playCombatMusic = parser.readBoolean();
            parser.readShort();
            voice = parser.readInt();
            voice = (int)(((uint)voice << 2) | ((uint)voice >> 30));
            music = parser.readInt();
            music = (int)(((uint)music << 1) | ((uint)music >> 31));
            sound = parser.readInt();
            sound = (int)(((uint)sound << 10) | ((uint)sound >> 22));
        }
    }
}
