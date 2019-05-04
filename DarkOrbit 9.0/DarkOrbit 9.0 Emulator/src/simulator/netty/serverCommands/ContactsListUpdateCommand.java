package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 17-03-2015.
 */
public class ContactsListUpdateCommand
        implements ServerCommand {

    public ContactsListUpdateCommand(class_762 param1, class_922 param2, class_875 param3) {
        this.var_3299 = param1;
        this.var_491 = param2;
        this.var_2408 = param3;
    }

    public static int ID = 12440;

    public class_762 var_3299;
    public class_922 var_491;
    public class_875 var_2408;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(13875);
            this.var_3299.write(param1);
            this.var_491.write(param1);
            this.var_2408.write(param1);
        } catch (IOException e) {
        }
    }
}
