package simulator.users;

import java.util.Arrays;
import java.util.List;

import mysql.QueryManager;
import simulator.map_entities.movable.Player;

/**
 Created by LEJYONER on 07/01/2018.
 */

public class BuyManager
        extends AbstractAccountInternalManager {

    public static final String ECO_10_BUY     = "equipment_extra_cpu_sle-01";
    public static final String SAR_02_BUY     = "equipment_extra_cpu_sle-02";
    public static final String HSTRM_01_BUY   = "equipment_extra_cpu_sle-03";
    public static final String UBR_100_BUY    = "equipment_extra_cpu_sle-04";
    
    public static final int ECO_10_PRICE   = 0;
    public static final int SAR_02_PRICE   = 0;
    
    public static final List<String> buyCategory = Arrays.asList(ECO_10_BUY, SAR_02_BUY);
    
    public BuyManager(final Account pAccount) {
        super(pAccount);
    }
    
    public void buyECO_10() {
    	final Account account = this.getAccount();    			
    	final Player player = account.getPlayer();    	
    	if(account.getUridium() >= ECO_10_PRICE) {
	    	this.getAccount().changeECO_10(100);
	    	player.sendPacketToBoundSessions("0|A|STD|100 adet ECO-10 alındı");
	    	player.sendCommandToBoundSessions(account.getAmmunitionManager().getRocketLauncherItemStatus(AmmunitionManager.ECO_10));
	    	QueryManager.saveAmmo(this.getAccount());
    	} else {
    		player.sendPacketToBoundSessions("0|A|STD|ECO-10 almak için yeterli uridiumun yok");
    	}
    }
    
    public void buySAR_02() {
    	final Account account = this.getAccount();    			
    	final Player player = account.getPlayer();    	
    	if(account.getUridium() >= SAR_02_PRICE) {
	    	this.getAccount().changeSAR_02(100);
	    	player.sendPacketToBoundSessions("0|A|STD|100 SAR-02 alındı");
	    	player.sendCommandToBoundSessions(account.getAmmunitionManager().getRocketLauncherItemStatus(AmmunitionManager.SAR_02));
	    	QueryManager.saveAmmo(this.getAccount());
    	} else {
    		player.sendPacketToBoundSessions("0|A|STD|SAR-02 almak için yeterli uridiumun yok");
    	}
    }
   
	public void setFromJSON(String pJSON) {
		
	}

	public void setNewAccount() {
		
	}

	public String packToJSON() {
		return null;
	}
}
