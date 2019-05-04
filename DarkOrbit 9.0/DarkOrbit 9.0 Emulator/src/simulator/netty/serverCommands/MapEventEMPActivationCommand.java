package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class MapEventEMPActivationCommand
        implements ServerCommand {

    public static int ID = 21200;

    public int var_3045 = 0;
    public int y        = 0;
    public int x        = 0;

    public MapEventEMPActivationCommand(int param1, int param2, int param3) {
        this.var_3045 = param1;
        this.x = param2;
        this.y = param3;
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
            param1.writeInt(this.var_3045 >>> 7 | this.var_3045 << 25);
            param1.writeInt(this.y << 1 | this.y >>> 31);
            param1.writeInt(this.x >>> 4 | this.x << 28);
            param1.writeShort(11807);
        } catch (IOException e) {
        }
    }
}