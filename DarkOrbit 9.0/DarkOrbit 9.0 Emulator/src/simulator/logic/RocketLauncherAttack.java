package simulator.logic;

import mysql.QueryManager;
import simulator.map_entities.Lockable;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.AttributeShieldUpdateCommand;
import simulator.system.clans.Diplomacy;
import simulator.users.AmmunitionManager;
import simulator.users.CpusManager;
import utils.MathUtils;
import utils.Settings;
import utils.Tools;

public class RocketLauncherAttack {

	private int mDamage;
    private static final int ECO_10_DAMAGE	= 10000;
    private static final int SAR_02_DAMAGE	= 20000;
    private long    mRocketLauncherCooldownFinish;
    private final MovableMapEntity mMovableMapEntity;
    
    private static final int PREMIUM_TIME_BETWEEN_ROCKET_FIRE = 5000;
    private static final int TIME_BETWEEN_ROCKET_FIRE = 6000;

    private long mLastAttackTime = 0L;

    public RocketLauncherAttack(final MovableMapEntity pMovableMapEntity) {
        this.mMovableMapEntity = pMovableMapEntity;
    }


    public void checkRocketLauncherAttackSystem() {
        if (this.mMovableMapEntity.getLaserAttack()
                                  .isAttackInProgress()) {
        	if(this.mMovableMapEntity instanceof Player)
        	{
        		final Player playerlocked = (Player) this.mMovableMapEntity;
        		if(playerlocked.getAccount().getCpusManager().getAutoRocketLauncherCPU() == true){
        			this.attack();
        		}
        	}
        }

    }
    
