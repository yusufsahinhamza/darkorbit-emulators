package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class class_695
        implements ServerCommand {

    public static int ID = 23155;

    public String var_3042 = "";

    public int y = 0;

    public int expansionStage = 0;

    public String name_78 = "";

    public ClanRelationModule var_135;

    public int var_3301 = 0;

    public boolean var_742 = false;

    public int var_920 = 0;

    public int var_2434 = 0;

    public int var_1079 = 0;

    public int var_1011 = 0;

    public boolean name_74 = false;

    public int x = 0;

    public int var_1577 = 0;

    public class_365 var_2990;

    public int var_1939 = 0;

    public class_695(int param1, int param2, int param3, int pExpansionStage, String param5, int param6, int param7,
                     int param8, String param9, ClanRelationModule out0, int pX, int pY, int out3, boolean out4, boolean out5,
                     class_365 out6) {
        this.var_1011 = param1;
        this.var_1577 = param2;
        this.var_1079 = param3;
        this.expansionStage = pExpansionStage;
        this.var_3042 = param5;
        this.var_1939 = param6;
        this.var_3301 = param7;
        this.var_2434 = param8;
        this.name_78 = param9;
        this.var_135 = out0;
        this.x = pX;
        this.y = pY;
        this.var_920 = out3;
        this.var_742 = out4;
        this.name_74 = out5;
        this.var_2990 = out6;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 38;
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
            out.writeUTF(this.var_3042);
            out.writeInt(this.y << 4 | this.y >>> 28);
            out.writeShort(this.expansionStage);
            out.writeUTF(this.name_78);
            this.var_135.write(out);
            out.writeInt(this.var_3301 << 13 | this.var_3301 >>> 19);
            out.writeBoolean(this.var_742);
            out.writeInt(this.var_920 << 6 | this.var_920 >>> 26);
            out.writeShort(this.var_2434);
            out.writeShort(this.var_1079);
            out.writeInt(this.var_1011 << 11 | this.var_1011 >>> 21);
            out.writeBoolean(this.name_74);
            out.writeInt(this.x >>> 8 | this.x << 24);
            out.writeInt(this.var_1577 << 8 | this.var_1577 >>> 24);
            out.writeShort(-13737);
            this.var_2990.write(out);
            out.writeShort(this.var_1939);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}