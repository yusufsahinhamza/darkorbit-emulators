package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 22-03-2015.
 */
public class AsteroidProgressCommand {

    public static int ID = 12826;

    public boolean               buildButtonActive;
    public long                  bestProgress;
    public String                ownClanName;
    public EquippedModulesModule state;
    public long                  ownProgress;
    public int                   battleStationId;
    public String                bestProgressClanName;

    public AsteroidProgressCommand(int param1, long param2, long param3, String param4, String param5,
                                   EquippedModulesModule param6, boolean param7) {
        this.battleStationId = param1;
        this.ownProgress = param2;
        this.bestProgress = param3;
        this.ownClanName = param4;
        this.bestProgressClanName = param5;
        this.state = param6;
        this.buildButtonActive = param7;
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
            param1.writeBoolean(this.buildButtonActive);
            param1.writeFloat(this.bestProgress);
            param1.writeShort(31634);
            param1.writeUTF(this.ownClanName);
            this.state.write(param1);
            param1.writeFloat(this.ownProgress);
            param1.writeInt(this.battleStationId << 14 | this.battleStationId >>> 18);
            param1.writeUTF(this.bestProgressClanName);
        } catch (IOException e) {
        }
    }
}
