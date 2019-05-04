package simulator.map_entities.movable;

import net.utils.ServerUtils;

import org.json.JSONArray;

import java.util.Vector;

import simulator.ia.IArtificialInteligence;
import simulator.ia.SpaceballAI;
import simulator.map_entities.AttackableMapEntity;
import simulator.map_entities.Lockable;
import simulator.map_entities.MapEntity;
import simulator.netty.serverCommands.AttributeHitpointUpdateCommand;
import simulator.netty.serverCommands.AttributeShieldUpdateCommand;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.ShipCreateCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.netty.serverCommands.class_365;
import simulator.system.SpaceMap;
import simulator.system.ships.AlienShip;
import simulator.system.ships.ShipFactory;
import utils.Settings;

/**
 Created by LEJYONER on 09/09/2017
 */

public class Spaceball
        extends AttackableMapEntity {

    private static final int ALIEN_DAMAGE_MIN = 0;

    private static final float ALIEN_POWER_MULTIPLIER_MIN     = 0.1f;
    private static final float ALIEN_POWER_MULTIPLIER_DEFAULT = 1.0f;

    private final AlienShip mAlienShip;

    private Lockable mLockedTarget;

    private int mCurrentHitPoints;
    private int mCurrentShieldPoints;
    private int mCurrentSpeed;
    private int mCurrentDamage;
    private int SelectedFactionID = 0;
    private int mMMODamage = 0;
    private int mEICDamage = 0;
    private int mVRUDamage = 0;
    private float mCurrentPowerMultiplier = ALIEN_POWER_MULTIPLIER_DEFAULT;

    private IArtificialInteligence mArtificialInteligence;

    public Spaceball(final int pShipId, final SpaceMap pCurrentSpaceMapId) {
        super(pCurrentSpaceMapId);

        this.mAlienShip = ShipFactory.getAlienShip(pShipId);

        this.setDefaultAttributes();
        
        this.setCurrentPositionX(21000);
        this.setCurrentPositionY(13200);
        
        this.mArtificialInteligence = new SpaceballAI(this);

    }

    // ==============================================================

    public void setDefaultAttributes(final boolean pSetPowerMultiplier) {
        this.setCurrentHitPoints(this.getBaseHitPoints());
        this.setCurrentShieldPoints(this.getBaseShieldPoints());
        this.setCurrentDamage(this.getBaseDamage());
        this.setCurrentSpeed(this.getBaseSpeed());

        if (pSetPowerMultiplier) {
            this.setCurrentPowerMultiplier(this.getCurrentPowerMultiplier());
        }
    }

    public void setDefaultAttributes() {
        this.setDefaultAttributes(false);
    }

    // ==============================================================

    /**
     @return Alien map entity ID of this NPC
     */
    public int getSpaceballId() {
        return this.getMapEntityId();
    }

    /**
     @return AlienShip of this NPC
     */
    public AlienShip getAlienShip() {
        return this.mAlienShip;
    }

    /**
     @return AlienShip name
     */
    public String getAlienShipName() {
        return this.getAlienShip()
                   .getShipName();
    }

    // ==============================================================

    @Override
    public void receivedAttack(final MovableMapEntity pMovableMapEntity) {
    	
    }

    /**
     @return base HP of this NPC's ship
     */
    public int getBaseHitPoints() {
        return this.getAlienShip()
                   .getBaseHitPoints();
    }

    /**
     @return base SHD of this NPC's ship
     */
    public int getBaseShieldPoints() {
        return this.getAlienShip()
                   .getBaseShieldPoints();
    }

    /**
     @return base SPD of this NPC's ship
     */
    public int getBaseSpeed() {
        return this.getAlienShip()
                   .getBaseSpeed();
    }

    /**
     @return base DMG of this NPC's ship
     */
    public int getBaseDamage() {
        return this.getAlienShip()
                   .getBaseDamage();
    }

    /**
     @return true if this NPC will attack first, false if only responds attacks
     */
    public boolean isAggressive() {
        return this.getAlienShip()
                   .isAggressive();
    }

    // ==============================================================

    /**
     @return reward EXP for destroying this Alien
     */
    public int getRewardExperience() {
        return this.getAlienShip()
                   .getRewardExperience();
    }

    /**
     @return reward HON for destroying this Alien
     */
    public int getRewardHonor() {
        return this.getAlienShip()
                   .getRewardHonor();
    }

    /**
     @return reward credits for destroying this Alien
     */
    public int getRewardCredits() {
        return this.getAlienShip()
                   .getRewardCredits();
    }

    /**
     @return reward URI for destroying this Alien
     */
    public int getRewardUridium() {
    	if(Settings.REWARD_DOUBLER_ENABLED) {
        return this.getAlienShip()
                   .getRewardUridium() * 2;
    	} else {
        return this.getAlienShip()
                    .getRewardUridium();	
    	}
    }

    /**
     @return reward resources array for destroying this Alien
     */
    public JSONArray getRewardResources() {
        return this.getAlienShip()
                   .getRewardResources();
    }

    // ==============================================================

    /**
     @return current NPC HP
     */
    public int getCurrentHitPoints() {
        return this.mCurrentHitPoints;
    }

    /**
     @param pCurrentHitPoints
     NPC HP new value
     */
    public void setCurrentHitPoints(final int pCurrentHitPoints) {
        this.mCurrentHitPoints = pCurrentHitPoints;
    }

    /**
     @param pDifferenceHitPoints
     NPC HP difference to apply
     */
    public int changeCurrentHitPoints(final int pDifferenceHitPoints) {
        return this.mCurrentHitPoints += pDifferenceHitPoints;
    }

    @Override
    public int getCurrentNanoHull() {
        return 0;
    }

    @Override
    public int getMaximumNanoHull() {
        return 0;
    }

    @Override
    public void changeCurrentNanoHull(final int pDifferenceNanoHull) {

    }

    /**
     @return current NPC SHD
     */
    public int getCurrentShieldPoints() {
        return this.mCurrentShieldPoints;
    }

    @Override
    public int changeCurrentShieldPoints(final int pDifferenceShieldPoints) {
        this.mCurrentShieldPoints += pDifferenceShieldPoints;
        return pDifferenceShieldPoints;
    }

    @Override
    public int getCurrentShieldAbsorb() {
        return this.getAlienShip()
                   .getBaseShieldAbsorption();
    }

    /**
     @param pCurrentShieldPoints
     NPC SHD new value
     */
    public void setCurrentShieldPoints(final int pCurrentShieldPoints) {
        this.mCurrentShieldPoints = pCurrentShieldPoints;
    }

    /**
     @return current NPC SPD
     */
    public int getCurrentSpeed() {
        return this.mCurrentSpeed;
    }

    /**
     @param pCurrentSpeed
     NPC SPD new value
     */
    public void setCurrentSpeed(final int pCurrentSpeed) {
        this.mCurrentSpeed = pCurrentSpeed;
    }

    /**
     @return current NPC DMG
     */
    public int getCurrentDamage() {
        return this.mCurrentDamage;
    }

    @Override
    public int getMaximumHitPoints() {
        return this.getAlienShip()
                   .getBaseHitPoints();
    }

    @Override
    public int getMaximumShieldPoints() {
        return this.getAlienShip()
                   .getBaseShieldPoints();
    }

    @Override
    public void destroy(final MapEntity pKillerMapEntity) {

    }

    /**
     Note: sets at least {@link Spaceball#ALIEN_DAMAGE_MIN}

     @param pCurrentDamage
     NPC DMG new value
     */
    public void setCurrentDamage(final int pCurrentDamage) {
        this.mCurrentDamage = Math.max(ALIEN_DAMAGE_MIN, pCurrentDamage);
    }

    /**
     @return current damage multiplier
     */
    public float getCurrentPowerMultiplier() {
        return this.mCurrentPowerMultiplier;
    }

    /**
     Used to create 0.5x, 2x, 3x powerful aliens

     @param pDamageMultiplier
     damage multiplier to set
     */
    public void setCurrentPowerMultiplier(final float pDamageMultiplier) {

        this.mCurrentPowerMultiplier = Math.max(ALIEN_POWER_MULTIPLIER_MIN, pDamageMultiplier);

        this.setCurrentHitPoints((int) (this.getBaseHitPoints() * this.getCurrentPowerMultiplier()));
        this.setCurrentShieldPoints((int) (this.getBaseShieldPoints() * this.getCurrentPowerMultiplier()));
        this.setCurrentDamage((int) (this.getBaseDamage() * this.getCurrentPowerMultiplier()));

    }

    // ==============================================================

    public ShipCreateCommand getShipCreateCommand(short relationType, boolean sameClan) {
        return new ShipCreateCommand(this.getMapEntityId(), String.valueOf(this.getAlienShip()
                                                                               .getShipId()), 3, "", this.getAlienShip()
                                                                                                         .getShipName(),
                                     this.getCurrentPositionX(), this.getCurrentPositionY(), 0, 0, 0,//
                                     false,//?
                                     new ClanRelationModule(ClanRelationModule.NONE),//?
                                     5,//?
                                     false,//?
                                     true,// not an NPC
                                     false,//
                                     7,//?
                                     9,//position index???
                                     new Vector<VisualModifierCommand>(),// visual modifiers
                                     new class_365(class_365.DEFAULT)//?
        );
    }

    @Override
    public AttributeHitpointUpdateCommand getHitpointsUpdateCommand() {
        return null;
    }

    @Override
    public AttributeShieldUpdateCommand getShieldUpdateCommand() {
        return null;
    }

    @Override
    public void doTick() {
        this.mArtificialInteligence.checkArtificialInteligence();
        this.getMovement()
            .move();
        this.checkSpaceballSpeed();
    }

    public void checkSpaceballSpeed() {
        if(this.getSelectedFactionID() == 1) {
        	if(this.getMMODamage() < 500000) {
        	    ServerUtils.sendPacketToAllUsers("0|n|sss|1|1");
        	} else if(this.getMMODamage() < 1000000 && this.getMMODamage() > 500000) {
        		ServerUtils.sendPacketToAllUsers("0|n|sss|1|2");
        		this.getMovement().changeMovementSpeed(250);
        	} else if(this.getMMODamage() < 10000000 && this.getMMODamage() > 1000000) {
        		ServerUtils.sendPacketToAllUsers("0|n|sss|1|3");
        		this.getMovement().changeMovementSpeed(300);
        	}
        } else if(this.getSelectedFactionID() == 2) {
        	if(this.getEICDamage() < 500000) {
        	    ServerUtils.sendPacketToAllUsers("0|n|sss|2|1");
        	} else if(this.getEICDamage() < 1000000 && this.getEICDamage() > 500000) {
        		ServerUtils.sendPacketToAllUsers("0|n|sss|2|2");
        		this.getMovement().changeMovementSpeed(250);
        	} else if(this.getEICDamage() < 10000000 && this.getEICDamage() > 1000000) {
        		ServerUtils.sendPacketToAllUsers("0|n|sss|2|3");
        		this.getMovement().changeMovementSpeed(300);
        	}
        } else if(this.getSelectedFactionID() == 3) {
        	if(this.getVRUDamage() < 500000) {
        	    ServerUtils.sendPacketToAllUsers("0|n|sss|3|1");
        	} else if(this.getVRUDamage() < 1000000 && this.getVRUDamage() > 500000) {
        		ServerUtils.sendPacketToAllUsers("0|n|sss|3|2");
        		this.getMovement().changeMovementSpeed(250);
        	} else if(this.getVRUDamage() < 10000000 && this.getVRUDamage() > 1000000) {
        		ServerUtils.sendPacketToAllUsers("0|n|sss|3|3");
        		this.getMovement().changeMovementSpeed(300);
        	}
        } else {
        	ServerUtils.sendPacketToAllUsers("0|n|sss|0|0");
        }
    }
    
    public int getMMODamage() {
    	return this.mMMODamage;
    }
    
    public void setMMODamage(final int pMMODamage) {
    	this.mMMODamage = pMMODamage;
    }
    
    public int getEICDamage() {
    	return this.mEICDamage;
    }
    
    public void setEICDamage(final int pEICDamage) {
    	this.mEICDamage = pEICDamage;
    }
    
    public int getVRUDamage() {
    	return this.mVRUDamage;
    }
    
    public void setVRUDamage(final int pVRUDamage) {
    	this.mVRUDamage = pVRUDamage;
    }
    
    public int getSelectedFactionID() {
    	return this.SelectedFactionID;
    }
    
    public void setSelectedFactionID(final int pFactionID) {
    	this.SelectedFactionID = pFactionID;
    }
    
    @Override
    public void setLockedTarget(final Lockable pLockedTarget) {
        this.mLockedTarget = pLockedTarget;
    }

    @Override
    public Lockable getLockedTarget() {
        return this.mLockedTarget;
    }

    @Override
    public boolean canBeTargeted() {
        return true;
    }

    @Override
    public boolean isInSecureZone() {
        return false;
    }

    public ShipRemoveCommand removeInRangeMapEntity(final MovableMapEntity pMovableMapEntity) {
        this.mInRangeMovableMapIntities.remove(pMovableMapEntity);
        return new ShipRemoveCommand(pMovableMapEntity.getMapEntityId());
    }

	@Override
	public boolean canBeShoot() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean isDestroyed() {
		// TODO Auto-generated method stub
		return false;
	}
}
