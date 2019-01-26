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
        public const short ID = 8845;

        public static byte[] write(int ownerId, int petId, short petDesignId, short expansionStage, string petName, short petFactionId, int petClanId, short petLevel, string clanTag, ClanRelationModule clanRelationship, int x, int y, int petSpeed, bool isInIdleMode, bool isVisible, class_11d varzL)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(ownerId);
            param1.writeInt(petId);
            param1.writeShort(petDesignId);
            param1.writeShort(expansionStage);
            param1.writeUTF(petName);
            param1.writeShort(petFactionId);
            param1.writeInt(petClanId);
            param1.writeShort(petLevel);
            param1.writeUTF(clanTag);
            param1.write(clanRelationship.write());
            param1.writeInt(x);
            param1.writeInt(y);
            param1.writeInt(petSpeed);
            param1.writeBoolean(isInIdleMode);
            param1.writeBoolean(isVisible);
            return param1.ToByteArray();
        }
    }
}
