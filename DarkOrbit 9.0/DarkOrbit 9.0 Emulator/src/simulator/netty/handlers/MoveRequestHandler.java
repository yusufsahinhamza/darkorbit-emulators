package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.MoveRequest;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class MoveRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final MoveRequest                mMoveRequest;

    public MoveRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                              final ClientCommand pMoveRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mMoveRequest = (MoveRequest) pMoveRequest;
    }

    @Override
    public void execute() {
        Log.pt("MOVE REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final int posX = this.mMoveRequest.positionX;
            final int posY = this.mMoveRequest.positionY;
            final int targetX = this.mMoveRequest.targetX;
            final int targetY = this.mMoveRequest.targetY;

            gameSession.getPlayer()
                       .initiateMovement(posX, posY, targetX, targetY);
        }
    }
}
