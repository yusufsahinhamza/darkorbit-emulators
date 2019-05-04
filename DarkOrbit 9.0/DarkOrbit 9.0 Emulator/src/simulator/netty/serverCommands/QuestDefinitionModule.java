package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class QuestDefinitionModule
        implements ServerCommand {

    public static int ID = 27154;

    public static final short b1 = 1;    
    public static final short b2 = 2;    
    public static final short b3 = 0;
         
    public short bilinmeyen = 0;    
    public int id = 0;    
    public QuestCaseModule rootCase;    
    public Vector<LootModule> rewards;    
    public Vector<QuestTypeModule> types;    
    public Vector<QuestIconModule> icons;

    public QuestDefinitionModule(short param1, Vector<QuestTypeModule> param2, short param3, QuestCaseModule param4, Vector<LootModule> param5, Vector<QuestIconModule> param6) {
        this.id = param1;       
        this.types = param2;        
        this.bilinmeyen = param3;       
        this.rootCase = param4;               
        this.rewards = param5;                
        this.icons = param6;       
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
            param1.writeShort(this.bilinmeyen);
            param1.writeInt(this.id >>> 4 | this.id << 28);
            this.rootCase.write(param1);
            param1.writeInt(this.rewards.size());
            for (LootModule i : this.rewards)
            {
               i.write(param1);
            }
            param1.writeInt(this.types.size());
            for (QuestTypeModule i : this.types)
            {
               i.write(param1);
            }
            param1.writeInt(this.icons.size());
            for (QuestIconModule i : this.icons)
            {
               i.write(param1);
            }
            param1.writeShort(-5789);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}