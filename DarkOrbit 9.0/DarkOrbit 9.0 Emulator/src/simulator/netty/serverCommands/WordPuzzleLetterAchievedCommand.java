package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 07/01/2018.
 */

public class WordPuzzleLetterAchievedCommand
        implements ServerCommand {

    public static int ID = 21819;

    public Vector<WordPuzzleLetterModule> letterValues;   
    public boolean complete = false;

    public WordPuzzleLetterAchievedCommand(Vector<WordPuzzleLetterModule> param1, boolean param2) {
        this.letterValues = param1;
        this.complete = param2;
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
            param1.writeInt(this.letterValues.size());
            for (WordPuzzleLetterModule c : this.letterValues) {
                c.write(param1);
            }
            param1.writeBoolean(this.complete);
        } catch (IOException e) {
        }
    }
}
