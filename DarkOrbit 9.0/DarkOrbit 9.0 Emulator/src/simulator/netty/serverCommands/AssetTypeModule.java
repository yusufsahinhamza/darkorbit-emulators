package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class AssetTypeModule
        implements ServerCommand {

    public static final short BATTLESTATION     = 36;
    public static final short LARGE_TURRETS     = 57;
    public static final short short_2146        = 39;
    public static final short QUEST_STATION     = 34;
    public static final short short_686         = 27;
    public static final short short_714         = 3;
    public static final short short_2443        = 8;
    public static final short short_1504        = 31;
    public static final short short_1827        = 0;
    public static final short short_2063        = 13;
    public static final short short_919         = 7;
    public static final short short_2210        = 2;
    public static final short short_169         = 22;
    public static final short short_330         = 18;
    public static final short WRECK             = 32;
    public static final short short_364         = 16;
    public static final short short_1147        = 23;
    public static final short short_1016        = 54;
    public static final short short_250         = 58;
    public static final short SMALL_TURRETS     = 55;
    public static final short short_2282        = 40;
    public static final short short_861         = 19;
    public static final short short_940         = 9;
    public static final short short_1965        = 4;
    public static final short short_725         = 5;
    public static final short short_2316        = 15;
    public static final short short_1793        = 45;
    public static final short BASE_COMPANY      = 46;
    public static final short short_202         = 51;
    public static final short short_2488        = 52;
    public static final short short_379         = 28;
    public static final short short_2613        = 38;
    public static final short short_2750        = 30;
    public static final short short_1564        = 1;
    public static final short short_947         = 17;
    public static final short ORE_TRADE_STATION = 50;
    public static final short short_2703        = 43;
    public static final short short_1919        = 20;
    public static final short short_1205        = 41;
    public static final short short_2424        = 26;
    public static final short short_2318        = 47;
    public static final short short_1153        = 25;
    public static final short short_1534        = 37;
    public static final short short_222         = 10;
    public static final short HEALING_POD        = 33;
    public static final short short_398         = 14;
    public static final short short_2114        = 29;
    public static final short HANGAR_OUTPOST    = 49;
    public static final short short_601         = 42;
    public static final short short_1917        = 44;
    public static final short short_372         = 56;
    public static final short short_1760        = 11;
    public static final short short_740         = 6;
    public static final short REPAIR_STATION    = 53;
    public static final short short_2116        = 12;
    public static final short HANGAR_HOME       = 48;
    public static final short ASTEROID          = 35;
    public static final short short_1248        = 21;
    public static final short short_1758        = 24;
    public static       int   ID                = 32718;

    public short typeValue = 0;

    public AssetTypeModule(short param1) {
        this.typeValue = param1;
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
            param1.writeShort(this.typeValue);
        } catch (IOException e) {
        }
    }
}