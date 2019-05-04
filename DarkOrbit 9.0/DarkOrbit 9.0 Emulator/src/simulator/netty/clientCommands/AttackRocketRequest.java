package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the attack rocket request

 @author Ordepsousa
 @date 22/07/2014
 @file AttackRocketRequest.java
 @package game.objects.netty.commands */
public class AttackRocketRequest
        extends ClientCommand {

    public static final int ID = 15316;
    public int mTargetPositionX;
    public int mTargetPositionY;
    public int mTargetId;

    /**
     Constructor
     */
    public AttackRocketRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.mTargetPositionX = in.readInt();
            this.mTargetPositionX = this.mTargetPositionX >>> 2 | this.mTargetPositionX << 30;
            this.mTargetPositionY = in.readInt();
            this.mTargetPositionY = this.mTargetPositionY >>> 13 | this.mTargetPositionY << 19;
            this.mTargetId = in.readInt();
            this.mTargetId = this.mTargetId << 7 | this.mTargetId >>> 25;
            in.readShort();
        } catch (IOException e) {
        }
    }
}