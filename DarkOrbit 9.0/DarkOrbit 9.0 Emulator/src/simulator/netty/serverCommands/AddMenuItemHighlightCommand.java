package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 08-04-2015.
 */
public class AddMenuItemHighlightCommand
        implements ServerCommand {

    public static int ID = 20015;

    public class_835 var_3212;
    public class_452 var_170;
    public class_829 var_2550;
    public String itemId = "";

    public AddMenuItemHighlightCommand(class_835 param1, String param2, class_452 param3, class_829 param4) {
        this.var_3212 = param1;
        this.itemId = param2;
        this.var_170 = param3;
        this.var_2550 = param4;
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
            this.var_3212.write(param1);
            this.var_170.write(param1);
            param1.writeShort(26737);
            this.var_2550.write(param1);
            param1.writeUTF(this.itemId);
        } catch (IOException e) {
        }
    }
}
