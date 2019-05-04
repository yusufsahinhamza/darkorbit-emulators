package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 13/01/2018.
 */

public class QuestConditionStateModule
        implements ServerCommand {

    public static int ID = 8820;

    public double currentValue = 0;   
    public boolean completed = false;    
    public boolean active = false;
    
    public QuestConditionStateModule(double param1, boolean param2, boolean param3) {
        this.currentValue = param1;
        this.active = param2;
        this.completed = param3;
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
            param1.writeDouble(this.currentValue);
            param1.writeBoolean(this.completed);
            param1.writeBoolean(this.active);
            param1.writeShort(-5899);
        } catch (IOException e) {
        }
    }
}
