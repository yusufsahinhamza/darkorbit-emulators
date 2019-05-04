package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the ship select request

 @author Ordepsousa
 @date 22/07/2014
 @file ShipSelectRequest.java
 @package game.objects.netty.commands */
public class ShipSelectRequest
        extends ClientCommand {

    public static final int ID = 2806;
    public int targetID, name_95, name_13, posX, posY;

    /**
     Constructor
     */
    public ShipSelectRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.name_13 = in.readInt();
            this.name_13 = this.name_13 >>> 6 | this.name_13 << 26;
            in.readShort();
            this.targetID = in.readInt();
            this.targetID = this.targetID << 14 | this.targetID >>> 18;
            this.posY = in.readInt();
            this.posY = this.posY << 12 | this.posY >>> 20;
            this.posX = in.readInt();
            this.posX = this.posX << 2 | this.posX >>> 30;
            this.name_95 = in.readInt();
            this.name_95 = this.name_95 >>> 12 | this.name_95 << 20;
        } catch (IOException e) {
        }
    }
}