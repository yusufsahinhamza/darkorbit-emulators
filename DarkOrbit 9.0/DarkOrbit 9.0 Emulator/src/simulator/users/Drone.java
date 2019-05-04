package simulator.users;

/**
 Created by bpdev on 30/01/2015.
 */
public class Drone {

    private int    mDroneId;
    private String mDroneLootId;
    private int    mDroneLevel;
    private String mDesignConfig1LootId;
    private String mDesignConfig2LootId;

    public Drone(final int pDroneId, final String pDroneLootId, final int pDroneLevel,
                 final String pDesignConfig1LootId, final String pDesignConfig2LootId) {
        this.setDroneId(pDroneId);
        this.setDroneLootId(pDroneLootId);
        this.setDroneLevel(pDroneLevel);
        this.setDesignConfig1LootId(pDesignConfig1LootId);
        this.setDesignConfig2LootId(pDesignConfig2LootId);
    }

    public int getDroneId() {
        return mDroneId;
    }

    public void setDroneId(final int pDroneId) {
        mDroneId = pDroneId;
    }

    public String getDroneLootId() {
        return mDroneLootId;
    }

    public void setDroneLootId(final String pDroneLootId) {
        mDroneLootId = pDroneLootId;
    }

    public int getDroneLevel() {
        return mDroneLevel;
    }

    public void setDroneLevel(final int pDroneLevel) {
        mDroneLevel = pDroneLevel;
    }

    public String getDesignConfig1LootId() {
        return mDesignConfig1LootId;
    }

    public void setDesignConfig1LootId(final String pDesignConfig1LootId) {
        mDesignConfig1LootId = pDesignConfig1LootId;
    }

    public String getDesignConfig2LootId() {
        return mDesignConfig2LootId;
    }

    public void setDesignConfig2LootId(final String pDesignConfig2LootId) {
        mDesignConfig2LootId = pDesignConfig2LootId;
    }
}
