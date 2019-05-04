package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class JackpotArenaMatchResultCommand
		implements ServerCommand {

	public static int ID = 632;
	
    public MessageLocalizedCommand hint;    
    public boolean isWinner = false;   
    public JackpotArenaMatchResultModule loserData;    
    public JackpotArenaMatchResultModule winnerData;
    
    public JackpotArenaMatchResultCommand(boolean param1, JackpotArenaMatchResultModule param2, JackpotArenaMatchResultModule param3, MessageLocalizedCommand param4) {
    	this.isWinner = param1;
    	this.winnerData = param2;
    	this.loserData = param3;
    	this.hint = param4;
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
            this.hint.write(param1);
            param1.writeBoolean(this.isWinner);
            this.loserData.write(param1);
            this.winnerData.write(param1);
            param1.writeShort(18355);
        } catch (IOException e) {
        	
        }
    }
}