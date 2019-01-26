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
        public const short ID = 28595;

        public static byte[] write(int shieldSkillId, int minSkinShieldTwinkle, int maxSkinShieldTwinkle)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(shieldSkillId);
            param1.writeInt(minSkinShieldTwinkle);
            param1.writeInt(maxSkinShieldTwinkle);
            return param1.ToByteArray();
        }
    }
}
