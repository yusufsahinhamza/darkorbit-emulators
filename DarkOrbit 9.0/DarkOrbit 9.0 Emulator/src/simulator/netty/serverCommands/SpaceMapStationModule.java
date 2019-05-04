package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class SpaceMapStationModule
        implements ServerCommand {

    public static int    ID                   = 17016;
    public        double lastChangedTimestamp = 0;
    public        int    y                    = 0;
    public        short  status               = 0;
    public        int    mapId                = 0;
    public        int    x                    = 0;
    public        String asteroidName         = "";
    public FactionModule owningFaction;
    public String clanName = "";

    public static short NEUTRAL_STATION = 2;
    public static short HOSTILE_STATION = 3;
    public static short OWN_STATION     = 1;
    public static short ASTEROID        = 0;

    public SpaceMapStationModule(int pMapId, int pX, int pY, short pStatus, double param5, String pClanName,
                                 String param7, FactionModule param8) {

        this.mapId = pMapId;
        this.x = pX;
        this.y = pY;
        this.status = pStatus;
        this.lastChangedTimestamp = param5;
        this.clanName = pClanName;
        this.asteroidName = param7;
        this.owningFaction = param8;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 24;
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
            out.writeUTF(this.clanName);
            out.writeInt(this.y << 3 | this.y >>> 29);
            this.owningFaction.write(out);
            out.writeDouble(this.lastChangedTimestamp);
            out.writeShort(this.status);
            out.writeInt(this.mapId << 13 | this.mapId >>> 19);
            out.writeInt(this.x >>> 5 | this.x << 27);
            out.writeUTF(this.asteroidName);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}