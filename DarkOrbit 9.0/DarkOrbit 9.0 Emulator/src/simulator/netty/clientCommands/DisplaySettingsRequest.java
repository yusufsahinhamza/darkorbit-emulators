package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class DisplaySettingsRequest
        extends ClientCommand {

    public static final int     ID                       = 25227;
    public              boolean displayNotFreeCargoBoxes = false;
    public              boolean displayResources         = false;
    public              boolean displayWindowsBackground = false;
    public              boolean displayBonusBoxes        = false;
    public              boolean displayNotifications     = false;
    public              boolean showPremiumQuickslotBar  = false;
    public              boolean displayPlayerNames       = false;
    public              boolean dragWindowsAlways        = false;
    public              boolean displayHitpointBubbles   = false;
    public              boolean displayDrones            = false;
    public              boolean preloadUserShips         = false;
    public              boolean hoverShips               = false;
    public              boolean allowAutoQuality         = false;
    public              boolean displayChat              = false;
    public              boolean displayFreeCargoBoxes    = false;
    public              boolean showNotOwnedItems        = false;

    public DisplaySettingsRequest(DataInputStream pIn) {
        super(pIn, ID);
    }

    public void read() {
        try {
            this.displayWindowsBackground = in.readBoolean();
            this.dragWindowsAlways = in.readBoolean();
            this.hoverShips = in.readBoolean();
            this.displayNotifications = in.readBoolean();
            this.displayNotFreeCargoBoxes = in.readBoolean();
            this.displayBonusBoxes = in.readBoolean();
            this.displayDrones = in.readBoolean();
            this.displayResources = in.readBoolean();
            this.displayChat = in.readBoolean();
            this.showPremiumQuickslotBar = in.readBoolean();
            this.displayFreeCargoBoxes = in.readBoolean();
            this.allowAutoQuality = in.readBoolean();
            this.displayPlayerNames = in.readBoolean();
            this.showNotOwnedItems = in.readBoolean();
            this.preloadUserShips = in.readBoolean();
            this.displayHitpointBubbles = in.readBoolean();
        } catch (IOException e) {
        }
    }
}