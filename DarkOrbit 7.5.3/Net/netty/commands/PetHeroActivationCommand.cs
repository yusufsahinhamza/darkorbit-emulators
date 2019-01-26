using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetHeroActivationCommand
    {
        public const short ID = 31487;

        public static byte[] write(int ownerId, int petId, short shipType, short expansionStage, string petName, short factionId, int clanId, short petLevel, string clanTag, int x, int y, int petSpeed)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(ownerId);
            param1.writeInt(petId);
            param1.writeShort(shipType);
            param1.writeShort(expansionStage);
            param1.writeUTF(petName);
            param1.writeShort(factionId);
            param1.writeInt(clanId);
            param1.writeShort(petLevel);
            param1.writeUTF(clanTag);
            param1.writeInt(x);
            param1.writeInt(y);
            param1.writeInt(petSpeed);
            return param1.ToByteArray();
        }
    }
}
