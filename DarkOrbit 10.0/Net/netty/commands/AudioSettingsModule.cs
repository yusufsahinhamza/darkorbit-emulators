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
        public const short ID = 27262;

        public bool NotSet = false;
        public bool PlayCombatMusic = false;
        public int Voice = 0;
        public int Sound = 0;
        public int Music = 0;

        public AudioSettingsModule(bool pNotSet, int pSound, int pMusic, int pVoice, bool pPlayCombatMusic)
        {
            this.NotSet = pNotSet;
            this.Sound = pSound;
            this.Music = pMusic;
            this.Voice = pVoice;
            this.PlayCombatMusic = pPlayCombatMusic;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-4407);
            param1.writeInt(this.Voice << 9 | this.Voice >> 23);
            param1.writeInt(this.Sound << 7 | this.Sound >> 25);
            param1.writeBoolean(this.PlayCombatMusic);
            param1.writeInt(this.Music >> 4 | this.Music << 28);
            param1.writeShort(-5063);
            param1.writeBoolean(this.NotSet);
            return param1.Message.ToArray();
        }
    }
}
