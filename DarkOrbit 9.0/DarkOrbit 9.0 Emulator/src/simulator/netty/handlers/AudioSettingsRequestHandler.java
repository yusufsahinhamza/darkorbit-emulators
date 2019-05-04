package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.AudioSettingsRequest;
import simulator.users.ClientSettings;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class AudioSettingsRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final AudioSettingsRequest       mAudioSettingsRequest;

    public AudioSettingsRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                       final ClientCommand pAudioSettingsRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mAudioSettingsRequest = (AudioSettingsRequest) pAudioSettingsRequest;
    }

    @Override
    public void execute() {
        Log.pt("AUDIO SETTINGS REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ClientSettings.Audio audio = gameSession.getAccount()
                                                          .getClientSettingsManager()
                                                          .getClientSettings()
                                                          .getAudio();
            audio.setNotSet(false);
            audio.setMusic(this.mAudioSettingsRequest.music);
            audio.setSound(this.mAudioSettingsRequest.sound);
            audio.setVoice(this.mAudioSettingsRequest.voice);
            audio.setPlayCombatMusic(this.mAudioSettingsRequest.playCombatMusic);
            QueryManager.saveAccount(gameSession.getAccount());
        }
    }
}
