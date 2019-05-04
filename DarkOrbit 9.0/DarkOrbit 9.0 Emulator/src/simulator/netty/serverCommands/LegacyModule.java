package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Description: This is the Legacy module

 @author Ordepsousa
 @date 22/07/2014
 @file LegacyModule.java
 @package game.objects.netty.commands */
public class LegacyModule
        implements ServerCommand {

    public static final int ID = 15145;

    public String message;

    /**
     constructor
     */
    public LegacyModule(String message) {
        this.message = message;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 2;
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
            param1.writeUTF(this.message);
            param1.writeShort(-28556);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}