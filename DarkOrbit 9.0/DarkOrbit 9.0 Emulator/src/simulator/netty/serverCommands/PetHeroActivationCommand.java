package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 21/09/2017.
*/

public class PetHeroActivationCommand
        implements ServerCommand {

    public static int ID = 10333;
    public int ownerId, petId, expansionStage, petLevel, x, y, petSpeed, clanId;
    public String petName, clanTag;  
    public class_365 varU1q;
    public short shipType, factionId;
    
    public PetHeroActivationCommand(int param1, int param2, short param3, int param4, String param5, short param6, int param7, 
    		int param8, String param9, int param10, int param11, int param12, class_365 param13) {
        this.ownerId = param1;
        this.petId = param2;
        this.shipType = param3;
        this.expansionStage = param4;
        this.petName = param5;
        this.factionId = param6;
        this.clanId = param7;
        this.petLevel = param8;
        this.clanTag = param9;
        this.x = param10;
        this.y = param11;
        this.petSpeed = param12;
        this.varU1q = param13;
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
            param1.writeShort(12244);
            param1.writeShort(this.shipType);
            param1.writeInt(this.ownerId >>> 11 | this.ownerId << 21);
            this.varU1q.write(param1);
            param1.writeInt(this.petId << 12 | this.petId >>> 20);
            param1.writeUTF(this.petName);
            param1.writeUTF(this.clanTag);
            param1.writeInt(this.y >>> 15 | this.y << 17);
            param1.writeShort(this.expansionStage);
            param1.writeInt(this.clanId << 5 | this.clanId >>> 27);
            param1.writeInt(this.petSpeed >>> 1 | this.petSpeed << 31);
            param1.writeShort(this.factionId);
            param1.writeInt(this.x << 3 | this.x >>> 29);
            param1.writeShort(this.petLevel);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}