package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class MessageLocalizedCommand
        implements ServerCommand {

    public static int ID = 23468;

    public String messageKey;

    public MessageLocalizedCommand(String param1) {
        this.messageKey = param1;
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
            param1.writeUTF(this.messageKey);
        } catch (IOException e) {
        }
    }
}
