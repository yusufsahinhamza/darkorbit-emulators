package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 08-04-2015.
 */
public class AddOreCommand
        implements ServerCommand {

    public static int ID = 29544;

    public int x = 0;
    public OreTypeModuleCommand oreType;
    public int    y    = 0;
    public String hash = "";

    public AddOreCommand(String param1, OreTypeModuleCommand param2, int param3, int param4) {
        this.hash = param1;
        this.oreType = param2;
        this.x = param3;
        this.y = param4;
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
            param1.writeInt(this.x << 14 | this.x >>> 18);
            this.oreType.write(param1);
            param1.writeInt(this.y >>> 8 | this.y << 24);
            param1.writeUTF(this.hash);
        } catch (IOException e) {
        }
    }
}
