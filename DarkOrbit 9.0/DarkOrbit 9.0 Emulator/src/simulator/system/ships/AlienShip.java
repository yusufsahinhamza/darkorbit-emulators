package simulator.system.ships;

import org.json.JSONObject;

/**
 Created by bpdev on 31/01/2015.
 */
public class AlienShip
        extends AbstractShip {

    // SHD count of alien
    private final int mBaseShieldPoints;

    // DMG of alien
    private final int mBaseDamage;

    // % of damage absorbed by shield
    private final int mBaseShieldAbsorption;

    private final boolean mAggressive;

    public AlienShip(//
                     final int pShipId,//
                     final String pShipName,//
                     final int pBaseHP,//
                     final int pBaseSpeed,//
                     final JSONObject pRewardJSON,//
                     final int pBaseShieldPoints,//
                     final int pBaseDamage,//
                     final int pBaseShieldAbsorption,//
                     final boolean pAggressive//
    ) {
        super(pShipId, pShipName, pBaseHP, pBaseSpeed, pRewardJSON);

        this.mBaseShieldPoints = pBaseShieldPoints;
        this.mBaseDamage = pBaseDamage;
        this.mBaseShieldAbsorption = pBaseShieldAbsorption;
        this.mAggressive = pAggressive;

    }

    public int getBaseShieldPoints() {
        return this.mBaseShieldPoints;
    }

    public int getBaseDamage() {
        return this.mBaseDamage;
    }

    public int getBaseShieldAbsorption() {
        return this.mBaseShieldAbsorption;
    }

    public boolean isAggressive() {
        return this.mAggressive;
    }

}
