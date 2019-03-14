using Ow.Game;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class RepairStationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            foreach (var station in player.Spacemap.Activatables.Values)
            {
                var inRangeStations = player.Storage.InRangeAssets;
                if (inRangeStations.ContainsKey(station.Id)) continue;

                if (station is RepairStation)
                    station.Click(gameSession);
            }
        }
    }
}
