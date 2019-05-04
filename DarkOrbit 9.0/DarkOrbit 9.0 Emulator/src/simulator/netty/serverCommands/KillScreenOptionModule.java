package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Shock & Pedro on 30-03-2015.
 */
public class KillScreenOptionModule
        implements ServerCommand {

    public static int ID = 30589;

    public KillScreenOptionTypeModule      mRepairType;
    public PriceModule                     mPrice;
    public boolean                         mAffordableForPlayer;
    public int                             mCooldownTime;
    public MessageLocalizedWildcardCommand mDescriptionKey;
    public MessageLocalizedWildcardCommand mDescriptionKeyAddon;
    public MessageLocalizedWildcardCommand mToolTipKey;
    public MessageLocalizedWildcardCommand mButtonKey;

    public KillScreenOptionModule(KillScreenOptionTypeModule pRepairType, PriceModule pPrice,
                                  boolean pAffordableForPlayer, int pCooldownTime,
                                  MessageLocalizedWildcardCommand pDescriptionKey,
                                  MessageLocalizedWildcardCommand pDescriptionKeyAddon,
                                  MessageLocalizedWildcardCommand pToolTipKey,
                                  MessageLocalizedWildcardCommand pButtonKey) {
        this.mRepairType = pRepairType;
        this.mPrice = pPrice;
        this.mAffordableForPlayer = pAffordableForPlayer;
        this.mCooldownTime = pCooldownTime;
        this.mDescriptionKey = pDescriptionKey;
        this.mDescriptionKeyAddon = pDescriptionKeyAddon;
        this.mToolTipKey = pToolTipKey;
        this.mButtonKey = pButtonKey;
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
            param1.writeShort(-22445);
            param1.writeInt(this.mCooldownTime >>> 7 | this.mCooldownTime << 25);
            this.mPrice.write(param1);
            this.mToolTipKey.write(param1);//
            this.mDescriptionKeyAddon.write(param1);//
            this.mRepairType.write(param1);
            this.mButtonKey.write(param1);//
            this.mDescriptionKey.write(param1);//
            param1.writeShort(30426);
            param1.writeBoolean(this.mAffordableForPlayer);
        } catch (IOException e) {
        }
    }
}
