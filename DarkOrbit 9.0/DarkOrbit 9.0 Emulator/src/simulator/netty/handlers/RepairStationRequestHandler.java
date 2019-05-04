package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.map_entities.movable.Player;
import simulator.netty.ClientCommand;
import utils.Log;

/**
 Created by LEJYONER on 16/09/2017.
 */

public class RepairStationRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;

    public RepairStationRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pAttackAbortLaserRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
    	Log.pt("REPAIR STATION REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        final long currentTime = System.currentTimeMillis();
        final Player player = gameSession.getPlayer();
        if (gameSession != null) {
        	if (player.getCurrentHitPoints() != player.getMaximumHitPoints()) {
            if ((currentTime - player.getLastDamagedTime()) >= 10000) {
            int heal = player.getMaximumHitPoints();
            player.healEntity(heal, player.HEAL_HITPOINTS);
            player.sendPacketToBoundSessions("0|A|STD|İstasyon tamiri tamamlandı!");
            } else {
            	final String petRepairModuleWarning = "0|A|STD|Alınan son hasardan 10 saniye sonra İstasyon Tamiri kullanılabilir!";
                player.sendPacketToBoundSessions(petRepairModuleWarning);
    		}
        	} else {
            	final String petRepairModuleWarning = "0|A|STD|Zaten maximum darbe-puanına sahipsin!";
                player.sendPacketToBoundSessions(petRepairModuleWarning);
    		}
        }
    }
}
