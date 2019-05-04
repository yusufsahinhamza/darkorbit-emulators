package simulator.logic;

import simulator.map_entities.Lockable;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;
import simulator.system.clans.Diplomacy;
import simulator.users.AmmunitionManager;
import simulator.users.CpusManager;
import utils.MathUtils;
import utils.Settings;
import utils.Tools;

public class RocketAttack {

	private int mDamage;
    private static final int R_310_DAMAGE    = 1000;
    private static final int PLT_2026_DAMAGE = 2000;
    private static final int PLT_2021_DAMAGE = 4000;
    private static final int PLT_3030_DAMAGE = 6000;
    private long    mRocketCooldownFinish;
    private final MovableMapEntity mMovableMapEntity;
    
    private static final int PREMIUM_TIME_BETWEEN_ROCKET_FIRE = 1500; // Sonradan eklendi (premiumlu 1 saniye)
    private static final int TIME_BETWEEN_ROCKET_FIRE = 3000; // Eski değeri 1000'dir. (premiumsuz 3 saniye)
//  private static final int ROCKETS_HIT_DELAY        = 1000;

    private long mLastAttackTime = 0L;

    public RocketAttack(final MovableMapEntity pMovableMapEntity) {
        this.mMovableMapEntity = pMovableMapEntity;
    }


    public void checkRocketAttackSystem() {
        if (this.mMovableMapEntity.getLaserAttack()
                                  .isAttackInProgress()) {
        	if(this.mMovableMapEntity instanceof Player)
        	{
        		final Player playerlocked = (Player) this.mMovableMapEntity;
        		if(playerlocked.getAccount().getCpusManager().getAutoRocketCPU() == true){
        			this.attack();
        		}
        	}
        }

    }
    
