package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 03-04-2015.
 */
public class LabUpdateItemCommand
        implements ServerCommand {

    public LabUpdateItemCommand(Vector<UpdateItemModule> param1) {
        this.var_1618 = param1;
    }

    public static int ID = 13925;

    public Vector<UpdateItemModule> var_1618;

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
            param1.writeInt(this.var_1618.size());
            for (UpdateItemModule updateItemModule : this.var_1618) {
                updateItemModule.write(param1);
            }
        } catch (IOException e) {
        }
    }
}
