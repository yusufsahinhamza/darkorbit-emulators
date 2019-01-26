using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class StationModuleModule
    {
        public const short ID = 8468;

        public const short NONE = 0;
        public const short DESTROYED = 1;
        public const short HULL = 2;
        public const short DEFLECTOR = 3;
        public const short REPAIR = 4;
        public const short LASER_HIGH_RANGE = 5;
        public const short LASER_MID_RANGE = 6;
        public const short LASER_LOW_RANGE = 7;
        public const short ROCKET_MID_ACCURACY = 8;
        public const short ROCKET_LOW_ACCURACY = 9;
        public const short HONOR_BOOSTER = 10;
        public const short DAMAGE_BOOSTER = 11;
        public const short EXPERIENCE_BOOSTER = 12;

        public int emergencyRepairSecondsLeft = 0;     
        public int currentShield = 0;     
        public int emergencyRepairSecondsTotal = 0;      
        public int upgradeLevel = 0;      
        public int emergencyRepairCost = 0;      
        public String ownerName = "";      
        public int installationSeconds = 0;      
        public int slotId = 0;     
        public short type = 0;      
        public int currentHitpoints = 0;      
        public int itemId = 0;      
        public int maxHitpoints = 0;     
        public int asteroidId = 0;
        public int maxShield = 0;     
        public int installationSecondsLeft = 0;

        public StationModuleModule(int asteroidId, int itemId, int slotId, short type, int currentHitpoints, int maxHitpoints, int currentShield, int maxShield, int upgradeLevel, string ownerName, int installationSeconds, int installationSecondsLeft, int emergencyRepairSecondsLeft, int emergencyRepairSecondsTotal, int emergencyRepairCost)
        {
            this.asteroidId = asteroidId;
            this.itemId = itemId;
            this.slotId = slotId;
            this.type = type;
            this.currentHitpoints = currentHitpoints;
            this.maxHitpoints = maxHitpoints;
            this.currentShield = currentShield;
            this.maxShield = maxShield;
            this.upgradeLevel = upgradeLevel;
            this.ownerName = ownerName;
            this.installationSeconds = installationSeconds;
            this.installationSecondsLeft = installationSecondsLeft;
            this.emergencyRepairSecondsLeft = emergencyRepairSecondsLeft;
            this.emergencyRepairSecondsTotal = emergencyRepairSecondsTotal;
            this.emergencyRepairCost = emergencyRepairCost;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(emergencyRepairSecondsLeft << 4 | emergencyRepairSecondsLeft >> 28);
            param1.writeInt(currentShield << 12 | currentShield >> 20);
            param1.writeInt(emergencyRepairSecondsTotal >> 7 | emergencyRepairSecondsTotal << 25);
            param1.writeInt(upgradeLevel << 14 | upgradeLevel >> 18);
            param1.writeInt(emergencyRepairCost >> 15 | emergencyRepairCost << 17);
            param1.writeUTF(ownerName);
            param1.writeInt(installationSeconds << 2 | installationSeconds >> 30);
            param1.writeShort(-29366);
            param1.writeInt(slotId << 2 | slotId >> 30);
            param1.writeShort(type);
            param1.writeInt(currentHitpoints << 2 | currentHitpoints >> 30);
            param1.writeInt(itemId << 9 | itemId >> 23);
            param1.writeInt(maxHitpoints << 11 | maxHitpoints >> 21);
            param1.writeInt(asteroidId >> 16 | asteroidId << 16);
            param1.writeShort(19020);
            param1.writeInt(maxShield << 6 | maxShield >> 26);
            param1.writeInt(installationSecondsLeft << 16 | installationSecondsLeft >> 16);
            return param1.Message.ToArray();
        }
    }
}
