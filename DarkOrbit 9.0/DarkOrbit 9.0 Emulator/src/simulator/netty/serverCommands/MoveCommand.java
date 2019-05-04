package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class MoveCommand
        implements ServerCommand {

    public static int ID           = 27383;
    public        int mTargetX     = 0;
    public        int mTargetY     = 0;
    public        int mMapEntityId = 0;
    public        int mDuration    = 0;

    public MoveCommand(int pMapEntityId, int pTargetX, int pTargetY, int pDuration) {
        this.mMapEntityId = pMapEntityId;
        this.mTargetX = pTargetX;
        this.mTargetY = pTargetY;
        this.mDuration = pDuration;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 16;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(16229);
            param1.writeInt(this.mTargetY >>> 14 | this.mTargetY << 18);
            param1.writeInt(this.mTargetX << 4 | this.mTargetX >>> 28);
            param1.writeInt(this.mDuration >>> 15 | this.mDuration << 17);
            param1.writeInt(this.mMapEntityId >>> 12 | this.mMapEntityId << 20);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}