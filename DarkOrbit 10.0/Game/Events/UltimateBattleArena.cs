using Ow.Game.Objects;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Game.Ticks;
using System.Collections.Concurrent;
using Ow.Managers;

namespace Ow.Game.Events
{
    class UltimateBattleArena : Tick
    {
        public static ConcurrentDictionary<int, Player> WaitingPlayers = new ConcurrentDictionary<int, Player>();

        public UltimateBattleArena()
        {
            Program.TickManager.AddTick(this);
        }

        public void Tick()
        {
            if (WaitingPlayers.Count >= 2)
            {
                foreach (var thisPlayer in WaitingPlayers.Values)
                {
                    foreach (var otherPlayer in WaitingPlayers.Values)
                    {
                        if (!thisPlayer.Equals(otherPlayer))
                        {
                            if (thisPlayer.Storage.Uba == null && otherPlayer.Storage.Uba == null)
                            {
                                var players = new ConcurrentDictionary<int, Player>();
                                players.TryAdd(thisPlayer.Id, thisPlayer);
                                players.TryAdd(otherPlayer.Id, otherPlayer);
                                new Uba(players);
                            }
                        }
                    }
                }
            }
        }

        public void AddWaitingPlayer(Player player)
        {
            if (!WaitingPlayers.ContainsKey(player.Id))
            {
                player.SendCommand(UbaWindowInitializationCommand.write(new UbaD26Module(), 2));
                WaitingPlayers.TryAdd(player.Id, player);
            }
        }

        public void RemoveWaitingPlayer(Player player)
        {
            if (WaitingPlayers.ContainsKey(player.Id))
            {
                player.Storage.Uba = null;
                WaitingPlayers.TryRemove(player.Id, out player);
            }
        }
    }

    class Uba : Tick
    {
        public bool PeaceArea = true;

        public ConcurrentDictionary<int, Player> Players { get; set; }
        public static Spacemap Spacemap = GameManager.GetSpacemap(121);

        public Position Position1 = new Position(4400, 3600);
        public Position Position2 = new Position(5600, 2400);

        public Uba(ConcurrentDictionary<int, Player> players)
        {
            Players = players;
            if (Players.Count > 2) return;

            foreach (var player in Players.Values)
            {
                if (player.GameSession == null) return;

                player.Storage.Uba = this;
                /*
                player.CpuManager.DisableCloak();

                foreach (var player2 in Players.Values)
                {
                    if (!player.Equals(player2))
                    {
                        player.Jump(Spacemap.Id, Position1);
                        player2.Jump(Spacemap.Id, Position2);
                    }
                }
                */
            }

            Page3Timer();
        }
        /*
        public DateTime countdown = new DateTime();
        public DateTime countdownTimer = new DateTime();
        public bool peaceArea = true;
        public bool rewarded = false;
        public bool Active = false;

        public int countdownTime = 20;
        */
        public void Tick()
        {
            /*
            if (Active)
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
            */
        }

        public async void Page3Timer()
        {
            for (int i = 5; i > 0; i--)
            {
                foreach(var player in Players.Values)
                    player.SendCommand(UbaWindowInitializationCommand.write(new Uba047Module(i * 1000, new UbaM1tModule(false)), 3));

                await Task.Delay(1000);

                if (i <= 1)
                {
                    foreach (var player in Players.Values)
                    {
                        player.SendCommand(UbaWindowInitializationCommand.write(new UbaD26Module(), 0));
                        EventManager.UltimateBattleArena.RemoveWaitingPlayer(player);
                    }
                    /*
                    foreach (var player in Players.Values)
                        if (!player.Storage.UbaMatchmakingAccepted)
                        player.SendCommand(UbaWindowInitializationCommand.write(new UbaD26Module(), 0));
                        */
                }
            }
               
        }

        public void SendReward(Player player)
        {

        }
    }
}
