using Ow.Game.Movements;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Npcs;
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
    class Npc : Character
    {
        public NpcAI NpcAI { get; set; }

        public Npc(int id, int shipId, Spacemap spacemap, Position position) : base(id, GameManager.GetShip(shipId).Name, 0, GameManager.GetShip(shipId), position, spacemap, GameManager.GetClan(0))
        {
            Spacemap.AddCharacter(this);

            Program.TickManager.AddTick(this);
        }

        public override void Tick()
        {

        }

        public override byte[] GetShipCreateCommand()
        {
            return ShipCreateCommand.write(
                Id,
                Convert.ToString(Ship.Id),
                3,
                "",
                Ship.Name,
                Position.X,
                Position.Y,
                FactionId,
                0,
                0,
                false,
                new ClanRelationModule(ClanRelationModule.AT_WAR),
                0,
                false,
                true,
                false,
                ClanRelationModule.AT_WAR,
                ClanRelationModule.AT_WAR,
                new List<VisualModifierCommand>(),
                new class_11d(class_11d.DEFAULT)
                );
        }
    }
}
