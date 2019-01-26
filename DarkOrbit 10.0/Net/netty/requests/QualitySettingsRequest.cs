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
        public const short ID = 20038;

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
            var parser = new ByteParser(bytes);
            qualityCollectable = parser.readShort();
            qualityEffect = parser.readShort();
            qualityExplosion = parser.readShort();
            qualityBackground = parser.readShort();
            qualityCustomized = parser.readBoolean();
            qualityShip = parser.readShort();
            qualityEngine = parser.readShort();
            qualityPresetting = parser.readShort();
            qualityAttack = parser.readShort();
            qualityPoizone = parser.readShort();
        }
    }
}
