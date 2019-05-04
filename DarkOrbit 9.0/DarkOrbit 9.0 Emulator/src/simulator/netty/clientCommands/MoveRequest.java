package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the move request

 @author Ordepsousa
 @date 22/07/2014
 @file MoveRequest.java
 @package game.objects.netty.commands */
public class MoveRequest
        extends ClientCommand {

    public static final int ID = 18484;
    public int positionX, targetY, targetX, positionY;

    /**
     Constructor
     */
    public MoveRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.positionY = in.readInt();
            this.positionY = this.positionY << 5 | this.positionY >>> 27;
            this.targetX = in.readInt();
            this.targetX = this.targetX >>> 9 | this.targetX << 23;
            this.positionX = in.readInt();
            this.positionX = this.positionX >>> 4 | this.positionX << 28;
            this.targetY = in.readInt();
            this.targetY = this.targetY >>> 14 | this.targetY << 18;
        } catch (IOException e) {
        }
    }
}