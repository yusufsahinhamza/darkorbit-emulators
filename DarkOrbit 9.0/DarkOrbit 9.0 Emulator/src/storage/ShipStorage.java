package storage;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import simulator.system.ships.StorageShip;


/**
 Stores all ship data loaded from DB. Provides static access to all stored data
 */
public class ShipStorage {

    private static Map<Integer, StorageShip> sStorageShips = new HashMap<>();

    public static void addStorageShip(final StorageShip pStorageShip) {
        sStorageShips.put(pStorageShip.getShipId(), pStorageShip);
    }

    public static String getShipLootID(final int pShipID) {
    	return getStorageShip(pShipID).getShipLootId();
    }
    
    public static StorageShip getStorageShip(final int pUserID) {
        return sStorageShips.get(pUserID);
    }

    public static void removeStorageShip(final int pUserID) {
        sStorageShips.remove(pUserID);
    }

    public static Collection<StorageShip> getStorageShipCollection() {
        return sStorageShips.values();
    }

    public static int getStorageShipCount() {
        return sStorageShips.size();
    }

}
