package simulator.map_entities.stationary.stations;


import net.game_server.GameSession;

import java.util.Vector;
import java.util.concurrent.atomic.AtomicInteger;

import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.netty.ServerCommand;
import simulator.netty.serverCommands.AssetCreateCommand;
import simulator.netty.serverCommands.AssetTypeModule;
import simulator.netty.serverCommands.AsteroidProgressCommand;
import simulator.netty.serverCommands.AvailableModulesCommand;
import simulator.netty.serverCommands.BattleStationBuildingUiInitializationCommand;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.EquippedModulesModule;
import simulator.netty.serverCommands.ShipWarpModule;
import simulator.netty.serverCommands.StationModuleModule;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.system.SpaceMap;
import simulator.system.clans.Clan;
import utils.Log;

/**
 Created by Pedro on 09-04-2015.
 */
public class BattleStation
        extends ActivatableStationaryMapEntity {

    private static final short ASSET_TYPE_CLAN_OWNING = AssetTypeModule.BATTLESTATION;
    private static final short ASSET_TYPE_NO_CLAN     = AssetTypeModule.BATTLESTATION;
    private static final short DESIGN_ID              = 0;

    private final AtomicInteger itemIds = new AtomicInteger(0);

    private Clan mClan;

    public BattleStation(final SpaceMap pCurrentSpaceMapId, final int pPositionX, final int pPositionY) {
        super(pCurrentSpaceMapId);

        this.setCurrentPositionX(pPositionX);
        this.setCurrentPositionY(pPositionY);
    }

    @Override
    public void handleClick(final GameSession pGameSession) {
        Log.p("BattleStation Handle Click");
        if (this.getAssetType() == ASSET_TYPE_NO_CLAN) {
        	
        	Vector<StationModuleModule> modulee = new Vector<StationModuleModule>();
        	modulee.add(new StationModuleModule(1,5,5,StationModuleModule.LASER_LOW_RANGE,10000,10000,1000,1000,16,"TESTTT",0,0,0,0,0));
        	
            pGameSession.getGameServerClientConnection()
                        .sendToSendCommand(new BattleStationBuildingUiInitializationCommand(this.getMapEntityId(), 1, "Test",
                                                                                      new AsteroidProgressCommand(
                                                                                              this.getMapEntityId(), 1,
                                                                                              1, "test2", "test3",
                                                                                              new EquippedModulesModule(
                                                                                            		  modulee),
                                                                                              true),
                                                                                      this.getBattleStationAvailableModulesCommand(),
                                                                                      0, 0, 0));
            Log.p("BatteStationBuildingUi Command Sent");
        }
    }

    private AvailableModulesCommand getBattleStationAvailableModulesCommand() {
        final Vector<StationModuleModule> stationModuleModules = new Vector<>();
        stationModuleModules.add(new StationModuleModule(1,5,5,StationModuleModule.LASER_LOW_RANGE,10000,10000,1000,1000,16,"TESTTT",0,0,0,0,0));
        return new AvailableModulesCommand(stationModuleModules);
    }

    @Override
    public String getAssetName() {
        return "Clan Battle Station";
    }

    @Override
    public short getAssetType() {
        return mClan == null ? ASSET_TYPE_NO_CLAN : ASSET_TYPE_CLAN_OWNING;
    }

    @Override
    public ServerCommand getAssetCreateCommand() {
        Log.p(this.getMapEntityId() + "");
        return new AssetCreateCommand(new AssetTypeModule(this.getAssetType()), this.getAssetName(),
                                      this.getFactionId(), "", this.getMapEntityId(), DESIGN_ID, 9990,
                                      this.getCurrentPositionX(), this.getCurrentPositionY(), 9991, true, true, true, true,
                                      new ClanRelationModule(ClanRelationModule.NONE),
                                      new Vector<VisualModifierCommand>());
    }

    private short getFactionId() {
        if (this.mClan != null) {
            return this.mClan.getFactionId();
        }
        return 0;
    }

    public Clan getClan() {
        return this.mClan;
    }

    public short getStatus() {
        return 0;
    }
}
