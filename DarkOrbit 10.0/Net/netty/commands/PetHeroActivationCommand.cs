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
        public const short ID = 32409;

        public static byte[] write(int ownerId, int petId, short shipType, short expansionStage, string petName, short factionId, int clanId, short petLevel, string clanTag, int x, int y, int petSpeed, class_11d varzL)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(x >> 4 | x << 28);
            param1.writeShort(factionId);
            param1.writeShort(petLevel);
            param1.write(varzL.write());
            param1.writeInt(y >> 6 | y << 26);
            param1.writeShort(-19144);
            param1.writeInt(petId << 13 | petId >> 19);
            param1.writeInt(ownerId << 2 | ownerId >> 30);
            param1.writeUTF(petName);
            param1.writeUTF(clanTag);
            param1.writeShort(expansionStage);
            param1.writeShort(shipType);
            param1.writeInt(clanId >> 12 | clanId << 20);
            param1.writeInt(petSpeed >> 1 | petSpeed << 31);
            return param1.ToByteArray();
        }
    }
}
