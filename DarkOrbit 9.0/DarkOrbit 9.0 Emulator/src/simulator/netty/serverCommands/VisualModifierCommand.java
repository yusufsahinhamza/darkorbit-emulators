package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class VisualModifierCommand
        implements ServerCommand {

    public static int ID = 21462;

    public static short BLUE_SIGNAL    = 34;
    public static short const_924     = 12;
    public static short const_1882    = 36;
    public static short const_2715    = 18;
    public static short const_2524    = 10;
    public static short BIG_EMP     = 38;
    public static short const_1220    = 11;
    public static short const_1736    = 1;
    public static short const_526     = 22;
    public static short const_1099    = 7;
    public static short SHIP_WARP     = 15;
    public static short const_1036    = 39;
    public static short const_55      = 35;
    public static short const_804     = 24;
    public static short JPA_CAMERA    = 45;
    public static short const_768     = 33;
    public static short const_2736    = 19;
    public static short const_1621    = 21;
    public static short const_1815    = 9;
    public static short INACTIVE      = 8;
    public static short SPEED    = 0;
    public static short RED_SIGNAL     = 3;
    public static short const_2411    = 25;
    public static short DIVERSE    = 41;
    public static short const_425     = 16;
    public static short const_523     = 5;
    public static short MOVE_REVERSE    = 20;
    public static short const_1677    = 29;
    public static short SINGULARITY   = 13;
    public static short DAMAGE_ICON    = 43;
    public static short const_366     = 42;
    public static short const_2653    = 31;
    public static short LEONOV_EFFECT     = 17;
    public static short const_174     = 14;
    public static short SELECTED_EFFECT_BLUE    = 47;
    public static short SELECTED_EFFECT_RED    = 46;
    public static short const_1852    = 4;
    public static short const_275     = 37;
    public static short const_1184    = 44;
    public static short const_1060    = 40;
    public static short INVINCIBILITY = 26;
    public static short const_2574    = 32;
    public static short const_1814    = 23;
    public static short const_84      = 27;
    public static short const_542     = 30;
    public static short const_1428    = 2;
    public static short const_1892    = 6;
    public static short const_2745    = 28;

    public int attribute = 0;

    public int name_84 = 0;

    public int count = 0;

    public String var_1770 = "";

    public short var_1913 = 0;

    public boolean activated = false;

    public VisualModifierCommand(int param1, short param2, int pAttribute, String param4, int pCount,
                                 boolean pActivated) {

        this.name_84 = param1;
        this.var_1913 = param2;
        this.attribute = pAttribute;
        this.var_1770 = param4;
        this.count = pCount;
        this.activated = pActivated;
    }


    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 15;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream out) {
        try {
            out.writeUTF(this.var_1770);
            out.writeShort(10961);
            out.writeInt(this.count << 15 | this.count >>> 17);
            out.writeShort(14097);
            out.writeShort(this.var_1913);
            out.writeInt(this.attribute >>> 13 | this.attribute << 19);
            out.writeInt(this.name_84 >>> 2 | this.name_84 << 30);
            out.writeBoolean(this.activated);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}