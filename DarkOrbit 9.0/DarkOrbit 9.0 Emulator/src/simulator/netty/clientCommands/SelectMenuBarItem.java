package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class SelectMenuBarItem
        extends ClientCommand {

    public static final int ID = 31171;

    public static short const_2391 = 0;
    public static short const_2225 = 1;
    public static short SELECT     = 0;
    public static short ACTIVATE   = 1;

    public short  var_1531 = 0;
    public String mItemId  = "";
    public short  var_1545 = 0;

    public SelectMenuBarItem(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
        try {
            this.mItemId = in.readUTF();
            this.var_1531 = in.readShort();
            this.var_1545 = in.readShort();
        } catch (IOException e) {
        }
    }
}