    public void attack() {

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
                            	damage = Tools.getRandomDamage(((Player) this.mMovableMapEntity).getCurrentRocketDamage());
                        	} else {
                        		damage = 0;
                        	}
                        } else {
                        	damage = Tools.getRandomDamage(((Player) this.mMovableMapEntity).getCurrentRocketDamage());
                        }
                        
                        this.setDamage(damage);
                        
                        lockedTarget.addHitPointsDamage(this.mMovableMapEntity, damage);
                        
                        final boolean precisionTargeterActivated = ((this.mMovableMapEntity instanceof Player) &&
                                                                    ((Player) this.mMovableMapEntity).getAccount()
                                                                                                     .getTechsManager()
                                                                                                     .isPrecisionTargeterEffectActivated());
                        int rocketArge = 0;
                        if(this.mMovableMapEntity instanceof Player){
                        	final Player player = (Player) this.mMovableMapEntity;
                        	if(player.getAccount().getRocketDmgUp() == 5) {
                        		rocketArge = 1;
                        	} else {
                        		rocketArge = 0;
                        	}
                        }
                        final String rocketRunPacket = "0|v|" + this.mMovableMapEntity.getMapEntityId() + "|" +
                                                       lockedTarget.getMapEntityId() +
                                                       "|H|" + this.getSelectedRocket() + "|"+ rocketArge +"|" +
                                                       (precisionTargeterActivated ? 1 : 0);
                    	// 5Lİ ROKET BUNUN GİBİ YAPILABİLİR!!
                        //        final String packet = "0|RL|A|"+this.mMovableMapEntity.getMapEntityId()+"|"+this.mMovableMapEntity.getLockedTarget().getMapEntityId()+"|5|7|1";
                        //        player3.sendPacketToBoundSessions(packet);
                        //        player3.sendPacketToInRange(packet);
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
            case AmmunitionManager.R_310:
                return 400;
            case AmmunitionManager.PLT_2026:
                return 600;
            case AmmunitionManager.PLT_2021:
                return 800;
            case AmmunitionManager.PLT_3030:
                return 800;
            default:
                return 0;
        }
    }
    
    public long getRocketCooldownTime() {
        final long timeLeft = this.mRocketCooldownFinish - System.currentTimeMillis();
        return timeLeft > 0 ? timeLeft : 0;
    }
    
    private void updateLastShotTime() {
        final long currentTime = System.currentTimeMillis();
        if (this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.R_310) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.PLT_2026) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.PLT_2021) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.PLT_3030)) {
        	final Player player2 = (Player) this.mMovableMapEntity;
        	if(player2.getAccount().isPremiumAccount() == true)
        	{
        		this.mRocketCooldownFinish = currentTime + PREMIUM_TIME_BETWEEN_ROCKET_FIRE;
        	}
        	else
        	{
        		this.mRocketCooldownFinish = currentTime + TIME_BETWEEN_ROCKET_FIRE;
        	}
            if (this.mMovableMapEntity instanceof Player) {
                final Player player = (Player) this.mMovableMapEntity;
                final AmmunitionManager ammunitionManager = player.getAccount()
                                                                  .getAmmunitionManager();
                if(player.getAccount().isPremiumAccount() == true)
                {
                    player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                            ammunitionManager.getCooldownType(AmmunitionManager.R_310),
                            ammunitionManager.getItemTimerState(AmmunitionManager.R_310), PREMIUM_TIME_BETWEEN_ROCKET_FIRE,
                            PREMIUM_TIME_BETWEEN_ROCKET_FIRE));
                    player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                            ammunitionManager.getCooldownType(AmmunitionManager.PLT_2026),
                            ammunitionManager.getItemTimerState(AmmunitionManager.PLT_2026), PREMIUM_TIME_BETWEEN_ROCKET_FIRE,
                            PREMIUM_TIME_BETWEEN_ROCKET_FIRE));
                    player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                            ammunitionManager.getCooldownType(AmmunitionManager.PLT_2021),
                            ammunitionManager.getItemTimerState(AmmunitionManager.PLT_2021), PREMIUM_TIME_BETWEEN_ROCKET_FIRE,
                            PREMIUM_TIME_BETWEEN_ROCKET_FIRE));
                    player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                            ammunitionManager.getCooldownType(AmmunitionManager.PLT_3030),
                            ammunitionManager.getItemTimerState(AmmunitionManager.PLT_3030), PREMIUM_TIME_BETWEEN_ROCKET_FIRE,
                            PREMIUM_TIME_BETWEEN_ROCKET_FIRE));
                }
                else{
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                        ammunitionManager.getCooldownType(AmmunitionManager.R_310),
                        ammunitionManager.getItemTimerState(AmmunitionManager.R_310), TIME_BETWEEN_ROCKET_FIRE,
                        TIME_BETWEEN_ROCKET_FIRE));
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                        ammunitionManager.getCooldownType(AmmunitionManager.PLT_2026),
                        ammunitionManager.getItemTimerState(AmmunitionManager.PLT_2026), TIME_BETWEEN_ROCKET_FIRE,
                        TIME_BETWEEN_ROCKET_FIRE));
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                        ammunitionManager.getCooldownType(AmmunitionManager.PLT_2021),
                        ammunitionManager.getItemTimerState(AmmunitionManager.PLT_2021), TIME_BETWEEN_ROCKET_FIRE,
                        TIME_BETWEEN_ROCKET_FIRE));
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                        ammunitionManager.getCooldownType(AmmunitionManager.PLT_3030),
                        ammunitionManager.getItemTimerState(AmmunitionManager.PLT_3030), TIME_BETWEEN_ROCKET_FIRE,
                        TIME_BETWEEN_ROCKET_FIRE));
                }
            }
        } else {
            this.mLastAttackTime = currentTime;
        }
    }
    
    public boolean checkAttackTime() {
        final long currentTime = System.currentTimeMillis();
        if (this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.R_310) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.PLT_2026) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.PLT_2021) || this.getSelectedItemId()
                .equalsIgnoreCase(AmmunitionManager.PLT_3030)) {
            return currentTime >= this.mRocketCooldownFinish;
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
                     .getSelectedRocketItem();
    }

    public long getLastAttackTime() {
        return mLastAttackTime;
    }

    public void setLastAttackTime(final long pLastAttackTime) {
        mLastAttackTime = pLastAttackTime;
    }

    public int getRocketDamage() {
        switch (this.getSelectedItemId()) {
            case AmmunitionManager.R_310:
                return R_310_DAMAGE;
            case AmmunitionManager.PLT_2026:
                return PLT_2026_DAMAGE;
            case AmmunitionManager.PLT_2021:
                return PLT_2021_DAMAGE;
            case AmmunitionManager.PLT_3030:
                return PLT_3030_DAMAGE;
            default:
                return 0;
        }
    }

    private int getSelectedRocket() {
        switch (this.getSelectedItemId()) {
            case AmmunitionManager.R_310:
                return 1;
            case AmmunitionManager.PLT_2026:
                return 2;
            case AmmunitionManager.PLT_2021:
                return 3;
            case AmmunitionManager.PLT_3030:
                return 4;
            default:
                return 0;
        }
    }
}
