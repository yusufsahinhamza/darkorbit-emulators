package storage;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import simulator.system.SpaceMap;


/**
 Stores all space map data loaded from DB. Provides static access to all stored data
 */
public class SpaceMapStorage {

    private static Map<Short, SpaceMap> sSpaceMaps = new HashMap<>();

    public static void addSpaceMap(final SpaceMap pSpaceMap) {
        sSpaceMaps.put(pSpaceMap.getSpaceMapId(), pSpaceMap);
    }

    public static SpaceMap getSpaceMap(final short pSpaceMapId) {
        return sSpaceMaps.get(pSpaceMapId);
    }

    public static void removeSpaceMap(final short pSpaceMapId) {
        sSpaceMaps.remove(pSpaceMapId);
    }

    public static Collection<SpaceMap> getSpaceMapCollection() {
        return sSpaceMaps.values();
    }

    public static int getSpaceMapCount() {
        return sSpaceMaps.size();
    }

}
