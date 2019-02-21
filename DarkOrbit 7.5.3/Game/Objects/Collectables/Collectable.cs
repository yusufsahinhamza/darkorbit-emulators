using Ow.Game.Movements;
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

namespace Ow.Game.Objects.Collectables
{
    abstract class Collectable : Tick
    {
        public int TickId { get; set; }
        public int CollectableId { get; set; }
        public string Hash { get; set; }
        public Position Position { get; set; }
        public Spacemap Spacemap { get; set; }
        public bool Respawnable { get; set; }
        public Player ToPlayer { get; set; }
        public bool Disposed = false;

        public Collectable(int collectableId, Position position, Spacemap spacemap, bool respawnable, Player toPlayer)
        {
            CollectableId = collectableId;
            Hash = Randoms.GenerateHash(16);
            Position = position;
            Spacemap = spacemap;
            Respawnable = respawnable;
            ToPlayer = toPlayer;
            Spacemap.Collectables.TryAdd(Hash, this);

            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;
        }

        public void Tick()
        {
            if (!Disposed)
                foreach (var character in Spacemap.Characters.Values)
                {
                    if (character.Position.X == Position.X && character.Position.Y == Position.Y && character is Pet)
                        Collect(character as Pet);
                }
        }

        public async void Collect(Character character)
        {
            if (Disposed) return;
            int second = CollectableId == 20 ? 5 : -1;

            character.Collecting = true;

            if (character is Player)
                (character as Player).SendPacket($"0|{ServerCommands.SET_ATTRIBUTE}|{ServerCommands.ASSEMBLE_COLLECTION_BEAM_ACTIVE}|0|{character.Id}|{second}");
            else if (character is Pet)
                (character as Pet).SendPacketToInRangePlayers($"0|{ServerCommands.SET_ATTRIBUTE}|{ServerCommands.ASSEMBLE_COLLECTION_BEAM_ACTIVE}|0|{character.Id}|{second}");

            await Task.Delay(CollectableId == 20 ? 5000 : 1);
            Dispose();
            Reward(character is Pet ? (character as Pet).Owner : character as Player);
        }

        public void Dispose()
        {
            Disposed = true;
            var collectable = this;
            Spacemap.Collectables.TryRemove(Hash, out collectable);
            GameManager.SendPacketToMap(Spacemap.Id, "0|"+ServerCommands.REMOVE_BOX+"|" + Hash);

            if (Respawnable)
                Respawn();
        }

        public void Respawn()
        {
            var newPos = Position.Random(Spacemap, 1000, 19800, 1000, 11800);
            Position = newPos;
            Disposed = false;
            Spacemap.Collectables.TryAdd(Hash, this);
        }

        public abstract void Reward(Player player);

        public string GetCollectableCreatePacket()
        {
            return "0|c|" + Hash + "|"+ CollectableId + "|" + Position.X + "|" + Position.Y;
        }
    }
}
