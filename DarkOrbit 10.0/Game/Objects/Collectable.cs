using Ow.Game.Movements;
using Ow.Game.Objects.Collectables;
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
    abstract class Collectable : Object, Tick
    {
        public int CollectableId { get; set; }
        public string Hash { get; set; }
        public bool Respawnable { get; set; }
        public Character Character { get; set; }
        public Player ToPlayer { get; set; }
        public bool Disposed = false;

        public int Seconds => CollectableId == AssetTypeModule.BOXTYPE_PIRATE_BOOTY ? 5 : -1;

        public Collectable(int collectableId, Position position, Spacemap spacemap, bool respawnable, Player toPlayer) : base(Randoms.CreateRandomID(), position, spacemap)
        {
            Hash = Randoms.GenerateHash(16);
            CollectableId = collectableId;
            Respawnable = respawnable;
            ToPlayer = toPlayer;

            if (this is CargoBox)
            {
                Program.TickManager.AddTick(this);
                disposeTime = DateTime.Now;
            }
        }

        public DateTime collectTime = new DateTime();
        public DateTime disposeTime = new DateTime();
        public void Tick()
        {
            if (!Disposed)
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

            Program.TickManager.AddTick(this);
        }

        public void Dispose()
        {
            Disposed = true;
            Character = null;
            Spacemap.Objects.TryRemove(Id, out var collectable);
            Program.TickManager.RemoveTick(this);
            GameManager.SendCommandToMap(Spacemap.Id, DisposeBoxCommand.write(Hash, true));

            if (Respawnable)
                Respawn();
        }

        public void Respawn()
        {
            Position = Position.Random(Spacemap, 1000, 19800, 1000, 11800);
            Spacemap.Objects.TryAdd(Id, this);

            if (this is CargoBox)
            {
                Program.TickManager.AddTick(this);
            }

            Disposed = false;
        }

        public abstract void Reward(Player player);

        public abstract byte[] GetCollectableCreateCommand();
    }
}
