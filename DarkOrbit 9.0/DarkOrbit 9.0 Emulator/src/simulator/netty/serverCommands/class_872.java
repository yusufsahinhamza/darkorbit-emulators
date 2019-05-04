package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_872
        implements ServerCommand {

    public static int ID = 27871;

    public int x = 0;
    public int y = 0;

    public class_872(int pX, int pY) {
        this.x = pX;
        this.y = pY;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 8;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeInt(this.y << 10 | this.y >>> 22);
            out.writeInt(this.x >>> 4 | this.x << 28);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}