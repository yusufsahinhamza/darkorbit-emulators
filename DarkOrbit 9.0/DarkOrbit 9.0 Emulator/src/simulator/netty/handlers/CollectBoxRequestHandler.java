package simulator.netty.handlers;

import java.util.Random;
import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.map_entities.movable.Player;
import simulator.netty.ClientCommand;
import simulator.users.Account;
import utils.Log;
import utils.Settings;

/**
Created by LEJYONER on 15/09/2017.
*/

public class CollectBoxRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    
    public CollectBoxRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pCollectBoxRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    public void execute() {
        Log.pt("COLLECT BOX REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        final Account account = gameSession.getAccount();
        final Player player = account.getPlayer();      
        if (gameSession != null) { 
        	if(player != null) {
        		String boxHash = player.boxHash;
        		int alienID = player.alienID;        		
        		if(boxHash != "" && alienID != 0) {
		        	player.sendPacketToBoundSessions("0|2|" + boxHash + "");

		        	boolean shouldContinue = true;

		        	int activeLetterCount = 0;
		            for(boolean active : account.puzzleLetters.values()){      			               
		                if(active) {
		                	activeLetterCount++;			                	
		                }        
		    		}
		            
		    		while(shouldContinue) {		    			
			        	Random randomGenerator = new Random();
			    		int letter = randomGenerator.nextInt(Settings.harfSayisi - 1 + 1) + 1; 			    		
			    		
			            if(activeLetterCount == Settings.harfSayisi) {
			            	for (int i = 10; i > 0; i--) {	        	
			                	account.puzzleLetters.put("puzzleLetter"+ i +"", false);	
			                }
			            	account.puzzleLetters.put("puzzleLetter"+ letter +"", true);
			            	player.sendCommandToBoundSessions(player.getWordPuzzleLetterAchievedCommand(false));
		    				shouldContinue = false;
		    				break;
			            } else {
		    				if(!account.getPuzzleLetter(letter)) {
			    				account.puzzleLetters.put("puzzleLetter"+ letter +"", true);
			    				player.sendCommandToBoundSessions(player.getWordPuzzleLetterAchievedCommand(activeLetterCount + 1 == Settings.harfSayisi ? true : false));		    				
			    				if(Settings.harfSayisi - 1 == activeLetterCount) {
			    					player.sendPacketToBoundSessions("0|A|STD|"+ Settings.harfSayisi +" Indoctrine-Oil aldın.");
			    					player.getAccount().changeIOil(Settings.harfSayisi);
			    				}			
			    				player.getAccount().changeIOil(Settings.harfSayisi);
			    				shouldContinue = false;
			    				break;		    				
		    				}		    				
			            }	            
		    		}

		        	QueryManager.saveAccount(player.getAccount());
		        	player.alienID = 0;
		        	player.boxHash = "";
        		}
        	}
        }
    }
}
