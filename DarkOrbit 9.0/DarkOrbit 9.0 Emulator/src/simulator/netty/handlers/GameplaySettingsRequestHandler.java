package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.GameplaySettingsRequest;
import simulator.users.ClientSettings;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class GameplaySettingsRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final GameplaySettingsRequest    mGameplaySettingsRequest;

    public GameplaySettingsRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                          final ClientCommand pGameplaySettingsRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mGameplaySettingsRequest = (GameplaySettingsRequest) pGameplaySettingsRequest;
    }

    @Override
    public void execute() {
        Log.pt("GAMEPLAY SETTINGS REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ClientSettings.Gameplay gameplay = gameSession.getAccount()
                                                                .getClientSettingsManager()
                                                                .getClientSettings()
                                                                .getGameplay();
            gameplay.setNotSet(false);
            gameplay.setDoubleclickAttackEnabled(this.mGameplaySettingsRequest.doubleclickAttackEnabled);
            gameplay.setAutochangeAmmo(this.mGameplaySettingsRequest.autochangeAmmo);
            gameplay.setAutoStartEnabled(this.mGameplaySettingsRequest.autoStartEnabled);
            gameplay.setAutoRefinement(this.mGameplaySettingsRequest.autoRefinement);
            gameplay.setAutoBoost(this.mGameplaySettingsRequest.autoBoost);
            gameplay.setAutoBuyBootyKeys(this.mGameplaySettingsRequest.autoBuyBootyKeys);
            gameplay.setQuickSlotStopAttack(this.mGameplaySettingsRequest.quickSlotStopAttack);
            //gameplay.getGameplay().setDisplayBattlerayNotifications(this.mGameplaySettingsRequest.displayBattlerayNotifications);
            QueryManager.saveAccount(gameSession.getAccount());
        }
    }
}
