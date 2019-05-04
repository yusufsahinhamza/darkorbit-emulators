package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class DroneFormationChangeCommand
        implements ServerCommand {

    public DroneFormationChangeCommand(int param1, int param2) {
        this.uid = param1;
        this.selectedFormationId = param2;
    }

    public static int ID = 8735;

    public int selectedFormationId = 0;
    public int uid                 = 0;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(-29201);
            param1.writeInt(this.selectedFormationId >>> 12 | this.selectedFormationId << 20);
            param1.writeInt(this.uid << 11 | this.uid >>> 21);
            param1.writeShort(29445);
        } catch (IOException e) {
        }
    }
}