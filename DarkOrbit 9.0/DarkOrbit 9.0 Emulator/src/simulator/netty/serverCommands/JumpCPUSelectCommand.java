package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class JumpCPUSelectCommand
        implements ServerCommand {

    public static int ID = 26184;
    
    public int price = 0;    
    public boolean seletionAllowed = false;    
    public int mapId = 0;

    public JumpCPUSelectCommand(int param1, int param2, boolean param3) {
        this.mapId = param1;
        this.price = param2;
        this.seletionAllowed = param3;
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
            param1.writeInt(this.price << 11 | this.price >>> 21);
            param1.writeShort(-10110);
            param1.writeBoolean(this.seletionAllowed);
            param1.writeInt(this.mapId >>> 5 | this.mapId << 27);
        } catch (IOException e) {
        }
    }
}