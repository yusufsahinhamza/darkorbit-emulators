package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class QuestCaseModule
        implements ServerCommand {

    public static int ID = 16029;

    public int id = 0;    
    public boolean active = false;    
    public boolean mandatory = false;    
    public boolean ordered = false;    
    public int mandatoryCount = 0;    
    public Vector<QuestElementModule> modifier;

    public QuestCaseModule(int param1, boolean param2, boolean param3, boolean param4, int param5, Vector<QuestElementModule> param6) {
        this.id = param1;
        this.active = param2;
        this.mandatory = param3;
        this.ordered = param4;
        this.mandatoryCount = param5;
        this.modifier = param6;        
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
            param1.writeBoolean(this.mandatory);
            param1.writeInt(this.id >>> 2 | this.id << 30);
            param1.writeBoolean(this.ordered);
            param1.writeShort(11608);
            param1.writeBoolean(this.active);
            param1.writeShort(25588);
            param1.writeInt(this.modifier.size());
            for (QuestElementModule i : this.modifier)
            {
               i.write(param1);
            }
            param1.writeInt(this.mandatoryCount >>> 2 | this.mandatoryCount << 30);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}