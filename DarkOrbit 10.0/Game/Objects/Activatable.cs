using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects
{
    abstract class Activatable : Attackable
    {
        public abstract byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE);

        public abstract void Click(GameSession gameSession);

        public short AssetTypeId { get; set; }

        public override string Name { get; set; }
        public override Clan Clan { get; set; }
        public override Position Position { get; set; }
        public override Spacemap Spacemap { get; set; }
        public override int FactionId { get; set; }
        public override int CurrentHitPoints { get; set; }
        public override int MaxHitPoints { get; set; }
        public override int CurrentNanoHull { get; set; }
        public override int MaxNanoHull { get; set; }
        public override int CurrentShieldPoints { get; set; }
        public override int MaxShieldPoints { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }

        public Activatable(Spacemap spacemap, int factionId, Position position, Clan clan, short assetTypeId = 0) : base(Randoms.CreateRandomID())
        {
            AssetTypeId = assetTypeId;
            Spacemap = spacemap;
            FactionId = factionId;
            Position = position;
            Clan = clan;

            if (!(this is Satellite))
                Spacemap.Activatables.TryAdd(Id, this);
        }

        public AssetTypeModule GetAssetType()
        {
            return new AssetTypeModule(AssetTypeId);
        }

        public override void Tick()
        {
            if (!Destroyed)
            {
                if (this is BattleStation battleStation)
                    battleStation.Tick();
                else if (this is Satellite satellite)
                    satellite.Tick();
            }
        }

        public void SendPacketToInRangeCharacters(string packet)
        {
            foreach (var character in Spacemap.Characters.Values)
                if (character is Player player && character.Position.DistanceTo(Position) < RenderRange)
                    player.SendPacket(packet);
        }

        public void SendCommandToInRangeCharacters(byte[] command, Attackable expectPlayer = null)
        {
            foreach (var character in Spacemap.Characters.Values)
                if (character is Player player && player != expectPlayer && character.Position.DistanceTo(Position) < RenderRange)
                    player.SendCommand(command);
        }
    }
}
