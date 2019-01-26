using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class DisplaySettingsModule
    {
        public const short ID = 1583;

        public Boolean notSet = false;

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

        public DisplaySettingsModule(Boolean notSet, Boolean displayPlayerName, Boolean displayResources, Boolean displayBoxes, Boolean displayHitpointBubbles,
            Boolean displayChat, Boolean displayDrones, Boolean displayCargoboxes, Boolean displayPenaltyCargoboxes, Boolean displayWindowBackground,
            Boolean displayNotifications, Boolean preloadUserShips, Boolean alwaysDraggableWindows, Boolean shipHovering, Boolean showSecondQuickslotBar,
            Boolean useAutoQuality)
        {
            this.notSet = notSet;
            this.displayPlayerName = displayPlayerName;
            this.displayResources = displayResources;
            this.displayBoxes = displayBoxes;
            this.displayHitpointBubbles = displayHitpointBubbles;
            this.displayChat = displayChat;
            this.displayDrones = displayDrones;
            this.displayCargoboxes = displayCargoboxes;
            this.displayPenaltyCargoboxes = displayPenaltyCargoboxes;
            this.displayWindowBackground = displayWindowBackground;
            this.displayNotifications = displayNotifications;
            this.preloadUserShips = preloadUserShips;
            this.alwaysDraggableWindows = alwaysDraggableWindows;
            this.shipHovering = shipHovering;
            this.showSecondQuickslotBar = showSecondQuickslotBar;
            this.useAutoQuality = useAutoQuality;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.notSet);
            param1.writeBoolean(this.displayPlayerName);
            param1.writeBoolean(this.displayResources);
            param1.writeBoolean(this.displayBoxes);
            param1.writeBoolean(this.displayHitpointBubbles);
            param1.writeBoolean(this.displayChat);
            param1.writeBoolean(this.displayDrones);
            param1.writeBoolean(this.displayCargoboxes);
            param1.writeBoolean(this.displayPenaltyCargoboxes);
            param1.writeBoolean(this.displayWindowBackground);
            param1.writeBoolean(this.displayNotifications);
            param1.writeBoolean(this.preloadUserShips);
            param1.writeBoolean(this.alwaysDraggableWindows);
            param1.writeBoolean(this.shipHovering);
            param1.writeBoolean(this.showSecondQuickslotBar);
            param1.writeBoolean(this.useAutoQuality);
            return param1.Message.ToArray();
        }
    }
}
