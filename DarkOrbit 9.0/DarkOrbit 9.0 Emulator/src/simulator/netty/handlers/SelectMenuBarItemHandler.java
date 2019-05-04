package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.SelectMenuBarItem;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class SelectMenuBarItemHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final SelectMenuBarItem          mSelectMenuBarItem;

    public SelectMenuBarItemHandler(final GameServerClientConnection pGameServerClientConnection,
                                    final ClientCommand pSelectMenuBarItem) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mSelectMenuBarItem = (SelectMenuBarItem) pSelectMenuBarItem;
    }

    @Override
    public void execute() {
        Log.pt("SELECT MENU BAR ITEM HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            gameSession.getPlayer()
                       .getAccount()
                       .getClientSettingsManager()
                       .selectMenuBarItem(this.mSelectMenuBarItem.mItemId);
        }
    }
}
