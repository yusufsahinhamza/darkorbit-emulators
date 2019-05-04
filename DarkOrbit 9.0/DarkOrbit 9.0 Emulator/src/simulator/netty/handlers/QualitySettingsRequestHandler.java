package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.QualitySettingsRequest;
import simulator.users.ClientSettings;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class QualitySettingsRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final QualitySettingsRequest     mQualitySettingsRequest;

    public QualitySettingsRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                         final ClientCommand pQualitySettingsRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mQualitySettingsRequest = (QualitySettingsRequest) pQualitySettingsRequest;
    }

    @Override
    public void execute() {
        Log.pt("QUALITY SETTINGS REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ClientSettings.Quality quality = gameSession.getAccount()
                                                              .getClientSettingsManager()
                                                              .getClientSettings()
                                                              .getQuality();
            quality.setNotSet(false);
            quality.setQualityEngine(this.mQualitySettingsRequest.qualityEngine);
            quality.setQualityEffect(this.mQualitySettingsRequest.qualityEffect);
            quality.setQualityCustomized(this.mQualitySettingsRequest.qualityCustomized);
            quality.setQualityCollectable(this.mQualitySettingsRequest.qualityCollectable);
            quality.setQualityPoizone(this.mQualitySettingsRequest.qualityPoizone);
            quality.setQualityPresetting(this.mQualitySettingsRequest.qualityPresetting);
            quality.setQualityBackground(this.mQualitySettingsRequest.qualityBackground);
            quality.setQualityAttack(this.mQualitySettingsRequest.qualityAttack);
            quality.setQualityExplosion(this.mQualitySettingsRequest.qualityExplosion);
            quality.setQualityShip(this.mQualitySettingsRequest.qualityShip);
            QueryManager.saveAccount(gameSession.getAccount());
        }
    }
}
