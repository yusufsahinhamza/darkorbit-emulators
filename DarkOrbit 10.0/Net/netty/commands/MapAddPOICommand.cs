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
        public const short ID = 7240;

        public const short CIRCLE = 0;     
        public const short POLYGON = 1;      
        public const short RECTANGLE = 2;

        public static byte[] write(string poiId, POITypeModule poiType, string poiTypeSpecification, POIDesignModule design, short shape, List<int> shapeCoordinates, bool inverted, bool active)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(shape);
            param1.writeUTF(poiId);
            param1.writeUTF(poiTypeSpecification);
            param1.write(poiType.write());
            param1.writeBoolean(active);
            param1.writeShort(10959);
            param1.writeBoolean(inverted);
            param1.writeInt(shapeCoordinates.Count);
            foreach(var coordinat in shapeCoordinates)
            {
                param1.writeInt(coordinat >> 7 | coordinat << 25);
            }
            param1.write(design.write());
            return param1.ToByteArray();
        }
    }
}