    public void attack() {
    	if(this.mMovableMapEntity instanceof Player) {
    		if(!Settings.SHOOT_ENABLED) {
    			return;
    		}
    		
    		final Lockable lockedTarget = this.mMovableMapEntity.getLockedTarget();    	
        	
        	if(!Settings.FRIEND_SHOOT_ENABLED) {
        	if(!(this.mMovableMapEntity instanceof Alien)) {
        		if(this.mMovableMapEntity.getLockedTarget() instanceof Player && this.mMovableMapEntity instanceof Player){
        			final Player thisPlayer = (Player) this.mMovableMapEntity;
                    final Player otherPlayer = (Player) this.mMovableMapEntity.getLockedTarget();

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
                        	this.mMovableMapEntity.getLaserAttack().setAttackInProgress(false);
                        	return;
                        }
                        
                        }
                    }
                    }
            	}
        	}
        	
        	
        if (lockedTarget != null) {        	
            if (!lockedTarget.isInSecureZone()) {
                if (MathUtils.hypotenuse(
                        this.mMovableMapEntity.getCurrentPositionX() - lockedTarget.getCurrentPositionX(),
                        this.mMovableMapEntity.getCurrentPositionY() - lockedTarget.getCurrentPositionY()) <=
                    this.getRange()) {

                    if (lockedTarget.getCurrentHitPoints() <= 0 || this.getDamage() >= lockedTarget.getCurrentHitPoints()) {
                    	if(lockedTarget instanceof Player) {
                    		if(!((Player) lockedTarget).isDestroyed()) {
                    			lockedTarget.destroy(this.mMovableMapEntity);
                    		}
                    	} else if(lockedTarget instanceof Alien) {
                    		if(!((Alien) lockedTarget).isDestroyed()) {
                    			if(((Alien) lockedTarget).getAlienShip().getShipId() == 80) {
                    				((Alien) lockedTarget).destroyCubikon();
                    			} else {
                    				lockedTarget.destroy(this.mMovableMapEntity);
                    			}
                    		}
                    	}
                    }
                    
                	if(this.mMovableMapEntity instanceof Player) {
                		final Player player = (Player) mMovableMapEntity;
                		if(this.getSelectedItemId().equalsIgnoreCase(AmmunitionManager.ECO_10) && player.getAccount().getECO_10() <= 0) {
                			return;
                		} else if(this.getSelectedItemId().equalsIgnoreCase(AmmunitionManager.SAR_02) && player.getAccount().getSAR_02() <= 0) {
                			return;
                		}
                	}             	
                	                	    	                
                    if (this.checkAttackTime()) {
                        this.updateLastShotTime();
                        
                    	if (lockedTarget != null) {
                    		
                        	if(this.mMovableMapEntity instanceof Player) {
                	        	if(lockedTarget instanceof Alien) {
                	        		if(((Alien) lockedTarget).getAlienShip().getShipId() == 80) {
                	        			((Alien) lockedTarget).mShooterPlayers.put(this.mMovableMapEntity.getMapEntityId(), (Player) this.mMovableMapEntity);
                	        		}
                	        	}
                        	}                       	
                    	}
                    	
                        if(this.mMovableMapEntity instanceof Player){                                	
                            if (this.mMovableMapEntity.getLockedTarget() instanceof Player) {                                   	
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
                            } else {
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
                            }                     
                        
                        int damage = 0;
                        
                        if(this.mMovableMapEntity.getLockedTarget() instanceof Player) {
                        	final Player player = (Player) this.mMovableMapEntity.getLockedTarget();
                        	if(player.canBeShoot()){
                            	damage = Tools.getRandomDamage(this.getRocketLauncherDamage());
                        	} else {
                        		damage = 0;
                        	}
                        } else {
                        	damage = Tools.getRandomDamage(this.getRocketLauncherDamage());
                        }
                        
                        this.setDamage(damage);
                        
                        if (this.getSelectedItemId()
                                .equalsIgnoreCase(AmmunitionManager.SAR_02)) {
                            damage = lockedTarget.damageShieldPoints(this.mMovableMapEntity, damage);
                            this.mMovableMapEntity.changeCurrentShieldPoints(damage);

                            final AttributeShieldUpdateCommand shieldUpdateCommand =
                                    this.mMovableMapEntity.getShieldUpdateCommand();
                            if (this.mMovableMapEntity instanceof Player) {
                                ((Player) this.mMovableMapEntity).sendCommandToBoundSessions(shieldUpdateCommand);
                            }
                            if (lockedTarget instanceof Player) {
                                final Player player = (Player) lockedTarget;
                                player.sendCommandToBoundSessions(player.getShieldUpdateCommand());
                            } else if(lockedTarget instanceof Alien) {
                                final Alien alien = (Alien) lockedTarget;
                                alien.sendCommandToInRange(alien.getShieldUpdateCommand());
                            }

                            //TODO send shield update to users locking the hero
                        } else {
                        	lockedTarget.addHitPointsDamage(this.mMovableMapEntity, damage);
                        }
                                              
                        final String rocketRunPacket = "0|RL|A|"+this.mMovableMapEntity.getMapEntityId()+"|"+lockedTarget.getMapEntityId()+"|5|"+ this.getRocketLauncherType() +"|1";

                        //CEPHANE EKSİLTME
                        if (this.getSelectedItemId().equalsIgnoreCase(AmmunitionManager.ECO_10)) {
                        	((Player) this.mMovableMapEntity).getAccount().changeECO_10(-5);
                        } else if(this.getSelectedItemId().equalsIgnoreCase(AmmunitionManager.SAR_02)) {
                        	((Player) this.mMovableMapEntity).getAccount().changeSAR_02(-5);
                        }
                        ((Player) this.mMovableMapEntity).sendCommandToBoundSessions(((Player) this.mMovableMapEntity).getAccount().getAmmunitionManager().getRocketLauncherItemStatus(this.mMovableMapEntity.getRocketLauncherAttack().getSelectedItemId()));
                        QueryManager.saveAmmo(((Player) this.mMovableMapEntity).getAccount());
                        //CEPHANE EKSİLTME
                        
                        this.mMovableMapEntity.sendPacketToInRange(rocketRunPacket);

                        if (this.mMovableMapEntity instanceof Player) {
                            final Player player = (Player) this.mMovableMapEntity;
                            player.sendPacketToBoundSessions(rocketRunPacket);
                        }

                        this.setLastAttackTime(System.currentTimeMillis());
                    }
                } else {
            //        final Player player = (Player) this.mMovableMapEntity;
            //        player.sendPacketToBoundSessions("0|A|STM|outofrange");
                    //Menzil dışı
                }
            } else {
            //    final Player player = (Player) this.mMovableMapEntity;
            //    player.sendPacketToBoundSessions("0|A|STM|peacearea");
                //Korumalı bölge
            }
        }
    	}
    }
    
  /**  private void sendHitCommand(final Lockable lockedTarget, final int pDamage) {
        new Timer().schedule(new TimerTask() {

            @Override
            public void run() {
                final AttackHitCommand attackHitCommand =
                        new AttackHitCommand(new AttackTypeModule(AttackTypeModule.ROCKET),
                                             mMovableMapEntity.getMapEntityId(), lockedTarget.getMapEntityId(),
                                             lockedTarget.getCurrentHitPoints(), lockedTarget.getCurrentShieldPoints(),
                                             lockedTarget.getCurrentNanoHull(), pDamage, false);

                mMovableMapEntity.sendCommandToInRange(attackHitCommand);

                if (mMovableMapEntity instanceof Player) {
                    final Player player = (Player) mMovableMapEntity;
                    player.sendCommandToBoundSessions(attackHitCommand);
                }
            }
        }, ROCKETS_HIT_DELAY);
    }
      */
        	
    public void setDamage(final int pDamage) {
    	this.mDamage = pDamage;
    }
            
    public int getDamage() {
    	return this.mDamage;
    }
            
    private int getRange() {
        switch (this.getSelectedItemId()) {
            case AmmunitionManager.ECO_10:
                return 800;
            case AmmunitionManager.SAR_02:
                return 800;
            default:
                return 0;
        }
    }
    
    public long getRocketLauncherCooldownTime() {
        final long timeLeft = this.mRocketLauncherCooldownFinish - System.currentTimeMillis();
        return timeLeft > 0 ? timeLeft : 0;
    }
    
    private void updateLastShotTime() {
        final long currentTime = System.currentTimeMillis();
        if (this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.ECO_10) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.SAR_02)) {
        	final Player player2 = (Player) this.mMovableMapEntity;
        	if(player2.getAccount().isPremiumAccount() == true) {
        		this.mRocketLauncherCooldownFinish = currentTime + PREMIUM_TIME_BETWEEN_ROCKET_FIRE;
        	} else {
        		this.mRocketLauncherCooldownFinish = currentTime + TIME_BETWEEN_ROCKET_FIRE;
        	}            
        } else {
            this.mLastAttackTime = currentTime;
        }
    }
    
    public boolean checkAttackTime() {
        final long currentTime = System.currentTimeMillis();
        if (this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.ECO_10) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.SAR_02)) {
            return currentTime >= this.mRocketLauncherCooldownFinish;
        }
        final Player player = (Player) this.mMovableMapEntity;
        
            if(player.getAccount().isPremiumAccount() == true)
            {
            return (currentTime - this.getLastAttackTime()) >= PREMIUM_TIME_BETWEEN_ROCKET_FIRE;
            }
            else
            {
            	return (currentTime - this.getLastAttackTime()) >= TIME_BETWEEN_ROCKET_FIRE;
            }
    }
    
    public String getSelectedItemId() {
        final Player player = (Player) this.mMovableMapEntity;
        return player.getAccount()
                     .getClientSettingsManager()
                     .getSelectedRocketLauncherItem();
    }

    public long getLastAttackTime() {
        return mLastAttackTime;
    }

    public void setLastAttackTime(final long pLastAttackTime) {
        mLastAttackTime = pLastAttackTime;
    }

    public int getRocketLauncherDamage() {
        switch (this.getSelectedItemId()) {
            case AmmunitionManager.ECO_10:
                return ECO_10_DAMAGE;
            case AmmunitionManager.SAR_02:
                return SAR_02_DAMAGE;
            default:
                return 0;
        }
    }

    public int getRocketLauncherType() {
        switch (this.getSelectedItemId()) {
            case AmmunitionManager.ECO_10:
                return 9;
            case AmmunitionManager.HSTRM_01:
                return 10;
            case AmmunitionManager.UBR_100:
                return 11;
            case AmmunitionManager.SAR_01:
                return 12;
            case AmmunitionManager.SAR_02:
                return 13;
            default:
                return 0;
        }
    }
    
}
