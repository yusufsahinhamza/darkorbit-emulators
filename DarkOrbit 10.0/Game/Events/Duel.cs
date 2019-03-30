using Ow.Game.Objects;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Game.Ticks;
using Ow.Managers;

namespace Ow.Game.Events
{
    class Duel : Tick
    {
        public int TickId { get; set; }
        public Player Player { get; set; }
        public Player OtherPlayer { get; set; }
        public static Spacemap Spacemap = GameManager.GetSpacemap(101);

        public Duel(Player player, Player otherPlayer)
        {
            Player = player;
            OtherPlayer = otherPlayer;

            Player.Storage.DuelOpponent = OtherPlayer;
            OtherPlayer.Storage.DuelOpponent = Player;

            Player.CpuManager.DisableCloak();
            OtherPlayer.CpuManager.DisableCloak();

            var position1 = new Position(3700, 3200);
            var position2 = new Position(6400, 3200);

            Player.Jump(Spacemap.Id, position1);
            OtherPlayer.Jump(Spacemap.Id, position2);

            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;

            countdown = DateTime.Now;
        }

        public DateTime countdown = new DateTime();
        public DateTime countdownTimer = new DateTime();
        public bool peaceArea = true;
        public bool rewarded = false;

        public int countdownTime = 20;
        public void Tick()
        {
            /*
            if (countdownTimer.AddSeconds(1) < DateTime.Now && peaceArea)
            {
                var packet = $"0|A|STD|-={countdownTime}=-";
                Player.SendPacket(packet);
                OtherPlayer.SendPacket(packet);
                countdownTime--;
                countdownTimer = DateTime.Now;
            }

            if (countdown.AddSeconds(21) < DateTime.Now && peaceArea)
                peaceArea = false;
            */
            if (!rewarded)
            {
                if (Player.Destroyed || Player.GameSession.InProcessOfDisconnection)
                    SendReward(OtherPlayer);
                else if (OtherPlayer.Destroyed || OtherPlayer.GameSession.InProcessOfDisconnection)
                    SendReward(Player);
            }
        }

        public async void SendReward(Player player)
        {
            Program.TickManager.RemoveTick(this);
            rewarded = true;

            Player.Storage.DuelOpponent = null;
            OtherPlayer.Storage.DuelOpponent = null;

            player.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");
            await Task.Delay(5000);
            player.SetPosition(player.FactionId == 1 ? Position.MMOPosition : player.FactionId == 2 ? Position.EICPosition : Position.VRUPosition);
            player.Jump(player.GetBaseMapId(), player.Position);
        }
    }
}
