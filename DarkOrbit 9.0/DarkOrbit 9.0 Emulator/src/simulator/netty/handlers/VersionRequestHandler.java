package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import net.game_server.logic.Login;

import simulator.GameManager;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.VersionRequest;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class VersionRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final VersionRequest             mVersionRequest;

    public VersionRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                 final ClientCommand pVersionRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mVersionRequest = (VersionRequest) pVersionRequest;
    }

    @Override
    public void execute() {
        Log.pt("VERSION REQUEST HANDLER");

        final int userID = this.mVersionRequest.major;
        final String sessionID = this.mVersionRequest.minor;

        final GameSession gameSession = Login.login(this.mGameServerClientConnection, userID, sessionID);
        if (gameSession != null) {
            GameManager.addGameSession(gameSession);
            this.mGameServerClientConnection.setGameSession(gameSession);
        }
    }
}
