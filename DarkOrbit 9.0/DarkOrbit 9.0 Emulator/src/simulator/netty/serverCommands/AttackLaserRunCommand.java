package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class AttackLaserRunCommand
        implements ServerCommand {

    public static int ID = 29819;

    public int     var_2879 = 0;
    public boolean name_75  = false;
    public boolean var_1774 = false;
    public int     name_87  = 0;
    public int     var_169  = 0;

    public AttackLaserRunCommand(int param1, int param2, int param3, boolean param4, boolean param5) {
        this.name_87 = param1;
        this.var_2879 = param2;
        this.var_169 = param3;
        this.var_1774 = param4;
        this.name_75 = param5;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 14;
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
            out.writeShort(26615);
            out.writeBoolean(this.var_1774);
            out.writeInt(this.var_169 >>> 12 | this.var_169 << 20);
            out.writeInt(this.var_2879 >>> 13 | this.var_2879 << 19);
            out.writeInt(this.name_87 >>> 3 | this.name_87 << 29);
            out.writeBoolean(this.name_75);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}