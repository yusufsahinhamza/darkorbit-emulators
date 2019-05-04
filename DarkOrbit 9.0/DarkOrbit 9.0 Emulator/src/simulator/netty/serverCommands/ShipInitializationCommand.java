package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ShipInitializationCommand
        implements ServerCommand {

    public static int ID = 15214;
    public int userID, mClanID, cargo, speed, mapID, expansionStage, rings, shield, maxCargo, x, y, nanoHull,
            maxNanoHull, hp, level, maxShield, factionID, maxHp, rankID;
    public double uridium, honor, experience, credits;
    public String shipType, clanTag, username;
    public boolean premium, var_3427, var_2560, isCloaked;
    public float                         jackpot;
    public Vector<VisualModifierCommand> modifiers;

    public ShipInitializationCommand(int userID, String username, String shipType, int speed, int shield, int maxShield,
                                     int hp, int maxHp, int cargo, int maxCargo, int nanoHull, int maxNanoHull, int x,
                                     int y, int mapID, int factionID, int pClanID, int expansionStage, boolean premium,
                                     double experience, double honor, short level, long credits, long uridium,
                                     float jackpot, int rankID, String clanTag, int rings, boolean unknown2,
                                     boolean cloaked, boolean unknown3, Vector<VisualModifierCommand> pModifiers) {
        this.userID = userID;
        this.username = username;
        this.shipType = shipType;
        this.speed = speed;
        this.shield = shield;
        this.maxShield = maxShield;
        this.hp = hp;
        this.maxHp = maxHp;
        this.cargo = cargo;
        this.maxCargo = maxCargo;
        this.nanoHull = nanoHull;
        this.maxNanoHull = maxNanoHull;
        this.x = x;
        this.y = y;
        this.mapID = mapID;
        this.factionID = factionID;
        this.mClanID = pClanID;
        this.expansionStage = expansionStage;
        this.premium = premium;
        this.experience = experience;
        this.honor = honor;
        this.level = level;
        this.credits = credits;
        this.uridium = uridium;
        this.jackpot = jackpot;
        this.rankID = rankID;
        this.clanTag = clanTag;
        this.rings = rings;
        this.var_2560 = unknown2;
        this.isCloaked = cloaked;
        this.var_3427 = unknown3;
        this.modifiers = pModifiers;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 126;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream param1) {
        try {
            param1.writeDouble(this.uridium);
            param1.writeInt(this.maxShield << 3 | this.maxShield >>> 29);
            param1.writeInt(0 << 12 | 0 >>> 20);
            param1.writeDouble(this.credits);
            param1.writeBoolean(this.premium);
            param1.writeInt(this.mClanID >>> 4 | this.mClanID << 28);
            param1.writeInt(0 << 12 | 0 >>> 20);
            param1.writeBoolean(this.var_3427);
            param1.writeDouble(this.experience);
            param1.writeInt(this.mapID >>> 6 | this.mapID << 26);
            param1.writeInt(this.shield >>> 8 | this.shield << 24);
            param1.writeInt(this.y << 11 | this.y >>> 21);
            param1.writeBoolean(this.var_2560);
            param1.writeInt(this.modifiers.size());
            for (VisualModifierCommand c : this.modifiers) {
                c.write(param1);
            }
            param1.writeUTF(this.shipType);
            param1.writeUTF(this.clanTag);
            param1.writeInt(this.userID >>> 7 | this.userID << 25);
            param1.writeInt(this.rings >>> 10 | this.rings << 22);
            param1.writeInt(this.hp << 2 | this.hp >>> 30);
            param1.writeInt(this.rankID >>> 9 | this.rankID << 23);
            param1.writeInt(0 << 4 | 0 >>> 28);
            param1.writeInt(this.speed >>> 6 | this.speed << 26);
            param1.writeBoolean(this.isCloaked);
            param1.writeFloat(this.jackpot);
            param1.writeInt(this.nanoHull >>> 13 | this.nanoHull << 19);
            param1.writeInt(this.x >>> 9 | this.x << 23);
            param1.writeInt(this.maxNanoHull >>> 3 | this.maxNanoHull << 29);
            param1.writeUTF(this.username);
            param1.writeInt(this.level << 15 | this.level >>> 17);
            param1.writeDouble(this.honor);
            param1.writeInt(this.maxHp >>> 6 | this.maxHp << 26);
            param1.writeInt(this.factionID >>> 13 | this.factionID << 19);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}