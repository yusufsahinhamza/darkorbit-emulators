package simulator.users;

import utils.Tools;

/**
 Created by LEJYONER on 27/06/2017.
 */
public class EquipmentManager
        extends AbstractAccountInternalManager {

    private int damageConfig1          = 0; //1. konfigırasyon hasarı
    private int damageConfig2          = 0; //2. konfigürasyon hasarı
    private int constantDamageConfig1  = 0; //1. konfigürasyon hasarı (sabit)
    private int constantDamageConfig2  = 0; //2. konfigürasyon hasarı (sabit)
    private int speedConfig1           = 0; //1. konfigürasyon hızı
    private int speedConfig2           = 0; //2. konfigürasyon hızı
    private int shieldConfig1          = 0; //1. konfigürasyon kalkanı
    private int shieldConfig2          = 0; //2. konfigürasyon kalkanı
    private int hpConfig1              = 0; //1. konfigırasyon canı
    private int hpConfig2              = 0; //1. konfigırasyon canı

    public EquipmentManager(final Account pAccount) {
        super(pAccount);
    }


    public void setConfig1(final int pMaxDamage, final int pMaxShield, final int pShipSpeed) {
    	this.damageConfig1          = pMaxDamage;
    	this.constantDamageConfig1  = pMaxDamage;
    	this.shieldConfig1          = pMaxShield;
    	this.speedConfig1           = pShipSpeed - 300;
    }
    
    public void setConfig2(final int pMaxDamage, final int pMaxShield, final int pShipSpeed) {
    	this.damageConfig2          = pMaxDamage;
    	this.constantDamageConfig2  = pMaxDamage;
    	this.shieldConfig2          = pMaxShield;
    	this.speedConfig2           = pShipSpeed - 300;
    }
 
    public void setDamageConfig1(final int pDamageConfig1) {
        damageConfig1 = pDamageConfig1;
    }

    public int getDamageConfig1() {
        return damageConfig1;
    }

    public int getConstantDamageConfig1() {
        return constantDamageConfig1;
    }
    
    public void setDamageConfig2(final int pDamageConfig2) {
        damageConfig2 = pDamageConfig2;
    }

    public int getDamageConfig2() {
        return damageConfig2;
    }

    public int getConstantDamageConfig2() {
        return constantDamageConfig2;
    }

    public void setSpeedConfig1(final int pSpeedConfig1) {
        speedConfig1 = pSpeedConfig1;
        if (this.mAccount.getPlayer()
                         .getCurrentConfiguration() == 1) {
            this.mAccount.getPlayer()
                         .sendCommandToBoundSessions(this.mAccount.getPlayer()
                                                                  .getSetSpeedCommand());
        }
    }

    public void setSpeedConfig2(final int pSpeedConfig2) {
        speedConfig2 = pSpeedConfig2;
        if (this.mAccount.getPlayer()
                         .getCurrentConfiguration() == 1) {
            this.mAccount.getPlayer()
                         .sendCommandToBoundSessions(this.mAccount.getPlayer()
                                                                  .getSetSpeedCommand());
        }
    }

    public int getSpeed(final short pCurrentConfigurationId) {
        if (pCurrentConfigurationId == 1) {
            return this.speedConfig1;
        }
        return this.speedConfig2;
    }

    public int getShieldPoints(final short pCurrentConfigurationId) {
        if (pCurrentConfigurationId == 1) {
            return this.shieldConfig1;
        }
        return this.shieldConfig2;
    }

    public int getHitpointsBoost(final short pCurrentConfigurationId, final int pHitpoints) {
        if (pCurrentConfigurationId == 1) {
            return (int) Tools.getBoost(pHitpoints, this.hpConfig1);
        }
        return (int) Tools.getBoost(pHitpoints, this.hpConfig2);
    }

	public void setFromJSON(String pJSON) {
		
	}

	public void setNewAccount() {
		
	}

	public String packToJSON() {
		return null;
	}

}
