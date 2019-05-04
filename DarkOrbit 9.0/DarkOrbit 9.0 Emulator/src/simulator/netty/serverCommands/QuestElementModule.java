package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class QuestElementModule
        implements ServerCommand {

    public static int ID = 10024;

    public QuestCaseModule questCase;    
    public QuestConditionModule condition;
    
    public QuestElementModule(QuestCaseModule param1, QuestConditionModule param2) {
    	this.questCase = param1;
    	this.condition = param2;
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
            this.questCase.write(param1);
            param1.writeShort(12265);
            this.condition.write(param1);
            param1.writeShort(-3758);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}