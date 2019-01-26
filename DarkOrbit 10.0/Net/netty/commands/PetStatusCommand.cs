using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetStatusCommand
    {
        public const short ID = 22037;

        public static byte[] write(int petId, int petLevel, double petExperiencePoints, double petExperiencePointsUntilNextLevel, int petHitPoints, int petHitPointsMax, int petShieldEnergyNow, int petShieldEnergyMax, int petCurrentFuel, int petMaxFuel, int petSpeed, string petName)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(petHitPoints << 15 | petHitPoints >> 17);
            param1.writeInt(petMaxFuel << 5 | petMaxFuel >> 27);
            param1.writeInt(petId << 13 | petId >> 19);
            param1.writeInt(petLevel << 7 | petLevel >> 25);
            param1.writeInt(petShieldEnergyNow >> 3 | petShieldEnergyNow << 29);
            param1.writeDouble(petExperiencePointsUntilNextLevel);
            param1.writeInt(petShieldEnergyMax << 2 | petShieldEnergyMax >> 30);
            param1.writeUTF(petName);
            param1.writeInt(petSpeed >> 9 | petSpeed << 23);
            param1.writeInt(petCurrentFuel >> 11 | petCurrentFuel << 21);
            param1.writeDouble(petExperiencePoints);
            param1.writeInt(petHitPointsMax << 4 | petHitPointsMax >> 28);
            return param1.ToByteArray();
        }
    }
}
