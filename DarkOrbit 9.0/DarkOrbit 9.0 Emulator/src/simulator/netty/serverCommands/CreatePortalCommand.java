package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class CreatePortalCommand
        implements ServerCommand {

    public static int     ID         = 18277;
    public        int     designId   = 0;
    public        boolean var_89     = false;
    public        boolean showBubble = false;
    public        int     var_3576   = 0;
    public Vector<Integer> var_1648;
    public int x         = 0;
    public int factionId = 0;
    public int y         = 0;

    public CreatePortalCommand(int param1, int param2, int param3, int param4, int param5, boolean param6,
                               boolean param7, Vector<Integer> param8) {
        this.var_3576 = param1;
        this.factionId = param2;
        this.designId = param3;
        this.x = param4;
        this.y = param5;
        this.var_89 = param6;
        this.showBubble = param7;
        this.var_1648 = param8;
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
            param1.writeInt(this.y << 4 | this.y >>> 28);
            param1.writeBoolean(this.showBubble);
            param1.writeInt(this.factionId << 6 | this.factionId >>> 26);
            param1.writeBoolean(this.var_89);
            param1.writeInt(this.designId >>> 4 | this.designId << 28);
            param1.writeInt(this.var_3576 << 9 | this.var_3576 >>> 23);
            param1.writeInt(this.var_1648.size());
            for (Integer i : this.var_1648) {
                param1.writeInt(i >>> 12 | i << 20);
            }
            param1.writeInt(this.x << 10 | this.x >>> 22);
        } catch (IOException e) {
        }
    }
}