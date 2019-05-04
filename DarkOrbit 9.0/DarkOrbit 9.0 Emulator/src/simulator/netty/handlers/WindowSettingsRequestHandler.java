package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.WindowSettingsRequest;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class WindowSettingsRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final WindowSettingsRequest             mWindowSettingsRequest;

    public WindowSettingsRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                 final ClientCommand pWindowSettingsRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mWindowSettingsRequest = (WindowSettingsRequest) pWindowSettingsRequest;
    }

    @Override
    public void execute() {
    	Log.pt("REPAIR STATION REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
        	
        	Log.pt(""+this.mWindowSettingsRequest.miniHaritaBuyuklugu);
        	
        	 /**System.out.println("0|A|STD|SCALE: " + this.mWindowSettingsRequest.sagUstMenuPozisyonu + "|" + this.mWindowSettingsRequest.miniHaritaBuyuklugu + "|" + this.mWindowSettingsRequest.normalSlotCubuguPozisyonu
        			+ "|" + this.mWindowSettingsRequest.solUstMenuPozisyonu + "|" + this.mWindowSettingsRequest.solUstMenuSiralamasi + "|" + this.mWindowSettingsRequest.premiumSlotCubuguPozisyonu + "|" + this.mWindowSettingsRequest.premiumSlotCubuguSiralamasi
        			+ "|" + this.mWindowSettingsRequest.normalSlotCubuguSiralamasi + "|" + this.mWindowSettingsRequest.kategoriCubuguPozisyonu + "|" + this.mWindowSettingsRequest.bilmiyorum11 + "|" + this.mWindowSettingsRequest.sagUstMenuSiralamasi
        			+ "|" + this.mWindowSettingsRequest.gemiPenceresiAyarlari);
        			*/
        }
    }
}
