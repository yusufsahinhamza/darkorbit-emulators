package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import utils.Log;

/**
 Created by LEJYONER on 18/09/2017.
 */

public class LogoutCancelRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;

    public LogoutCancelRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                      final ClientCommand pLogoutCancelRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
        Log.pt("LOGOUT CANCEL REQUEST HANDLER");

        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            gameSession.getPlayer().stopLogoutProcess();
        }
    }

}
