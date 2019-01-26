using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class QualitySettingsRequest
    {
        public const short ID = 26693;

        public short qualityEngine = 0;
        public short qualityEffect = 0;
        public bool qualityCustomized = false;
        public short qualityCollectable = 0;
        public short qualityPoizone = 0;
        public short qualityPresetting = 0;
        public short qualityBackground = 0;
        public short qualityAttack = 0;
        public short qualityExplosion = 0;
        public short qualityShip = 0;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.qualityAttack = param1.readShort();
            this.qualityBackground = param1.readShort();
            this.qualityPresetting = param1.readShort();
            this.qualityCustomized = param1.readBoolean();
            this.qualityPoizone = param1.readShort();
            this.qualityShip = param1.readShort();
            this.qualityEngine = param1.readShort();
            this.qualityExplosion = param1.readShort();
            this.qualityCollectable = param1.readShort();
            this.qualityEffect = param1.readShort();
        }
    }
}
