using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Events
{
    class JackpotBattle : Tick
    {
        public int TickId { get; set; }
        public string Name = "*****";
        public bool Active = false;
        public Spacemap Spacemap = GameManager.GetSpacemap(42);

        public async void Start()
        {
            if (!Active)
            {
                Active = true;

                for (int i = 60; i > 0; i--)
                {
                    var packet = $"0|A|STD|-={i}=-";
                    GameManager.SendPacketToAll(packet);
                    await Task.Delay(1000);

                    if (i <= 1)
                    {
                        foreach (var gameSession in GameManager.GameSessions.Values)
                        {
                            if (gameSession != null && gameSession.Player.Storage.DuelOpponent == null)
                            {
                                var player = gameSession.Player;

                                player.CurrentHitPoints = player.MaxHitPoints;
                                player.CurrentShieldConfig1 = player.MaxShieldPoints;
                                player.CurrentShieldConfig2 = player.MaxShieldPoints;
                                player.CpuManager.DisableCloak();

                                var mmoPosition = new Position(2000, 1500);
                                var eicPosition = new Position(3800, 10600);
                                var vruPosition = new Position(18300, 6700);

                                player.Jump(Spacemap.Id, player.FactionId == 1 ? mmoPosition : player.FactionId == 2 ? eicPosition : vruPosition);
                            }
                        }

                        var tickId = -1;
                        Program.TickManager.AddTick(this, out tickId);
                        TickId = tickId;

                        jpbTimer = DateTime.Now;
                        jpbStartTime = DateTime.Now;
                    }
                }
            }
        }

        public void Tick()
        {
            if (Active)
            {
                CheckRadiation();

                if (Spacemap.Characters.Count == 1)
                {
                    var lastPlayer = Spacemap.Characters.First().Value;
                    SendRewardAndStop(lastPlayer as Player);
                }
            }
        }

        public DateTime jpbTimer = new DateTime();
        public DateTime jpbStartTime = new DateTime();
        public int radiationX = 12800;
        public int radiationY = 1600;

        public int timerSecond = 1;

        public void CheckRadiation()
        {
            if (jpbTimer.AddSeconds(timerSecond) < DateTime.Now && jpbStartTime.AddSeconds(30) < DateTime.Now)
            {
                if ((radiationX - 100 == 6400 && timerSecond == 1) || (radiationX - 100 == 3200 && timerSecond == 1) || (radiationX - 100 == 1600 && timerSecond == 1))
                {
                    timerSecond = 30;
                    radiationX -= 100;
                }
                else if (radiationX - 100 > 800)
                {
                    timerSecond = 1;
                    radiationX -= 100;
                }

                var newPoi = new POI("jpb_poi", POITypes.RADIATION, POIDesigns.SIMPLE, POIShapes.CIRCLE, new List<Position> { new Position(10400, 6400), new Position(radiationX, radiationY) }, true, true);
                var oldPoi = Spacemap.POIs.FirstOrDefault(x => x.Value.Id == newPoi.Id).Value;

                if (oldPoi != null)
                    Spacemap.POIs.TryRemove(oldPoi.Id, out oldPoi);

                Spacemap.POIs.TryAdd("jpb_poi", newPoi);

                GameManager.SendCommandToMap(Spacemap.Id, newPoi.GetPOICreateCommand());

                jpbTimer = DateTime.Now;
            }
        }

        public async void SendRewardAndStop(Player player)
        {
            player.ChangeData(DataType.URIDIUM, 10000);
            player.ChangeData(DataType.EXPERIENCE, 100000);
            player.ChangeData(DataType.HONOR, 10000);

            GameManager.SendPacketToAll($"0|A|STD|{player.Name} has won the Jacpot Battle!");
            player.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");
            await Task.Delay(5000);
            player.SetPosition(player.FactionId == 1 ? Position.MMOPosition : player.FactionId == 2 ? Position.EICPosition : Position.VRUPosition);
            player.Jump(player.GetBaseMapId(), player.Position);

            Active = false;
            Program.TickManager.RemoveTick(this);
            Spacemap.Characters.Clear();
            Spacemap.Collectables.Clear();
            Spacemap.Mines.Clear();
            Spacemap.POIs.Clear();
        }

        public bool InActiveEvent(Player player)
        {
            return Active && player.Spacemap.Id == Spacemap.Id && Spacemap.Characters.ContainsKey(player.Id);
        }
    }
}
