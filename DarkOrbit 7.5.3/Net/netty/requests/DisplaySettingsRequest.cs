using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class DisplaySettingsRequest
    {
        public const short ID = 15703;

        public Boolean displayPlayerName = false;

        public Boolean displayResources = false;

        public Boolean displayBoxes = false;

        public Boolean displayHitpointBubbles = false;

        public Boolean displayChat = false;

        public Boolean displayDrones = false;

        public Boolean displayCargoboxes = false;

        public Boolean displayPenaltyCargoboxes = false;

        public Boolean displayWindowBackground = false;

        public Boolean displayNotifications = false;

        public Boolean preloadUserShips = false;

        public Boolean alwaysDraggableWindows = false;

        public Boolean shipHovering = false;

        public Boolean showSecondQuickslotBar = false;

        public Boolean useAutoQuality = false;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.displayPlayerName = param1.readBoolean();
            this.displayResources = param1.readBoolean();
            this.displayBoxes = param1.readBoolean();
            this.displayHitpointBubbles = param1.readBoolean();
            this.displayChat = param1.readBoolean();
            this.displayDrones = param1.readBoolean();
            this.displayCargoboxes = param1.readBoolean();
            this.displayPenaltyCargoboxes = param1.readBoolean();
            this.displayWindowBackground = param1.readBoolean();
            this.displayNotifications = param1.readBoolean();
            this.preloadUserShips = param1.readBoolean();
            this.alwaysDraggableWindows = param1.readBoolean();
            this.shipHovering = param1.readBoolean();
            this.showSecondQuickslotBar = param1.readBoolean();
            this.useAutoQuality = param1.readBoolean();
        }
    }
}
