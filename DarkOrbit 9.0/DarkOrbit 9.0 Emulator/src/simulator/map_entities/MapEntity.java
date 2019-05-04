package simulator.map_entities;

import java.util.concurrent.atomic.AtomicInteger;

import simulator.system.SpaceMap;
import storage.SpaceMapStorage;

/**
 Represents something that is located on SpaceMap
 */
public abstract class MapEntity
        extends MapEntityPosition {

    private static final int FIRST_MAP_ENTITY_ID = 1000000000;

    private static final AtomicInteger MAP_ENTITY_ID = new AtomicInteger(FIRST_MAP_ENTITY_ID);

    // id of on-map object
    protected int mMapEntityId;

    // current map id
    protected SpaceMap mCurrentSpaceMap;

    // ==============================================================

    protected MapEntity(final SpaceMap pCurrentSpaceMap) {

        mCurrentSpaceMap = pCurrentSpaceMap;

        this.mMapEntityId = MAP_ENTITY_ID.getAndIncrement();

    }

    /**
     Intended to be used in Player class to assign custom map entity ID

     @param pCurrentSpaceMapId
     space map where entity is located
     @param pMapEntityIdOverride
     map entity ID to override
     */
    protected MapEntity(final SpaceMap pCurrentSpaceMap, final int pMapEntityIdOverride) {

        mCurrentSpaceMap = pCurrentSpaceMap;

        this.mMapEntityId = pMapEntityIdOverride;

    }

    // ==============================================================

    public short getCurrentSpaceMapId() {
        if(this.mCurrentSpaceMap == null || this.mCurrentSpaceMap.getSpaceMapId() == 0) return 16;
        return this.mCurrentSpaceMap.getSpaceMapId();
    }

    public int getMapEntityId() {
        return this.mMapEntityId;
    }

    public SpaceMap getCurrentSpaceMap() {
        return mCurrentSpaceMap;
    }

    public void setCurrentSpaceMap(final short pSpaceMapId){
        mCurrentSpaceMap = SpaceMapStorage.getSpaceMap(pSpaceMapId);
    }
}
