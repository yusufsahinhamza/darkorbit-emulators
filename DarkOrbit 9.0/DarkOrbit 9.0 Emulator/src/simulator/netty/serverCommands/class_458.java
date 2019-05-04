package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_458
        implements ServerCommand {

    public static int    ID       = 29414;
    public        double var_2893 = 0;
    public Vector<class_559> stations;

    public class_458(double param1, Vector<class_559> pStations) {
        this.var_2893 = param1;
        this.stations = pStations;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 12;
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
            out.writeDouble(this.var_2893);
            out.writeInt(this.stations.size());
            for (class_559 c : this.stations) {
                c.write(out);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}