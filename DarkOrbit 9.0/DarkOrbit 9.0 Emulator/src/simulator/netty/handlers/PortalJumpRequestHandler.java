package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.map_entities.stationary.Portal;
import simulator.netty.ClientCommand;
import simulator.system.SpaceMap;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class PortalJumpRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    
    public PortalJumpRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                    final ClientCommand pPortalJumpRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
        Log.pt("PORTAL JUMP REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        final long currentTime = System.currentTimeMillis();
        if (gameSession != null) {
        	final SpaceMap spaceMap = gameSession.getPlayer().getCurrentSpaceMap();
            final ActivatableStationaryMapEntity activatableStationaryMapEntity = gameSession.getPlayer()
                                                                                             .getCurrentSpaceMap()
                                                                                             .getActivatableMapEntity(
                                                                                                     gameSession.getPlayer()            
                                                                                                     .getCurrentInRangePortalId());         
            final Portal portalMapEntity = (Portal) gameSession.getPlayer()
                    .getCurrentSpaceMap()
                    .getActivatableMapEntity(
                            gameSession.getPlayer()            
                            .getCurrentInRangePortalId());
            
            if (activatableStationaryMapEntity != null && portalMapEntity != null) {
            	if(spaceMap.isPvpMap()) {
	                if ((currentTime - gameSession.getPlayer().getLastDamagedTime()) >= 10000) {
	                	portalMapEntity.handleClick(gameSession);
	                } else {
	                	final String jumpError = "0|A|STM|jumpgate_failed_pvp_map";
	                	gameSession.getPlayer().sendPacketToBoundSessions(jumpError);
	                }
            	} else {
            		portalMapEntity.handleClick(gameSession);
            	}
            } else {
            	final String warning = "0|A|STM|jumpgate_failed_no_gate";
            	gameSession.getPlayer().sendPacketToBoundSessions(warning);
            }
        }
    }
}