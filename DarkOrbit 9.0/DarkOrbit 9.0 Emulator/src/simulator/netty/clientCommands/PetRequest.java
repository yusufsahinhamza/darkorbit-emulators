package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Created by LEJYONER on 21/09/2017.
*/

public class PetRequest
        extends ClientCommand {

    public static final int ID = 9683;

    public static final short LAUNCH = 0;    
    public static final short DEACTIVATE = 1;    
    public static final short TOGGLE_ACTIVATION = 2;    
    public static final short HOTKEY_GUARD_MODE = 3;   
    public static final short REPAIR_DESTROYED_PET = 4;    
    public static final short HOTKEY_REPAIR_SHIP = 5;
     
    public short petRequestType = 0;
    
    public PetRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
    	try {
	        this.petRequestType = in.readShort();
	        in.readShort();
	        in.readShort();
    	} catch (IOException e) {    		
    	}
    }
}