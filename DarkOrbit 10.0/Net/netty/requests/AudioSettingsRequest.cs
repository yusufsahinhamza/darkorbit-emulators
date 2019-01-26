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
            voice = voice << 2 | voice >> 30;
            music = parser.readInt();
            music = music << 1 | music >> 31;
            sound = parser.readInt();
            sound = sound << 10 | sound >> 22;
        }
    }
}
