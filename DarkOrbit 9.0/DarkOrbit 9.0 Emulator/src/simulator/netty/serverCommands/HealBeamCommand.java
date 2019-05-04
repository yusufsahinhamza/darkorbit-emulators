package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 13/01/2018.
 */

public class HealBeamCommand
        implements ServerCommand {

    public static int ID = 13749;

    public int sourcePositionY = 0;    
    public int targetId = 0;    
    public int sourcePositionX = 0;    
    public String sourceId = "";

    public HealBeamCommand(String param1, int param2, int param3, int param4) {
        this.sourceId = param1;
        this.sourcePositionX = param2;
        this.sourcePositionY = param3;
        this.targetId = param4;
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
            param1.writeInt(this.sourcePositionY >>> 10 | this.sourcePositionY << 22);
            param1.writeInt(this.targetId >>> 16 | this.targetId << 16);
            param1.writeInt(this.sourcePositionX << 3 | this.sourcePositionX >>> 29);
            param1.writeShort(30097);
            param1.writeUTF(this.sourceId);
        } catch (IOException e) {
        }
    }
}
