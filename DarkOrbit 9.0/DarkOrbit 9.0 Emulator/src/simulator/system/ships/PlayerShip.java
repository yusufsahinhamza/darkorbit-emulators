package simulator.system.ships;

import org.json.JSONObject;
import utils.Tools;

/**
 Created by bpdev on 31/01/2015.
 */
public class PlayerShip
        extends AbstractShip {

    // ship_phoenix etc (String ship identifier)
    private final String mShipLootId;

    private final int mBaseCargoCapacity;

    // equipment-related
    private final int mBaseLasersCount;
    private final int mBaseHeavyGunsCount;
    private final int mBaseGeneratorsCount;
    private final int mBaseExtrasCount;

    // shop-related
    private final int     mShopPrice;
    private final boolean mElite;

    public PlayerShip(//
                      final int pShipId,//
                      final String pShipName,//
                      final int pBaseHP,//
                      final int pBaseSpeed,//
                      final JSONObject pRewardJSON,//
                      final String pShipLootId,//
                      final int pBaseCargoCapacity,//
                      final int pBaseLasersCount,//
                      final int pBaseHeavyGunsCount,//
                      final int pBaseGeneratorsCount,//
                      final int pBaseExtrasCount,//
                      final int pShopPrice,//
                      final boolean pElite //
    ) {
        super(pShipId, pShipName, pBaseHP, pBaseSpeed, pRewardJSON);

        this.mShipLootId = pShipLootId;
        this.mBaseCargoCapacity = pBaseCargoCapacity;
        this.mBaseLasersCount = pBaseLasersCount;
        this.mBaseHeavyGunsCount = pBaseHeavyGunsCount;
        this.mBaseGeneratorsCount = pBaseGeneratorsCount;
        this.mBaseExtrasCount = pBaseExtrasCount;
        this.mShopPrice = pShopPrice;
        this.mElite = pElite;

    }

    public String getShipLootId() {
        return this.mShipLootId;
    }

    public int getBaseCargoCapacity() {
        return this.mBaseCargoCapacity;
    }

    public int getBaseLasersCount() {
        return this.mBaseLasersCount;
    }

    public int getBaseHeavyGunsCount() {
        return 1;
    }

    public int getBaseGeneratorsCount() {
        return this.mBaseGeneratorsCount;
    }

    public int getBaseExtrasCount() {
        return this.mBaseExtrasCount;
    }

    public int getShopPrice() {
        return this.mShopPrice;
    }

    public boolean isElite() {
        return this.mElite;
    }


    public static PlayerShip forNewAccount() {
        // TODO
        return ShipFactory.getPlayerShip(1);
    }

    public int getHitPointsBoost(final int pHitPoints) {
        switch (this.getShipId()) {
        	case ShipsConstants.HAMMERCLAW:
            return (int) Tools.getBoost(pHitPoints, 20);
            case ShipsConstants.GOLIATH_CENTAUR:
                return (int) Tools.getBoost(pHitPoints, 10);
            case ShipsConstants.GOLIATH_SATURN:
                return (int) Tools.getBoost(pHitPoints, 20);
            default:
                return pHitPoints;
        }
    }

    public int getShieldPointsBoost(final int pShieldPoints) {
        switch (this.getShipId()) {
            case ShipsConstants.GOLIATH_BASTION:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_KICK:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_SENTINEL:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_SENTINEL_FROST:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_SENTINEL_LEGEND:
                return (int) Tools.getBoost(pShieldPoints, 15);
            case ShipsConstants.GOLIATH_SOLACE:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_SPECTRUM:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_SPECTRUM_FROST:
                return (int) Tools.getBoost(pShieldPoints, 10);
            case ShipsConstants.GOLIATH_SPECTRUM_LEGEND:
                return (int) Tools.getBoost(pShieldPoints, 15);
            default:
                return pShieldPoints;
        }
    }

    public int getLaserDamageBoost(final int pDamage, final int pTargetFactionId) {
        switch (this.getShipId()) {
        	case ShipsConstants.CYBORG:
            return (int) Tools.getBoost(pDamage, 10);
            case ShipsConstants.GOLIATH_DIMINISHER:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_ENFORCER:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_PEACEMAKER:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_REFEREE:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_SOVEREIGN:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_VANQUISHER:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_VENOM:
                return (int) Tools.getBoost(pDamage, 5);
            case ShipsConstants.GOLIATH_HEZARFEN:
                return (int) Tools.getBoost(pDamage, 10);
            case ShipsConstants.GOLIATH_INDEPENDENCE:
                return (int) Tools.getBoost(pDamage, 10);
            case ShipsConstants.G_CHAMPION_LEGEND:
                return (int) Tools.getBoost(pDamage, 10);
            case ShipsConstants.SURGEON:
                return (int) Tools.getBoost(pDamage, 7);
            case ShipsConstants.SURGEON_CICADA:
                return (int) Tools.getBoost(pDamage, 7);
            case ShipsConstants.SURGEON_LOCUST:
                return (int) Tools.getBoost(pDamage, 7);
            default:
                return pDamage;
        }
    }

    public int getHonorBoost(final int pHonor) {
        switch (this.getShipId()) {
            case ShipsConstants.GOLIATH_EXALTED:
                return (int) Tools.getBoost(pHonor, 10);
            case ShipsConstants.GOLIATH_GOAL:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_PEACEMAKER:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_SOVEREIGN:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_VANQUISHER:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.G_CHAMPION_LEGEND:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_HEZARFEN:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_INDEPENDENCE:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_SPECTRUM_LEGEND:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.GOLIATH_SENTINEL_LEGEND:
                return (int) Tools.getBoost(pHonor, 15);
            case ShipsConstants.SURGEON:
                return (int) Tools.getBoost(pHonor, 7);
            case ShipsConstants.SURGEON_CICADA:
                return (int) Tools.getBoost(pHonor, 7);
            case ShipsConstants.SURGEON_LOCUST:
                return (int) Tools.getBoost(pHonor, 7);
            default:
                return pHonor;
        }
    }

    public int getExperienceBoost(final int pExperience) {
        switch (this.getShipId()) {
            case ShipsConstants.GOLIATH_GOAL:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.GOLIATH_VETERAN:
                return (int) Tools.getBoost(pExperience, 10);
            case ShipsConstants.SURGEON:
                return (int) Tools.getBoost(pExperience, 7);
            case ShipsConstants.SURGEON_CICADA:
                return (int) Tools.getBoost(pExperience, 7);
            case ShipsConstants.SURGEON_LOCUST:
                return (int) Tools.getBoost(pExperience, 7);
            case ShipsConstants.GOLIATH_HEZARFEN:
                return (int) Tools.getBoost(pExperience, 15); 
            case ShipsConstants.GOLIATH_INDEPENDENCE:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.GOLIATH_SPECTRUM_LEGEND:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.GOLIATH_SENTINEL_LEGEND:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.GOLIATH_PEACEMAKER:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.GOLIATH_SOVEREIGN:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.GOLIATH_VANQUISHER:
                return (int) Tools.getBoost(pExperience, 15);
            case ShipsConstants.G_CHAMPION_LEGEND:
                return (int) Tools.getBoost(pExperience, 15);
            default:
                return pExperience;
        }
    }
}
