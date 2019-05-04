package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 22-03-2015.
 */
public class BattleStationBuildingUiInitializationCommand
        implements ServerCommand {

    public static int ID = 28731;

    public int                     buildTimeInMinutesIncrement;
    public AvailableModulesCommand availableModules;
    public int                     battleStationId;
    public int                     buildTimeInMinutesMax;
    public String                  battleStationName;
    public AsteroidProgressCommand progress;
    public int                     buildTimeInMinutesMin;
    public int                     mapAssetId;

    public BattleStationBuildingUiInitializationCommand(int param1, int param2, String param3,
                                                        AsteroidProgressCommand param4, AvailableModulesCommand param5,
                                                        int param6, int param7, int param8) {
        this.mapAssetId = param1;
        this.battleStationId = param2;
        this.battleStationName = param3;
        this.progress = param4;
        this.availableModules = param5;
        this.buildTimeInMinutesMin = param6;
        this.buildTimeInMinutesMax = param7;
        this.buildTimeInMinutesIncrement = param8;
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
            param1.writeInt(this.buildTimeInMinutesIncrement >>> 15 | this.buildTimeInMinutesIncrement << 17);
            this.availableModules.write(param1);
            param1.writeInt(this.battleStationId << 15 | this.battleStationId >>> 17);
            param1.writeInt(this.buildTimeInMinutesMax << 13 | this.buildTimeInMinutesMax >>> 19);
            param1.writeUTF(this.battleStationName);
            param1.writeShort(9415);
            this.progress.write(param1);
            param1.writeInt(this.buildTimeInMinutesMin >>> 15 | this.buildTimeInMinutesMin << 17);
            param1.writeInt(this.mapAssetId << 10 | this.mapAssetId >>> 22);
        } catch (IOException e) {
        }
    }
}
