package simulator.map_entities.stationary;

import simulator.system.SpaceMap;
import net.game_server.GameSession;

/**
 Created by bpdev on 17/02/2015.
 */
public abstract class ActivatableStationaryMapEntity
        extends StationaryMapEntity {

    public static final int ACTIVATED_RANGE = 500;

    protected ActivatableStationaryMapEntity(final SpaceMap pSpaceMap) {
        super(pSpaceMap);
    }

    public int getActivatedRange() {
        return ACTIVATED_RANGE;
    }

    public abstract void handleClick(final GameSession pGameSession);
}
