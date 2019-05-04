package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class BeaconCommand
        implements ServerCommand {

    public static int     ID       = 14338;
    public        boolean var_753  = false;
    public        int     var_2302 = 0;
    public        boolean var_2884 = false;
    public        int     var_10   = 0;
    public        int     var_2491 = 0;
    public        boolean var_2157 = false;
    public        String  var_2742 = "";
    public        boolean var_348  = false;
    public        int     var_3097 = 0;

    public BeaconCommand(int param1, int param2, int param3, int param4, boolean param5, boolean param6, boolean param7,
                         String param8, boolean param9) {
        this.var_2302 = param1;
        this.var_10 = param2;
        this.var_2491 = param3;
        this.var_3097 = param4;
        this.var_2884 = param5;
        this.var_753 = param6;
        this.var_2157 = param7;
        this.var_2742 = param8;
        this.var_348 = param9;
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
            param1.writeInt(this.var_2302 << 6 | this.var_2302 >>> 26);
            param1.writeInt(this.var_10 >>> 9 | this.var_10 << 23);
            param1.writeBoolean(this.var_2884);
            param1.writeUTF(this.var_2742);
            param1.writeInt(this.var_2491 << 13 | this.var_2491 >>> 19);
            param1.writeShort(16312);
            param1.writeBoolean(this.var_753);
            param1.writeBoolean(this.var_2157);
            param1.writeBoolean(this.var_348);
            param1.writeInt(this.var_3097 << 7 | this.var_3097 >>> 25);
            param1.writeShort(-24355);
        } catch (IOException e) {
        }
    }
}