package simulator.system.ships;

import org.json.JSONArray;
import org.json.JSONObject;

/**
 This class represents a ship and should be extended to
 */
public abstract class AbstractShip {

    private static final String REWARD_JSON_EXPERIENCE = "experience";
    private static final String REWARD_JSON_HONOR      = "honor";
    private static final String REWARD_JSON_CREDITS    = "credits";
    private static final String REWARD_JSON_URIDIUM    = "uridium";
    private static final String REWARD_JSON_RESOURCES  = "resources";

    // index column in DB
    protected int mShipId;

    protected final String mShipName;

    protected int mBaseHitPoints;
    protected int mBaseSpeed;

    protected final JSONObject mRewardJSON;

    public AbstractShip(final int pShipId, final String pShipName, final int pBaseHP, final int pBaseSpeed,
                        final JSONObject pRewardJSON) {

        this.mShipId = pShipId;
        this.mShipName = pShipName;
        this.mBaseHitPoints = pBaseHP;
        this.mBaseSpeed = pBaseSpeed;
        this.mRewardJSON = pRewardJSON;

    }

    public int getShipId() {
        return this.mShipId;
    }

    public String getShipName() {
        return this.mShipName;
    }

    public int getBaseHitPoints() {
        return this.mBaseHitPoints;
    }

    public int getBaseSpeed() {
        return this.mBaseSpeed;
    }

    public int getRewardExperience() {
        return this.mRewardJSON.getInt(REWARD_JSON_EXPERIENCE);
    }

    public int getRewardHonor() {
        return this.mRewardJSON.getInt(REWARD_JSON_HONOR);
    }

    public int getRewardCredits() {
        return this.mRewardJSON.getInt(REWARD_JSON_CREDITS);
    }

    public int getRewardUridium() {
        return this.mRewardJSON.getInt(REWARD_JSON_URIDIUM);
    }

    public JSONArray getRewardResources() {
        return this.mRewardJSON.getJSONArray(REWARD_JSON_RESOURCES);
    }

}
