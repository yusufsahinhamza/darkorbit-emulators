package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class ShipWarpWindowCommand
        implements ServerCommand {

    public static int ID = 17427;
        
    public boolean isNearSpacestation = false;
    
    public int jumpVoucherCount = 0;
    
    public int uridium = 0;
    
    public Vector<ShipWarpModule> ships;

    public ShipWarpWindowCommand(int param1, int param2, boolean param3, Vector<ShipWarpModule> param4) {
        this.jumpVoucherCount = param1;
        this.uridium = param2;
        this.isNearSpacestation = param3;
        this.ships = param4;
    }
    
    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeBoolean(this.isNearSpacestation);
            param1.writeInt(this.jumpVoucherCount << 14 | this.jumpVoucherCount >>> 18);
            param1.writeInt(this.uridium >>> 8 | this.uridium << 24);
            param1.writeShort(-32403);
            param1.writeInt(this.ships.size());
            for (ShipWarpModule c : this.ships) {
                c.write(param1);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}