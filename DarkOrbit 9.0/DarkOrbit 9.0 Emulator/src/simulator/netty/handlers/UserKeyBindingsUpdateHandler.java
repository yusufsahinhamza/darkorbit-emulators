package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.UserKeyBindingsModule;
import simulator.netty.clientCommands.UserKeyBindingsUpdate;
import simulator.users.ClientSettings;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class UserKeyBindingsUpdateHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final UserKeyBindingsUpdate      mUserKeyBindingsUpdate;

    public UserKeyBindingsUpdateHandler(final GameServerClientConnection pGameServerClientConnection,
                                        final ClientCommand pUserKeyBindingsUpdate) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mUserKeyBindingsUpdate = (UserKeyBindingsUpdate) pUserKeyBindingsUpdate;
    }

    @Override
    public void execute() {
        Log.pt("USER KEY BINDINGS UPDATE HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ClientSettings.Keys keys = gameSession.getAccount()
                                                        .getClientSettingsManager()
                                                        .getClientSettings()
                                                        .getKeys();
            keys.clearActions();
            for (UserKeyBindingsModule action : this.mUserKeyBindingsUpdate.changedKeyBindings) {
                keys.addAction(action.actionType, action.charCode, action.parameter, action.keyCodes);
            }
            QueryManager.saveAccount(gameSession.getAccount());
        }
    }
}
