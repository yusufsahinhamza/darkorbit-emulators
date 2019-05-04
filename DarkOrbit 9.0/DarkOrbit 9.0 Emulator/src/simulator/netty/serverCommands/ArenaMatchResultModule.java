package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class ArenaMatchResultModule
        implements ServerCommand {

	public static int ID = 11002;
	
    public String playerName = "";    
    public int durationInSeconds = 0;    
    public int damageDealt = 0;    
    public int damageRecieved = 0;    
    public int peakDamage = 0;
    
    public ArenaMatchResultModule(String param1, int param2, int param3, int param4, int param5) {
        this.playerName = param1;
        this.durationInSeconds = param2;
        this.damageDealt = param3;
        this.damageRecieved = param4;
        this.peakDamage = param5;
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
            param1.writeInt(this.durationInSeconds << 11 | this.durationInSeconds >>> 21);
            param1.writeShort(-31603);
            param1.writeUTF(this.playerName);
            param1.writeInt(this.damageDealt >>> 1 | this.damageDealt << 31);
            param1.writeInt(this.damageRecieved << 1 | this.damageRecieved >>> 31);
            param1.writeInt(this.peakDamage << 16 | this.peakDamage >>> 16);
        } catch (IOException e) {
        	
        }
    }
}