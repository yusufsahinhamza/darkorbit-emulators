package simulator.logic;

import java.util.Collection;

import simulator.map_entities.Lockable;
import simulator.map_entities.MapEntity;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.AttackLaserRunCommand;
import simulator.netty.serverCommands.AttributeShieldUpdateCommand;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;
import simulator.system.clans.Diplomacy;
import simulator.users.AmmunitionManager;
import simulator.users.CpusManager;
import utils.MathUtils;
import utils.Settings;
import utils.Tools;

public class LaserAttack {

    private final MovableMapEntity mMovableMapEntity;
    public       Lockable         mLockedTarget;

    public static final int ATTACK_RANGE             = 700;
    public static final int TIME_BETWEEN_LASER_SHOTS = 1000;
    public static final int RSB_COOLDOWN_TIME        = 3000;

    private boolean mAttackInProgress;
    public long    mLastShotTime;
    public long    mRsbCooldownFinish;
    private int mDamage;
    
    public LaserAttack(final MovableMapEntity pMovableMapEntity) {
        this.mMovableMapEntity = pMovableMapEntity;
    }

    public void initiate(final Lockable pLockedTarget) {
        this.mLockedTarget = pLockedTarget;
        this.setAttackInProgress(true);
    }

    public boolean isAttackInProgress() {
        return mAttackInProgress;
    }

    public void setAttackInProgress(final boolean pAttackInProgress) {
        mAttackInProgress = pAttackInProgress;
    }
    
