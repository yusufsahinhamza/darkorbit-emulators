using Ow.Game.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class MoveManager
    {
        public Player Player { get; set; }

        public MoveManager(Player player) { Player = player; SetPosition(); }

        public void SetPosition()
        {
            switch (Player.FactionId)
            {
                case 1:
                    Player.Position = new Position(1600, 1600);
                    break;
                case 2:
                    Player.Position = new Position(19500, 1500);
                    break;
                case 3:
                    Player.Position = new Position(19500, 11600);
                    break;
            }
        }

        public void Tick()
        {
            Movement.ActualPosition(Player);
            Player.Spacemap.OnPlayerMovement(Player);
        }
    }
}
