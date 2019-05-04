package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class AttackAbortLaserRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;

    public AttackAbortLaserRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pAttackAbortLaserRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
        Log.pt("ATTACK ABORT LASER REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            gameSession.getPlayer()
                       .getLaserAttack()
                       .setAttackInProgress(false);
        }
    }
}
