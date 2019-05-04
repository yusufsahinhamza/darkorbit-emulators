package simulator.netty.clientCommands;

import java.io.DataInputStream;

import simulator.netty.ClientCommand;

/**
 Description: This is the attack abort laser request

 @author Ordepsousa
 @date 22/07/2014
 @file AttackAbortLaserRequest.java
 @package game.objects.netty.commands */
public class AttackAbortLaserRequest
        extends ClientCommand {

    public static final int ID = 30889;

    /**
     Constructor
     */
    public AttackAbortLaserRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
    }
}