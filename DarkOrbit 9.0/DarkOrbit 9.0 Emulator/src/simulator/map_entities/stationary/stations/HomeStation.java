package simulator.map_entities.stationary.stations;

import net.game_server.GameSession;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Set;
import java.util.Vector;

import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.map_entities.stationary.StationaryMapEntity;
import simulator.netty.serverCommands.AssetCreateCommand;
import simulator.netty.serverCommands.AssetTypeModule;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.system.SpaceMap;

/**
 Created by bpdev on 31/01/2015.
 */
public class HomeStation
        extends ActivatableStationaryMapEntity {

    private static final String ASSET_NAME = "HQ";
    private static final short  ASSET_TYPE = AssetTypeModule.BASE_COMPANY;
    private static final short  DESIGN_ID  = 0;

    private static final int[][] REPAIR_STATIONS      = new int[][]{{0, -1080}};
    private static final int[][] HANGAR_STATIONS      = new int[][]{{+1080, 0}};
    private static final int[][] QUEST_GIVER_STATIONS = new int[][]{{0, +1080}};
    private static final int[][] ORE_TRADE_STATIONS   = new int[][]{{-1080, -1}};
    private static final int[][] SMALL_TURRETS        =
            new int[][]{{+465, +1738}, {-157, +1793}, {-761, +1631}, {-1273, +1272}, {-1632, +760}, {-1794, +156},
                        {-1739, -466}, {-1475, -1033}, {-1033, -1475}, {-466, -1739}, {+156, -1794}, {+760, -1632},
                        {+1272, -1273}, {+1631, -761}, {+1793, -157}, {+1738, +465}};
    private static final int[][] LARGE_TURRETS        = new int[][]{{+1474, +1032}, {+1032, +1474}};

    private short mFactionId;
    private HashMap<Integer, RepairStation>     mRepairStations     = new HashMap<>();
    private HashMap<Integer, HangarStation>     mHangarStations     = new HashMap<>();
    private HashMap<Integer, QuestGiverStation> mQuestGiverStations = new HashMap<>();
    private HashMap<Integer, OreTradeStation>   mOreTradeStations   = new HashMap<>();
    private HashMap<Integer, SmallTurret>       mSmallTurrets       = new HashMap<>();
    private HashMap<Integer, LargeTurret>       mLargeTurrets       = new HashMap<>();

    public static final int SECURE_ZONE_RANGE = 1750;

    public HomeStation(final SpaceMap pCurrentSpaceMapId, final short pFactionId, final int pPositionX,
                       final int pPositionY) {
        super(pCurrentSpaceMapId);

        this.setFactionId(pFactionId);

        this.setCurrentPositionX(pPositionX);
        this.setCurrentPositionY(pPositionY);

    }

    public void prepareStations() {
        //load repair stations
        for (final int[] position : REPAIR_STATIONS) {
            final int posX = this.getCurrentPositionX() + position[0];
            final int posY = this.getCurrentPositionY() + position[1];
            final RepairStation repairStation =
                    new RepairStation(this.getCurrentSpaceMap(), this.getFactionId(), posX, posY);
            this.mRepairStations.put(repairStation.getMapEntityId(), repairStation);
        }
        //load hangar stations
        for (final int[] position : HANGAR_STATIONS) {
            final int posX = this.getCurrentPositionX() + position[0];
            final int posY = this.getCurrentPositionY() + position[1];
            final HangarStation hangarStation =
                    new HangarStation(this.getCurrentSpaceMap(), this.getFactionId(), posX, posY);
            this.mHangarStations.put(hangarStation.getMapEntityId(), hangarStation);
        }
        //load quest giver stations
        for (final int[] position : QUEST_GIVER_STATIONS) {
            final int posX = this.getCurrentPositionX() + position[0];
            final int posY = this.getCurrentPositionY() + position[1];
            final QuestGiverStation questGiverStation =
                    new QuestGiverStation(this.getCurrentSpaceMap(), this.getFactionId(), posX, posY);
            this.mQuestGiverStations.put(questGiverStation.getMapEntityId(), questGiverStation);
        }
        //load ore trade stations
        for (final int[] position : ORE_TRADE_STATIONS) {
            final int posX = this.getCurrentPositionX() + position[0];
            final int posY = this.getCurrentPositionY() + position[1];
            final OreTradeStation oreTradeStation =
                    new OreTradeStation(this.getCurrentSpaceMap(), this.getFactionId(), posX, posY);
            this.mOreTradeStations.put(oreTradeStation.getMapEntityId(), oreTradeStation);
        }
        //load small turrets stations
        for (final int[] position : SMALL_TURRETS) {
            final int posX = this.getCurrentPositionX() + position[0];
            final int posY = this.getCurrentPositionY() + position[1];
            final SmallTurret smallTurret =
                    new SmallTurret(this.getCurrentSpaceMap(), this.getFactionId(), posX, posY);
            this.mSmallTurrets.put(smallTurret.getMapEntityId(), smallTurret);
        }
        //load large turrets stations
        for (final int[] position : LARGE_TURRETS) {
            final int posX = this.getCurrentPositionX() + position[0];
            final int posY = this.getCurrentPositionY() + position[1];
            final LargeTurret largeTurret =
                    new LargeTurret(this.getCurrentSpaceMap(), this.getFactionId(), posX, posY);
            this.mLargeTurrets.put(largeTurret.getMapEntityId(), largeTurret);
        }
    }

    public Collection<StationaryMapEntity> getAllStations() {
        final ArrayList<StationaryMapEntity> stations = new ArrayList<>();
        stations.addAll(this.mRepairStations.values());
        stations.addAll(this.mHangarStations.values());
        stations.addAll(this.mQuestGiverStations.values());
        stations.addAll(this.mOreTradeStations.values());
        stations.addAll(this.mSmallTurrets.values());
        stations.addAll(this.mLargeTurrets.values());
        return stations;
    }

    public Collection<ActivatableStationaryMapEntity> getActivatables() {
        final ArrayList<ActivatableStationaryMapEntity> activatables = new ArrayList<>();
        activatables.addAll(this.mRepairStations.values());
        activatables.addAll(this.mHangarStations.values());
        activatables.addAll(this.mQuestGiverStations.values());
        activatables.addAll(this.mOreTradeStations.values());
        return activatables;
    }

    public Set<Integer> getQuestGivers() {
        return this.mQuestGiverStations.keySet();
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
                                      this.getCurrentPositionX(), this.getCurrentPositionY(), 5, false, true, true, true,
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
