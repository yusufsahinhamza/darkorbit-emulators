using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class VideoWindowCreateCommand
    {
        public const short ID = 1271;

        public static byte[] write(int windowID, string windowAlign, bool showButtons, List<string> languageKeys,
            int videoID, short videoType)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(videoID << 11 | videoID >> 21);
            param1.writeBoolean(showButtons);
            param1.writeShort(videoType);
            param1.writeUTF(windowAlign);
            param1.writeInt(languageKeys.Count);
            foreach(var key in languageKeys)
            {
                param1.writeUTF(key);
            }
            param1.writeShort(-12347);
            param1.writeInt(windowID << 11 | windowID >> 21);
            param1.writeShort(1928);
            return param1.ToByteArray();
        }
    }
}
