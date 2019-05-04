package simulator.system.ships;

import storage.ShipStorage;

/**
 Created by bpdev on 31/01/2015.
 */
public class ShipFactory {

    public static AlienShip getAlienShip(final int pShipId) {
        return ShipStorage.getStorageShip(pShipId)
                          .toAlienShip();
    }

    public static PlayerShip getPlayerShip(final int pShipId) {
        return ShipStorage.getStorageShip(pShipId)
                          .toPlayerShip();
    }

}
