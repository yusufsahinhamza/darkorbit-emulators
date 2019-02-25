using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetExperiencePointsUpdateCommand
    {
        public const short ID = 8216;

        public static byte[] write(int currentExperiencePoints, int maxExperiencePoints)
        {
            var param1 = new ByteArray(ID);
            param1.writeDouble(maxExperiencePoints);
            param1.writeShort(-31735);
            param1.writeDouble(currentExperiencePoints);
            return param1.ToByteArray();
        }
    }
}
