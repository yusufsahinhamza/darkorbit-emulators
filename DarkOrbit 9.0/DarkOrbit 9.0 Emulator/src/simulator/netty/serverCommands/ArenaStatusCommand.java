package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class ArenaStatusCommand
        implements ServerCommand {

	public static int ID = 31290;
	
    public static short JACKPOT = 0;    
    public static short SCHEDULED = 0;    
    public static short WAITING_FOR_PLAYERS = 1;    
    public static short COUNTDOWN = 2;   
    public static short FIGHTING = 3;    
    public static short RADIATION_ACTIVE = 4;    
    public static short DONE = 5;    
    public static short DESTROYABLE = 6;    
    public static short NONE = 7;   
         
    public short arenaType = 0;    
    public short status = 0;    
    public int currentRound = 0;    
    public int survivors = 0;    
    public int participants = 0;    
    public String opponentName = "";    
    public int opponentId = 0;    
    public int opponentInstanceId = 0;    
    public int secondsLeftInPhase = 0;   
    public int warpWarningOffsetSeconds = 0;
    
    public ArenaStatusCommand(short param1, short param2, int param3, int param4, int param5, String param6, int param7, int param8, int param9, int param10) {
        this.arenaType = param1;
        this.status = param2;
        this.currentRound = param3;
        this.survivors = param4;
        this.participants = param5;
        this.opponentName = param6;
        this.opponentId = param7;
        this.opponentInstanceId = param8;
        this.secondsLeftInPhase = param9;
        this.warpWarningOffsetSeconds = param10;
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
            param1.writeInt(this.warpWarningOffsetSeconds >>> 7 | this.warpWarningOffsetSeconds << 25);
            param1.writeShort(this.status);
            param1.writeInt(this.currentRound >>> 8 | this.currentRound << 24);
            param1.writeShort(24150);
            param1.writeInt(this.opponentId << 16 | this.opponentId >>> 16);
            param1.writeUTF(this.opponentName);
            param1.writeInt(this.participants >>> 12 | this.participants << 20);
            param1.writeInt(this.survivors << 6 | this.survivors >>> 26);
            param1.writeInt(this.opponentInstanceId >>> 10 | this.opponentInstanceId << 22);
            param1.writeInt(this.secondsLeftInPhase << 15 | this.secondsLeftInPhase >>> 17);
            param1.writeShort(this.arenaType);
        } catch (IOException e) {
        	
        }
    }
}