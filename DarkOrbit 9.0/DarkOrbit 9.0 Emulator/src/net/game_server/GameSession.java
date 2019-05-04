package net.game_server;

import mysql.QueryManager;
import net.chat_server.ChatClientConnection;
import net.utils.ServerUtils;
import simulator.GameManager;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.users.Account;
import simulator.system.SpaceMap;
import storage.SpaceMapStorage;

/**
 This class will hold all user data when user is in game
 */
public class GameSession {

    // user's account
    private Account mAccount;

    // user connection socket
    public final GameServerClientConnection mGameServerClientConnection;

    //chat connection socket
    public ChatClientConnection mChatClientConnection = null;

    private boolean mClosed = false;

    // XXX NOTE: based on Player object packets are sent to client
    // XXX => if we change Player object to another Account's one we may enter spectate mode
    // user's on-map object
    private Player mPlayer;

    public GameSession(final GameServerClientConnection pGameServerClientConnection, final Account pAccount) {

        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mAccount = pAccount;
        this.setPlayer(pAccount.getPlayer());

    }
    
    public void close() {
    	try {
    		if(!this.isClosed()) {
     
            	this.getAccount().setOnline(false);
            	this.getPlayer().setInEquipZone(true);  	    	   	
                QueryManager.saveAccount(this.getAccount());               
    	        
                if(this.getAccount().getPetManager().isActivated()) {
                	this.getAccount().getPetManager().Deactivate();
                }
                
                ServerUtils.sendCommandToAllInMap(this.getPlayer().getCurrentSpaceMapId(), new ShipRemoveCommand(this.getAccount().getUserId()));
                
                this.getPlayer().getCurrentSpaceMap().removePlayer(this.getAccount().getUserId());
                
			    this.getPlayer().unbindClosedGameSessions();
			    
			    this.setChatClientConnection(null);
			    
			    this.mClosed = true;			    
			    
				GameManager.removeGameSession(this.getAccount().getUserId());

			    this.getGameServerClientConnection().close();
			    
			    if(this.isClosed()) {
			    	System.out.println("GameSession kapatıldı!");
			    }
			    
    		}
    	} catch (Exception e) {
    		System.out.println("GameSession kapatılamadı!");
    	}
    }

    public Account getAccount() {
        return this.mAccount;
    }
    
    public void setAccount(final Account pAccount) {
        this.mAccount = pAccount;
    }
    
    public GameServerClientConnection getGameServerClientConnection() {
        return this.mGameServerClientConnection;
    }

    public ChatClientConnection getChatClientConnection() {
        return this.mChatClientConnection;
    }

    public void setChatClientConnection(final ChatClientConnection pChat) {
        this.mChatClientConnection = pChat;
    }

    public Player getPlayer() {
        return this.mPlayer;
    }
    
    // XXX experimental (for spectator mode)
    public void setPlayer(final Player pPlayer) {

        this.mPlayer = pPlayer;
        this.mPlayer.bindGameSession(this);

    }

    public boolean isClosed() {
        return this.mClosed;
    }
    
    public void tryJump(final short map){
        try{
            SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(this.getPlayer()
                    .getCurrentSpaceMapId());
            spaceMap.removePlayer(this.getPlayer().getMapEntityId());

            SpaceMapStorage.getSpaceMap(map)
                    .addAndInitGameSession(this);

        }catch(Exception e){
        	System.out.print("GameSession'daki tryJump sorunu");
        }
    }
}
