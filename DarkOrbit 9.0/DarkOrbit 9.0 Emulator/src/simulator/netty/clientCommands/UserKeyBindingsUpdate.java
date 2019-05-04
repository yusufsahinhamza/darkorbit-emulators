package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ClientCommand;

/**
 Description: This is the user key bindings update

 @author Asus
 @date 23/07/2014
 @file UserKeyBindingsUpdate.java
 @package game.objects.netty.commands */
public class UserKeyBindingsUpdate
        extends ClientCommand {

    public static final int ID = 8739;
    public Vector<UserKeyBindingsModule> changedKeyBindings;
    public boolean                       remove;

    /**
     Constructor
     */
    public UserKeyBindingsUpdate(DataInputStream in) {
        super(in, ID);
        this.changedKeyBindings = new Vector<>();
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            int i = 0;
            int length = in.readInt();
            while (i < length) {
                in.readShort();
                UserKeyBindingsModule ukbm = new UserKeyBindingsModule(in);
                ukbm.readInternal();
                changedKeyBindings.add(ukbm);
                i++;
            }
            remove = in.readBoolean();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}
