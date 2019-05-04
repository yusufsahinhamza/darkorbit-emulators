package simulator.netty;

import java.io.DataOutputStream;

/**
 Description: Global class for commands

 @author Ordepsousa
 @date 24/07/2014
 @file ServerCommand.java
 @package game.objects.netty; */
public interface ServerCommand {

    void write(DataOutputStream dataOut);
}
