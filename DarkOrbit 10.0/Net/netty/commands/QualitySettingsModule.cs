using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class QualitySettingsModule
    {
        public const short ID = 21137;

        public Boolean notSet = false;
      
        public short qualityAttack = 0;
      
        public short qualityBackground = 0;
      
        public short qualityPresetting = 0;
      
        public Boolean qualityCustomized = false;
      
        public short qualityPOIzone = 0;
      
        public short qualityShip = 0;
      
        public short qualityEngine = 0;
      
        public short qualityExplosion = 0;
      
        public short qualityCollectables = 0;
      
        public short qualityEffect = 0;

        public QualitySettingsModule(bool param1 = false, short param2 = 0, short param3 = 0, short param4 = 0, bool param5 = false, short param6 = 0, short param7 = 0, short param8 = 0, short param9 = 0, short param10 = 0, short param11 = 0)
        {
            this.notSet = param1;
            this.qualityAttack = param2;
            this.qualityBackground = param3;
            this.qualityPresetting = param4;
            this.qualityCustomized = param5;
            this.qualityPOIzone = param6;
            this.qualityShip = param7;
            this.qualityEngine = param8;
            this.qualityExplosion = param9;
            this.qualityCollectables = param10;
            this.qualityEffect = param11;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(15684);
            param1.writeShort(qualityEffect);
            param1.writeShort(qualityPresetting);
            param1.writeShort(qualityBackground);
            param1.writeShort(qualityAttack);
            param1.writeBoolean(qualityCustomized);
            param1.writeBoolean(notSet);
            param1.writeShort(qualityShip);
            param1.writeShort(qualityExplosion);
            param1.writeShort(qualityEngine);
            param1.writeShort(qualityPOIzone);
            param1.writeShort(8709);
            param1.writeShort(qualityCollectables);
            return param1.Message.ToArray();
        }
    }
}
