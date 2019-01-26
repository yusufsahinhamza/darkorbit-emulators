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
        public const short ID = 27211;

        public static byte[] write(int petId, int petLevel, double petExperiencePoints, double petExperiencePointsUntilNextLevel, int petHitPoints, int petHitPointsMax, int petShieldEnergyNow, int petShieldEnergyMax, int petCurrentFuel, int petMaxFuel, int petSpeed, string petName)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(petId);
            param1.writeInt(petLevel);
            param1.writeDouble(petExperiencePoints);
            param1.writeDouble(petExperiencePointsUntilNextLevel);
            param1.writeInt(petHitPoints);
            param1.writeInt(petHitPointsMax);
            param1.writeInt(petShieldEnergyNow);
            param1.writeInt(petShieldEnergyMax);
            param1.writeInt(petCurrentFuel);
            param1.writeInt(petMaxFuel);
            param1.writeInt(petSpeed);
            param1.writeUTF(petName);
            return param1.ToByteArray();
        }
    }
}
