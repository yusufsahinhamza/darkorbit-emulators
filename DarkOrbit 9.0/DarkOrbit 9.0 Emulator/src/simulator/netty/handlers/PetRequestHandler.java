package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.PetRequest;
import simulator.users.Account;
import utils.Log;

/**
 Created by LEJYONER on 21/09/2017.
 */

public class PetRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final PetRequest 			 	 mPetRequest;
    
    public PetRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pPetRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mPetRequest = (PetRequest) pPetRequest;
        
    }

    @Override
    public void execute() {
    	Log.pt("ACTIVE PET REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
        	final Account account = gameSession.getAccount();       	
        	if(account.isAdmin() || account.havePet()) {
        		switch(mPetRequest.petRequestType) {
        			case PetRequest.LAUNCH:
        				account.getPetManager().Activate();
        				break;
        			case PetRequest.TOGGLE_ACTIVATION:
        				account.getPetManager().Activate();
        				break;
        			case PetRequest.DEACTIVATE:
        				account.getPetManager().Deactivate();
        				break;
        			case PetRequest.HOTKEY_GUARD_MODE:
        				account.getPetManager().GuardMode();
        			case PetRequest.HOTKEY_REPAIR_SHIP:
        				account.getPetManager().ComboShipRepair();
        				break;
        		}
        	}
        }
    }
}
