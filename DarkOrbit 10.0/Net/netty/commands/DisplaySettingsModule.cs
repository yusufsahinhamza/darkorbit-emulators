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
        public const short ID = 16484;

        public bool NotSet = false;
        public bool DisplayPlayerNames = false;
        public bool DisplayResources = false;
        public bool ShowPremiumQuickslotBar = false;
        public bool AllowAutoQuality = false;
        public bool PreloadUserShips = false;
        public bool DisplayHitpointBubbles = false;
        public bool ShowNotOwnedItems = false;
        public bool DisplayChat = false;
        public bool DisplayWindowsBackground = false;
        public bool DisplayNotFreeCargoBoxes = false;
        public bool DragWindowsAlways = false;
        public bool DisplayNotifications = false;
        public bool HoverShips = false;
        public bool DisplayDrones = false;
        public bool DisplayBonusBoxes = false;
        public bool DisplayFreeCargoBoxes = false;

        public bool var12P = false;
        public bool varb3N = false;
        public int DisplaySetting3DqualityAntialias = 0;
        public int varp3M = 0;
        public int DisplaySetting3DqualityEffects = 0;
        public int DisplaySetting3DqualityLights = 0;
        public int DisplaySetting3DqualityTextures = 0;
        public int var03r = 0;
        public int DisplaySetting3DsizeTextures = 0;
        public int DisplaySetting3DtextureFiltering = 0;
        public bool ProActionBarEnabled = false;
        public bool ProActionBarKeyboardInputEnabled = false;
        public bool ProActionBarAutohideEnabled = false;

        public DisplaySettingsModule(bool param1, bool param2, bool param3, bool param4, bool param5, bool param6, bool param7, bool param8, bool param9, bool param10,
                                     bool param11, bool param12, bool param13, bool param14, bool param15, bool param16, bool param17, bool param18, bool param19,
                                     int param20, int param21, int param22, int param23, int param24, int param25, int param26, int param27, bool param28,
                                     bool param29, bool param30)
        {
            this.NotSet = param1;
            this.DisplayPlayerNames = param2;
            this.DisplayResources = param3;
            this.DisplayBonusBoxes = param4;
            this.DisplayHitpointBubbles = param5;
            this.DisplayChat = param6;
            this.DisplayDrones = param7;
            this.DisplayFreeCargoBoxes = param8;
            this.DisplayNotFreeCargoBoxes = param9;
            this.ShowNotOwnedItems = param10;
            this.DisplayWindowsBackground = param11;
            this.var12P = param12;
            this.DisplayNotifications = param13;
            this.PreloadUserShips = param14;
            this.DragWindowsAlways = param15;
            this.HoverShips = param16;
            this.ShowPremiumQuickslotBar = param17;
            this.AllowAutoQuality = param18;
            this.varb3N = param19;
            this.DisplaySetting3DqualityAntialias = param20;
            this.varp3M = param21;
            this.DisplaySetting3DqualityEffects = param22;
            this.DisplaySetting3DqualityLights = param23;
            this.DisplaySetting3DqualityTextures = param24;
            this.var03r = param25;
            this.DisplaySetting3DsizeTextures = param26;
            this.DisplaySetting3DtextureFiltering = param27;
            this.ProActionBarEnabled = param28;
            this.ProActionBarKeyboardInputEnabled = param29;
            this.ProActionBarAutohideEnabled = param30;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(DisplaySetting3DqualityAntialias << 11 | DisplaySetting3DqualityAntialias >> 21);
            param1.writeShort(30800);
            param1.writeBoolean(ProActionBarEnabled);
            param1.writeBoolean(ProActionBarAutohideEnabled);
            param1.writeShort(-21589);
            param1.writeInt(DisplaySetting3DqualityTextures << 11 | DisplaySetting3DqualityTextures >> 21);
            param1.writeBoolean(varb3N);
            param1.writeInt(var03r << 14 | var03r >> 18);
            param1.writeBoolean(ShowNotOwnedItems);
            param1.writeBoolean(DisplayDrones);
            param1.writeBoolean(DisplayNotifications);
            param1.writeInt(DisplaySetting3DqualityLights << 1 | DisplaySetting3DqualityLights >> 31);
            param1.writeBoolean(DisplayResources);
            param1.writeBoolean(DisplayPlayerNames);
            param1.writeBoolean(DisplayChat);
            param1.writeBoolean(PreloadUserShips);
            param1.writeInt(DisplaySetting3DsizeTextures >> 12 | DisplaySetting3DsizeTextures << 20);
            param1.writeBoolean(DisplayWindowsBackground);
            param1.writeInt(DisplaySetting3DtextureFiltering >> 16 | DisplaySetting3DtextureFiltering << 16);
            param1.writeBoolean(var12P);
            param1.writeInt(varp3M << 9 | varp3M >> 23);
            param1.writeInt(DisplaySetting3DqualityEffects >> 4 | DisplaySetting3DqualityEffects << 28);
            param1.writeBoolean(DisplayBonusBoxes);
            param1.writeBoolean(DisplayFreeCargoBoxes);
            param1.writeBoolean(DragWindowsAlways);
            param1.writeBoolean(ProActionBarKeyboardInputEnabled);
            param1.writeBoolean(NotSet);
            param1.writeBoolean(DisplayHitpointBubbles);
            param1.writeBoolean(AllowAutoQuality);
            param1.writeBoolean(HoverShips);
            param1.writeBoolean(ShowPremiumQuickslotBar);
            param1.writeBoolean(DisplayNotFreeCargoBoxes);
            return param1.Message.ToArray();
        }
    }
}
