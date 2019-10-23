using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects
{
    abstract class Mine : Object, Tick
    {
        public const int RANGE = 200;
        public const int ACTIVATION_TIME = 1750;

        public int MineTypeId { get; set; }
        public string Hash { get; set; }
        public Player Player { get; set; }
        public bool Lance { get; set; }
        public bool Pulse { get; set; }
        public bool Detonation { get; set; }
        public bool Active = true;
        public int ExplodeRange = 275;

        public Mine(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(Randoms.CreateRandomID(), position, spacemap)
        {
            Hash = Randoms.GenerateHash(10);
            Player = player;
            MineTypeId = mineTypeId;

            Lance = Player.Settings.InGameSettings.selectedFormation == DroneManager.LANCE_FORMATION;
            Detonation = (Player.SkillTree.detonation1 + Player.SkillTree.detonation2 == 5);
            Pulse = Player.SkillTree.explosives == 5;
            ExplodeRange += Maths.GetPercentage(ExplodeRange, Player.GetSkillPercentage("Explosives"));

            activationTime = DateTime.Now;

            Program.TickManager.AddTick(this);
        }

        public abstract void Action(Player player);

        public void Explode()
        {
            foreach (var character in Spacemap.Characters.Values)
            {
                if (character is Player player && player.Position.DistanceTo(Position) < ExplodeRange)
                {
                    if (Player == player || !Duel.InDuel(player) || (Duel.InDuel(player) && player.Storage.Duel?.GetOpponent(player) == Player))
                        Action(player);
                }
            }
        }

        public DateTime activationTime = new DateTime();
        public void Tick()
        {
            if (Active && activationTime.AddMinutes(3) < DateTime.Now)
                Remove(true);
        }

        public void Remove(bool timeOut = false)
        {
            Active = false;

            foreach (var gameSession in GameManager.GameSessions?.Values)
            {
                var player = gameSession.Player;

                if (player.Storage.InRangeObjects.ContainsKey(Id))
                    player.Storage.InRangeObjects.TryRemove(Id, out var mine);
            }

            Spacemap.Objects.TryRemove(Id, out var obj);

            var packet = timeOut ? $"0|{ServerCommands.REMOVE_ORE}|{Hash}" : $"0|n|MIN|{Hash}";

            GameManager.SendPacketToMap(Spacemap.Id, packet);

            Program.TickManager.RemoveTick(this);
        }

        public byte[] GetMineCreateCommand()
        {
            return MineCreateCommand.write(Hash, Pulse, Detonation, MineTypeId, Position.Y, Position.X);
        }
    }
}
