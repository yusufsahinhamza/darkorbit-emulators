using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AudioSettingsModule
    {
        public const short ID = 26805;

        public Boolean notSet = false;
        public Boolean sound = false;
        public Boolean music = false;

        public AudioSettingsModule(Boolean notSet, Boolean sound, Boolean music)
        {
            this.notSet = notSet;
            this.sound = sound;
            this.music = music;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.notSet);
            param1.writeBoolean(this.sound);
            param1.writeBoolean(this.music);
            return param1.Message.ToArray();
        }
    }
}
