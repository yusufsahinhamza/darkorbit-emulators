package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class AttackHitCommand
        implements ServerCommand {

    public static int ID = 19338;
    public AttackTypeModule var_1975;
    public int     var_1507 = 0;
    public int     damage   = 0;
    public int     var_1066 = 0;
    public int     var_1116 = 0;
    public int     var_3467 = 0;
    public int     name_87  = 0;
    public boolean var_3512 = false;

    public AttackHitCommand(AttackTypeModule param1, int param2, int param3, int param4, int param5, int param6,
                            int param7, boolean param8) {
        this.var_1975 = param1;
        this.name_87 = param2;
        this.var_3467 = param3;
        this.var_1507 = param4;
        this.var_1066 = param5;
        this.var_1116 = param6;
        this.damage = param7;
        this.var_3512 = param8;
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
            param1.writeBoolean(this.var_3512);
            this.var_1975.write(param1);
            param1.writeShort(5675);
            param1.writeInt(this.var_1116 << 6 | this.var_1116 >>> 26);
            param1.writeInt(this.var_3467 >>> 3 | this.var_3467 << 29);
            param1.writeInt(this.name_87 >>> 6 | this.name_87 << 26);
            param1.writeInt(this.var_1066 >>> 3 | this.var_1066 << 29);
            param1.writeShort(24643);
            param1.writeInt(this.var_1507 >>> 10 | this.var_1507 << 22);
            param1.writeInt(this.damage >>> 7 | this.damage << 25);
        } catch (IOException e) {
        }
    }
}