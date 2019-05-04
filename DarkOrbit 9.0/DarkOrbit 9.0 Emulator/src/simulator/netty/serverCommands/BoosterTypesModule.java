package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 11/12/2017.
*/

public class BoosterTypesModule
        implements ServerCommand {

    public static int ID = 24292;

    public static short DMG_B01   = 0;
    public static short DMG_B02   = 1;
    public static short EP_B01    = 2;
    public static short EP_B02    = 3;
    public static short EP50      = 4;
    public static short HON_B01   = 5;
    public static short HON_B02   = 6;
    public static short HON50     = 7;
    public static short HP_B01    = 8;
    public static short HP_B02    = 9;
    public static short REP_B01   = 10;
    public static short REP_B02   = 11;
    public static short REP_S01   = 12;
    public static short RES_B01   = 13;
    public static short RES_B02   = 14;
    public static short SHD_B01   = 15;
    public static short SHD_B02   = 16;
    public static short SREG_B01  = 17;
    public static short SREG_B02  = 18;
    public static short BB_01     = 19;
    public static short QR_01     = 20;
    public static short CD_B01    = 21;
    public static short CD_B02    = 22;
    public static short KAPPA_B01 = 23;
    public static short HONM_1    = 24;
    public static short XPM_1     = 25;
    public static short DMGM_1    = 26;
    
    public short boosterType;
    
    public BoosterTypesModule(short param1) {
        this.boosterType = param1;
    }
    
    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(-9718);
            param1.writeShort(this.boosterType);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}