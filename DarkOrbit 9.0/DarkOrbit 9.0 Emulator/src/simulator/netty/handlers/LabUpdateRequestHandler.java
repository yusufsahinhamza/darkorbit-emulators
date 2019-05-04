package simulator.netty.handlers;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import java.util.Vector;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.OreTypeModule;
import simulator.netty.serverCommands.LabItemModule;
import simulator.netty.serverCommands.LabOreCountUpdate;
import simulator.netty.serverCommands.LabUpdateItemCommand;
import simulator.netty.serverCommands.OreCountModuleCommand;
import simulator.netty.serverCommands.OreTypeModuleCommand;
import simulator.netty.serverCommands.UpdateItemModule;
import simulator.users.ResourcesManager;
import utils.Log;

/**
 Created by Pedro on 31-03-2015.
 */
public class LabUpdateRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;

    public LabUpdateRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                                   final ClientCommand pLabUpdateRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
    }

    @Override
    public void execute() {
        Log.pt("LAB UPDATE REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        if (gameSession != null) {
            final ResourcesManager resourcesManager = gameSession.getAccount()
                                                                 .getResourcesManager();
            Vector<OreCountModuleCommand> oreCountModuleCommandVector = new Vector<>();
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.PROMETIUM),
                                                                      resourcesManager.getResources()
                                                                                      .getPrometium()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.ENDURIUM),
                                                                      resourcesManager.getResources()
                                                                                      .getEndurium()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.TERBIUM),
                                                                      resourcesManager.getResources()
                                                                                      .getTerbium()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.XENOMIT),
                                                                      resourcesManager.getResources()
                                                                                      .getXenomit()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.PALLADIUM),
                                                                      resourcesManager.getResources()
                                                                                      .getPalladium()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.PROMETID),
                                                                      resourcesManager.getResources()
                                                                                      .getPrometid()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.DURANIUM),
                                                                      resourcesManager.getResources()
                                                                                      .getDuranium()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModule.PROMERIUM),
                                                                      resourcesManager.getResources()
                                                                                      .getPromerium()));
            oreCountModuleCommandVector.add(new OreCountModuleCommand(new OreTypeModuleCommand(OreTypeModuleCommand.SEPROM),
                                                                      resourcesManager.getResources()
                                                                                      .getSeprom()));
            
            final Vector<UpdateItemModule> updateItemModules = new Vector<>();
            updateItemModules.add(new UpdateItemModule(new LabItemModule(LabItemModule.LASER),
                                                       new OreCountModuleCommand(new OreTypeModuleCommand(
                                                               resourcesManager.getResources()
                                                                               .getLasersResourceType()),
                                                                                 resourcesManager.getResources()
                                                                                                 .getLasersResourceAmount())));
            updateItemModules.add(new UpdateItemModule(new LabItemModule(LabItemModule.ROCKETS),
                                                       new OreCountModuleCommand(new OreTypeModuleCommand(
                                                               resourcesManager.getResources()
                                                                               .getRocketsResourceType()),
                                                                                 resourcesManager.getResources()
                                                                                                 .getRocketsResourceAmount())));
            updateItemModules.add(new UpdateItemModule(new LabItemModule(LabItemModule.DRIVING),
                                                       new OreCountModuleCommand(new OreTypeModuleCommand(
                                                               resourcesManager.getResources()
                                                                               .getGeneratorsResourceType()),
                                                                                 resourcesManager.getGeneratorsTimeLeft())));
            updateItemModules.add(new UpdateItemModule(new LabItemModule(LabItemModule.SHIELD),
                                                       new OreCountModuleCommand(new OreTypeModuleCommand(
                                                               resourcesManager.getResources()
                                                                               .getShieldsResourceType()),
                                                                                 resourcesManager.getShieldsTimeLeft())));

            gameSession.getGameServerClientConnection()
                       .sendToSendCommand(new LabOreCountUpdate(oreCountModuleCommandVector));
            gameSession.getGameServerClientConnection()
                       .sendToSendCommand(new LabUpdateItemCommand(updateItemModules));
            QueryManager.saveAccount(gameSession.getAccount());
        }
    }
}
