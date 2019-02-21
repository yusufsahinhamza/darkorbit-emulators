using Ow.Game.Objects;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Game.Ticks;

namespace Ow.Game.Events
{
    class Duel : Tick
    {
        public int TickId { get; set; }
        public Player Player { get; set; }
        public Player OtherPlayer { get; set; }

        public Duel(Player player, Player otherPlayer)
        {
            Player = player;
            OtherPlayer = otherPlayer;

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
         

            if (!rewarded)
            {
                if (Player.Destroyed)
                    SendReward(OtherPlayer);
                else if (OtherPlayer.Destroyed)
                    SendReward(Player);
            }
        }

        public async void SendReward(Player player)
        {
            Player.Storage.Duel = null;
            OtherPlayer.Storage.Duel = null;

            rewarded = true;
            Program.TickManager.RemoveTick(this);

            player.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");
            await Task.Delay(5000);
            player.MoveManager.SetPosition();
            player.Jump(player.FactionId == 1 ? 13 : player.FactionId == 2 ? 14 : 15, player.Position);
        }
    }
}
