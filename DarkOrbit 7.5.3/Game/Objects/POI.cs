using Ow.Game.Movements;
using Ow.Net.netty.commands;
using System.Collections.Generic;

namespace Ow.Game.Objects
{
    class POI
    {
        public string Id { get; }
        public POITypes Type { get; set; }
        public POIDesigns Design { get; set; }
        public POIShapes Shape { get; set; }
        public List<Position> ShapeCords { get; set; }
        public bool Inverted { get; set; }
        public string TypeSpecification { get; set; }
        public bool Active { get; set; }

        public POI(string id, POITypes type, POIDesigns design, POIShapes shape, List<Position> shapeCords, bool active = true, bool inverted = false, string poiTypeSpecification = "")
        {
            Id = id;
            Type = type;
            Design = design;
            Shape = shape;
            ShapeCords = shapeCords;
            Inverted = inverted;
            TypeSpecification = poiTypeSpecification;
            Active = active;
        }

        public List<int> ShapeCordsToInts()
        {
            List<int> cords = new List<int>();
            foreach (var cord in ShapeCords)
            {
                cords.Add(cord.X);
                cords.Add(cord.Y);
            }
            return cords;
        }

        public byte[] GetPOICreateCommand()
        {
            return MapAddPOICommand.write(Id, new POITypeModule((short)Type), TypeSpecification, new POIDesignModule((short)Design), (short)Shape, ShapeCordsToInts(), Inverted, Active);
        }
    }
}
