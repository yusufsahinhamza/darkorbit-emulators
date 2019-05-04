package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

/**
 Created by Pedro on 17-03-2015.
 */
public class class_762 {

    public class_762(Vector<class_917> param1) {
        this.contacts = param1;
    }

    public static int ID = 4219;

    public Vector<class_917> contacts;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.contacts.size());
            for (class_917 c : this.contacts) {
                c.write(param1);
            }
            param1.writeShort(1035);
        } catch (IOException e) {
        }
    }
}
