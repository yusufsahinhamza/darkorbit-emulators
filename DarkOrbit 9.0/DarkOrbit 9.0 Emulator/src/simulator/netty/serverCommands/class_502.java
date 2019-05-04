package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

/**
 Created by Pedro on 17-03-2015.
 */
public class class_502 {

    public class_502(int param1, Vector<class_470> param2) {
        this.name_90 = param1;
        this.attributes = param2;
    }

    public static int ID = 29437;

    public int               name_90;
    public Vector<class_470> attributes;


    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.name_90 >>> 5 | this.name_90 << 27);
            param1.writeInt(this.attributes.size());
            for (class_470 c : this.attributes) {
                c.write(param1);
            }

            param1.writeShort(20849);
        } catch (IOException e) {
        }
    }
}
