package simulator.map_entities.stationary.stations;

import net.game_server.GameSession;

import java.util.Vector;

import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.netty.serverCommands.AssetCreateCommand;
import simulator.netty.serverCommands.AssetTypeModule;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.system.SpaceMap;

/**
 Created by Pedro on 22-03-2015.
 */
public class RepairStation
        extends ActivatableStationaryMapEntity {

    private static final String ASSET_NAME = "RepairDock";
    private static final short  ASSET_TYPE = AssetTypeModule.REPAIR_STATION;
    private static final short  DESIGN_ID  = 0;

    private short mFactionId;

    public RepairStation(final SpaceMap pCurrentSpaceMapId, final short pFactionId, final int pCurrentPositionX,
                         final int pCurrentPositionY) {
        super(pCurrentSpaceMapId);

        this.setFactionId(pFactionId);

        this.setCurrentPositionX(pCurrentPositionX);
        this.setCurrentPositionY(pCurrentPositionY);
    }

    @Override
    public String getAssetName() {
        return ASSET_NAME;
    }

    @Override
    public short getAssetType() {
        return ASSET_TYPE;
    }

    @Override
    public AssetCreateCommand getAssetCreateCommand() {
        return new AssetCreateCommand(new AssetTypeModule(this.getAssetType()), this.getAssetName(),
                                      this.getFactionId(), "", this.getMapEntityId(), DESIGN_ID, 0,
                                      this.getCurrentPositionX(), this.getCurrentPositionY(), 5, true, true, true, true,
                                      new ClanRelationModule(ClanRelationModule.NONE),
                                      new Vector<VisualModifierCommand>());
    }

    public short getFactionId() {
        return mFactionId;
    }

    public void setFactionId(final short pFactionId) {
        mFactionId = pFactionId;
    }

    @Override
    public void handleClick(final GameSession pGameSession) {

    }
}
