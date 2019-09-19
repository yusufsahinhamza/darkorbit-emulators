using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Mines
{
    class SLM_01 : Mine
    {
        public SLM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId) { }

        public override void Action(Player player)
        {
            if (player.Attackable())
            {
                player.Storage.underSLM_01 = true;
                player.Storage.underSLM_01Time = DateTime.Now;
                player.SendPacket("0|n|fx|start|SABOTEUR_DEBUFF|" + player.Id + "");
                player.SendPacketToInRangePlayers("0|n|fx|start|SABOTEUR_DEBUFF|" + player.Id + "");
                player.SendCommand(SetSpeedCommand.write(player.Speed, player.Speed));
            }
        }
    }
}
