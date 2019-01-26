using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UserSettingsCommand
    {
        public const short ID = 18381;

        public static byte[] write(QualitySettingsModule qualitySettingsModule, DisplaySettingsModule displaySettingsModule, AudioSettingsModule audioSettingsModule,
            WindowSettingsModule windowSettingsModule, GameplaySettingsModule gameplaySettingsModule, class_y2t y2t)
        {
            var param1 = new ByteArray(ID);
            param1.write(qualitySettingsModule.write());
            param1.write(y2t.write());
            param1.write(displaySettingsModule.write());
            param1.write(windowSettingsModule.write());
            param1.write(gameplaySettingsModule.write());
            param1.writeShort(-23775);
            param1.write(audioSettingsModule.write());
            return param1.ToByteArray();
        }
    }
}
