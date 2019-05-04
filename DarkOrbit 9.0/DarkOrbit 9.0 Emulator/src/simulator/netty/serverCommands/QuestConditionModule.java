package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class QuestConditionModule
        implements ServerCommand {

    public static int ID = 23685;

    public static final short DAMAGE = 7;    
    public static final short AVOID_DAMAGE = 8;   
    public static final short WEB = 46;    
    public static final short BILMIYORUM1 = 73;    
    public static final short VISIT_MAP_ASSET = 71;    
    public static final short PUT_ITEM_IN_SLOT_BAR = 66;    
    public static final short USE_ORE_UPDATE = 68;    
    public static final short AVOID_KILL_NPCS = 34;    
    public static final short STAY_AWAY = 43;    
    public static final short COLLECT_LOOT = 63;    
    public static final short MISCELLANEOUS = 19;    
    public static final short BILMIYORUM2 = 67;    
    public static final short MAP_DIVERSE = 17;    
    public static final short COORDINATES = 11;    
    public static final short LEVEL = 50;    
    public static final short BILMIYORUM3 = 25;    
    public static final short VISIT_MAP = 31;    
    public static final short BILMIYORUM4 = 51;    
    public static final short KILL_NPCS = 27;    
    public static final short REAL_TIME_HASTE = 60;    
    public static final short FINISH_STARTER_GATE = 64;    
    public static final short PREVENT = 38;    
    public static final short STEADINESS = 41;    
    public static final short AVOID_JUMP = 40;    
    public static final short QUICK_BUY = 54;    
    public static final short FUEL_SHORTAGE = 14;    
    public static final short AMMUNITION = 20;    
    public static final short BILMIYORUM5 = 26;    
    public static final short COUNTDOWN = 4;    
    public static final short GAIN_INFLUENCE = 76;    
    public static final short DISTANCE = 12;    
    public static final short BILMIYORUM6 = 53;    
    public static final short JUMP = 39;    
    public static final short ACTIVATE_MAP_ASSET_TYPE = 70;    
    public static final short MAP = 16;    
    public static final short DAMAGE_NPCS = 29;    
    public static final short AVOID_DAMAGE_NPCS = 36;    
    public static final short AVOID_DEATH = 10;    
    public static final short FINISH_GALAXY_GATE = 75;    
    public static final short MULTIPLIER = 42;   
    public static final short BILMIYORUM7 = 0;    
    public static final short SPEND_AMMUNITION = 22;    
    public static final short IN_CLAN = 62;    
    public static final short RESTRICT_AMMUNITION_KILL_PLAYER = 57;    
    public static final short VISIT_QUEST_GIVER = 59;    
    public static final short AVOID_KILL_NPC = 33;   
    public static final short BILMIYORUM8 = 58;    
    public static final short KILL_NPC = 6;    
    public static final short KILL_ANY = 45;    
    public static final short EMPTY = 18;    
    public static final short REFINE_ORE = 65;    
    public static final short IN_GROUP = 44;    
    public static final short SALVAGE = 23;    
    public static final short CLIENT = 47;    
    public static final short HASTE = 2;    
    public static final short SAVE_AMMUNITION = 21;    
    public static final short BEACON_TAKEOVER = 74;    
    public static final short STEAL = 24;    
    public static final short AVOID_DAMAGE_PLAYERS = 37;    
    public static final short VISIT_JUMP_GATE_TO_MAP_TYPE = 69;    
    public static final short DAMAGE_PLAYERS = 30;    
    public static final short CARGO = 48;    
    public static final short UPDATE_SKYLAB_TO_LEVEL = 72;    
    public static final short ENDURANCE = 3;    
    public static final short TAKE_DAMAGE = 9;    
    public static final short TIMER = 1;    
    public static final short DIE = 32;    
    public static final short BILMIYORUM9 = 61;    
    public static final short KILL_PLAYERS = 28;   
    public static final short COLLECT_BONUS_BOX = 52;    
    public static final short TRAVEL = 13;    
    public static final short RESTRICT_AMMUNITION_KILL_NPC = 56;    
    public static final short COLLECT = 5;    
    public static final short AVOID_KILL_PLAYERS = 35;    
    public static final short ENTER_GROUP = 55;    
    public static final short PROXIMITY = 15;   
    public static final short SELL_ORE = 49;     
    
    public Vector<QuestConditionModule> subConditions;    
    public QuestConditionStateModule state;    
    public Vector<String> bilinmeyen;    
    public int id = 0;    
    public short type = 0;    
    public short displayType = 0;    
    public double targetValue = 0;    
    public boolean mandatory = false;

    public QuestConditionModule(int param1, Vector<String> param2, short param3, short param4, double param5, boolean param6, QuestConditionStateModule param7, Vector<QuestConditionModule> param8) {
        this.id = param1; 
        this.bilinmeyen = param2;       
        this.type = param3;
        this.displayType = param4;
        this.targetValue = param5;
        this.mandatory = param6;    
        this.state = param7;        
        this.subConditions = param8;        
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
            param1.writeInt(this.subConditions.size());
            for (QuestConditionModule i : this.subConditions)
            {
               i.write(param1);
            }
            this.state.write(param1);
            param1.writeInt(this.bilinmeyen.size());
            for (String i : this.bilinmeyen)
            {
               param1.writeUTF(i);
            }
            param1.writeInt(this.id << 12 | this.id >>> 20);
            param1.writeShort(this.type);
            param1.writeShort(this.displayType);
            param1.writeDouble(this.targetValue);
            param1.writeBoolean(this.mandatory);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}