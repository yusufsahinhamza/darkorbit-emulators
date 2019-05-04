package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 11/01/2018.
 */

public class CompanyHierarchyRankingModule
        implements ServerCommand {

    public static int ID = 22766;

    public int rank = 0;    
    public String clanName = "";    
    public int clanId = 0;
    public String cbsNamesAndLocations = "";    
    public int rankingPoints = 0;    
    public String leaderName = "";

    public CompanyHierarchyRankingModule(int param1, int param2, String param3, String param4, String param5, int param6) {
        this.clanId = param1;
        this.rank = param2;
        this.clanName = param3;
        this.leaderName = param4;
        this.cbsNamesAndLocations = param5;
        this.rankingPoints = param6;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.rank >>> 13 | this.rank << 19);
            param1.writeUTF(this.clanName);
            param1.writeInt(this.clanId >>> 13 | this.clanId << 19);
            param1.writeUTF(this.cbsNamesAndLocations);
            param1.writeInt(this.rankingPoints >>> 1 | this.rankingPoints << 31);
            param1.writeUTF(this.leaderName);
        } catch (IOException e) {
        }
    }
}
