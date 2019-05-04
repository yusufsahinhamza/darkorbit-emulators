package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class ClientUISlotBarCategoryItemTimerModule
        implements ServerCommand {

    public static int ID = 4902;
    public ClientUISlotBarCategoryItemTimerStateModule timerState;
    public String  var_1474    = "";
    public boolean activatable = false;
    public double  time        = 0;
    public double  maxTime     = 0;

    public ClientUISlotBarCategoryItemTimerModule(double pTime, ClientUISlotBarCategoryItemTimerStateModule pTimerState,
                                                  double pMaxTime, String param1, boolean pActivatable) {
        this.var_1474 = param1;
        this.timerState = pTimerState;
        this.time = pTime;
        this.maxTime = pMaxTime;
        this.activatable = pActivatable;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 19;
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
            out.writeShort(-30833);
            out.writeBoolean(this.activatable);
            out.writeUTF(this.var_1474);
            out.writeShort(29191);
            out.writeDouble(this.maxTime);
            this.timerState.write(out);
            out.writeDouble(this.time);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}