    public void attack() {
	
		if(!Settings.SHOOT_ENABLED) {
			this.setAttackInProgress(false);
			return;
		}
		
        if (this.isAttackInProgress()) {
        	
        	if(this.mMovableMapEntity instanceof Player) {
	        	if(this.mLockedTarget instanceof Alien) {
	        		if(((Alien) this.mLockedTarget).getAlienShip().getShipId() == 80) {
	        			((Alien) this.mLockedTarget).mShooterPlayers.put(this.mMovableMapEntity.getMapEntityId(), (Player) this.mMovableMapEntity);
	        		}
	        	}
        	}       	
        	
	        	if(this.mMovableMapEntity instanceof Player) {	            	
	            	if(!Settings.FRIEND_SHOOT_ENABLED) {
	                	if(!(this.mMovableMapEntity instanceof Alien)) {
	                	if(this.mMovableMapEntity instanceof Player) {
	                		final Player player = (Player) this.mMovableMapEntity;
	                		if(player.getAccount().getEquipmentManager().getDamageConfig1() == 0 && player.getCurrentConfiguration() == 1) {
	                			player.sendPacketToBoundSessions("0|A|STM|msg_laser_not_equipped");
	                			player.getLaserAttack().setAttackInProgress(false);
	                			return;
	                		} else if (player.getAccount().getEquipmentManager().getDamageConfig2() == 0 && player.getCurrentConfiguration() == 2) {
	                			player.sendPacketToBoundSessions("0|A|STM|msg_laser_not_equipped");
	                			player.getLaserAttack().setAttackInProgress(false);
	                			return;
	                		}
	                	  }
	                	
	            		if(this.mLockedTarget instanceof Player && this.mMovableMapEntity instanceof Player){
	            			final Player thisPlayer = (Player) this.mMovableMapEntity;
	                        final Player otherPlayer = (Player) this.mLockedTarget;
	
	                        if(thisPlayer.getAccount().getClan() != null && otherPlayer.getAccount().getClan() != null) {
	                            boolean isWar;
	                            if(thisPlayer.getAccount().getFactionId() == otherPlayer.getAccount().getFactionId()){
	                                isWar = false;
	                                for(Diplomacy dip : thisPlayer.getAccount().getClan().getDiplomacies()){
	                                    if(dip.relationType == 3 && (dip.clanID1 == otherPlayer.getAccount().getClanId() || dip.clanID2 == otherPlayer.getAccount().getClanId())){
	                                        isWar = true;
	                                    }
	                                }
	                            
	                            if(!isWar) {
	                            	this.setAttackInProgress(false);
	                            	return;
	                            }
	                            
	                            }
	                        }
	                        }
	                	}
	                }
	            	
	            if (this.mLockedTarget != null) {
	                if (!this.mLockedTarget.isInSecureZone()) {      	
	                    if (MathUtils.hypotenuse(
	                            this.mMovableMapEntity.getCurrentPositionX() - this.mLockedTarget.getCurrentPositionX(),
	                            this.mMovableMapEntity.getCurrentPositionY() - this.mLockedTarget.getCurrentPositionY()) <=
	                        ATTACK_RANGE) {
	                    		
                        	if (this.mLockedTarget != null) {
                                if (this.mLockedTarget.getCurrentHitPoints() <= 0 || this.getDamage() >= this.mLockedTarget.getCurrentHitPoints()) {
                                	if(this.mLockedTarget instanceof Player) {
                                		if(!((Player) this.mLockedTarget).isDestroyed()) {
                                			this.mLockedTarget.destroy(this.mMovableMapEntity);
                                		}
                                	} else if(this.mLockedTarget instanceof Alien) {
                                		if(!((Alien) this.mLockedTarget).isDestroyed()) {
                                			if(((Alien) this.mLockedTarget).getAlienShip().getShipId() == 80) {
                                				((Alien) this.mLockedTarget).destroyCubikon();
                                			} else {
                                				this.mLockedTarget.destroy(this.mMovableMapEntity);
                                			}
                                		}
                                	}
                                 }
                        	 }
                        	
	                            if (this.checkAttackTime()) {
	                                this.updateLastShotTime();
	                                
	                                final AttackLaserRunCommand laserRunCommand = this.createAttackLaserRunCommand();
	                            	
	                                this.mMovableMapEntity.sendCommandToInRange(laserRunCommand);
	                            	
	                                if(this.mMovableMapEntity instanceof Player){                                	
	                                    if (this.mLockedTarget instanceof Player) {                                   	
	                                    	final Player player = (Player) this.mMovableMapEntity;
	                                        if(player.getAccount().isCloaked())
	                                        {
	                                        	final String cloakRemovedPacket = "0|A|STM|msg_uncloaked";
	                                        player.sendPacketToBoundSessions(cloakRemovedPacket);
	                                        final String cloakPacket = "0|n|INV|" + player.getAccount().getUserId() + "|0";
	                                        player.getAccount().setCloaked(false);
	                                        player.sendPacketToBoundSessions(cloakPacket);
	                                        player.sendPacketToInRange(cloakPacket);
	                                        
	                                        player.getAccount().getCpusManager().getSelectedCpus().remove(CpusManager.CLK_XL);
	                                        player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(CpusManager.CLK_XL));
	                                        }
	                                    }
	                                    else
	                                    {
	                                    	final Player player = (Player) this.mMovableMapEntity;
	                                        if(player.getAccount().isCloaked()) {
	                                        	final String cloakRemovedPacket = "0|A|STM|msg_uncloaked";
	                                        player.sendPacketToBoundSessions(cloakRemovedPacket);
	                                        final String cloakPacket = "0|n|INV|" + player.getAccount().getUserId() + "|0";
	                                        player.getAccount().setCloaked(false);
	                                        player.sendPacketToBoundSessions(cloakPacket);
	                                        player.sendPacketToInRange(cloakPacket);
	                                        
	                                        player.getAccount().getCpusManager().getSelectedCpus().remove(CpusManager.CLK_XL);
	                                        player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(CpusManager.CLK_XL));
	                                        }
	                                    }                                    
	                                    }
	                                
	                                int damage = 0;
	                                
	                                if(this.mLockedTarget instanceof Player) {
	                                	final Player thisPlayer = (Player) this.mMovableMapEntity;
	                                	final Player player = (Player) this.mLockedTarget;
	                                	if(player.canBeShoot()){                               		
	                                		if(thisPlayer.getAccount().getSkillsManager().isSpectrumAbilityActivated()) {
	                                        		damage = (Tools.getRandomDamage(
	                                                    this.getDamageMultiplier() * this.mMovableMapEntity.getCurrentDamage()) / 100) * 70;
	                                		} else {
	                                    		if(player.getAccount().getSkillsManager().isSpectrumAbilityActivated()) {
	                                            	damage = (Tools.getRandomDamage(
	                                                        this.getDamageMultiplier() * this.mMovableMapEntity.getCurrentDamage()) / 100) * 50;
	                                        		} else {
	                                            	damage = Tools.getRandomDamage(
	                                                        this.getDamageMultiplier() * this.mMovableMapEntity.getCurrentDamage());
	                                        		}
	                                		}                                	                                		                                		                                		
	                                	} else {
	                                		damage = 0;
	                                	}
	                                } else {
	                                	final Player thisPlayer = (Player) this.mMovableMapEntity;
	                            		if(thisPlayer.getAccount().getSkillsManager().isSpectrumAbilityActivated()) {
	                                		damage = (Tools.getRandomDamage(
	                                            this.getDamageMultiplier() * this.mMovableMapEntity.getCurrentDamage()) / 100) * 70;
	                            		} else {
	                            			damage = Tools.getRandomDamage(
	                                            this.getDamageMultiplier() * this.mMovableMapEntity.getCurrentDamage());
	                            		}
	                                }
	                                
	                                this.setDamage(damage);
	                                
	                                if (this.getSelectedLaserItem()
	                                        .equalsIgnoreCase(AmmunitionManager.SAB_50)) {
	                                    damage = this.mLockedTarget.damageShieldPoints(this.mMovableMapEntity, damage);
	                                    this.mMovableMapEntity.changeCurrentShieldPoints(damage);
	
	                                    final AttributeShieldUpdateCommand shieldUpdateCommand =
	                                            this.mMovableMapEntity.getShieldUpdateCommand();
	                                    if (this.mMovableMapEntity instanceof Player) {
	                                        ((Player) this.mMovableMapEntity).sendCommandToBoundSessions(shieldUpdateCommand);
	                                    }
	                                    if (this.mLockedTarget instanceof Player) {
	                                        final Player player = (Player) this.mLockedTarget;
	                                        player.sendCommandToBoundSessions(player.getShieldUpdateCommand());
	                                    }
	
	                                    //TODO send shield update to users locking the hero
	                                } else {
	                                    this.mLockedTarget.addHitPointsDamage(this.mMovableMapEntity, damage);
	                                }
	
	                                
	
	                                if (this.mMovableMapEntity instanceof Player) {
	                                    final Player player = (Player) this.mMovableMapEntity;
	                                	if (!this.getSelectedLaserItem()
	                                            .equalsIgnoreCase(AmmunitionManager.SAB_50)) {
	                                    player.getAccount()
	                                          .getTechsManager()
	                                          .onLaserAttack(damage);
	                                	}
	                                    player.sendCommandToBoundSessions(laserRunCommand);
	                                    //TODO remove amount of selected ammo
	                                }
	
	                            }
	                    } else {
	                         //player.sendPacketToBoundSessions("0|A|STM|outofrange");
	                        //Menzil dışı
	                    }
	                } else {
	                	 //player.sendPacketToBoundSessions("0|A|STM|peacearea");
	                    //Korumalı bölge
	                    this.setAttackInProgress(false);
	                }
	            } else {
	                this.setAttackInProgress(false);
	            }
	        } else if(this.mMovableMapEntity instanceof Alien){

            	if (this.mLockedTarget != null) {
                    
	                if (!this.mLockedTarget.isInSecureZone()) {      	
	                    if (MathUtils.hypotenuse(
	                            this.mMovableMapEntity.getCurrentPositionX() - this.mLockedTarget.getCurrentPositionX(),
	                            this.mMovableMapEntity.getCurrentPositionY() - this.mLockedTarget.getCurrentPositionY()) <=
	                        ATTACK_RANGE) {
	
	                            if (this.checkAttackTime()) {	                            	
	                            	if(this.mLockedTarget instanceof Player) {
	                            		final Player player = (Player) this.mLockedTarget;
		                                this.updateLastShotTime();
		                                
		                                int damage;
		                                
		                                if(player.canBeShoot()) {
		                                	damage = Tools.getRandomDamage(
			                                        this.getDamageMultiplier() * this.mMovableMapEntity.getCurrentDamage());
		                                } else {
		                                	damage = 0;
		                                }
		                                
		                                final AttackLaserRunCommand laserRunCommand = this.createAttackLaserRunCommand();
	
		                                this.mMovableMapEntity.sendCommandToInRange(laserRunCommand);
		                                
		                                this.mLockedTarget.addHitPointsDamage(this.mMovableMapEntity, damage);
	                            	}
	                            }
	                    }
	                }          
            	 }
	        }
        }
    }
    
    public void setDamage(final int pDamage) {
    	this.mDamage = pDamage;
    }
    
    public int getDamage() {
    	return this.mDamage;
    }
    
    private void updateLastShotTime() {
        final long currentTime = System.currentTimeMillis();
        if (this.getSelectedLaserItem()
                .equalsIgnoreCase(AmmunitionManager.RSB_75)) {
            this.mRsbCooldownFinish = currentTime + RSB_COOLDOWN_TIME;
            if (this.mMovableMapEntity instanceof Player) {
                final Player player = (Player) this.mMovableMapEntity;
                final AmmunitionManager ammunitionManager = player.getAccount()
                                                                  .getAmmunitionManager();
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                        ammunitionManager.getCooldownType(AmmunitionManager.RSB_75),
                        ammunitionManager.getItemTimerState(AmmunitionManager.RSB_75), RSB_COOLDOWN_TIME,
                        RSB_COOLDOWN_TIME));
            }
        } else {
            this.mLastShotTime = currentTime;
        }
    }

    public long getLastShotTime() {
        return this.mLastShotTime;
    }

    public boolean checkAttackTime() {
        final long currentTime = System.currentTimeMillis();
        if (this.getSelectedLaserItem()
                .equalsIgnoreCase(AmmunitionManager.RSB_75)) {
            return currentTime >= this.mRsbCooldownFinish;
        }
        return (currentTime - this.getLastShotTime()) >= TIME_BETWEEN_LASER_SHOTS;

    }
    
    public long getRsbCooldownTime() {
        final long timeLeft = this.mRsbCooldownFinish - System.currentTimeMillis();
        return timeLeft > 0 ? timeLeft : 0;
    }

    public AttackLaserRunCommand createAttackLaserRunCommand() {
    	if(this.mMovableMapEntity instanceof Alien) {
            return new AttackLaserRunCommand(this.mMovableMapEntity.getMapEntityId(), this.mLockedTarget.getMapEntityId(),
                    this.getSelectedLaser(), false, false);
    	}
        return new AttackLaserRunCommand(this.mMovableMapEntity.getMapEntityId(), this.mLockedTarget.getMapEntityId(),
                                         this.getSelectedLaser(), true, true);
    }

    public String getSelectedLaserItem() {
        if (this.mMovableMapEntity instanceof Player) {
            final Player player = (Player) this.mMovableMapEntity;
            return player.getAccount()
                         .getClientSettingsManager()
                         .getSelectedLaserItem();
        }
        return AmmunitionManager.LCB_10;
    }

    public int getSelectedLaser() {
        switch (this.getSelectedLaserItem()) {
            case AmmunitionManager.LCB_10:
                return 0;
            case AmmunitionManager.MCB_25:
                return 1;
            case AmmunitionManager.MCB_50:
                return 2;
            case AmmunitionManager.UCB_100:
                return 3;
            case AmmunitionManager.RSB_75:
                return 6;
            case AmmunitionManager.SAB_50:
                return 4;
            case AmmunitionManager.JOB_100:
                return 9;
            case AmmunitionManager.RB_214:
                return 10;
            case AmmunitionManager.CBO_100:
                return 8;
            default:
                return 0;
        }
    }
    
    public int getDamageMultiplier() {

            switch (this.getSelectedLaserItem()) {
            case AmmunitionManager.LCB_10:
                return 1;
            case AmmunitionManager.MCB_25:
                return 2;
            case AmmunitionManager.MCB_50:
                return 3;
            case AmmunitionManager.UCB_100:
                return 4;
            case AmmunitionManager.RSB_75:
                return 5;
            case AmmunitionManager.SAB_50:
                return 2;
            default:
                return 1;
    	}
    }
}
