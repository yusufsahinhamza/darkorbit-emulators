package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.DisplaySettingsRequest;
import simulator.users.ClientSettings;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class DisplaySettingsRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final DisplaySettingsRequest     mDisplaySettingsRequest;

    public DisplaySettingsRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                         final ClientCommand pDisplaySettingsRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mDisplaySettingsRequest = (DisplaySettingsRequest) pDisplaySettingsRequest;
    }

    @Override
    public void execute() {
        Log.pt("DISPLAY SETTINGS REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ClientSettings.Display display = gameSession.getAccount()
                                                              .getClientSettingsManager()
                                                              .getClientSettings()
                                                              .getDisplay();
            display.setNotSet(false);
            display.setDisplayNotFreeCargoBoxes(this.mDisplaySettingsRequest.displayNotFreeCargoBoxes);
            display.setDisplayResources(this.mDisplaySettingsRequest.displayResources);
            display.setDisplayWindowsBackground(this.mDisplaySettingsRequest.displayWindowsBackground);
            display.setDisplayBonusBoxes(this.mDisplaySettingsRequest.displayBonusBoxes);
            display.setDisplayNotifications(this.mDisplaySettingsRequest.displayNotifications);
            display.setShowPremiumQuickslotBar(this.mDisplaySettingsRequest.showPremiumQuickslotBar);
            display.setDisplayPlayerNames(this.mDisplaySettingsRequest.displayPlayerNames);
            display.setDragWindowsAlways(this.mDisplaySettingsRequest.dragWindowsAlways);
            display.setDisplayHitpointBubbles(this.mDisplaySettingsRequest.displayHitpointBubbles);
            display.setDisplayDrones(this.mDisplaySettingsRequest.displayDrones);
            display.setPreloadUserShips(this.mDisplaySettingsRequest.preloadUserShips);
            display.setHoverShips(this.mDisplaySettingsRequest.hoverShips);
            display.setAllowAutoQuality(this.mDisplaySettingsRequest.allowAutoQuality);
            display.setDisplayChat(this.mDisplaySettingsRequest.displayChat);
            display.setDisplayFreeCargoBoxes(this.mDisplaySettingsRequest.displayFreeCargoBoxes);
            display.setShowNotOwnedItems(this.mDisplaySettingsRequest.showNotOwnedItems);
            QueryManager.saveAccount(gameSession.getAccount());
        }
    }
}
