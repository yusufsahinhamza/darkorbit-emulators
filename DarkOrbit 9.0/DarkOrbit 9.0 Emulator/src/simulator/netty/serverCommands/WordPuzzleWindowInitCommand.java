package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 12-03-2015.
 */
public class WordPuzzleWindowInitCommand
        implements ServerCommand {

    public static int ID = 2734;

    public int letterCount = 0;

    public WordPuzzleWindowInitCommand(int param1) {
        this.letterCount = param1;
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
            param1.writeShort(15268);
            param1.writeInt(this.letterCount >>> 8 | this.letterCount << 24);
        } catch (IOException e) {
        }
    }
}
