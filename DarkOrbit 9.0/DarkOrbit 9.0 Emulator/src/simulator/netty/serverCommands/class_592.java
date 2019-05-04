package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_592
        implements ServerCommand {

    public static int ID = 4323;
    public boolean var_1151;
    public boolean var_2239;
    public boolean mQuestsLevelOrderDescending;
    public boolean mQuestsAvailableFilter;
    public boolean mQuestsUnavailableFilter;
    public boolean mQuestsCompletedFilter;

    public class_592(boolean pQuestsAvailableFilter, boolean pQuestsUnavailableFilter, boolean pQuestsCompletedFilter,
                     boolean var_1151, boolean var_2239, boolean pQuestsLevelOrderDescending) {
        this.mQuestsAvailableFilter = pQuestsAvailableFilter;
        this.mQuestsUnavailableFilter = pQuestsUnavailableFilter;
        this.mQuestsCompletedFilter = pQuestsCompletedFilter;
        this.var_1151 = var_1151;
        this.var_2239 = var_2239;
        this.mQuestsLevelOrderDescending = pQuestsLevelOrderDescending;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 6;
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
            out.writeBoolean(this.mQuestsAvailableFilter);
            out.writeBoolean(this.mQuestsUnavailableFilter);
            out.writeBoolean(this.var_2239);
            out.writeBoolean(this.mQuestsCompletedFilter);
            out.writeShort(22624);
            out.writeBoolean(this.mQuestsLevelOrderDescending);
            out.writeBoolean(this.var_1151);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}