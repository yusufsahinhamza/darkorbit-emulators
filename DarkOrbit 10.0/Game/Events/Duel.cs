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
using System.Collections.Concurrent;

namespace Ow.Game.Events
{
    class Duel : Tick
    {
        public bool PeaceArea = true;

        public ConcurrentDictionary<int, Player> Players { get; set; }
        public static Spacemap Spacemap = GameManager.GetSpacemap(101);

        public Position Position1 = new Position(3700, 3200);
        public Position Position2 = new Position(6400, 3200);

        public Duel(ConcurrentDictionary<int, Player> players)
        {
            Players = players;
            if (Players.Count < 2 || Players.Count > 2) return;

            foreach (var player in Players.Values)
            {
                if (player.GameSession != null)
                {
                    player.AddVisualModifier(VisualModifierCommand.CAMERA, 0, "", 0, true);
                    player.Storage.Duel = this;
                    player.CpuManager.DisableCloak();

                    foreach (var player2 in Players.Values)
                    {
                        if (!player.Equals(player2))
                        {
                            player.Jump(Spacemap.Id, Position1);
                            player2.Jump(Spacemap.Id, Position2);
                        }
                    }
                }
            }

            Start();
        }

        public async void Start()
        {
            for (int i = 25; i > 0; i--)
            {
                var packet = $"0|A|STM|jp_no_attack_n_seconds|%!|{i}";

                foreach (var player in Players.Values)
                    player.SendPacket(packet);

                await Task.Delay(1000);

                if (i <= 1)
                {
                    PeaceArea = false;
                    Program.TickManager.AddTick(this);
                }
            }
        }

        public void Tick()
        {
            if (Players.Count == 1)
                SendRewardAndStop(Players.FirstOrDefault().Value);
        }

        public async void SendRewardAndStop(Player winnerPlayer)
        {
            Program.TickManager.RemoveTick(this);

            if (winnerPlayer != null)
            {
                winnerPlayer.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");
                await Task.Delay(2500);

                winnerPlayer.SetPosition(winnerPlayer.GetBasePosition());
                winnerPlayer.Jump(winnerPlayer.GetBaseMapId(), winnerPlayer.Position);
            }

            foreach (var player in Players.Values)
            {
                if (player.GameSession != null)
                {
                    var objects = Spacemap.Objects.Values.Where(x => x is Mine mine && mine.Player == player);
                    foreach (var obj in objects)
                        (obj as Mine).Remove(true);

                    player.RemoveVisualModifier(VisualModifierCommand.CAMERA);
                    RemovePlayer(player);
                }
            }
        }

        public static void RemovePlayer(Player player)
        {
            player.RemoveVisualModifier(VisualModifierCommand.CAMERA);

            if (InDuel(player))
            {
                player.Storage.Duel.Players.TryRemove(player.Id, out player);
                player.Storage.Duel = null;
            }
        }

        public static bool InDuel(Player player)
        {
            return player.Storage.Duel != null && player.Spacemap.Id == Spacemap.Id && Spacemap.Characters.ContainsKey(player.Id);
        }

        public Player GetOpponent(Player player)
        {
            return Players?.Where(x => x.Value.Id != player.Id).FirstOrDefault().Value;
        }
    }
}
