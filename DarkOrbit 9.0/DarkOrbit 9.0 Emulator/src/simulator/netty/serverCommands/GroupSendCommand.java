package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 11/12/2017.
*/

public class GroupSendCommand
        implements ServerCommand {

    public static int ID = 30020;
    
    public String name;
    
    public GroupSendCommand(String param1) {
    	this.name = param1;
    }
    
    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream param1) {
        try {
            param1.writeUTF(this.name);
            param1.writeShort(27936);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}