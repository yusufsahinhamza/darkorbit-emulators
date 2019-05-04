package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.AssetHandleClickRequest;

/**
 Created by Pedro on 09-04-2015.
 */
public class AssetHandleClickHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final AssetHandleClickRequest    mAssetHandleClickRequest;

    public AssetHandleClickHandler(final GameServerClientConnection pGameServerClientConnection,
                                   final ClientCommand pAssetHandleClickRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mAssetHandleClickRequest = (AssetHandleClickRequest) pAssetHandleClickRequest;
    }

    @Override
    public void execute() {
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ActivatableStationaryMapEntity activatableStationaryMapEntity = gameSession.getPlayer()
                                                                                             .getCurrentSpaceMap()
                                                                                             .getActivatableMapEntity(
                                                                                                     this.mAssetHandleClickRequest.mAssetId);
            if (activatableStationaryMapEntity != null) {
                activatableStationaryMapEntity.handleClick(gameSession);
            }
        }
    }
}
