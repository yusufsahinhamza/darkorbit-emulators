package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ShipCreateCommand
        implements ServerCommand {

    public static int ID = 18098;

    public ClanRelationModule var_2321;
    public int     mRingsCount     = 0;
    public int     mExpansionStage = 0;
    public int     mPositionY      = 0;
    public boolean mCloaked        = false;
    public boolean var_2560        = false;
    public String  mUsername       = "";
    public int     mPositionX      = 0;
    public int     mFactionId      = 0;
    public int     mMapEntityId    = 0;
    public boolean mIsNpc          = false;
    public Vector<VisualModifierCommand> mModifiers;
    public String  mClanTag    = "";
    public String  mShipLootId = "";
    public int     var_2736    = 0;
    public boolean var_3223    = false;
    public class_365 var_2990;
    public int mPositionIndex = 0;
    public int var_1795       = 0;
    public int mRankId        = 0;

    public ShipCreateCommand(int pMapEntityId, String pShipLootId, int pExpansionStage, String pClanTag,
                             String pUsername, int pPositionX, int pPositionY, int pFactionId, int pRingsCount,
                             int pRankId, boolean param11, ClanRelationModule param12, int param13, boolean param14,
                             boolean pIsNpc, boolean pCloaked, int param17, int pPositionIndex,
                             Vector<VisualModifierCommand> pModifiers, class_365 param20) {
        this.mMapEntityId = pMapEntityId;// User ID or NPC ID
        this.mShipLootId = pShipLootId;//pShipLootId
        this.mExpansionStage = pExpansionStage;
        this.mClanTag = pClanTag;
        this.mUsername = pUsername;
        this.mPositionX = pPositionX;
        this.mPositionY = pPositionY;
        this.mFactionId = pFactionId;
        this.mRingsCount = pRingsCount; //fixledim gerizekalı
        this.mRankId = pRankId;
        this.var_3223 = param11;
        this.var_2321 = param12;
        this.var_2736 = param13;
        this.var_2560 = param14;
        this.mIsNpc = pIsNpc;
        this.mCloaked = pCloaked;
        this.var_1795 = param17;
        this.mPositionIndex = pPositionIndex;
        this.mModifiers = pModifiers;
        this.var_2990 = param20;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 54;
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
            param1.writeUTF(this.mUsername);
            param1.writeBoolean(this.var_2560);
            param1.writeInt(this.mFactionId << 7 | this.mFactionId >>> 25);
            param1.writeBoolean(this.var_3223);
            param1.writeInt(this.mMapEntityId >>> 9 | this.mMapEntityId << 23);
            param1.writeInt(this.mPositionY << 7 | this.mPositionY >>> 25);
            this.var_2321.write(param1);
            param1.writeUTF(this.mShipLootId);
            param1.writeUTF(this.mClanTag);
            param1.writeInt(this.var_2736 << 13 | this.var_2736 >>> 19);
            this.var_2990.write(param1);
            param1.writeInt(this.mPositionIndex << 15 | this.mPositionIndex >>> 17);
            param1.writeInt(this.var_1795 >>> 11 | this.var_1795 << 21);
            param1.writeBoolean(this.mCloaked);
            param1.writeInt(this.mExpansionStage << 7 | this.mExpansionStage >>> 25);
            param1.writeInt(this.mPositionX >>> 13 | this.mPositionX << 19);
            param1.writeInt(this.mRingsCount >>> 5 | this.mRingsCount << 27);
            param1.writeInt(this.mRankId >>> 4 | this.mRankId << 28);
            param1.writeInt(this.mModifiers.size());
            for (VisualModifierCommand c : this.mModifiers) {
                c.write(param1);
            }
            param1.writeBoolean(this.mIsNpc);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}