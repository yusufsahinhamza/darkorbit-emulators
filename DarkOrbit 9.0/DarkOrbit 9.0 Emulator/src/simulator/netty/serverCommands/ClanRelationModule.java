package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ClanRelationModule
        implements ServerCommand {

    public static short NONE = 0;
    public static short ALLIED = 1;
    public static short NON_AGGRESSION_PACT  = 2;
    public static short AT_WAR  = 3;

    public static int ID = 2061;

    public short type = 0;

    public ClanRelationModule(short pType) {
        this.type = pType;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 0;
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
            out.writeShort(this.type);
            out.writeShort(15770);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}