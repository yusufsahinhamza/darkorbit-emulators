package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.ShipSelectRequest;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class ShipSelectRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final ShipSelectRequest          mShipSelectRequest;

    public ShipSelectRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                    final ClientCommand pShipSelectRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mShipSelectRequest = (ShipSelectRequest) pShipSelectRequest;
    }

    @Override
    public void execute() {
        Log.pt("SHIP SELECT REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {

            gameSession.getPlayer()
                       .selectShip(this.mShipSelectRequest.targetID);
        }
    }
}
