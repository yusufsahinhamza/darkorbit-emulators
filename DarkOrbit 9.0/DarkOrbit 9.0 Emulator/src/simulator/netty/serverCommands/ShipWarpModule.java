package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class ShipWarpModule
        implements ServerCommand {

	public static int ID = 7764;
    
    public int shipId = 0;
    public String shipTypeId = "";
    public String shipDesignName = "";
    public int uridiumPrice = 0;
    public int voucherPrice = 0;
    public int hangarId = 0;
    public String hangarName = "";

    public ShipWarpModule(int param1, String param2, String param3, int param4, int param5, int param6, String param7) {
        this.shipId = param1;
        this.shipTypeId = param2;
        this.shipDesignName = param3;
        this.uridiumPrice = param4;
        this.voucherPrice = param5;
        this.hangarId = param6;
        this.hangarName = param7;
    }


    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeUTF(this.hangarName);
            param1.writeInt(this.voucherPrice << 16 | this.voucherPrice >>> 16);
            param1.writeUTF(this.shipTypeId);
            param1.writeShort(6463);
            param1.writeInt(this.shipId << 5 | this.shipId >>> 27);
            param1.writeInt(this.uridiumPrice << 9 | this.uridiumPrice >>> 23);
            param1.writeUTF(this.shipDesignName);
            param1.writeInt(this.hangarId >>> 11 | this.hangarId << 21);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}