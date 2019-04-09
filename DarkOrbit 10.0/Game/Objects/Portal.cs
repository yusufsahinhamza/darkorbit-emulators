using Ow.Game.Movements;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.handlers;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects
{
    class PortalBase
    {
        public int TargetSpaceMapId { get; set; }
        public List<int> Position { get; set; }
        public List<int> TargetPosition { get; set; }
        public int GraphicId { get; set; }
        public int FactionId { get; set; }
        public bool Visible { get; set; }
        public bool Working { get; set; }
    }

    class Portal : Activatable
    {
        public static int JUMP_DELAY = 1000;
        public static int SECURE_ZONE_RANGE = 1500;

        public Position TargetPosition { get; set; }
        public int TargetSpaceMapId { get; set; }
        public int GraphicsId { get; set; }
        public bool Visible { get; set; }
        public bool Working { get; set; }

        public Portal(Spacemap spacemap, Position position, Position targetPosition, int targetSpacemapId, int graphicsId, int factionId, bool visible, bool working) : base(spacemap, factionId, position, GameManager.GetClan(0))
        {
            TargetPosition = targetPosition;
            TargetSpaceMapId = targetSpacemapId;
            GraphicsId = graphicsId;
            FactionId = factionId;
            Visible = visible;
            Working = working;
        }

        public override async void Click(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (!Working || GameManager.GetSpacemap(TargetSpaceMapId) == null || TargetPosition == null) return;
            if (player.Storage.Jumping) return;

            player.Storage.Jumping = true;
            var apc = ActivatePortalCommand.write(TargetSpaceMapId, Id);
            player.SendCommand(apc);
            await Task.Delay(JUMP_DELAY);

            var pet = player.Pet.Activated;
            var gearId = player.Pet.GearId;
            player.Pet.Deactivate(true);

            player.CurrentInRangePortalId = -1;
            player.Deselection();
            player.Spacemap.RemoveCharacter(player);
            player.Storage.InRangeAssets.Clear();
            player.InRangeCharacters.Clear();
            player.SetPosition(TargetPosition);

            var targetSpacemap = GameManager.GetSpacemap(TargetSpaceMapId);
            player.Spacemap = targetSpacemap;

            player.Spacemap.AddAndInitPlayer(player);
            player.Storage.Jumping = false;

            if (pet)
            {
                player.Pet.Activate();
                player.Pet.SwitchGear(gearId);
            }
        }

        public void Remove()
        {
            var portal = this as Activatable;
            Spacemap.Activatables.TryRemove(Id, out portal);
            GameManager.SendCommandToMap(Spacemap.Id, RemovePortalCommand.write(Id));
        }

        public override short GetAssetType() { return 0; }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return CreatePortalCommand.write(Id, FactionId, GraphicsId,
                                           Position.X, Position.Y, true,
                                           Visible, new List<int>());
        }

    }
}
