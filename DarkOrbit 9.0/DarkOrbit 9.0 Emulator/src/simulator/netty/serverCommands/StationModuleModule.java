package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 22-03-2015.
 */
public class StationModuleModule {

    public static short HONOR_BOOSTER       = 10;
    public static short LASER_MID_RANGE     = 6;
    public static short ROCKET_LOW_ACCURACY = 9;
    public static short DAMAGE_BOOSTER      = 11;
    public static short ROCKET_MID_ACCURACY = 8;
    public static short DESTROYED           = 1;
    public static short HULL                = 2;
    public static short LASER_LOW_RANGE     = 7;
    public static short LASER_HIGH_RANGE    = 5;
    public static short NONE                = 0;
    public static short EXPERIENCE_BOOSTER  = 12;
    public static short DEFLECTOR           = 3;
    public static short REPAIR              = 4;

    public static int ID = 18588;

    public int    installationSecondsLeft;
    public String ownerName;
    public int    itemId;
    public int    emergencyRepairCost;
    public int    maxHitpoints;
    public int    maxShield;
    public int    currentShield;
    public int    slotId;
    public int    upgradeLevel;
    public int    currentHitpoints;
    public int    asteroidId;
    public short  type;
    public int    emergencyRepairSecondsLeft;
    public int    installationSeconds;
    public int    emergencyRepairSecondsTotal;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    public StationModuleModule(int param1, int param2, int param3, short param4, int param5, int param6, int param7,
                               int param8, int param9, String param10, int param11, int param12, int param13,
                               int param14, int param15) {
        this.asteroidId = param1;
        this.itemId = param2;
        this.slotId = param3;
        this.type = param4;
        this.currentHitpoints = param5;
        this.maxHitpoints = param6;
        this.currentShield = param7;
        this.maxShield = param8;
        this.upgradeLevel = param9;
        this.ownerName = param10;
        this.installationSeconds = param11;
        this.installationSecondsLeft = param12;
        this.emergencyRepairSecondsLeft = param13;
        this.emergencyRepairSecondsTotal = param14;
        this.emergencyRepairCost = param15;
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.installationSecondsLeft << 12 | this.installationSecondsLeft >>> 20);
            param1.writeUTF(this.ownerName);
            param1.writeInt(this.itemId << 4 | this.itemId >>> 28);
            param1.writeInt(this.emergencyRepairCost >>> 9 | this.emergencyRepairCost << 23);
            param1.writeInt(this.maxHitpoints >>> 10 | this.maxHitpoints << 22);
            param1.writeInt(this.maxShield >>> 14 | this.maxShield << 18);
            param1.writeInt(this.currentShield >>> 12 | this.currentShield << 20);
            param1.writeInt(this.slotId >>> 7 | this.slotId << 25);
            param1.writeInt(this.upgradeLevel >>> 5 | this.upgradeLevel << 27);
            param1.writeShort(24397);
            param1.writeInt(this.currentHitpoints << 15 | this.currentHitpoints >>> 17);
            param1.writeInt(this.asteroidId << 16 | this.asteroidId >>> 16);
            param1.writeShort(this.type);
            param1.writeInt(this.emergencyRepairSecondsLeft << 13 | this.emergencyRepairSecondsLeft >>> 19);
            param1.writeInt(this.installationSeconds >>> 10 | this.installationSeconds << 22);
            param1.writeInt(this.emergencyRepairSecondsTotal << 11 | this.emergencyRepairSecondsTotal >>> 21);
            param1.writeShort(2503);
        } catch (IOException e) {
        }
    }
}
