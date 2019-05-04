package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class UICamLockToCoordinatesCommand
        implements ServerCommand {

    public static int ID = 16642;

    public int x = 0;   
    public int y = 0;    
    public float duration = 0;

    public UICamLockToCoordinatesCommand(int param1, int param2, float param3) {
        this.x = param1;
        this.y = param2;
        this.duration = param3;
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
            param1.writeInt(this.x >>> 5 | this.x << 27);
            param1.writeShort(12321);
            param1.writeInt(this.y << 8 | this.y >>> 24);
            param1.writeFloat(this.duration);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}