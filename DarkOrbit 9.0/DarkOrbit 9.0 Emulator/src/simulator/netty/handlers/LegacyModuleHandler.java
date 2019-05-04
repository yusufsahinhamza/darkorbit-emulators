package simulator.netty.handlers;

import net.ClientCommands;
import net.ServerCommands;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.LegacyModuleRequest;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class LegacyModuleHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final LegacyModuleRequest        mLegacyModuleRequest;

    public LegacyModuleHandler(final GameServerClientConnection pGameServerClientConnection,
                               final ClientCommand pLegacyModuleRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mLegacyModuleRequest = (LegacyModuleRequest) pLegacyModuleRequest;
    }

    @Override
    public void execute() {
        Log.pt("LEGACY MODULE REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final String[] packet = this.mLegacyModuleRequest.message.split("\\|");
            switch (packet[0]) {
                case ServerCommands.SET_STATUS:
                    switch (packet[1]) {
                        case ClientCommands.CONFIGURATION:
                        	gameSession.getPlayer().changeConfiguration(Short.parseShort(packet[2]));
                            break;
                    }
                    break;
            }
        }
    }
}
