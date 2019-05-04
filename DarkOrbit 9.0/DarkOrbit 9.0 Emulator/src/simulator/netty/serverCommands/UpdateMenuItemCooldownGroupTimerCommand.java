package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 04-04-2015.
 */
public class UpdateMenuItemCooldownGroupTimerCommand
        implements ServerCommand {

    public CooldownTypeModule                          var_2679;
    public long                                        maxTime;
    public long                                        time;
    public ClientUISlotBarCategoryItemTimerStateModule timerState;

    public UpdateMenuItemCooldownGroupTimerCommand(CooldownTypeModule param1,
                                                   ClientUISlotBarCategoryItemTimerStateModule param2, long param3,
                                                   long param4) {
        this.var_2679 = param1;
        this.timerState = param2;
        this.time = param3;
        this.maxTime = param4;
    }

    public static int ID = 413;

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        {
            try {
                this.var_2679.write(param1);
                param1.writeDouble(this.maxTime);
                param1.writeShort(16635);
                param1.writeDouble(this.time);
                this.timerState.write(param1);
            } catch (IOException e) {
            }
        }
    }
}
