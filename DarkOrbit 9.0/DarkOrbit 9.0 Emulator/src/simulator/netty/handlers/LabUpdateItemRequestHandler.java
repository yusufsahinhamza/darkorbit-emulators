package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.LabItemModule;
import simulator.netty.clientCommands.LabUpdateItemRequest;
import simulator.users.ResourcesManager;
import utils.Log;

/**
 Created by Pedro on 02-04-2015.
 */
public class LabUpdateItemRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final LabUpdateItemRequest       mLabUpdateItemRequest;

    public LabUpdateItemRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                       final ClientCommand pLabUpdateItemRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mLabUpdateItemRequest = (LabUpdateItemRequest) pLabUpdateItemRequest;
    }

    @Override
    public void execute() {
        Log.pt("LAB UPDATE ITEM REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ResourcesManager resourcesManager = gameSession.getAccount()
                                                                 .getResourcesManager();
            switch (this.mLabUpdateItemRequest.itemToUpdate.itemValue) {
                case LabItemModule.LASER:
                    resourcesManager.upgradeLasers(this.mLabUpdateItemRequest.updateWith.oreType,
                                                   this.mLabUpdateItemRequest.updateWith.count.longValue());
                    break;
                case LabItemModule.ROCKETS:
                    resourcesManager.upgradeRockets(this.mLabUpdateItemRequest.updateWith.oreType,
                                                    this.mLabUpdateItemRequest.updateWith.count.longValue());
                    break;
                case LabItemModule.DRIVING:
                    resourcesManager.upgradeGenerators(this.mLabUpdateItemRequest.updateWith.oreType,
                                                       this.mLabUpdateItemRequest.updateWith.count.longValue());
                    break;
                case LabItemModule.SHIELD:
                    resourcesManager.upgradeShields(this.mLabUpdateItemRequest.updateWith.oreType,
                                                    this.mLabUpdateItemRequest.updateWith.count.longValue());
                    break;
            }
        }
    }
}
