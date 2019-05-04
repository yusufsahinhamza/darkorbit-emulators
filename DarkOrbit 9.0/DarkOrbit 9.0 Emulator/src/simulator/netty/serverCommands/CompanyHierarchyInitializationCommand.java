package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 11/01/2018.
 */

public class CompanyHierarchyInitializationCommand
        implements ServerCommand {

    public static int ID = 21942;

    public Vector<CompanyHierarchyRankingModule> vruRanking;    
    public Vector<CompanyHierarchyRankingModule> eicRanking;
    public FactionModule ownFaction;    
    public Vector<CompanyHierarchyRankingModule> mmoRanking;   
    public CompanyHierarchyRankingModule ownRanking;

    public CompanyHierarchyInitializationCommand(Vector<CompanyHierarchyRankingModule> param1, Vector<CompanyHierarchyRankingModule> param2, Vector<CompanyHierarchyRankingModule> param3, CompanyHierarchyRankingModule param4, FactionModule param5) {
    	this.mmoRanking = param1;
    	this.eicRanking = param2;
    	this.vruRanking = param3;
    	this.ownRanking = param4;
    	this.ownFaction = param5;
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
            param1.writeInt(this.vruRanking.size());
            for (CompanyHierarchyRankingModule vru : this.vruRanking) {
            	vru.write(param1);
            }
            param1.writeInt(this.eicRanking.size());
            for (CompanyHierarchyRankingModule eic : this.eicRanking) {
            	eic.write(param1);
            }
            param1.writeShort(22819);
            this.ownFaction.write(param1);
            param1.writeInt(this.mmoRanking.size());
            for (CompanyHierarchyRankingModule mmo : this.mmoRanking) {
            	mmo.write(param1);
            }
            this.ownRanking.write(param1);
        } catch (IOException e) {
        }
    }
}
