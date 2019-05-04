package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 24-03-2015.
 */
public class HealCommand
        implements ServerCommand {

    public static short HITPOINTS;
    public static short SHIELD;

    public static int ID = 27966;

    public short healType;
    public int   currentHitpoints;
    public int   healAmount;
    public int   healerId;
    public int   healedId;

    public HealCommand(short param1, int param2, int param3, int param4, int param5) {
        this.healType = param1;
        this.healerId = param2;
        this.healedId = param3;
        this.currentHitpoints = param4;
        this.healAmount = param5;
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
            param1.writeShort(this.healType);
            param1.writeShort(-20306);
            param1.writeInt(this.currentHitpoints >>> 4 | this.currentHitpoints << 28);
            param1.writeInt(this.healAmount << 2 | this.healAmount >>> 30);
            param1.writeInt(this.healerId >>> 14 | this.healerId << 18);
            param1.writeShort(-10690);
            param1.writeInt(this.healedId >>> 12 | this.healedId << 20);
        } catch (IOException e) {
        }
    }
}
