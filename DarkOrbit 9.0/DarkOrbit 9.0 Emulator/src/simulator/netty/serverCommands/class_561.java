package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 16-03-2015.
 */
public class class_561 {

    public class_561(int param1, int param2, ClientUITooltipsCommand param3) {
        this.var_1173 = param1;
        this.var_1842 = param2;
        this.toolTip = param3;
    }

    public static int ID = 21162;

    public int var_1173 = 0;
    public int var_1842 = 0;
    public ClientUITooltipsCommand toolTip;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.var_1173 << 2 | this.var_1173 >>> 30);
            param1.writeInt(this.var_1842 << 7 | this.var_1842 >>> 25);
            this.toolTip.write(param1);
        } catch (IOException e) {
        }
    }
}
