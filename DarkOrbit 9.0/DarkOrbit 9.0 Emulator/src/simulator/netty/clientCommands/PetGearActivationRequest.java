package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;
import simulator.netty.ClientCommand;

/**
 Created by LEJYONER on 21/09/2017.
*/

public class PetGearActivationRequest
        extends ClientCommand {

    public static final int ID = 5170;
    
    public short bilmiyorum1 = 0;
    public short bilmiyorum2 = 0;
    public short gearID = 0;
    
    public PetGearActivationRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
    	try {
    		bilmiyorum2 = in.readShort();
        	bilmiyorum1 = in.readShort();
        	gearID = in.readShort();          
    	} catch (IOException e) {    		
    	}
    }
}