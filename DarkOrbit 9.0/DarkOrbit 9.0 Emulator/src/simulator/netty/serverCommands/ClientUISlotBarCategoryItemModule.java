package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ClientUISlotBarCategoryItemModule
        implements ServerCommand {

    public static int   ID          = 16680;
    public static short short_554   = 1;
    public static short TIMER       = 3;
    public static short SELECTION   = 2;
    public static short short_2465  = 0;
    public static short NUMBER      = 1;
    public static short short_1659  = 3;
    public static short BAR         = 2;
    public static short NONE        = 0;
    public        short counterType = 0;
    public ClientUISlotBarCategoryItemTimerModule  timer;
    public ClientUISlotBarCategoryItemStatusModule status;
    public int var_586 = 0;
    public CooldownTypeModule var_854;
    public short actionStyle = 0;

    public ClientUISlotBarCategoryItemModule(int param1, ClientUISlotBarCategoryItemStatusModule pStatus,
                                             short pActionStyle, short pCounterType, CooldownTypeModule param4,
                                             ClientUISlotBarCategoryItemTimerModule pTimer) {
        this.var_586 = param1;
        this.status = pStatus;
        this.timer = pTimer;
        this.var_854 = param4;
        this.counterType = pCounterType;
        this.actionStyle = pActionStyle;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 4;
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
            out.writeShort(this.counterType);
            this.var_854.write(out);
            this.status.write(out);
            out.writeInt(this.var_586 >>> 13 | this.var_586 << 19);
            this.timer.write(out);
            out.writeShort(this.actionStyle);
            out.writeShort(-19107);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}