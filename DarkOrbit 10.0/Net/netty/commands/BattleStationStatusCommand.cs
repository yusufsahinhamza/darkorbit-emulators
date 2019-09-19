using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationStatusCommand
    {
        public const short ID = 21495;

        public int mapAssetId = 0;
      
        public int battleStationId = 0;
      
        public String battleStationName = "";
      
        public Boolean deflectorShieldActive = false;
        
        public int deflectorShieldSeconds = 0;
      
        public int deflectorShieldSecondsMax = 0;
      
        public int attackRating = 0;
      
        public int defenceRating = 0;
      
        public int repairRating = 0;
      
        public int honorBoosterRating = 0;
      
        public int experienceBoosterRating = 0;
       
        public int damageBoosterRating = 0;
         
        public int deflectorShieldRate = 0;
      
        public int repairPrice = 0;
      
        public EquippedModulesModule equipment;

        public BattleStationStatusCommand(int mapAssetId, int battleStationId, string battleStationName, bool deflectorShieldActive, int deflectorShieldSeconds, int deflectorShieldSecondsMax, int attackRating, int defenceRating, int repairRating, int honorBoosterRating, int experienceBoosterRating, int damageBoosterRating, int deflectorShieldRate, int repairPrice, EquippedModulesModule equipment)
        {
            this.mapAssetId = mapAssetId;
            this.battleStationId = battleStationId;
            this.battleStationName = battleStationName;
            this.deflectorShieldActive = deflectorShieldActive;
            this.deflectorShieldSeconds = deflectorShieldSeconds;
            this.deflectorShieldSecondsMax = deflectorShieldSecondsMax;
            this.attackRating = attackRating;
            this.defenceRating = defenceRating;
            this.repairRating = repairRating;
            this.honorBoosterRating = honorBoosterRating;
            this.experienceBoosterRating = experienceBoosterRating;
            this.damageBoosterRating = damageBoosterRating;
            this.deflectorShieldRate = deflectorShieldRate;
            this.repairPrice = repairPrice;
            this.equipment = equipment;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(deflectorShieldRate >> 9 | deflectorShieldRate << 23);
            param1.writeInt(deflectorShieldSecondsMax << 13 | deflectorShieldSecondsMax >> 19);
            param1.writeInt(attackRating << 14 | attackRating >> 18);
            param1.writeInt(repairRating >> 6 | repairRating << 26);
            param1.writeInt(defenceRating >> 9 | defenceRating << 23);
            param1.writeInt(battleStationId >> 13 | battleStationId << 19);
            param1.writeShort(23737);
            param1.writeInt(mapAssetId >> 10 | mapAssetId << 22);
            param1.writeInt(damageBoosterRating << 3 | damageBoosterRating >> 29);
            param1.writeInt(repairPrice >> 14 | repairPrice << 18);
            param1.write(equipment.write());
            param1.writeInt(deflectorShieldSeconds << 8 | deflectorShieldSeconds >> 24);
            param1.writeUTF(battleStationName);
            param1.writeInt(honorBoosterRating << 4 | honorBoosterRating >> 28);
            param1.writeBoolean(deflectorShieldActive);
            param1.writeBoolean(deflectorShieldActive);
            param1.writeShort(16248);
            param1.writeInt(experienceBoosterRating << 3 | experienceBoosterRating >> 29);
            return param1.Message.ToArray();
        }

        public byte[] writeCommand()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(deflectorShieldRate >> 9 | deflectorShieldRate << 23);
            param1.writeInt(deflectorShieldSecondsMax << 13 | deflectorShieldSecondsMax >> 19);
            param1.writeInt(attackRating << 14 | attackRating >> 18);
            param1.writeInt(repairRating >> 6 | repairRating << 26);
            param1.writeInt(defenceRating >> 9 | defenceRating << 23);
            param1.writeInt(battleStationId >> 13 | battleStationId << 19);
            param1.writeShort(23737);
            param1.writeInt(mapAssetId >> 10 | mapAssetId << 22);
            param1.writeInt(damageBoosterRating << 3 | damageBoosterRating >> 29);
            param1.writeInt(repairPrice >> 14 | repairPrice << 18);
            param1.write(equipment.write());
            param1.writeInt(deflectorShieldSeconds << 8 | deflectorShieldSeconds >> 24);
            param1.writeUTF(battleStationName);
            param1.writeInt(honorBoosterRating << 4 | honorBoosterRating >> 28);
            param1.writeBoolean(deflectorShieldActive);
            param1.writeBoolean(deflectorShieldActive);
            param1.writeShort(16248);
            param1.writeInt(experienceBoosterRating << 3 | experienceBoosterRating >> 29);
            return param1.ToByteArray();
        }
    }
}
