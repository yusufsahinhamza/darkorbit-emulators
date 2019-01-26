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
        public const short ID = 8020;

        public bool displayNotFreeCargoBoxes = false;
        public bool displayResources = false;
        public bool displayWindowsBackground = false;
        public bool displayBonusBoxes = false;
        public bool displayNotifications = false;
        public bool showPremiumQuickslotBar = false;
        public bool displayPlayerNames = false;
        public bool dragWindowsAlways = false;
        public bool displayHitpointBubbles = false;
        public bool displayDrones = false;
        public bool preloadUserShips = false;
        public bool hoverShips = false;
        public bool allowAutoQuality = false;
        public bool displayChat = false;
        public bool displayFreeCargoBoxes = false;
        public bool showNotOwnedItems = false;
        public int varp3M = 0;
        public bool varb3N = false;
        public int displaySetting3DqualityAntialias = 0;
        public int displaySetting3DqualityEffects = 0;
        public bool proActionBarAutohideEnabled = false;
        public bool proActionBarEnabled = false;
        public int var03r = 0;
        public int displaySetting3DtextureFiltering = 0;
        public bool proActionBarKeyboardInputEnabled = false;
        public int displaySetting3DqualityLights = 0;
        public bool var12P = false;
        public int displaySetting3DsizeTextures = 0;
        public int displaySetting3DqualityTextures = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            displayChat = parser.readBoolean();
            displayWindowsBackground = parser.readBoolean();
            hoverShips = parser.readBoolean();
            displayFreeCargoBoxes = parser.readBoolean();
            varp3M = parser.readInt();
            varp3M = varp3M << 7 | varp3M >> 25;
            varb3N = parser.readBoolean();
            displaySetting3DqualityAntialias = parser.readInt();
            displaySetting3DqualityAntialias = displaySetting3DqualityAntialias << 4 | displaySetting3DqualityAntialias >> 28;
            displaySetting3DqualityEffects = parser.readInt();
            displaySetting3DqualityEffects = displaySetting3DqualityEffects << 16 | displaySetting3DqualityEffects >> 16;
            showNotOwnedItems = parser.readBoolean();
            showPremiumQuickslotBar = parser.readBoolean();
            displayPlayerNames = parser.readBoolean();
            proActionBarAutohideEnabled = parser.readBoolean();
            displayNotifications = parser.readBoolean();
            displayNotFreeCargoBoxes = parser.readBoolean();
            proActionBarEnabled = parser.readBoolean();
            dragWindowsAlways = parser.readBoolean();
            var03r = parser.readInt();
            var03r = var03r << 14 | var03r >> 18;
            displayHitpointBubbles = parser.readBoolean();
            displaySetting3DtextureFiltering = parser.readInt();
            displaySetting3DtextureFiltering = displaySetting3DtextureFiltering << 2 | displaySetting3DtextureFiltering >> 30;
            proActionBarKeyboardInputEnabled = parser.readBoolean();
            displayBonusBoxes = parser.readBoolean();
            displaySetting3DqualityLights = parser.readInt();
            displaySetting3DqualityLights = displaySetting3DqualityLights >> 8 | displaySetting3DqualityLights << 24;
            var12P = parser.readBoolean();
            preloadUserShips = parser.readBoolean();
            displayResources = parser.readBoolean();
            allowAutoQuality = parser.readBoolean();
            displayDrones = parser.readBoolean();
            displaySetting3DsizeTextures = parser.readInt();
            displaySetting3DsizeTextures = displaySetting3DsizeTextures << 7 | displaySetting3DsizeTextures >> 25;
            displaySetting3DqualityTextures = parser.readInt();
            displaySetting3DqualityTextures = displaySetting3DqualityTextures << 5 | displaySetting3DqualityTextures >> 27;
        }
    }
}
