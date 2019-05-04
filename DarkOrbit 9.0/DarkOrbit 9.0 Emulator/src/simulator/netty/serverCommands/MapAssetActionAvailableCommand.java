package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class MapAssetActionAvailableCommand implements ServerCommand
{
    public static int ID = 23533;
    public static short OFF = 1;
    public static short ON = 0;

    public ClientUITooltipsCommand toolTip;
    public short state = 0;
    public int var_2240 = 0;
    public boolean activatable = false;
    public class_580 var_3122;

    public MapAssetActionAvailableCommand(int param1, short param2, boolean param3, ClientUITooltipsCommand param4, class_580 param5)
    {
        this.var_2240 = param1;
        this.state = param2;
        this.activatable = param3;
        this.toolTip = param4;
        this.var_3122 = param5;
    }

    public void write(DataOutputStream param1)
    {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch(IOException e) {}
    }

    protected void writeInternal(DataOutputStream param1)
    {
        try {
            this.var_3122.write(param1);
            param1.writeShort(-27081);
            param1.writeShort(16683);
            param1.writeShort(this.state);
            this.toolTip.write(param1);
            param1.writeBoolean(this.activatable);
            param1.writeInt(this.var_2240 << 9 | this.var_2240 >>> 23);
        } catch(IOException e) {}
    }
}