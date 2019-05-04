package simulator.map_entities.stationary;

import simulator.map_entities.MapEntity;
import simulator.system.SpaceMap;

/**
 Created by bpdev on 09/02/2015.
 */
public abstract class StationaryMapEntity
        extends MapEntity
        implements IStationary {

    //TODO impl

    protected StationaryMapEntity(final SpaceMap pSpaceMap) {
        super(pSpaceMap);
    }
}
