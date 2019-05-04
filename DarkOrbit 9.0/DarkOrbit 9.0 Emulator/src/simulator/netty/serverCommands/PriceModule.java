package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 30-03-2015.
 */
public class PriceModule
        implements ServerCommand {

    public static short URIDIUM = 1;
    public static short CREDITS = 0;

    public static int ID = 7671;

    public int   mAmount = 0;
    public short mType   = 0;

    public PriceModule(short pType, int pAmount) {
        this.mType = pType;
        this.mAmount = pAmount;
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
            param1.writeShort(20005);
            param1.writeInt(this.mAmount >>> 15 | this.mAmount << 17);
            param1.writeShort(this.mType);
        } catch (IOException e) {
        }
    }
}
