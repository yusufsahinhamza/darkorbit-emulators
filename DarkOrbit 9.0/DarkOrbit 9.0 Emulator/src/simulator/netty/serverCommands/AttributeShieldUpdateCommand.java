package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class AttributeShieldUpdateCommand
        implements ServerCommand {

    public AttributeShieldUpdateCommand(int pShieldNow, int pShieldMax) {
        this.shieldNow = pShieldNow;
        this.shieldMax = pShieldMax;
    }

    public static int ID = 1343;

    public int shieldNow = 0;
    public int shieldMax = 0;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(-18399);
            param1.writeInt(this.shieldMax >>> 9 | this.shieldMax << 23);
            param1.writeInt(this.shieldNow << 14 | this.shieldNow >>> 18);
        } catch (IOException e) {
        }
    }
}