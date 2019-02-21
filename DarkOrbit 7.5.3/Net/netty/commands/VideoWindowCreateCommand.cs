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
        public const short ID = 11048;

        public static short HELPMOVIE = 0;    
        public static short COMMANDER = 1;

        public static byte[] write(int windowID, string windowAlign, bool showButtons, List<string> languageKeys, int videoID, short videoType)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(windowID);
            param1.writeUTF(windowAlign);
            param1.writeBoolean(showButtons);
            param1.writeInt(languageKeys.Count);
            foreach(var _loc2_ in languageKeys)
            {
                param1.writeUTF(_loc2_);
            }
            param1.writeInt(videoID);
            param1.writeShort(videoType);
            return param1.ToByteArray();
        }
    }
}
