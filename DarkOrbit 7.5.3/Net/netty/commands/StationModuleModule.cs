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
        public const short ID = 30874;

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
            param1.writeInt(asteroidId);
            param1.writeInt(itemId);
            param1.writeInt(slotId);
            param1.writeShort(type);
            param1.writeInt(currentHitpoints);
            param1.writeInt(maxHitpoints);
            param1.writeInt(currentShield);
            param1.writeInt(maxShield);
            param1.writeInt(upgradeLevel);
            param1.writeUTF(ownerName);
            param1.writeInt(installationSeconds);
            param1.writeInt(installationSecondsLeft);
            param1.writeInt(emergencyRepairSecondsLeft);
            param1.writeInt(emergencyRepairSecondsTotal);
            param1.writeInt(emergencyRepairCost);
            return param1.Message.ToArray();
        }
    }
}
