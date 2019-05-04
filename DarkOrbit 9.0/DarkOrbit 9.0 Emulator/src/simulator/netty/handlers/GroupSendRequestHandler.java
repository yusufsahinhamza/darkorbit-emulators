package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.GameManager;
import simulator.map_entities.movable.Player;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.GroupSendRequest;
import simulator.netty.serverCommands.GroupSendCommand;
import utils.Log;

/**
 Created by LEJYONER on 23/09/2017.
 */

public class GroupSendRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final GroupSendRequest           mGroupSendRequest;

    public GroupSendRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pGroupSendRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mGroupSendRequest = (GroupSendRequest) pGroupSendRequest;
    }

    @Override
    public void execute() {
        Log.pt("GROUP SEND REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
        	gameSession.getPlayer().sendCommandToBoundSessions(new GroupSendCommand(this.mGroupSendRequest.name));  
        	
        	for(final GameSession gameSessions : GameManager.getGameSessions()) {
        		if(gameSessions != null) {
        			final Player player = gameSessions.getPlayer();
        			if(player != null) {
        				if(player.getAccount().getShipUsername() == this.mGroupSendRequest.name) {
        					player.sendPacketToBoundSessions("0|A|STD|"+gameSession.getAccount().getShipUsername()+" size davetiye gönderdi!");
        				}
        			}
        		}
        	}
        	
        }
    }
}
