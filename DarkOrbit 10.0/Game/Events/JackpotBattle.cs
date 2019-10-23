using Newtonsoft.Json;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Managers.MySQLManager;
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
        public string Name = "*****";
        public bool Active = false;
        public Spacemap Spacemap = GameManager.GetSpacemap(42);

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ConcurrentDictionary<int, Player> Players = new ConcurrentDictionary<int, Player>();
        public List<int> Finalists = new List<int>();

        public async void Start()
        {
            if (!Active)
            {
                Active = true;

                for (int i = 120; i > 0; i--)
                {
                    var packet = $"0|A|STD|Jackpot Battle starting in {i} seconds...";
                    GameManager.SendPacketToAll(packet);
                    await Task.Delay(1000);

                    if (i <= 1)
                    {
                        foreach (var gameSession in GameManager.GameSessions?.Values)
                        {
                            var player = gameSession.Player;

                            if (!Duel.InDuel(player) && player.RankId != 21)
                            {
                                Players.TryAdd(player.Id, player);

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

                        Program.TickManager.AddTick(this);

                        StartDate = DateTime.Now;
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

                if (Spacemap.Characters.Count == 2 && Finalists.Count < 2)
                {
                    foreach (var player in Spacemap.Characters.Values)
                        Finalists.Add(player.Id);
                }

                if (Spacemap.Characters.Count == 1)
                    SendRewardAndStop(Spacemap.Characters.FirstOrDefault().Value as Player);
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
            EndDate = DateTime.Now;
            Program.TickManager.RemoveTick(this);

            player.ChangeData(DataType.URIDIUM, 25000);
            player.ChangeData(DataType.EXPERIENCE, 2500000);
            player.ChangeData(DataType.HONOR, 25000);

            GameManager.SendPacketToAll($"0|A|STD|{player.Name} has won the Jacpot Battle!");
            player.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");

            var title = "title_jackpot_battle_winner";
            var oldWinner = GameManager.GameSessions.FirstOrDefault(x => x.Value?.Player.Title == title).Value?.Player;
            if (oldWinner != null) 
                oldWinner.SetTitle("");

            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                mySqlClient.ExecuteNonQuery($"INSERT INTO log_event_jpb (players, finalists, winner_id, start_date, end_date) VALUES ('{JsonConvert.SerializeObject(Players.Keys.ToList())}', '{JsonConvert.SerializeObject(Finalists.ToList())}', {player.Id}, '{StartDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{EndDate.ToString("yyyy-MM-dd HH:mm:ss")}')");
                mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET title = '' WHERE title = '{title}'");
            }

            player.SetTitle(title, true);

            await Task.Delay(5000);
            player.SetPosition(player.GetBasePosition());
            player.Jump(player.GetBaseMapId(), player.Position);

            Active = false;
            radiationX = 12800;
            radiationY = 1600;
            timerSecond = 1;
            Players.Clear();
            Finalists.Clear();
            Spacemap.Characters.Clear();
            Spacemap.Objects.Clear();
            Spacemap.POIs.Clear();
        }

        public bool InEvent(Player player)
        {
            return Active && player.Spacemap.Id == Spacemap.Id && Spacemap.Characters.ContainsKey(player.Id);
        }
    }
}
