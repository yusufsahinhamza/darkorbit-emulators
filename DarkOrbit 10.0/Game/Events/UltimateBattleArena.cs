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
    class UltimateBattleArena : Tick
    {
        public int TickId { get; set; }
        public static List<Player> WaitingPlayers = new List<Player>();

        public UltimateBattleArena()
        {
            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;
        }

        public void Tick()
        {
            if (WaitingPlayers.Count >= 2)
            {
                foreach (var thisPlayer in WaitingPlayers)
                {
                    foreach (var otherPlayer in WaitingPlayers)
                    {
                        if (!thisPlayer.Equals(otherPlayer))
                        {
                            if (thisPlayer.UbaOpponent == null && otherPlayer.UbaOpponent == null)
                            {
                                thisPlayer.UbaOpponent = otherPlayer;
                                otherPlayer.UbaOpponent = thisPlayer;
                                thisPlayer.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 3));
                                thisPlayer.UbaOpponent.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 3));
                            }
                        }
                    }
                }
            }
        }

        public void AddPlayer(Player player)
        {
            if (!WaitingPlayers.Contains(player))
                WaitingPlayers.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            if (WaitingPlayers.Contains(player))
                WaitingPlayers.Remove(player);
        }
    }

    class Uba : Tick
    {
        public int TickId { get; set; }
        public Player Player { get; set; }
        public Player OtherPlayer { get; set; }

        public Uba(Player player, Player otherPlayer)
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
            {
                Player.SendCommand(MapRemovePOICommand.write("uba_poi2"));
                Player.SendCommand(MapRemovePOICommand.write("uba_poi3"));
                OtherPlayer.SendCommand(MapRemovePOICommand.write("uba_poi2"));
                OtherPlayer.SendCommand(MapRemovePOICommand.write("uba_poi3"));
                peaceArea = false;
            }

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
            rewarded = true;
            Program.TickManager.RemoveTick(this);

            player.UbaMatchmakingAccepted = false;
            player.UbaOpponent = null;
            OtherPlayer.UbaMatchmakingAccepted = false;
            OtherPlayer.UbaOpponent = null;

            player.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");
            await Task.Delay(5000);
            //TODO: player.MoveManager.SetPosition();
            player.Jump(player.GetBaseMapId(), player.Position);
        }
    }
}
