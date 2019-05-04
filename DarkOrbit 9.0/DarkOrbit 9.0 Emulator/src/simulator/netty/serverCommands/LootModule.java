package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

public class LootModule
        implements ServerCommand {

    public static int ID = 31673;

    public String lootId = "";    
    public int amount = 0;

    public LootModule(String param1, int param2) {
        this.lootId = param1;
        this.amount = param2;       
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
            param1.writeUTF(this.lootId);
            param1.writeInt(this.amount >>> 3 | this.amount << 29);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}