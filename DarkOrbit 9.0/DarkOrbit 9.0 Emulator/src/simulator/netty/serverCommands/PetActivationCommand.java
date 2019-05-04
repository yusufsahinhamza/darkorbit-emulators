package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 21/09/2017.
*/

public class PetActivationCommand
        implements ServerCommand {

    public static int ID = 15414;
    public int ownerId, petId, petDesignId, expansionStage, petFactionId, petClanID, petLevel, x, y, petSpeed;
    public String petName, clanTag;
    public boolean isInIdleMode, isVisible;   
    public ClanRelationModule clanRelationship;
    public class_365 varU1q;
    
    public PetActivationCommand(int param1, int param2, int param3, int param4, String param5, int param6, int param7, 
    		int param8, String param9, ClanRelationModule param10, int param11, int param12, int param13, 
    		boolean param14, boolean param15, class_365 param16) {
    	this.ownerId = param1;
        this.petId = param2;
        this.petDesignId = param3;
        this.expansionStage = param4;
        this.petName = param5;
        this.petFactionId = param6;
        this.petClanID = param7;
        this.petLevel = param8;
        this.clanTag = param9;
        this.clanRelationship = param10;
        this.x = param11;
        this.y = param12;
        this.petSpeed = param13;
        this.isInIdleMode = param14;
        this.isVisible = param15;
        this.varU1q = param16;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.ownerId << 6 | this.ownerId >>> 26);
            param1.writeInt(this.petSpeed >>> 14 | this.petSpeed << 18);
            param1.writeShort(this.petDesignId);
            param1.writeShort(2978);
            param1.writeBoolean(this.isVisible);
            param1.writeBoolean(this.isInIdleMode);
            this.clanRelationship.write(param1);
            param1.writeInt(this.petClanID >>> 14 | this.petClanID << 18);
            this.varU1q.write(param1);
            param1.writeUTF(this.petName);
            param1.writeInt(this.y >>> 1 | this.y << 31);
            param1.writeShort(this.petLevel);
            param1.writeInt(this.petId << 10 | this.petId >>> 22);
            param1.writeShort(this.petFactionId);
            param1.writeShort(this.expansionStage);
            param1.writeInt(this.x >>> 5 | this.x << 27);
            param1.writeUTF(this.clanTag);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}