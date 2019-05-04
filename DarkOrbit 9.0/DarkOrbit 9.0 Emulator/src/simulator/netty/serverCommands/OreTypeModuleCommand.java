package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 14-03-2015.
 */
public class OreTypeModuleCommand {

    public OreTypeModuleCommand(short param1) {
        this.var_1313 = param1;
    }

    public static short DURANIUM = 5;
    public static short PROMETIUM   = 0;
    public static short PROMERIUM  = 6;
    public static short XENOMIT  = 3;
    public static short PALLADIUM  = 8;
    public static short PROMETID  = 4;
    public static short ENDURIUM  = 1;
    public static short SEPROM     = 7;
    public static short TERBIUM = 2;

    public static int ID = 20950;

    public short var_1313 = 0;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(this.var_1313);
        } catch (IOException e) {
        }
    }
}
