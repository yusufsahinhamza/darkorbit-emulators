package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Shock & Pedro on 30-03-2015.
 */
public class DestructionTypeModule {

    public static short PLAYER        = 0;
    public static short NPC           = 1;
    public static short RADITATION    = 2;
    public static short MINE          = 3;
    public static short MISC          = 4;
    public static short BATTLESTATION = 5;

    public static int ID = 8935;

    public short mCause = 0;

    public DestructionTypeModule(short pCause) {
        this.mCause = pCause;
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
            param1.writeShort(-20405);
            param1.writeShort(this.mCause);
        } catch (IOException e) {
        }
    }
}
