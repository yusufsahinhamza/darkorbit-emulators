package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 03-04-2015.
 */
public class AttributeSkillShieldUpdateCommand
        implements ServerCommand {

    public static int ID = 7235;

    public int mShieldSkillId        = 0;
    public int mMaxSkinShieldTwinkle = 0;
    public int mMinSkinShieldTwinkle = 0;

    public AttributeSkillShieldUpdateCommand(int param1, int param2, int param3) {
        this.mShieldSkillId = param1;
        this.mMinSkinShieldTwinkle = param2;
        this.mMaxSkinShieldTwinkle = param3;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        {
            try {
                param1.writeInt(this.mShieldSkillId << 12 | this.mShieldSkillId >>> 20);
                param1.writeShort(-2798);
                param1.writeInt(this.mMaxSkinShieldTwinkle << 10 | this.mMaxSkinShieldTwinkle >>> 22);
                param1.writeInt(this.mMinSkinShieldTwinkle >>> 4 | this.mMinSkinShieldTwinkle << 28);
                param1.writeShort(11372);
            } catch (IOException e) {
            }
        }
    }
}
