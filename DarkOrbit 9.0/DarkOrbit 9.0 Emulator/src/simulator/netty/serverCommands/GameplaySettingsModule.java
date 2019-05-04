package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class GameplaySettingsModule
        implements ServerCommand {

    public static int     ID                             = 23515;
    public        boolean mNotSet                        = false;
    public        boolean mAutoRefinement                = false;
    public        boolean mQuickSlotStopAttack           = false;
    public        boolean mAutoBoost                     = false;
    public        boolean mAutoBuyBootyKeys              = false;
    public        boolean mDoubleclickAttackEnabled      = false;
    public        boolean mAutochangeAmmo                = false;
    public        boolean mAutoStartEnabled              = false;
    public        boolean mDisplayBattlerayNotifications = false;

    public GameplaySettingsModule(boolean pNotSet, boolean pAutoBoost, boolean pAutoRefinement,
                                  boolean pQuickSlotStopAttack, boolean pDoubleclickAttackEnabled,
                                  boolean pAutochangeAmmo, boolean pAutoStartEnabled, boolean pAutoBuyBootyKeys,
                                  boolean pDisplayBattlerayNotifications) {
        this.mNotSet = pNotSet;
        this.mAutoBoost = pAutoBoost;
        this.mAutoRefinement = pAutoRefinement;
        this.mQuickSlotStopAttack = pQuickSlotStopAttack;
        this.mDoubleclickAttackEnabled = pDoubleclickAttackEnabled;
        this.mAutochangeAmmo = pAutochangeAmmo;
        this.mAutoStartEnabled = pAutoStartEnabled;
        this.mAutoBuyBootyKeys = pAutoBuyBootyKeys;
        this.mDisplayBattlerayNotifications = pDisplayBattlerayNotifications;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 8;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeBoolean(this.mAutoBoost);
            param1.writeBoolean(this.mQuickSlotStopAttack);
            param1.writeBoolean(this.mDoubleclickAttackEnabled);
            param1.writeBoolean(this.mAutoBuyBootyKeys);
            param1.writeBoolean(this.mNotSet);
            param1.writeBoolean(this.mAutoRefinement);
            param1.writeBoolean(this.mAutoStartEnabled);
            param1.writeBoolean(this.mDisplayBattlerayNotifications);
            param1.writeBoolean(this.mAutochangeAmmo);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}