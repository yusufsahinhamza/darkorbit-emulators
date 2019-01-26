using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MapAddPOICommand
    {
        public const short ID = 578;

        public const short CIRCLE = 0;     
        public const short POLYGON = 1;      
        public const short RECTANGLE = 2;

        public static byte[] write(string poiId, POITypeModule poiType, string poiTypeSpecification, POIDesignModule design, short shape, List<int> shapeCoordinates, bool inverted, bool active)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(poiId);
            param1.write(poiType.write());
            param1.writeUTF(poiTypeSpecification);
            param1.write(design.write());
            param1.writeShort(shape);
            param1.writeInt(shapeCoordinates.Count);
            foreach(var _loc2_ in shapeCoordinates)
            {
                param1.writeInt(_loc2_);
            }
            param1.writeBoolean(inverted);
            param1.writeBoolean(active);
            return param1.ToByteArray();
        }
    }
}
