package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 21/09/2017.
*/

public class PetStatusCommand
        implements ServerCommand {

    public static int ID = 20686;

    public int petId; 
    public int petLevel; 
    public double petExperiencePoints; 
    public double petExperiencePointsUntilNextLevel; 
    public int petHitPoints;
    public int petHitPointsMax; 
    public int petShieldEnergyNow; 
    public int petShieldEnergyMax; 
    public int petCurrentFuel; 
    public int petMaxFuel;
    public int petSpeed; 
    public String petName;

    
    public PetStatusCommand(int param1, int param2, double param3, double param4, int param5,
    		int param6, int param7, int param8, int param9, int param10, int param11, String param12) {
        this.petId = param1;
        this.petLevel = param2;
        this.petExperiencePoints = param3;
        this.petExperiencePointsUntilNextLevel = param4;
        this.petHitPoints = param5;
        this.petHitPointsMax = param6;
        this.petShieldEnergyNow = param7;
        this.petShieldEnergyMax = param8;
        this.petCurrentFuel = param9;
        this.petMaxFuel = param10;
        this.petSpeed = param11;
        this.petName = param12;
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
            param1.writeInt(this.petShieldEnergyNow >>> 8 | this.petShieldEnergyNow << 24);
            param1.writeInt(this.petId << 11 | this.petId >>> 21);
            param1.writeInt(this.petCurrentFuel >>> 4 | this.petCurrentFuel << 28);
            param1.writeDouble(this.petExperiencePointsUntilNextLevel);
            param1.writeUTF(this.petName);
            param1.writeDouble(this.petExperiencePoints);
            param1.writeInt(this.petHitPointsMax >>> 9 | this.petHitPointsMax << 23);
            param1.writeInt(this.petMaxFuel << 16 | this.petMaxFuel >>> 16);
            param1.writeInt(this.petShieldEnergyMax >>> 8 | this.petShieldEnergyMax << 24);
            param1.writeInt(this.petLevel << 13 | this.petLevel >>> 19);
            param1.writeInt(this.petSpeed >>> 15 | this.petSpeed << 17);
            param1.writeInt(this.petHitPoints >>> 5 | this.petHitPoints << 27);
            param1.writeShort(32638);
        } catch (IOException e) {
        }
    }

}
