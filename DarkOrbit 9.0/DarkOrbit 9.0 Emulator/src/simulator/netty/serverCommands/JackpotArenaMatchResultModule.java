package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class JackpotArenaMatchResultModule
	extends ArenaMatchResultModule implements ServerCommand {

	public static int ID = 1219;
	
    public int jackpotWins = 0;    
    public int jackpotLosses = 0;    
    public float jackpotWinRate = 0;
    
    public JackpotArenaMatchResultModule(int param1, int param2, String param3, float param4, int param5, int param6, int param7, int param8) {
    	super(param3,param2,param1,param5,param6);
        this.jackpotWins = param8;
        this.jackpotLosses = param7;
        this.jackpotWinRate = param4;
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
        	super.writeInternal(param1);
            param1.writeFloat(this.jackpotWinRate);
            param1.writeShort(-7597);
            param1.writeInt(this.jackpotLosses >>> 2 | this.jackpotLosses << 30);
            param1.writeInt(this.jackpotWins >>> 11 | this.jackpotWins << 21);
            param1.writeShort(26660);
        } catch (IOException e) {
        	
        }
    }
}