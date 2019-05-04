package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.LabRefinementRequest;
import simulator.netty.clientCommands.OreTypeModule;
import simulator.users.ResourcesManager;
import utils.Log;

/**
 Created by Pedro on 02-04-2015.
 */
public class LabRefinementRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final LabRefinementRequest       mLabRefinementRequest;

    public LabRefinementRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                       final ClientCommand pLabRefinementRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mLabRefinementRequest = (LabRefinementRequest) pLabRefinementRequest;
    }

    @Override
    public void execute() {
        Log.pt("LAB REFINEMENT REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ResourcesManager resourcesManager = gameSession.getAccount()
                                                                 .getResourcesManager();
            switch (this.mLabRefinementRequest.toProduce.oreType.typeValue) {
                case OreTypeModule.PROMETID:
                    resourcesManager.refinePrometid(this.mLabRefinementRequest.toProduce.count.longValue());
                    break;
                case OreTypeModule.DURANIUM:
                    resourcesManager.refineDuranium(this.mLabRefinementRequest.toProduce.count.longValue());
                    break;
                case OreTypeModule.PROMERIUM:
                    resourcesManager.refinePromerium(this.mLabRefinementRequest.toProduce.count.longValue());
                    break;
                default:
                    //no more types can be refined
                    break;
            }
        }
    }
}
