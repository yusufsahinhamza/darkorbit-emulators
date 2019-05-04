package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

public class QuestTypeModule
        implements ServerCommand {

    public static int ID = 11392;

    public static final short UNDEFINED = 0;    
    public static final short STARTER = 1;    
    public static final short MISSION = 2;    
    public static final short DAILY = 3;    
    public static final short CHALLENGE = 4;    
    public static final short EVENT = 5;

    public short type = 0;

    public QuestTypeModule(short param1) {
        this.type = param1;     
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
            param1.writeShort(this.type);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}