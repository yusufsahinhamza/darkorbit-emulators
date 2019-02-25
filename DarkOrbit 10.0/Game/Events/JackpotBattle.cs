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
        public ConcurrentDictionary<int, Player> Players = new ConcurrentDictionary<int, Player>();

        public async void Start()
        {
            if (!Active)
            {
                Active = true;

                for (int i = 10; i > 0; i--)
                {
                    var packet = $"0|A|STD|-={i}=-";
                    GameManager.SendPacketToAll(packet);
                    await Task.Delay(1000);

                    if (i <= 1)
                    {
                        foreach (var gameSession in GameManager.GameSessions.Values)
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
                            Players.TryAdd(player.Id, player);
                        }

                        var tickId = -1;
                        Program.TickManager.AddTick(this, out tickId);
                        TickId = tickId;
                    }
                }
            }
        }

        public void Tick()
        {
            if (Active)
            {
                if (Players.Count == 1)
                {
                    var lastPlayer = Players.First().Value;
                    SendRewardAndStop(lastPlayer);
                }
            }
        }

        public async void SendRewardAndStop(Player player)
        {
            Active = false;
            Program.TickManager.RemoveTick(this);
            Players.Clear();
            Spacemap.Characters.Clear();
            Spacemap.Mines.Clear();

            var uridium = 10000;
            player.ChangeData(DataType.URIDIUM, uridium);
            var experience = 100000;
            player.ChangeData(DataType.EXPERIENCE, experience);
            var honor = 10000;
            player.ChangeData(DataType.HONOR, honor);

            GameManager.SendPacketToAll($"0|A|STD|{player.Name} has won the Jacpot Battle!");
            player.SendPacket("0|n|KSMSG|label_traininggrounds_results_victory");
            await Task.Delay(5000);
            player.SetPosition(player.FactionId == 1 ? Position.MMOPosition : player.FactionId == 2 ? Position.EICPosition : Position.VRUPosition);
            player.Jump(player.FactionId == 1 ? 13 : player.FactionId == 2 ? 14 : 15, player.Position);
        }
    }
}
