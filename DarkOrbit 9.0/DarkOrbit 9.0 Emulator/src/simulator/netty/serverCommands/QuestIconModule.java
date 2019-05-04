package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

public class QuestIconModule
        implements ServerCommand {

    public static int ID = 2820;

    public static final short KILL = 0;   
    public static final short COLLECT = 1;    
    public static final short DISCOVER = 2;    
    public static final short PVP = 3;    
    public static final short TIME = 4;    
    public static final short SUMMERGAMES3 = 5;    
    public static final short WINTERGAMES09 = 6;    
    public static final short HALLOWEEN2012 = 7;
     
    public short icon = 0;

    public QuestIconModule(short param1) {
        this.icon = param1;     
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
            param1.writeShort(this.icon);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}