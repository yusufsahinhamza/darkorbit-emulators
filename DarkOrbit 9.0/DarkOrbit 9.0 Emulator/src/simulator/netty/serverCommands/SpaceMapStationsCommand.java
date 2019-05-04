package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class SpaceMapStationsCommand
        implements ServerCommand {

    public static int    ID       = 15562;
    public        double var_2893 = 0;
    public Vector<SpaceMapStationModule> stations;

    public SpaceMapStationsCommand(double param1, Vector<SpaceMapStationModule> pStations) {
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
            out.writeShort(-14172);
            out.writeInt(this.stations.size());
            for (SpaceMapStationModule c : this.stations) {
                c.write(out);
            }
            out.writeShort(-6389);
            out.writeDouble(this.var_2893);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}