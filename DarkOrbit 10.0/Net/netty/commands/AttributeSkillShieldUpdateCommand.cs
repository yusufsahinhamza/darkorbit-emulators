using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttributeSkillShieldUpdateCommand
    {
        public const short ID = 4062;

        public static byte[] write(int shieldSkillId, int minSkinShieldTwinkle, int maxSkinShieldTwinkle)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(shieldSkillId >> 14 | shieldSkillId << 18);
            param1.writeInt(maxSkinShieldTwinkle << 7 | maxSkinShieldTwinkle >> 25);
            param1.writeInt(minSkinShieldTwinkle << 1 | minSkinShieldTwinkle >> 31);
            return param1.ToByteArray();
        }
    }
}
