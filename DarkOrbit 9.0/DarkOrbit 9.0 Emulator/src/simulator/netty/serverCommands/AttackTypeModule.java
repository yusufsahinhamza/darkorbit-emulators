package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 25-02-2015.
 */
public class AttackTypeModule {

    public static short ROCKET       = 0;
    public static short REPAIR       = 10;
    public static short const_1488   = 15;
    public static short SINGULARITY  = 8;
    public static short DECELERATION = 11;
    public static short CID          = 7;
    public static short KAMIKAZE     = 9;
    public static short STICKY_BOMB  = 14;
    public static short PLASMA       = 4;
    public static short MINE         = 2;
    public static short const_383    = 13;
    public static short RADIATION    = 3;
    public static short SL           = 6;
    public static short ECI          = 5;
    public static short LASER        = 1;
    public static short const_236    = 12;
    public static int   ID           = 14619;
    public        short var_305      = 0;

    public AttackTypeModule(short param1) {
        this.var_305 = param1;
    }


    public void write(DataOutputStream pDataout) {
        try {
            pDataout.writeShort(ID);
            this.writeInternal(pDataout);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream pDataout) {
        try {
            pDataout.writeShort(-20422);
            pDataout.writeShort(this.var_305);
        } catch (IOException e) {
        }
    }
}
