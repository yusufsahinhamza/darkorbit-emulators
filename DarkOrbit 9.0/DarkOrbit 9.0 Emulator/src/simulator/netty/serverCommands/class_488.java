package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

/**
 Created by Pedro on 17-03-2015.
 */
public class class_488 {

    public class_488(int param1, Vector<class_873> param2) {
        this.name_90 = param1;
        this.var_1539 = param2;
    }

    public static int ID = 29655;

    public Vector<class_873> var_1539;
    public int               name_90;


    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(21974);
            param1.writeInt(this.var_1539.size());
            for (class_873 c : this.var_1539) {
                c.write(param1);
            }
            param1.writeInt(this.name_90 >>> 3 | this.name_90 << 29);
        } catch (IOException e) {
        }
    }
}
