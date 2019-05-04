package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class AttackRocketRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;

    public AttackRocketRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                      final ClientCommand pAttackRocketRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
        Log.pt("ATTACK ROCKET REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            gameSession.getPlayer()
                       .getRocketAttack()
                       .attack();
        }
    }
}
