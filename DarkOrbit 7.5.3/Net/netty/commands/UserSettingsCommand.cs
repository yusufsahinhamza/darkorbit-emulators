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
        public const short ID = 11343;

        public static byte[] write(QualitySettingsModule qualitySettingsModule, DisplaySettingsModule displaySettingsModule, AudioSettingsModule audioSettingsModule,
            WindowSettingsModule windowSettingsModule, GameplaySettingsModule gameplaySettingsModule)
        {
            var param1 = new ByteArray(ID);
            param1.write(qualitySettingsModule.write());
            param1.write(displaySettingsModule.write());
            param1.write(audioSettingsModule.write());
            param1.write(windowSettingsModule.write());
            param1.write(gameplaySettingsModule.write());
            return param1.ToByteArray();
        }
    }
}
