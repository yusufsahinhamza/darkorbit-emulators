package simulator.system.clans;

/**
 Created by LEJYONER on 22/06/2017.
 */
public class Diplomacy {
 public int clanID1;
 public int clanID2;
 public int relationType;
 public String endDate;

 public Diplomacy(int clan1, int clan2, int relation, String date){
  this.clanID1 = clan1;
  this.clanID2 = clan2;
  this.relationType = relation;
  this.endDate = date;
 }
}
