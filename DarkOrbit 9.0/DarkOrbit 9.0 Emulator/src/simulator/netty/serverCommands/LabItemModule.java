package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 03-04-2015.
 */
public class LabItemModule {

    public static final short LASER   = 0;
    public static final short ROCKETS = 1;
    public static final short DRIVING = 2;
    public static final short SHIELD  = 3;

    public static int ID = 29012;

    public short itemValue = 0;

    public LabItemModule(short param1) {
        this.itemValue = param1;
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
            param1.writeShort(this.itemValue);
            param1.writeShort(-1916);
            param1.writeShort(7945);
        } catch (IOException e) {
        }
    }
}
