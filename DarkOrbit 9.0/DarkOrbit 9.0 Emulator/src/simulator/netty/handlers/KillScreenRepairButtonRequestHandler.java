package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.system.SpaceMap;
import storage.SpaceMapStorage;
import utils.Log;

/**
 Created by LEJYONER on 08/10/2017.
 */

public class KillScreenRepairButtonRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;

    public KillScreenRepairButtonRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pAttackAbortLaserRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
        Log.pt("KILL SCREEN REPAIR BUTTON REQUEST");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            try{
        	final long currentTime = System.currentTimeMillis(); 
        	gameSession.getPlayer().removeAllInRangeMapIntities();
            gameSession.getPlayer().setCurrentInRangePortalId(-1);
            gameSession.getPlayer().setJumping(false);
        	gameSession.getPlayer().getMovement()
      	  		   .setMovementInProgress(false);
        	gameSession.getPlayer().getCurrentSpaceMap()
      	  		  .removePlayer(gameSession.getPlayer().getMapEntityId());
        	gameSession.getPlayer().setDestroyed(false);
        	gameSession.getPlayer().setLastSendKillScreenTime(currentTime);
        	gameSession.getPlayer().setCurrentHitPoints(gameSession.getPlayer().getMaximumHitPoints());
        	gameSession.getPlayer().setCurrentShieldPointsConfig1(gameSession.getPlayer().getMaximumShieldPoints());
        	gameSession.getPlayer().setCurrentShieldPointsConfig2(gameSession.getPlayer().getMaximumShieldPoints());
        	if(gameSession.getPlayer().getCurrentSpaceMapId() == 13 || gameSession.getPlayer().getCurrentSpaceMapId() == 14
        	   || gameSession.getPlayer().getCurrentSpaceMapId() == 15 || gameSession.getPlayer().getCurrentSpaceMapId() == 16) {	        	   
            	if(gameSession.getAccount().getFactionId() == 1) {
            		gameSession.getPlayer().setPositionXY(1600, 1600);
            		gameSession.getPlayer().setCurrentSpaceMap((short) 13);
            	} else if(gameSession.getAccount().getFactionId() == 2) {
            		gameSession.getPlayer().setPositionXY(19500, 1500);
            		gameSession.getPlayer().setCurrentSpaceMap((short) 14);
            	} else if(gameSession.getAccount().getFactionId() == 3) {
            		gameSession.getPlayer().setPositionXY(19500, 11600);
            		gameSession.getPlayer().setCurrentSpaceMap((short) 15);
            	}	
        	} else {
            	if(gameSession.getAccount().getFactionId() == 1) {
            		gameSession.getPlayer().setPositionXY(2000, 6000);
            		gameSession.getPlayer().setCurrentSpaceMap((short) 20);
            	} else if(gameSession.getAccount().getFactionId() == 2) {
            		gameSession.getPlayer().setPositionXY(10000, 2000);
            		gameSession.getPlayer().setCurrentSpaceMap((short) 24);
            	} else if(gameSession.getAccount().getFactionId() == 3) {
            		gameSession.getPlayer().setPositionXY(18500, 6000);
            		gameSession.getPlayer().setCurrentSpaceMap((short) 28);
            	}
        	}       	
        	final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(gameSession.getPlayer().getCurrentSpaceMapId());
			spaceMap.addAndInitGameSession(gameSession);
            QueryManager.saveAccount(gameSession.getAccount());
            }catch(Exception e) {
            }
        }
    }
}
