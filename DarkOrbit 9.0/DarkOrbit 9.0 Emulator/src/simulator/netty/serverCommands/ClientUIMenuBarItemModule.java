package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ClientUIMenuBarItemModule
        implements ServerCommand {

    public static int     ID      = 22023;
    public        String  itemId  = "";
    public        boolean visible = false;
    public ClientUITooltipsCommand toolTip;

    public ClientUIMenuBarItemModule(boolean pVisible, ClientUITooltipsCommand pTooltipCommand, String pItemId) {
        this.visible = pVisible;
        this.toolTip = pTooltipCommand;
        this.itemId = pItemId;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 3;
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
            out.writeBoolean(this.visible);
            out.writeUTF(this.itemId);
            out.writeShort(-6305);
            this.toolTip.write(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}