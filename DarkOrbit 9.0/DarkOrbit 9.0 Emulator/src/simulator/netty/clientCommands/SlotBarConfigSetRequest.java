package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class SlotBarConfigSetRequest
        extends ClientCommand {

    public static final int ID = 23820;

    public int    mFromIndex     = 0;
    public String mItemId        = "";
    public String mFromSlotBarId = "";
    public int    mToIndex       = 0;
    public String mToSlotBarId   = "";

    public SlotBarConfigSetRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
        try {
            this.mItemId = in.readUTF();
            this.mFromSlotBarId = in.readUTF();
            this.mToSlotBarId = in.readUTF();
            in.readShort();
            in.readShort();
            this.mToIndex = in.readInt();
            this.mToIndex = this.mToIndex << 13 | this.mToIndex >>> 19;
            this.mFromIndex = in.readInt();
            this.mFromIndex = this.mFromIndex >>> 2 | this.mFromIndex << 30;
        } catch (IOException e) {
        }
    }
}