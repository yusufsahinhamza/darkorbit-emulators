package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_559
        implements ServerCommand {

    public static int    ID      = 10202;
    public        double name_88 = 0;
    public        int    name_64 = 0;
    public        short  status  = 0;
    public        int    mapId   = 0;
    public        int    name_97 = 0;
    public        String name_6  = "";
    public FactionModule var_1854;
    public String clanName = "";

    public static short const_187  = 2;
    public static short const_2485 = 3;
    public static short const_2232 = 1;
    public static short const_2392 = 0;

    public class_559(int param1, int param2, int param3, short pStatus, double param5, String pCleanName, String param7,
                     FactionModule param8) {

        this.mapId = param1;
        this.name_97 = param2;
        this.name_64 = param3;
        this.status = pStatus;
        this.name_88 = param5;
        this.clanName = pCleanName;
        this.name_6 = param7;
        this.var_1854 = param8;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 24;
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
            out.writeDouble(this.name_88);
            out.writeShort(22140);
            out.writeShort(3321);
            out.writeInt(this.name_64 >>> 7 | this.name_64 << 25);
            out.writeShort(this.status);
            out.writeInt(this.mapId >>> 5 | this.mapId << 27);
            out.writeInt(this.name_97 << 8 | this.name_97 >>> 24);
            out.writeUTF(this.name_6);
            this.var_1854.write(out);
            out.writeUTF(this.clanName);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}