package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_653
        implements ServerCommand {

    public static int   ID      = 15084;
    public        short content = 0;

    public static short CONTACT_LIST             = 34;
    public static short ITEM_UPGRADE             = 23;
    public static short WELCOME                  = 12;
    public static short PALLADIUM                = 28;
    public static short ROCKET_LAUNCHER          = 30;
    public static short SHIP_REPAIR              = 0;
    public static short const_932                = 15;
    public static short REQUEST_MISSION          = 14;
    public static short THE_SHOP                 = 3;
    public static short AUCTION_HOUSE            = 29;
    public static short SHIP_DESIGN              = 32;
    public static short TRAINING_GROUNDS         = 36;
    public static short HOW_TO_FLY               = 13;
    public static short GET_MORE_AMMO            = 8;
    public static short SELL_RESOURCE            = 10;
    public static short SECOND_CONFIGURATION     = 22;
    public static short TECH_FACTORY             = 26;
    public static short POLICY_CHANGES           = 16;
    public static short UNKOWN_DANGERS           = 18;
    public static short EQUIP_YOUR_ROCKETS       = 17;
    public static short SKYLAB                   = 1;
    public static short FULL_CARGO               = 21;
    public static short JUMP_DEVICE              = 20;
    public static short CHANGING_SHIPS           = 4;
    public static short GALAXY_GATE              = 24;
    public static short JUMP_GATES               = 6;
    public static short SKILL_TREE               = 999;
    public static short WEALTHY_FAMOUS           = 11;
    public static short PVP_WARNING              = 2;
    public static short LOOKING_FOR_GROUPS       = 33;
    public static short ORE_TRANSFER             = 35;
    public static short EXTRA_CPU                = 31;
    public static short BOOST_YOUR_EQUIP         = 9;
    public static short ATTACK                   = 19;
    public static short CLAN_BATTLE_STATION      = 27;
    public static short INSTALLING_NEW_EQUIPMENT = 5;
    public static short PREPARE_BATTLE           = 7;

    public class_653(short pContent) {
        this.content = pContent;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 0;
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
            out.writeShort(26872);
            out.writeShort(this.content);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}