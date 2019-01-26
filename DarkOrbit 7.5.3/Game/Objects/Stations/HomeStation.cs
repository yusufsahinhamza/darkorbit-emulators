using Ow.Game.Clans;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations
{
    class StationBase
    {
        public int TypeId { get; set; }
        public int FactionId { get; set; }
        public List<int> Position { get; set; }
    }

    class HomeStation : Station
    {
        public static int SECURE_ZONE_RANGE = 1500;

        public HomeStation(Spacemap spacemap, int factionId, Position position) : base(spacemap, factionId, position) { }

        public string GetStationType()
        {
            switch(FactionId)
            {
                case 1:
                    return "redStation";
                case 2:
                    return "blueStation";
                case 3:
                    return "greenStation";
                default:
                    return "";
            }
        }

        public override string GetAssetCreatePacket()
        {
            return "0|s|"+ Id +"|1|"+ GetStationType() + "|"+ FactionId +"|1500|"+ Position.X +"|"+ Position.Y +"";
        }
    }
}
