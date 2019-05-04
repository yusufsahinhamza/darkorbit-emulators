package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class DisplaySettingsModule
        implements ServerCommand {

    public static int     ID                        = 2654;
    public        boolean mNotSet                   = false;
    public        boolean mDisplayPlayerNames       = false;
    public        boolean mDisplayResources         = false;
    public        boolean mShowPremiumQuickslotBar  = false;
    public        boolean mAllowAutoQuality         = false;
    public        boolean mPreloadUserShips         = false;
    public        boolean mDisplayHitpointBubbles   = false;
    public        boolean mShowNotOwnedItems        = false;
    public        boolean mDisplayChat              = false;
    public        boolean mDisplayWindowsBackground = false;
    public        boolean mDisplayNotFreeCargoBoxes = false;
    public        boolean mDragWindowsAlways        = false;
    public        boolean mDisplayNotifications     = false;
    public        boolean mHoverShips               = false;
    public        boolean mDisplayDrones            = false;
    public        boolean mDisplayBonusBoxes        = false;
    public        boolean mDisplayFreeCargoBoxes    = false;

    public DisplaySettingsModule(boolean pNotSet, boolean pDisplayPlayerNames, boolean pDisplayResources,
                                 boolean pDisplayBonusBoxes, boolean pDisplayHitpointBubbles, boolean pDisplayChat,
                                 boolean pDisplayDrones, boolean pDisplayFreeCargoBoxes,
                                 boolean pDisplayNotFreeCargoBoxes, boolean pShowNotOwnedItems,
                                 boolean pDisplayWindowsBackground, boolean pDisplayNotifications,
                                 boolean pPreloadUserShips, boolean pDragWindowsAlways, boolean pHoverShips,
                                 boolean pShowPremiumQuickslotBar, boolean pAllowAutoQuality) {
        this.mNotSet = pNotSet;
        this.mDisplayPlayerNames = pDisplayPlayerNames;
        this.mDisplayResources = pDisplayResources;
        this.mDisplayBonusBoxes = pDisplayBonusBoxes;
        this.mDisplayHitpointBubbles = pDisplayHitpointBubbles;
        this.mDisplayChat = pDisplayChat;
        this.mDisplayDrones = pDisplayDrones;
        this.mDisplayFreeCargoBoxes = pDisplayFreeCargoBoxes;
        this.mDisplayNotFreeCargoBoxes = pDisplayNotFreeCargoBoxes;
        this.mShowNotOwnedItems = pShowNotOwnedItems;
        this.mDisplayWindowsBackground = pDisplayWindowsBackground;
        this.mDisplayNotifications = pDisplayNotifications;
        this.mPreloadUserShips = pPreloadUserShips;
        this.mDragWindowsAlways = pDragWindowsAlways;
        this.mHoverShips = pHoverShips;
        this.mShowPremiumQuickslotBar = pShowPremiumQuickslotBar;
        this.mAllowAutoQuality = pAllowAutoQuality;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 17;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeBoolean(this.mHoverShips);
            param1.writeShort(32677);
            param1.writeBoolean(this.mDisplayWindowsBackground);
            param1.writeBoolean(this.mAllowAutoQuality);
            param1.writeBoolean(this.mShowNotOwnedItems);
            param1.writeShort(-13272);
            param1.writeBoolean(this.mDisplayChat);
            param1.writeBoolean(this.mNotSet);
            param1.writeBoolean(this.mDisplayFreeCargoBoxes);
            param1.writeBoolean(this.mDisplayNotifications);
            param1.writeBoolean(this.mDisplayDrones);
            param1.writeBoolean(this.mDisplayBonusBoxes);
            param1.writeBoolean(this.mDisplayResources);
            param1.writeBoolean(this.mDisplayHitpointBubbles);
            param1.writeBoolean(this.mDragWindowsAlways);
            param1.writeBoolean(this.mPreloadUserShips);
            param1.writeBoolean(this.mDisplayPlayerNames);
            param1.writeBoolean(this.mShowPremiumQuickslotBar);
            param1.writeBoolean(this.mDisplayNotFreeCargoBoxes);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}