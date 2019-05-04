package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ClientUITooltipModule
        implements ServerCommand {

    public static int   ID       = 32073;
    public        short var_1715 = 0;
    public ClientUITooltipTextFormatModule       var_3482;
    public Vector<ClientUITextReplacementModule> var_3038;
    public String baseKey = "";

    public static short STANDARD = 0;
    public static short RED      = 1;

    public ClientUITooltipModule(ClientUITooltipTextFormatModule param1, short param2, String param3,
                                 Vector<ClientUITextReplacementModule> param4) {
        this.var_3482 = param1;
        this.baseKey = param3;
        this.var_1715 = param2;
        this.var_3038 = param4;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 6;
    }

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
            param1.writeShort(this.var_1715);
            param1.writeShort(-28187);
            param1.writeInt(this.var_3038.size());
            for (ClientUITextReplacementModule c : this.var_3038) {
                c.write(param1);
            }
            param1.writeUTF(this.baseKey);
            param1.writeShort(-20865);
            this.var_3482.write(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}