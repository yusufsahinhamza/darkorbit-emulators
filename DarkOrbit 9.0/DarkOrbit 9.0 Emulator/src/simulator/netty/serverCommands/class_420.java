package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;


public class class_420
        implements ServerCommand {

    public static short const_2453 = 1;

    public static short const_301 = 0;

    public static int ID = 31526;

    public int var_1271 = 0;

    public short var_922 = 0;

    public String var_3607 = "";

    public Vector<String> languageKeys;

    public boolean showButtons = false;

    public int name_29 = 0;

    public class_420(int param1, String param2, boolean pShowButtons, Vector<String> pLanguageKeys, int param5,
                     short param6) {

        this.name_29 = param1;
        this.var_3607 = param2;
        this.showButtons = pShowButtons;
        this.languageKeys = pLanguageKeys;
        this.var_1271 = param5;
        this.var_922 = param6;

    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 15;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeInt(this.var_1271 >>> 9 | this.var_1271 << 23);
            out.writeShort(15867);
            out.writeShort(this.var_922);
            out.writeUTF(this.var_3607);
            out.writeInt(this.languageKeys.size());
            for (String str : this.languageKeys) {
                out.writeUTF(str);
            }
            out.writeBoolean(this.showButtons);
            out.writeInt(this.name_29 << 3 | this.name_29 >>> 29);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}