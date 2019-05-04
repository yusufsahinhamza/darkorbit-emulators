package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 07/01/2018.
 */

public class WordPuzzleLetterModule
        implements ServerCommand {

    public static int ID = 29776;

    public String letterValue = "";    
    public int letterIndex = 0;

    public WordPuzzleLetterModule(String param1, int param2) {
        this.letterValue = param1;
        this.letterIndex = param2;
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
            param1.writeInt(this.letterIndex >>> 15 | this.letterIndex << 17);
            param1.writeUTF(this.letterValue);
        } catch (IOException e) {
        }
    }
}
