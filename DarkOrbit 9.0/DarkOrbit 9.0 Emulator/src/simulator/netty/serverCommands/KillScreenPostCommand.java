package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Shock & Pedro on 30-03-2015.
 */
public class KillScreenPostCommand
        implements ServerCommand {

    public static int ID = 7217;

    public String                         mKillerName;
    public String                         mKillerEpppLink;
    public String                         mPlayerShipAlias;
    public DestructionTypeModule          mCause;
    public Vector<KillScreenOptionModule> mOptions;

    public KillScreenPostCommand(String pKillerName, String pKillerEpppLink, String pPlayerShipAlias,
                                 DestructionTypeModule pCause, Vector<KillScreenOptionModule> pOptions) {
        this.mKillerName = pKillerName;
        this.mKillerEpppLink = pKillerEpppLink;
        this.mPlayerShipAlias = pPlayerShipAlias;
        this.mCause = pCause;
        this.mOptions = pOptions;
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
            param1.writeUTF(this.mPlayerShipAlias);
            param1.writeUTF(this.mKillerName);
            this.mCause.write(param1);
            param1.writeInt(this.mOptions.size());
            for (KillScreenOptionModule c : this.mOptions) {
                c.write(param1);
            }
            param1.writeUTF(this.mKillerEpppLink);
        } catch (IOException e) {
        }
    }
}
