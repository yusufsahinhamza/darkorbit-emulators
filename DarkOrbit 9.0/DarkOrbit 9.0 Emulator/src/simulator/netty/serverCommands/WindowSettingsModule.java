package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class WindowSettingsModule
        implements ServerCommand {

    public static int     ID              = 319;
    public        boolean mHideAllWindows = false;
    public        int     mScale          = 0;
    public        String  mBarState       = "";

    public WindowSettingsModule(int param1, String param2, boolean param3) {
        this.mScale = param1;
        this.mBarState = param2;
        this.mHideAllWindows = param3;
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
            param1.writeShort(-8579);
            param1.writeBoolean(this.mHideAllWindows);
            param1.writeUTF(this.mBarState);
            param1.writeInt(this.mScale << 9 | this.mScale >>> 23);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}