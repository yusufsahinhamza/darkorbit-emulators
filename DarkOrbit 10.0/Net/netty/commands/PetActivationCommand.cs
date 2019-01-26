using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetActivationCommand
    {
        public const short ID = 21825;

        public static byte[] write(int ownerId, int petId, short petDesignId, short expansionStage, string petName, short petFactionId, int petClanId, short petLevel, string clanTag, ClanRelationModule clanRelationship, int x, int y, int petSpeed, bool isInIdleMode, bool isVisible, class_11d varzL)
        {
            var param1 = new ByteArray(ID);
            param1.write(clanRelationship.write());
            param1.writeShort(petDesignId);
            param1.write(varzL.write());
            param1.writeShort(expansionStage);
            param1.writeInt(petSpeed >> 5 | petSpeed << 27);
            param1.writeUTF(petName);
            param1.writeInt(x << 5 | x >> 27);
            param1.writeBoolean(isInIdleMode);
            param1.writeShort(-20860);
            param1.writeInt(petClanId << 2 | petClanId >> 30);
            param1.writeInt(y >> 15 | y << 17);
            param1.writeInt(petId >> 7 | petId << 25);
            param1.writeInt(ownerId >> 16 | ownerId << 16);
            param1.writeBoolean(isVisible);
            param1.writeUTF(clanTag);
            param1.writeShort(petLevel);
            param1.writeShort(petFactionId);
            return param1.ToByteArray();
        }
    }
}
