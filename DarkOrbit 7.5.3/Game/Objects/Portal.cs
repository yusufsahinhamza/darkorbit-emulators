using Ow.Game.Movements;
using Ow.Game.Objects.Activatables;
using Ow.Managers;
using Ow.Net.netty;
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
        public static int JUMP_DELAY_NOW = 1000;
        public static int JUMP_DELAY = 3250;
        public static int SECURE_ZONE_RANGE = 1500;

        public Position TargetPosition { get; set; }
        public int TargetSpaceMapId { get; set; }
        public int GraphicsId { get; set; }
        public bool Visible { get; set; }
        public bool Working { get; set; }

        public Portal(Spacemap spacemap, Position position, Position targetPosition, int targetSpacemapId, int graphicsId, int factionId, bool visible, bool working) : base(spacemap, factionId, position, null)
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
            if (!Working) return;
            if (player.Storage.Jumping) return;

            player.Storage.Jumping = true;
            player.SendPacket("0|"+ ServerCommands.PLAY_PORTAL_ANIMATION + "|"+TargetSpaceMapId+"|"+Id+"");
            await Task.Delay(JUMP_DELAY);

            var pet = player.Pet.Activated;
            var gearId = player.Pet.GearId;
            player.Pet.Deactivate(true);

            player.CurrentInRangePortalId = -1;
            player.Selected = null;
            player.DisableAttack(player.SettingsManager.SelectedLaser);
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
            GameManager.SendPacketToMap(Spacemap.Id, "0|n|p|REM|" + Id);
        }

        public override short GetAssetType() { return 0; }

        public string GetAssetCreatePacket()
        {
            return "0|"+ServerCommands.CREATE_PORTAL+"|" + Id + "|" + FactionId + "|" + GraphicsId + "|" + Position.X + "|" + Position.Y + "|" + Convert.ToInt32(Visible) + "|" + 0;
        }

        public override byte[] GetAssetCreateCommand()
        {
            return null;
        }
    }
}
