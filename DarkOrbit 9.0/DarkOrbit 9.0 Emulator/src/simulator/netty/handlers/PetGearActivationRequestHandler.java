package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.PetGearActivationRequest;
import simulator.netty.serverCommands.PetGearTypeModule;
import simulator.users.Account;
import utils.Log;

/**
 Created by LEJYONER on 23/09/2017.
 */

public class PetGearActivationRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final PetGearActivationRequest   mPetGearActivationRequest;

    public PetGearActivationRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pPetGearActivationRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mPetGearActivationRequest = (PetGearActivationRequest) pPetGearActivationRequest;
    }

    public void execute() {
        Log.pt("SELECT GEAR REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
        	final Account account = gameSession.getAccount();    
        	if(account.isAdmin() || account.havePet()) {
        		switch(mPetGearActivationRequest.gearID) {
    				case PetGearTypeModule.PASSIVE:
    					account.getPetManager().PassiveMode();
    					break;        		
        			case PetGearTypeModule.GUARD:
        				account.getPetManager().GuardMode();
        				break;
        			case PetGearTypeModule.COMBO_SHIP_REPAIR:
        				account.getPetManager().ComboShipRepair();
        				break;
        			case PetGearTypeModule.KAMIKAZE:
        				account.getPetManager().KamikazeModule();
        				break;
        		}
        	}
        }
    }
}
