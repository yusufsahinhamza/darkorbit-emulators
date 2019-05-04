package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.map_entities.movable.Player;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.UIOpenCommand;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class UIOpenCommandHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final UIOpenCommand              mUIOpenCommand;

    public UIOpenCommandHandler(final GameServerClientConnection pGameServerClientConnection,
                                final ClientCommand pUIOpenCommand) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mUIOpenCommand = (UIOpenCommand) pUIOpenCommand;
    }

    @Override
    public void execute() {
        Log.pt("UI OPEN COMMAND HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        final long currentTime = System.currentTimeMillis();
        final Player player = gameSession.getPlayer();
        if (gameSession != null) {
            switch (this.mUIOpenCommand.getAction()) {

                case UIOpenCommand.ACTION_LOGOUT:
                	player.startLogoutProcess(); //Çıkış  
                    break;

                case UIOpenCommand.ACTION_SHIP_WARP:
    	        	if(player.isInSecureZone()) {
    	        		if(!player.getLaserAttack().isAttackInProgress()) {
    	        			if((currentTime - player.getLastDamagedTime()) >= 10000) {
                		player.sendCommandToBoundSessions(player.getShipWarpWindowCommand());
    	        			} else {
    	        				player.sendPacketToBoundSessions("0|A|STD|Alınan son hasardan 10 saniye sonra gemi değiştirme kullanılabilir!");
    	        			}
    	        		} else {
            				player.sendPacketToBoundSessions("0|A|STD|Saldırırken gemi değiştiremezsin!");
            			}
    	        	} else {
        				player.sendPacketToBoundSessions("0|A|STD|Gemi değiştirme yalnızca güvenli bölgelerde mümkündür!");
        			}
                    break;

                default:
                    break;
            }
        }
    }   
}
