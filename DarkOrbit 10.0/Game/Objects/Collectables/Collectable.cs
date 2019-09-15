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
        public Character Character { get; set; }
        public Player ToPlayer { get; set; }
        public bool Disposed = false;
        public DateTime disposeTime = new DateTime();

        public int Seconds => CollectableId == AssetTypeModule.BOXTYPE_PIRATE_BOOTY ? 5 : -1;

        public Collectable(int collectableId, Position position, Spacemap spacemap, bool respawnable, Player toPlayer)
        {
            CollectableId = collectableId;
            Hash = Randoms.GenerateHash(16);
            Position = position;
            Spacemap = spacemap;
            Respawnable = respawnable;
            ToPlayer = toPlayer;
            Spacemap.Collectables.TryAdd(Hash, this);
            disposeTime = DateTime.Now;

            if (this is CargoBox)
                Program.TickManager.AddTick(this, out var tickId);
        }

        public DateTime collectTime = new DateTime();
        public void Tick()
        {
            if (this is CargoBox && disposeTime.AddMinutes(2) < DateTime.Now)
                Dispose();

            if (Character != null && Character.Collecting)
            {
                if (!Character.Moving)
                {
                    if (collectTime.AddSeconds(Seconds) < DateTime.Now)
                    {
                        Reward(Character is Pet pet ? pet.Owner : Character as Player);
                        Dispose();
                    }
                }
                else
                {
                    Character.Collecting = false;

                    var packet = $"0|{ServerCommands.SET_ATTRIBUTE}|{ServerCommands.ASSEMBLE_COLLECTION_BEAM_CANCELLED}|0|{Character.Id}|-1";

                    if (Character is Player player)
                        player.SendPacket(packet);
                    else if (Character is Pet pet)
                        pet.SendPacketToInRangePlayers(packet);

                    Character = null;
                }
            }

            /*
            if (!Disposed)
                foreach (var character in Spacemap.Characters.Values)
                {
                    if (character.Position.X == Position.X && character.Position.Y == Position.Y && character is Pet)
                        Collect(character as Pet);
                }
                */
        }

        public void Collect(Character character)
        {
            if (Disposed) return;

            Character = character;
            Character.Collecting = true;
            Character.Moving = false;
            collectTime = DateTime.Now;

            var packet = $"0|{ServerCommands.SET_ATTRIBUTE}|{ServerCommands.ASSEMBLE_COLLECTION_BEAM_ACTIVE}|0|{Character.Id}|{Seconds}";

            if (Character is Player player)
                player.SendPacket(packet);
            else if (Character is Pet pet)
                pet.SendPacketToInRangePlayers(packet);

            Program.TickManager.AddTick(this, out var tickId);
        }

        public void Dispose()
        {
            Disposed = true;
            Spacemap.Collectables.TryRemove(Hash, out var collectable);
            Program.TickManager.RemoveTick(this);
            GameManager.SendCommandToMap(Spacemap.Id, DisposeBoxCommand.write(Hash, true));

            if (Respawnable)
                Respawn();
        }

        public void Respawn()
        {
            Position = Position.Random(Spacemap, 1000, 19800, 1000, 11800);
            Spacemap.Collectables.TryAdd(Hash, this);
            Program.TickManager.AddTick(this, out var tickId);

            Character = null;
            Disposed = false;
        }

        public abstract void Reward(Player player);

        public abstract byte[] GetCollectableCreateCommand();
    }
}
