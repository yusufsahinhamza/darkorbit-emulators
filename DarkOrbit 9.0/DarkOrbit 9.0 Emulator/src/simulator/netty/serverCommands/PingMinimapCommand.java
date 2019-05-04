package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 10/01/2018.
*/

public class PingMinimapCommand
        implements ServerCommand {

    public static int ID = 3515;

    public int x;   
    public int y;
   
    public PingMinimapCommand(int param1, int param2) {
    	this.x = param1;
    	this.y = param2;
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
            param1.writeInt(this.x >>> 1 | this.x << 31);
            param1.writeInt(this.y >>> 3 | this.y << 29);
            param1.writeShort(2842);
        } catch (IOException e) {
        }
    }

}
