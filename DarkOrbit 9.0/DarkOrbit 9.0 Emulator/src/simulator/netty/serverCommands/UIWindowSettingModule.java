package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class UIWindowSettingModule
        implements ServerCommand {

    public UIWindowSettingModule(WindowIDModule param1, int param2, int param3, boolean param4) {
        this.name_29 = param1;
        this.x = param2;
        this.y = param3;
        this.maximized = param4;
    }

    public static int ID = 1662;

    public int x = 0;
    public WindowIDModule name_29;
    public int     y         = 0;
    public boolean maximized = false;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.x << 12 | this.x >>> 20);
            param1.writeShort(13509);
            this.name_29.write(param1);
            param1.writeInt(this.y >>> 13 | this.y << 19);
            param1.writeShort(18764);
            param1.writeBoolean(this.maximized);
        } catch (IOException e) {
        }
    }
}