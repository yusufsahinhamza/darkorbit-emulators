package simulator.map_entities;


import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.AttackHitCommand;
import simulator.netty.serverCommands.AttackTypeModule;
import simulator.netty.serverCommands.AttributeSkillShieldUpdateCommand;
import simulator.system.SpaceMap;

/**
 Created by Pedro on 23-03-2015.
 */
public abstract class AttackableMapEntity
        extends MovableMapEntity {

    private static final int HITPOINTS_REPAIR_DELAY_AFTER_ATTACK = 10000;
    private static final int SHIELD_REPAIR_DELAY_AFTER_ATTACK    = 15000;
    private static final int HITPOINTS_REPAIR_RATE               = 1000;
    private static final int SHIELD_REPAIR_RATE                  = 1000;
    private static final int HITPOINTS_REPAIR_TIME_SECONDS       = 73;
    private static final int PREMIUM_OR_ARGE_HITPOINTS_REPAIR_TIME_SECONDS       = 36;
    private static final int PREMIUM_AND_ARGE_HITPOINTS_REPAIR_TIME_SECONDS       = 24;
    private static final int SHIELD_REPAIR_TIME_SECONDS          = 25;
    private static final int SHIELD_DEFAULT_ID                   = 1;

    private static final int CHECK_DAMAGE_DELAY = 300;

    public final String HEAL_HITPOINTS = "HPT";
    public final String HEAL_SHIELD    = "SHD";

    public int mHitPointsDamageCollector = 0;

    private long mLastCheckedDamageTime = 0L;

    private long    mLastDamagedTime           = 0L;
    private long    mLastShieldSkillRepairTime = 0L;
    private long    mLastRepairBotRepairTime   = 0L;
    private boolean mIsShieldSkillActivated    = false;
    private boolean mIsRepairBotActivated      = false;

    public AttackableMapEntity(final SpaceMap pCurrentSpaceMapId) {
        super(pCurrentSpaceMapId);
    }

    public AttackableMapEntity(final SpaceMap pCurrentSpaceMapId, final int pMapEntityIdOverride) {
        super(pCurrentSpaceMapId, pMapEntityIdOverride);
    }

    public void doTick() {
        this.checkHitpointsRepair();
        this.checkShieldPointsRepair();
    }

    private void checkHitpointsRepair() {
        final long currentTime = System.currentTimeMillis();
        if(!this.getLaserAttack().isAttackInProgress()) {
        if ((currentTime - this.getLastDamagedTime()) >= HITPOINTS_REPAIR_DELAY_AFTER_ATTACK) {
            if ((currentTime - this.getLastRepairBotRepairTime()) >= HITPOINTS_REPAIR_RATE) {
                if (this.getCurrentHitPoints() != this.getMaximumHitPoints()) {
                    if (!this.isRepairBotActivated()) {
                        this.setRepairBotActivated(true);
                    }
                    if(this instanceof Player) {
	                    final Player player = (Player) this;
	                    if(player.getAccount().getRepairUp() == 5 && player.getAccount().isPremiumAccount()) {
	                        final int repairHitpoints = this.getMaximumHitPoints() / PREMIUM_AND_ARGE_HITPOINTS_REPAIR_TIME_SECONDS;
	                        this.healEntity(repairHitpoints, HEAL_HITPOINTS);
	                    } else if(player.getAccount().getRepairUp() == 5 || player.getAccount().isPremiumAccount()) {
	                        final int repairHitpoints = this.getMaximumHitPoints() / PREMIUM_OR_ARGE_HITPOINTS_REPAIR_TIME_SECONDS;
	                        this.healEntity(repairHitpoints, HEAL_HITPOINTS);
	                    } else {
	                        final int repairHitpoints = this.getMaximumHitPoints() / HITPOINTS_REPAIR_TIME_SECONDS;
	                        this.healEntity(repairHitpoints, HEAL_HITPOINTS);
	                    }
                    } else {
                        final int repairHitpoints = this.getMaximumHitPoints() / HITPOINTS_REPAIR_TIME_SECONDS;
                        this.healEntity(repairHitpoints, HEAL_HITPOINTS);
                    }
                } else if (this.isRepairBotActivated()) {
                    this.setRepairBotActivated(false);
                }
                this.setLastRepairBotRepairTime(currentTime);
            }
        } else if (this.isRepairBotActivated()) {
            this.setRepairBotActivated(false);
        }
      } else {
    	  this.setRepairBotActivated(false);
      }
    }

    private void checkShieldPointsRepair() {
        final long currentTime = System.currentTimeMillis();
        if ((currentTime - this.getLastDamagedTime()) >= SHIELD_REPAIR_DELAY_AFTER_ATTACK) {
            if ((currentTime - this.getLastShieldSkillRepairTime()) >= SHIELD_REPAIR_RATE) {
                if (this.getCurrentShieldPoints() != this.getMaximumShieldPoints()) {                
                    if (this instanceof Player) {
                    	final Player player = (Player) this;
                        if(player.getAccount().getDroneManager().getSelectedFormation() == 9) {
                    		//oyuncu güve kullanıyorsa kalkan yenilemez
                    	} else {
                            if (!this.isShieldSkillActivated()) {
                                this.setShieldSkillActivated(true);
                            }
                            final int repairShield = this.getMaximumShieldPoints() / SHIELD_REPAIR_TIME_SECONDS;
                            this.changeCurrentShieldPoints(+repairShield);
                        ((Player) this).sendCommandToBoundSessions(this.getShieldUpdateCommand());
                    	}
                    }   
                } else if (this.isShieldSkillActivated()) {
                    this.setShieldSkillActivated(false);
                }
                this.setLastShieldSkillRepairTime(currentTime);
            }
        } else if (this.isShieldSkillActivated()) {
            this.setShieldSkillActivated(false);
        }
    }

    public void damageEntity() {
        final long currentTime = System.currentTimeMillis();
        if ((currentTime - this.getLastCheckedDamageTime()) >= CHECK_DAMAGE_DELAY) {
            if (this.mHitPointsDamageCollector > 0) {
                int damageShd, damageHp;

                final double shieldAbsorb = (double) this.getCurrentShieldAbsorb() / 100;

                if ((this.getCurrentShieldPoints() - this.mHitPointsDamageCollector) >= 0) {
                    damageShd = (int) (this.mHitPointsDamageCollector * shieldAbsorb);//500 * 0,8 = 400
                    damageHp = this.mHitPointsDamageCollector - damageShd;//500 - 400
                } else {
                    final int newDamage =
                            this.mHitPointsDamageCollector - this.getCurrentShieldPoints();//500 - 200 = 300
                    damageShd = this.getCurrentShieldPoints();//200
                    damageHp = (int) (newDamage + (damageShd * shieldAbsorb));//300 + (200 * 0,8) = 460
                }

                if ((this.getCurrentHitPoints() - damageHp) < 0) {
                    damageHp = this.getCurrentHitPoints();
                }

                this.changeCurrentHitPoints(-damageHp);
                this.changeCurrentShieldPoints(-damageShd);

                final AttackHitCommand attackHitCommand =
                        new AttackHitCommand(new AttackTypeModule(AttackTypeModule.LASER), 0,//attackerID
                                             this.getMapEntityId(), this.getCurrentHitPoints(),
                                             this.getCurrentShieldPoints(), this.getCurrentNanoHull(),
                                             this.mHitPointsDamageCollector, false);
                
                this.sendCommandToInRange(attackHitCommand);
                
                if (this instanceof Player) {
                    final Player player = (Player) this;
                    player.sendCommandToBoundSessions(attackHitCommand);
                }

                this.mHitPointsDamageCollector = 0;

                this.setLastCheckedDamageTime(currentTime);
                this.setLastDamagedTime(currentTime);
            }
        }
    }

    public void addHitPointsDamage(final MovableMapEntity pMovableMapEntity, final int damage) {
        this.mHitPointsDamageCollector += damage;
        this.receivedAttack(pMovableMapEntity);
    }

    public int damageShieldPoints(final MovableMapEntity pMovableMapEntity, int pDamage) {
        if ((this.getCurrentShieldPoints() - pDamage) < 0) {
            pDamage = this.getCurrentShieldPoints();
        }
        final long currentTime = System.currentTimeMillis();
        this.setLastDamagedTime(currentTime);
        this.changeCurrentShieldPoints(-pDamage);
        this.receivedAttack(pMovableMapEntity);
        
        final AttackHitCommand attackHitCommand =
                new AttackHitCommand(new AttackTypeModule(AttackTypeModule.LASER), 0,//attackerID
                                     this.getMapEntityId(), this.getCurrentHitPoints(),
                                     this.getCurrentShieldPoints(), this.getCurrentNanoHull(),
                                     pDamage, false);
        
        this.sendCommandToInRange(attackHitCommand);
       
        if (this instanceof Player) {
            final Player player = (Player) this;
            player.sendCommandToBoundSessions(attackHitCommand);
            
        }
        return pDamage;
    }

    public void healEntity(int pHeal, final String pHealType) {
        String healString = "0|A|HL|" + this.getMapEntityId() + "|" + this.getMapEntityId() + "|" + pHealType + "|";
        if (pHealType == HEAL_HITPOINTS) {
            pHeal = this.changeCurrentHitPoints(+pHeal);
            healString += this.getCurrentHitPoints();
        } else if (pHealType == HEAL_SHIELD) {
            pHeal = this.changeCurrentShieldPoints(+pHeal);
            healString += this.getCurrentShieldPoints();
        }
        healString += "|" + pHeal;
        //TODO send only to locked players
        if (this instanceof Player) {
        	final Player thisPlayer = (Player) this;
        	if(!thisPlayer.getAccount().isCloaked()) {
        		this.sendPacketToInRange(healString);
        	} 
            thisPlayer.sendPacketToBoundSessions(healString);
        }
    }

    public long getLastCheckedDamageTime() {
        return mLastCheckedDamageTime;
    }

    public void setLastCheckedDamageTime(final long pLastCheckedDamageTime) {
        mLastCheckedDamageTime = pLastCheckedDamageTime;
    }

    public long getLastDamagedTime() {
        return mLastDamagedTime;
    }

    public void setLastDamagedTime(final long pLastDamagedTime) {
        mLastDamagedTime = pLastDamagedTime;
    }

    public boolean isShieldSkillActivated() {
        return mIsShieldSkillActivated;
    }

    public void setShieldSkillActivated(final boolean pShieldSkillActivated) {
        mIsShieldSkillActivated = pShieldSkillActivated;
        if (pShieldSkillActivated) {
            if (this instanceof Player) {
                ((Player) this).sendCommandToBoundSessions(
                        new AttributeSkillShieldUpdateCommand(SHIELD_DEFAULT_ID, 1, 0));
            }
        } else {
            if (this instanceof Player) {
                ((Player) this).sendCommandToBoundSessions(new AttributeSkillShieldUpdateCommand(0, 0, 0));
            }
        }
    }

    public long getLastShieldSkillRepairTime() {
        return mLastShieldSkillRepairTime;
    }

    public void setLastShieldSkillRepairTime(final long pLastShieldSkillRepairTime) {
        mLastShieldSkillRepairTime = pLastShieldSkillRepairTime;
    }

    public long getLastRepairBotRepairTime() {
        return mLastRepairBotRepairTime;
    }

    public void setLastRepairBotRepairTime(final long pLastRepairBotRepairTime) {
        mLastRepairBotRepairTime = pLastRepairBotRepairTime;
    }

    public boolean isRepairBotActivated() {
        return mIsRepairBotActivated;
    }

    public void setRepairBotActivated(final boolean pIsRepairBotActivated) {
        mIsRepairBotActivated = pIsRepairBotActivated;
        if (this instanceof Player) {
            final Player player = (Player) this;
            player.sendCommandToBoundSessions(player.getBeaconCommand());
        }
    }
}