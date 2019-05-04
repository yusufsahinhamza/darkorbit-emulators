package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ClientUIMenuBarModule
        implements ServerCommand {

    public static short NOT_ASSIGNED        = 0;
    public static short GENERIC_FEATURE_BAR = 2;
    public static short GAME_FEATURE_BAR    = 1;

    public static int ID = 7181;

    public String mPosition = "";
    public short  mMenuId   = 0;
    public Vector<ClientUIMenuBarItemModule> mMenuBarItems;
    public String var_792 = "";

    public ClientUIMenuBarModule(String pPosition, short pMenuId, Vector<ClientUIMenuBarItemModule> pMenuBarItems,
                                 String param4) {
        this.mPosition = pPosition;
        this.mMenuId = pMenuId;
        this.mMenuBarItems = pMenuBarItems;
        this.var_792 = param4;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 8;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream out) {
        try {
            out.writeShort(-10485);
            out.writeUTF(this.mPosition);
            out.writeShort(-15203);
            out.writeUTF(this.var_792);
            out.writeInt(this.mMenuBarItems.size());
            for (ClientUIMenuBarItemModule menuBarItem : this.mMenuBarItems) {
                menuBarItem.write(out);
            }
            out.writeShort(this.mMenuId);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}