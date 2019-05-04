package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 11-03-2015.
 */
public class UIWindowSettingsCommand
        implements ServerCommand {

    public UIWindowSettingsCommand(Vector<UIWindowSettingModule> param1) {
        this.var_2048 = param1;
    }

    public static int ID = 18;

    public Vector<UIWindowSettingModule> var_2048;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(26603);
            param1.writeInt(this.var_2048.size());
            for (UIWindowSettingModule settingModule : this.var_2048) {
                settingModule.write(param1);
            }
            //param1.writeShort(23255);
        } catch (IOException e) {
        }
    }
}
