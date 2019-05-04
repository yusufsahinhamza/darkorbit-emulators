package simulator.system.ships;

import org.json.JSONObject;

/**
 Class used to store single ship data from DB
 */
public final class StorageShip {

    // index column in DB
    private final int mShipId;

    private final String mShipName;

    private final int mBaseHitPoints;
    private final int mBaseSpeed;

    private final JSONObject mRewardJSON;

    // SHD count of alien
    private final int mBaseShieldPoints;

    // DMG of alien
    private final int mBaseDamage;

    // % of damage absorbed by shield
    private final int mBaseShieldAbsorption;

    private final boolean mAggressive;

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

    public StorageShip(// base
                       final int pShipId, //
                       final String pShipName, //
                       final int pBaseHP, //
                       final int pBaseSpeed, //
                       final JSONObject pRewardJSON, //
                       // alien
                       final int pBaseShieldPoints,//
                       final int pBaseDamage,//
                       final int pBaseShieldAbsorption,//
                       final boolean pAggressive,//
                       // player
                       final String pShipLootId,//
                       final int pBaseCargoCapacity,//
                       final int pBaseLasersCount,//
                       final int pBaseHeavyGunsCount,//
                       final int pBaseGeneratorsCount,//
                       final int pBaseExtrasCount,//
                       final int pShopPrice,//
                       final boolean pElite //
    ) {

        this.mShipId = pShipId;
        this.mShipName = pShipName;
        this.mBaseHitPoints = pBaseHP;
        this.mBaseSpeed = pBaseSpeed;
        this.mRewardJSON = pRewardJSON;

        this.mBaseShieldPoints = pBaseShieldPoints;
        this.mBaseDamage = pBaseDamage;
        this.mBaseShieldAbsorption = pBaseShieldAbsorption;
        this.mAggressive = pAggressive;

        this.mShipLootId = pShipLootId;
        this.mBaseCargoCapacity = pBaseCargoCapacity;
        this.mBaseLasersCount = pBaseLasersCount;
        this.mBaseHeavyGunsCount = pBaseHeavyGunsCount;
        this.mBaseGeneratorsCount = pBaseGeneratorsCount;
        this.mBaseExtrasCount = pBaseExtrasCount;
        this.mShopPrice = pShopPrice;
        this.mElite = pElite;
    }

    public int getBaseHitPoints() {
        return mBaseHitPoints;
    }

    public int getBaseSpeed() {
        return mBaseSpeed;
    }

    public JSONObject getRewardJSON() {
        return mRewardJSON;
    }

    public int getShipId() {
        return mShipId;
    }

    public String getShipName() {
        return mShipName;
    }

    public boolean isAggressive() {
        return this.mAggressive;
    }

    public int getBaseCargoCapacity() {
        return this.mBaseCargoCapacity;
    }

    public int getBaseDamage() {
        return this.mBaseDamage;
    }

    public int getBaseExtrasCount() {
        return this.mBaseExtrasCount;
    }

    public int getBaseGeneratorsCount() {
        return this.mBaseGeneratorsCount;
    }

    public int getBaseHeavyGunsCount() {
        return this.mBaseHeavyGunsCount;
    }

    public int getBaseLasersCount() {
        return this.mBaseLasersCount;
    }

    public float getBaseShieldAbsorption() {
        return this.mBaseShieldAbsorption;
    }

    public int getBaseShieldPoints() {
        return this.mBaseShieldPoints;
    }

    public boolean isElite() {
        return this.mElite;
    }

    public String getShipLootId() {
        return this.mShipLootId;
    }

    public int getShopPrice() {
        return this.mShopPrice;
    }

    public PlayerShip toPlayerShip() {
        return new PlayerShip(this.mShipId, this.mShipName, this.mBaseHitPoints, this.mBaseSpeed, this.mRewardJSON,
                              this.mShipLootId, this.mBaseCargoCapacity, this.mBaseLasersCount,
                              this.mBaseHeavyGunsCount, this.mBaseGeneratorsCount, this.mBaseExtrasCount,
                              this.mShopPrice, this.mElite);
    }

    public AlienShip toAlienShip() {
        return new AlienShip(this.mShipId, this.mShipName, this.mBaseHitPoints, this.mBaseSpeed, this.mRewardJSON,
                             this.mBaseShieldPoints, this.mBaseDamage, this.mBaseShieldAbsorption, this.mAggressive);
    }

}
