package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Created by Pedro on 22-03-2015.
 */
public class AssetHandleClickRequest
        extends ClientCommand {

    public DataInputStream in;
    public static int ID = 9044;

    public int mAssetId;

    public AssetHandleClickRequest(DataInputStream in) {
        super(in, ID);
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(18873);
            param1.writeInt(this.mAssetId << 4 | this.mAssetId >>> 28);
        } catch (IOException e) {
        }
    }
}
