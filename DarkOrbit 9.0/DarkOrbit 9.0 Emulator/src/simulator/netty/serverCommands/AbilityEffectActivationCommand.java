package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 13/01/2018.
 */

public class AbilityEffectActivationCommand
        implements ServerCommand {

    public static int ID = 12237;

    public Vector<Integer> targetIds;   
    public int selectedAbilityId = 0;    
    public int activatorId = 0;

    public AbilityEffectActivationCommand(int param1, int param2, Vector<Integer> param3) {
        this.selectedAbilityId = param1;
        this.activatorId = param2;
        this.targetIds = param3;
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
            param1.writeInt(this.activatorId << 7 | this.activatorId >>> 25);
            param1.writeInt(this.selectedAbilityId >>> 2 | this.selectedAbilityId << 30);
            param1.writeInt(this.targetIds.size());
            for (Integer i : this.targetIds)
            {
               param1.writeInt(i >>> 16 | i << 16);
            }
            param1.writeShort(28313);
        } catch (IOException e) {
        }
    }
}
