package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.SlotBarConfigSetRequest;
import simulator.users.ClientSettingsManager;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class SlotBarConfigSetRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final SlotBarConfigSetRequest    mSlotBarConfigSetRequest;

    public SlotBarConfigSetRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pSlotBarConfigSetRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mSlotBarConfigSetRequest = (SlotBarConfigSetRequest) pSlotBarConfigSetRequest;
    }

    @Override
    public void execute() {
        Log.pt("SLOT BAR CONFIG SET REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ClientSettingsManager clientSettingsManager = gameSession.getAccount()
                                                                           .getClientSettingsManager();
            if (this.mSlotBarConfigSetRequest.mFromIndex != 0) {
                switch (this.mSlotBarConfigSetRequest.mFromSlotBarId) {
                    case ClientSettingsManager.STANDARD_SLOT_BAR:
                        clientSettingsManager.removeSlotBarItem((short) this.mSlotBarConfigSetRequest.mFromIndex);
                        break;
                    case ClientSettingsManager.PREMIUM_SLOT_BAR:
                        clientSettingsManager.removePremiumSlotBarItem(
                                (short) this.mSlotBarConfigSetRequest.mFromIndex);
                        break;
                }
            }
            if (this.mSlotBarConfigSetRequest.mToIndex != 0) {
                switch (this.mSlotBarConfigSetRequest.mToSlotBarId) {
                    case ClientSettingsManager.STANDARD_SLOT_BAR:
                        clientSettingsManager.addSlotBarItem((short) this.mSlotBarConfigSetRequest.mToIndex,
                                                             this.mSlotBarConfigSetRequest.mItemId);
                        QueryManager.saveAccount(gameSession.getAccount());
                        break;
                    case ClientSettingsManager.PREMIUM_SLOT_BAR:
                        clientSettingsManager.addPremiumSlotBarItem((short) this.mSlotBarConfigSetRequest.mToIndex,
                                                                    this.mSlotBarConfigSetRequest.mItemId);
                        QueryManager.saveAccount(gameSession.getAccount());
                        break;
                }
            }
            this.mGameServerClientConnection.sendToSendCommand(clientSettingsManager.getClientUISlotBarsCommand());
        }
    }
}
