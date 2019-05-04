package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the attack laser request

 @author Ordepsousa
 @date 22/07/2014
 @file AttackLaserRequest.java
 @package game.objects.netty.commands */
public class AttackLaserRequest
        extends ClientCommand {

    public static final int ID = 19032;
    public int var_2879, name_13, name_95;

    /**
     Constructor
     */
    public AttackLaserRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.name_95 = in.readInt();
            this.name_95 = this.name_95 >>> 13 | this.name_95 << 19;
            this.name_13 = in.readInt();
            this.name_13 = this.name_13 >>> 1 | this.name_13 << 31;
            this.var_2879 = in.readInt();
            this.var_2879 = this.var_2879 >>> 2 | this.var_2879 << 30;
        } catch (IOException e) {
        }
    }
}