package simulator.ia;

import mysql.QueryManager;
import net.game_server.GameSession;
import net.utils.ServerUtils;
import simulator.GameManager;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.map_entities.movable.Spaceball;
import simulator.netty.serverCommands.ShipDeselectionCommand;
import simulator.users.Account;
import simulator.users.AmmunitionManager;
import utils.MathUtils;

/**
 Created by LEJYONER on 09/09/2017.
 */
public class SpaceballAI
        implements IArtificialInteligence {

    private final MovableMapEntity mMovableMapEntity;
    
    private long mLastDamageTime         = 0L;
    private long mLastShootedLaserTime   = 0L;
    
    private int mSelectedFactionID = 0;
    
    private int mMmo = 0;
    private int mEic = 0;
    private int mVru = 0;
    
    private int mMMODamage = 0;
    private int mEICDamage = 0;
    private int mVRUDamage = 0;
    
    private int mPosX = 0;
    private int mPosY = 0;
    
    public SpaceballAI(final MovableMapEntity pMovableMapEntity) {
        this.mMovableMapEntity = pMovableMapEntity;
    }

    @Override
    public void checkArtificialInteligence() {
    	final long currentTime = System.currentTimeMillis();
        if (!this.mMovableMapEntity.getMovement()
                .isMovementInProgress()) {
            if(this.getSelectedFactionID() != 0) {
                 this.generateNextMovement();
          }
        }
        for (final MovableMapEntity thisMapEntity : this.mMovableMapEntity.getInRangeMovableMapEntities()) {
        	if(thisMapEntity instanceof Player) {
        		final Player player = (Player) thisMapEntity;
        		if(player.getLaserAttack().isAttackInProgress()) {
        		if(player.getLockedTarget() == this.mMovableMapEntity) {
        			 if (MathUtils.hypotenuse(
        					 player.getCurrentPositionX() - this.mMovableMapEntity.getCurrentPositionX(),
        					 player.getCurrentPositionY() - this.mMovableMapEntity.getCurrentPositionY()) <=
                         700) {
        				 if ((currentTime - this.getLastDamageTime()) >= this.getLastShootedLaserTime()) {
        			if(player.getAccount().getFactionId() == 1) {
        				this.changeMMODamage(player.getLaserAttack().getDamage());
        				this.updateSelectedFactionId();
        			//	player.sendPacketToBoundSessions("0|A|STD|"+this.getMMODamage()+"");        				
        				this.setLastDamageTime(currentTime);        				
        				if(player.getLaserAttack().getSelectedLaserItem()
                        .equalsIgnoreCase(AmmunitionManager.RSB_75)) {
        				this.setLastShootedLaserTime(3000);
        				} else {
        				this.setLastShootedLaserTime(1000);
        				}        				
        			} else if(player.getAccount().getFactionId() == 2) {
        				this.changeEICDamage(player.getLaserAttack().getDamage());
        				this.updateSelectedFactionId();
        			//	player.sendPacketToBoundSessions("0|A|STD|"+this.getEICDamage()+"");
        				this.setLastDamageTime(currentTime);        				
        				if(player.getLaserAttack().getSelectedLaserItem()
                        .equalsIgnoreCase(AmmunitionManager.RSB_75)) {
        				this.setLastShootedLaserTime(3000);
        				} else {
        				this.setLastShootedLaserTime(1000);
        				} 
        			} else if(player.getAccount().getFactionId() == 3) {
        				this.changeVRUDamage(player.getLaserAttack().getDamage());
        				this.updateSelectedFactionId();
        			//	player.sendPacketToBoundSessions("0|A|STD|"+this.getVRUDamage()+"");
        				this.setLastDamageTime(currentTime);        				
        				if(player.getLaserAttack().getSelectedLaserItem()
                        .equalsIgnoreCase(AmmunitionManager.RSB_75)) {
        				this.setLastShootedLaserTime(3000);
        				} else {
        				this.setLastShootedLaserTime(1000);
        				} 
        			}
        			}
        			 }
        		}
        		}
        	} 
        }
        
        if (this.spaceballIsOnPortal()) {
        	givePoints();       	
            this.setMMODamage(0);
            this.setEICDamage(0);
            this.setVRUDamage(0);
            this.setSelectedFactionID(0);
        	final Spaceball spaceball2 = (Spaceball) this.mMovableMapEntity;
        	spaceball2.setSelectedFactionID(0);
        	this.setPosX(0);
        	this.setPosY(0);
            this.mMovableMapEntity.getCurrentSpaceMap().removeSpaceball(this.mMovableMapEntity.getMapEntityId());
            this.mMovableMapEntity.getCurrentSpaceMap().removeSpaceballForAllInRangeMapIntities(this.mMovableMapEntity.getMapEntityId());
            final Spaceball spaceball = (Spaceball) this.mMovableMapEntity;
            spaceball.setPositionXY(21000, 13200);
            spaceball.getCurrentSpaceMap()
			.addSpaceball(spaceball); 
            spaceball.getMovement()
            .initiate(spaceball.getCurrentPositionX(),
            		spaceball.getCurrentPositionY(), 21000, 13200,
                      2000);
        }
        ServerUtils.sendPacketToAllUsers("0|n|ssi|" + this.getMMOPoints() + "|" + this.getEICPoints() + "|" + this.getVRUPoints());
    }
    
    @Override
    public void receivedAttack(final MovableMapEntity pMovableMapEntity) {

    }
         
    public void givePoints() {
        switch (this.getSelectedFactionID()) {
            case 1:
            	this.changeMMOPoints(1);
            	ServerUtils.sendPacketToAllUsers("0|A|STD|MMO Spaceball'u kapıya getirdi!");
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                	final Account account = gameSessionAll.getAccount();             	
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) { 
                        	if(player.getCurrentSpaceMapId() == 16) {
                        	if(player.getAccount().getFactionId() == 1) {
                            	player.getAccount().changeUridium(1024);
                            	player.getAccount().changeHonor(1024);
                            	player.getAccount().changeExperience(102400);
                            	QueryManager.saveAccount(player.getAccount());
                            	player.sendPacketToBoundSessions("0|LM|ST|URI|" + 1024 + "|" + player.getAccount()
                                        .getUridium());
                            	player.sendPacketToBoundSessions("0|LM|ST|HON|" + 1024 + "|" + player.getAccount()
                                        .getHonor());
                            	player.sendPacketToBoundSessions("0|LM|ST|EP|" + 102400 + "|" + player.getAccount()
                                        .getExperience());
                            	if(player.getLockedTarget() == this.mMovableMapEntity) {
                            	player.sendCommandToBoundSessions(new ShipDeselectionCommand());
                            	player.getLaserAttack().setAttackInProgress(false);
                            	player.setLockedTarget(null);
                            	}
                            }
                         }
                      } 
                   }
                }
                break;
            case 2:
            	this.changeEICPoints(1);
            	ServerUtils.sendPacketToAllUsers("0|A|STD|EIC Spaceball'u kapıya getirdi!");
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                	final Account account = gameSessionAll.getAccount();             	
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) { 
                        	if(player.getCurrentSpaceMapId() == 16) {
                        	if(player.getAccount().getFactionId() == 2) {
                            	player.getAccount().changeUridium(1024);
                            	player.getAccount().changeHonor(1024);
                            	player.getAccount().changeExperience(102400);
                            	QueryManager.saveAccount(player.getAccount());
                            	player.sendPacketToBoundSessions("0|LM|ST|URI|" + 1024 + "|" + player.getAccount()
                                        .getUridium());
                            	player.sendPacketToBoundSessions("0|LM|ST|HON|" + 1024 + "|" + player.getAccount()
                                        .getHonor());
                            	player.sendPacketToBoundSessions("0|LM|ST|EP|" + 102400 + "|" + player.getAccount()
                                        .getExperience());
                            	if(player.getLockedTarget() == this.mMovableMapEntity) {
                            	player.sendCommandToBoundSessions(new ShipDeselectionCommand());
                            	player.getLaserAttack().setAttackInProgress(false);
                            	player.setLockedTarget(null);
                            	};
                            }
                         }
                      } 
                   }
                }
                break;
            case 3:
                this.changeVRUPoints(1);
                ServerUtils.sendPacketToAllUsers("0|A|STD|VRU Spaceball'u kapıya getirdi!");
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                	final Account account = gameSessionAll.getAccount();             	
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) { 
                        	if(player.getCurrentSpaceMapId() == 16) {
                        	if(player.getAccount().getFactionId() == 3) {
                            	player.getAccount().changeUridium(1024);
                            	player.getAccount().changeHonor(1024);
                            	player.getAccount().changeExperience(102400);
                            	QueryManager.saveAccount(player.getAccount());
                            	player.sendPacketToBoundSessions("0|LM|ST|URI|" + 1024 + "|" + player.getAccount()
                                        .getUridium());
                            	player.sendPacketToBoundSessions("0|LM|ST|HON|" + 1024 + "|" + player.getAccount()
                                        .getHonor());
                            	player.sendPacketToBoundSessions("0|LM|ST|EP|" + 102400 + "|" + player.getAccount()
                                        .getExperience());
                            	if(player.getLockedTarget() == this.mMovableMapEntity) {
                            	player.sendCommandToBoundSessions(new ShipDeselectionCommand());
                            	player.getLaserAttack().setAttackInProgress(false);
                            	player.setLockedTarget(null);
                            	}
                            }
                         }
                      } 
                   }
                }
                break;
        }
    }
    
    public void updateSelectedFactionId() {
        if (this.getMMODamage() > this.getEICDamage() && this.getMMODamage() > this.getVRUDamage()) {
        	this.setSelectedFactionID(1);
        	final Spaceball spaceball = (Spaceball) this.mMovableMapEntity;
        	spaceball.setSelectedFactionID(1);
        	spaceball.setMMODamage(this.getMMODamage());
        	this.setPosX(7000);
        	this.setPosY(13500);
            this.mMovableMapEntity.getMovement()
            .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                      this.mMovableMapEntity.getCurrentPositionY(), 7000, 13500,
                      this.mMovableMapEntity.getBaseSpeed());
        } else if (this.getEICDamage() > this.getMMODamage() && this.getEICDamage() > this.getVRUDamage()) {
        	this.setSelectedFactionID(2);
        	final Spaceball spaceball = (Spaceball) this.mMovableMapEntity;
        	spaceball.setSelectedFactionID(2);
        	spaceball.setEICDamage(this.getEICDamage());
            this.setPosX(28000);
            this.setPosY(1200);
            this.mMovableMapEntity.getMovement()
            .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                      this.mMovableMapEntity.getCurrentPositionY(), 28000, 1200,
                      this.mMovableMapEntity.getBaseSpeed());
        } else if (this.getVRUDamage() > this.getMMODamage() && this.getVRUDamage() > this.getEICDamage()) {
        	this.setSelectedFactionID(3);
        	final Spaceball spaceball = (Spaceball) this.mMovableMapEntity;
        	spaceball.setSelectedFactionID(3);
        	spaceball.setVRUDamage(this.getVRUDamage());
        	this.setPosX(28000);
        	this.setPosY(25000);
            this.mMovableMapEntity.getMovement()
            .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                      this.mMovableMapEntity.getCurrentPositionY(), 28000, 25000,
                      this.mMovableMapEntity.getBaseSpeed());
        }
    }
    
    public boolean spaceballIsOnPortal() {
        if (MathUtils.hypotenuse(this.mMovableMapEntity.getCurrentPositionX() - 7000,
        		                 this.mMovableMapEntity.getCurrentPositionY() - 13500) <= 2000) {
        	return true;
        } else if (MathUtils.hypotenuse(this.mMovableMapEntity.getCurrentPositionX() - 28000,
                                        this.mMovableMapEntity.getCurrentPositionY() - 1200) <= 2000) {
            return true;
        } else if (MathUtils.hypotenuse(this.mMovableMapEntity.getCurrentPositionX() - 28000,
                                        this.mMovableMapEntity.getCurrentPositionY() - 25000) <= 2000) {
            return true;
        } else {
            return false;
        }
    }
    
    private void generateNextMovement() {
        this.mMovableMapEntity.getMovement()
                              .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                                        this.mMovableMapEntity.getCurrentPositionY(), this.getPosX(), this.getPosY(),
                                        this.mMovableMapEntity.getBaseSpeed());
    }
    
    public long getLastShootedLaserTime() {
        return mLastShootedLaserTime;
    }

    public void setLastShootedLaserTime(final long pLastShootedLaserTime) {
    	this.mLastShootedLaserTime = pLastShootedLaserTime;
    }
    
    public long getLastDamageTime() {
        return mLastDamageTime;
    }

    public void setLastDamageTime(final long pLastDamageTime) {
    	this.mLastDamageTime = pLastDamageTime;
    }
    
    public int getMMODamage() {
    	return this.mMMODamage;
    }
    
    public void changeMMODamage(final int pMMODamage) {
        this.mMMODamage += pMMODamage;
    }
    
    public void setMMODamage(final int pMMODamage) {
        this.mMMODamage = pMMODamage;
    }
    
    public int getEICDamage() {
    	return this.mEICDamage;
    }
    
    public void changeEICDamage(final int pEICDamage) {
        this.mEICDamage += pEICDamage;
    }
    
    public void setEICDamage(final int pEICDamage) {
        this.mEICDamage = pEICDamage;
    }
    
    public int getVRUDamage() {
    	return this.mVRUDamage;
    }
    
    public void changeVRUDamage(final int pVRUDamage) {
        this.mVRUDamage += pVRUDamage;
    }
    
    public void setVRUDamage(final int pVRUDamage) {
        this.mVRUDamage = pVRUDamage;
    }
    
    public int getSelectedFactionID() {
    	return this.mSelectedFactionID;
    }
    
    public void setSelectedFactionID(final int pSelectedFactionID) {
        this.mSelectedFactionID = pSelectedFactionID;
    }
    
    public int getPosX() {
    	return this.mPosX;
    }
    
    public void setPosX(final int pPosX) {
        this.mPosX = pPosX;
    }
    
    public int getPosY() {
    	return this.mPosY;
    }
    
    public void setPosY(final int pPosY) {
        this.mPosY = pPosY;
    }
    
    public int getMMOPoints() {
    	return this.mMmo;
    }
    
    public void changeMMOPoints(final int pDifferenceMMOPoints) {
        this.mMmo += pDifferenceMMOPoints;
    }
    
    public int getEICPoints() {
    	return this.mEic;
    }
    
    public void changeEICPoints(final int pDifferenceEICPoints) {
        this.mEic += pDifferenceEICPoints;
    }
    
    public int getVRUPoints() {
    	return this.mVru;
    }
    
    public void changeVRUPoints(final int pDifferenceVRUPoints) {
        this.mVru += pDifferenceVRUPoints;
    }
    
}
