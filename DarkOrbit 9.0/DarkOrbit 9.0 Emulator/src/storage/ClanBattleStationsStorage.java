package storage;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import simulator.map_entities.stationary.ClanBattleStation;


/**
 Stores all CBS data loaded from DB. Provides static access to all stored data
 */
public class ClanBattleStationsStorage {

    private static Map<Short, ClanBattleStation> sClanBattleStations = new HashMap<>();

    public static void addClanBattleStation(final ClanBattleStation pClanBattleStation) {
        sClanBattleStations.put(pClanBattleStation.getClanBattleStationId(), pClanBattleStation);
    }

    public static ClanBattleStation getClanBattleStation(final short pClanBattleStationId) {
        return sClanBattleStations.get(pClanBattleStationId);
    }

    public static void removeClanBattleStation(final short pClanBattleStationId) {
        sClanBattleStations.remove(pClanBattleStationId);
    }

    public static Collection<ClanBattleStation> getClanBattleStationCollection() {
        return sClanBattleStations.values();
    }

    public static int getClanBattleStationCount() {
        return sClanBattleStations.size();
    }


}
