package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 30-03-2015.
 */
public class KillScreenOptionTypeModule
        implements ServerCommand {

    public static short FREE_PHOENIX                 = 0;
    public static short BASIC_REPAIR                 = 1;
    public static short AT_JUMPGATE_REPAIR           = 2;
    public static short AT_DEATHLOCATION_REPAIR      = 3;
    public static short AT_SECTOR_CONTROL_SPAWNPOINT = 4;
    public static short EXIT_SECTOR_CONTROL          = 5;
    public static short BASIC_FULL_REPAIR            = 6;
    public static short short_1942                   = 7;
    public static short short_564                    = 8;
    public static short const_2728                   = 9;

    public static int ID = 29900;

    public short mRepairTypeValue = 0;

    public KillScreenOptionTypeModule(short pRepairTypeValue) {
        this.mRepairTypeValue = pRepairTypeValue;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(this.mRepairTypeValue);
            param1.writeShort(17851);
        } catch (IOException e) {
        }
    }
}
