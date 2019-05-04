package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.map_entities.movable.Player;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.ShipWarpRequest;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.system.ships.ShipFactory;
import utils.Log;

/**
 Created by LEJYONER on 07/01/2018.
 */

public class ShipWarpRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final ShipWarpRequest          mShipWarpRequest;

    public ShipWarpRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                    final ClientCommand pShipWarpRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mShipWarpRequest = (ShipWarpRequest) pShipWarpRequest;
    }

    @Override
    public void execute() {
    	Log.pt("SHIP WARP REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        final long currentTime = System.currentTimeMillis();
        Thread mWarpThread = null;
        if (gameSession != null) {
        	final Player player = gameSession.getPlayer();
        	if(mShipWarpRequest.shipID != player.getPlayerShip().getShipId()) {
	        			player.sendPacketToBoundSessions("0|A|STM|msg_swstarted");
	        	        if (mWarpThread == null || !mWarpThread.isAlive()) {
	        	        	mWarpThread = new Thread() {
	        	                public void run() {
	        	                    try {
	        	                        int i = 11;
	        	                        while (true) {
	        	                        	if(!player.getMovement().isMovementInProgress()) {
		        	                        	if(i == 3) {
		        	                        		final VisualModifierCommand visualModifierCommand = new VisualModifierCommand(gameSession.getPlayer().getAccount().getUserId(), VisualModifierCommand.SHIP_WARP, 0, "", 1, true);
		        	                        		player.sendCommandToBoundSessions(visualModifierCommand);
		        	                        		player.sendCommandToInRange(visualModifierCommand);     
		        	                        	}
		        	                            if (i <= 0) {
		        	                            	final VisualModifierCommand visualModifierCommand = new VisualModifierCommand(gameSession.getPlayer().getAccount().getUserId(), VisualModifierCommand.DIVERSE, 0, "", 1, true);
		        	                            	player.sendCommandToBoundSessions(visualModifierCommand);
		        	                            	player.sendCommandToInRange(visualModifierCommand);                   	
		        	                            	player.changePlayerShip(ShipFactory.getPlayerShip(mShipWarpRequest.shipID));
		        	                            	player.sendPacketToBoundSessions("0|A|STM|msg_switchship_success");
		        	                            	QueryManager.saveShip(player.getAccount());
		        	                                break;
		        	                            }
		        	                            Thread.sleep(1000);
		        	                            i--;
		        	                            player.sendPacketToBoundSessions("0|A|STD|Değiştirme süresi: "+i+"");
	        	                        } else {
	        	                        	player.sendPacketToBoundSessions("0|A|STM|msg_swblocked");
	        	                        	break;
	        	                        }
	        	                        }
	        	                    } catch (InterruptedException e) {
	        	                        
	        	                    }
	        	                }
	        	            };
	        	            mWarpThread.start();
	        	        }
	        			
	        		
	        	
        	}
    }   
}
}
