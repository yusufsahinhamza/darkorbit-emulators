package simulator.map_entities.movable;

import org.json.JSONArray;

import java.util.Random;
import java.util.Vector;
import java.util.concurrent.ConcurrentHashMap;

import simulator.ia.AIOption;
import simulator.ia.AlienAI;
import simulator.ia.CubikonAI;
import simulator.ia.IArtificialInteligence;
import simulator.ia.IceMeteoroidAI;
import simulator.ia.ProtegitAI;
import simulator.map_entities.AttackableMapEntity;
import simulator.map_entities.Lockable;
import simulator.map_entities.MapEntity;
import simulator.netty.serverCommands.AttributeHitpointUpdateCommand;
import simulator.netty.serverCommands.AttributeShieldUpdateCommand;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.ShipCreateCommand;
import simulator.netty.serverCommands.ShipDestroyCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.netty.serverCommands.class_365;
import simulator.system.SpaceMap;
import simulator.system.ships.AlienShip;
import simulator.system.ships.ShipFactory;
import simulator.system.ships.ShipsConstants;
import utils.Settings;
import utils.Tools;

/**
 Alien class is responsible for holding alien on-map state
 */
public class Alien
        extends AttackableMapEntity {

    private static final int ALIEN_DAMAGE_MIN = 0;

    private static final float ALIEN_POWER_MULTIPLIER_MIN     = 0.1f; // why would we create weaker aliens
    private static final float ALIEN_POWER_MULTIPLIER_DEFAULT = 1.0f;

    private final AlienShip mAlienShip;

    private Lockable mLockedTarget;

    private int mCurrentHitPoints;
    private int mCurrentShieldPoints;
    private int mCurrentSpeed;
    private int mCurrentDamage;

    // modifies aliens stats (HP, SHD, DMG) by multiplying by this value
    // used to create 0.5x, 2x, 3x etc powerful aliens
    private float mCurrentPowerMultiplier = ALIEN_POWER_MULTIPLIER_DEFAULT;

    private IArtificialInteligence mArtificialInteligence;
    private boolean mIsDestroyed = false;
    public final ConcurrentHashMap<Integer, Player> mShooterPlayers = new ConcurrentHashMap<>();
    
    // ==============================================================

    public Alien(final int pShipId, final SpaceMap pCurrentSpaceMapId) {
        super(pCurrentSpaceMapId);

        this.mAlienShip = ShipFactory.getAlienShip(pShipId);

        this.setDefaultAttributes();

        if (this.getAlienShip()
                .getShipId() == ShipsConstants.CUBIKON_ID) {
            this.mArtificialInteligence = new CubikonAI(this);
        } else if(this.getAlienShip()
                .getShipId() == ShipsConstants.ICY_METEOROID_ID) { 
        	this.mArtificialInteligence = new IceMeteoroidAI(this);
        } else {
            this.mArtificialInteligence = new AlienAI(this);
        }
    }

    public int constantPositionX = 0;
    public int constantPositionY = 0;
    public Alien(final int pShipId, final SpaceMap pCurrentSpaceMapId, int positionX, int positionY) {
        super(pCurrentSpaceMapId);

        this.mAlienShip = ShipFactory.getAlienShip(pShipId);

        this.setDefaultAttributes();
        this.setCurrentPositionX(positionX);
        this.setCurrentPositionY(positionY);
        
        if (this.getAlienShip()
                .getShipId() == ShipsConstants.CUBIKON_ID) {
            this.mArtificialInteligence = new CubikonAI(this);
        } else if(this.getAlienShip()
                .getShipId() == ShipsConstants.ICY_METEOROID_ID) { 
        	this.mArtificialInteligence = new IceMeteoroidAI(this);
        } else {
            this.mArtificialInteligence = new AlienAI(this);
        }
    }
    
    public Alien(final int pShipId, final SpaceMap pCurrentSpaceMapId, final MovableMapEntity pCubikonEntity) {
        super(pCurrentSpaceMapId);

        this.mAlienShip = ShipFactory.getAlienShip(pShipId);

        this.setDefaultAttributes();
        this.setCurrentPositionX(pCubikonEntity.getCurrentPositionX());
        this.setCurrentPositionY(pCubikonEntity.getCurrentPositionY());

        this.mArtificialInteligence = new ProtegitAI(this, pCubikonEntity);
    }

    // ==============================================================

    public void setDefaultAttributes(final boolean pSetPowerMultiplier) {

        final int x = Tools.sRandom.nextInt(20000);
        final int y = Tools.sRandom.nextInt(12800);
        final int mapID = getCurrentSpaceMapId();
        final int multiplier = (mapID == 29 || mapID == 91 || mapID == 93) ? 2 : 1;

        this.setCurrentPositionX(x * multiplier);
        this.setCurrentPositionY(y * multiplier);
        this.setCurrentHitPoints(this.getBaseHitPoints());
        this.setCurrentShieldPoints(this.getBaseShieldPoints());
        this.setCurrentDamage(this.getBaseDamage());
        this.setCurrentSpeed(this.getBaseSpeed());

        if (pSetPowerMultiplier) {
            this.setCurrentPowerMultiplier(this.getCurrentPowerMultiplier());
        }

    }

    public boolean isDestroyed() {
        return this.mIsDestroyed;
    }

    public void setDestroyed(final boolean pIsDestroyed) {
        this.mIsDestroyed = pIsDestroyed;
    }
    
    public void setDefaultAttributes() {
        this.setDefaultAttributes(false);
    }

    // ==============================================================

    /**
     @return Alien map entity ID of this NPC
     */
    public int getAlienId() {
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
    //    Log.p("Alien Attack Received");
        this.getLaserAttack()
            .initiate(pMovableMapEntity);
        this.mArtificialInteligence.receivedAttack(pMovableMapEntity);
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

    private String generateHash() {
        String alphabet = new String("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"); //9
        int n = alphabet.length(); //10

        String result = new String();
        Random r = new Random(); //11

        for (int i = 0; i < 15; i++) {
            result = result + alphabet.charAt(r.nextInt(n)); //13
        }

        return result;
    }       
    
    private Thread mProtegitThread;
    
    public void removeProtegits() {
    	if(this.getAlienShip().getShipId() == 80) {
        	if (this.mProtegitThread == null || !this.mProtegitThread.isAlive()) {
        		this.mProtegitThread = new Thread() {
                    public void run() {
                        try {
                            int i = 0;
                            while (true) {
                                if (i >= 15) {                              	
                                    for (Alien protegits : ((CubikonAI) mArtificialInteligence).mProtegits.values()) {
                                    	final ShipRemoveCommand shipRemoveCommand = new ShipRemoveCommand(protegits.getMapEntityId());
                                    	final SpaceMap spaceMap = getCurrentSpaceMap();
                                    	protegits.setLockedTarget(null);
                                    	protegits.getLaserAttack().setAttackInProgress(false);
                                        spaceMap.removeAlien(protegits.getAlienId());
                                        
                                        for (MovableMapEntity movableMapEntity : protegits.getInRangeMovableMapEntities()) {
                                        	if(movableMapEntity != null) {
                                        		if(movableMapEntity instanceof Player) {
                                        			((Player) movableMapEntity).sendCommandToBoundSessions(shipRemoveCommand);
                                        		}
                                        	}
                                        }
                                	}                                    
                                    break;
                                }
                                Thread.sleep(1000);
                                i++;
                            }
                        } catch (InterruptedException e) {
                            
                        }
                    }
                };
                this.mProtegitThread.start();
            }
    	}
    }
    
    private Thread mCubikonThread;
    
    public void createCubikon() {
    	if (this.mCubikonThread == null || !this.mCubikonThread.isAlive()) {
    		this.mCubikonThread = new Thread() {
                public void run() {
                    try {
                        int i = 0;
                        while (true) {
                            if (i >= 60) {
                    	        final SpaceMap spaceMap = getCurrentSpaceMap();
                    	        spaceMap.addCubikon(constantPositionX, constantPositionY);
                                break;
                            }
                            Thread.sleep(1000);
                            i++;
                        }
                    } catch (InterruptedException e) {
                        
                    }
                }
            };
            this.mCubikonThread.start();
        }
    }
    
    @Override
    public void destroy(final MapEntity pKillerMapEntity) {  
    	this.setDestroyed(true);    	
		
        for (MovableMapEntity movableMapEntity : this.getInRangeMovableMapEntities()) {
        	if(movableMapEntity != null) {
        		if(movableMapEntity instanceof Player) {
        			final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(this.getMapEntityId(), 1);
        			((Player) movableMapEntity).sendCommandToBoundSessions(shipDestroyCommand);
        		}
        	}
        }
        
        if(this.getAlienShip().getShipId() != 80 && this.getAlienShip().getShipId() != 81) {
	        final SpaceMap spaceMap = this.getCurrentSpaceMap();
	        spaceMap.removeAlien(this.getAlienId());  
	        final Alien alien = new Alien(this.getAlienShip().getShipId(), this.getCurrentSpaceMap());
	        this.getCurrentSpaceMap()
			.addAlien(alien);
        } else if(this.getAlienShip().getShipId() == 80) {
	        final SpaceMap spaceMap = this.getCurrentSpaceMap();
	        spaceMap.removeAlien(this.getAlienId());
	        this.removeProtegits();
        	this.createCubikon();
        }
        
        if (pKillerMapEntity instanceof Player) {
            final Player killerPlayer = (Player) pKillerMapEntity;
            final int exp = this.getRewardExperience();
            final int hon = this.getRewardHonor();
            final int cre = this.getRewardCredits();
            final int uri = this.getRewardUridium();
            killerPlayer.getAccount()
                        .changeExperience(+exp);
            killerPlayer.getAccount()
                        .changeHonor(+hon);
            killerPlayer.getAccount()
                        .changeCredits(+cre);
            killerPlayer.getAccount()
                        .changeUridium(+uri);
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|EP|" + exp + "|" + killerPlayer.getAccount()
                                                                                           .getExperience() + "|" +
                                                   killerPlayer.getAccount()
                                                               .getLevel());
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|HON|" + hon + "|" + killerPlayer.getAccount()
                                                                                            .getHonor());
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|CRE|" + cre + "|" + killerPlayer.getAccount()
                                                                                            .getCredits());
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|URI|" + uri + "|" + killerPlayer.getAccount()
                                                                                            .getUridium());
            killerPlayer.setLockedTarget(null);
            killerPlayer.getLaserAttack().setAttackInProgress(false);
            
        	Random randomGenerator = new Random(); 
    		float chance = randomGenerator.nextFloat();

    	//	if(this.getAlienShip().getShipId() == 35 || this.getAlienShip().getShipId() == 79) {
    	//		if(chance <= 0.50f) {
    				if(killerPlayer.boxHash == "" && killerPlayer.alienID == 0) {
    					killerPlayer.sendPacketToBoundSessions("0|A|STD|Indoctrine-Oil sandığı düşürdün!");
    			    	String hash = generateHash();    	
    			    	killerPlayer.sendPacketToBoundSessions("0|c|" + hash + "|40|" + this.getCurrentPositionX() + "|" + this.getCurrentPositionY());
    			    	killerPlayer.boxHash = hash;
    			    	killerPlayer.alienID = this.getAlienShip().getShipId();
    		//		}
    		//	}
    		}
        }
                
    }

    public void destroyCubikon() {  	
    	this.setDestroyed(true);    	
		
        for (MovableMapEntity movableMapEntity : this.getInRangeMovableMapEntities()) {
        	if(movableMapEntity != null) {
        		if(movableMapEntity instanceof Player) {
        			final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(this.getMapEntityId(), 1);
        			((Player) movableMapEntity).sendCommandToBoundSessions(shipDestroyCommand);
        		}
        	}
        }
       
	    final SpaceMap spaceMap = this.getCurrentSpaceMap();
	    spaceMap.removeAlien(this.getAlienId());
	    this.removeProtegits();
        this.createCubikon();       
		
        for(final Player player : this.mShooterPlayers.values()) {
        	if(player.getCurrentSpaceMapId() == this.getCurrentSpaceMapId()) {
	            final int exp = this.getRewardExperience();
	            final int hon = this.getRewardHonor();
	            final int cre = this.getRewardCredits();
	            final int uri = this.getRewardUridium() / this.mShooterPlayers.size();
	            player.getAccount().changeExperience(+exp);
	            player.getAccount().changeHonor(+hon);
	            player.getAccount().changeCredits(+cre);
	            player.getAccount().changeUridium(+uri);
	            player.sendPacketToBoundSessions("0|LM|ST|EP|" + exp + "|" + player.getAccount().getExperience() + "|" + player.getAccount().getLevel());
	            player.sendPacketToBoundSessions("0|LM|ST|HON|" + hon + "|" + player.getAccount().getHonor());
	            player.sendPacketToBoundSessions("0|LM|ST|CRE|" + cre + "|" + player.getAccount().getCredits());
	            player.sendPacketToBoundSessions("0|LM|ST|URI|" + uri + "|" + player.getAccount().getUridium());												
				player.setLockedTarget(null);
				player.getLaserAttack().setAttackInProgress(false);
				this.mShooterPlayers.remove(player);
				
		    	Random randomGenerator = new Random(); 
				float chance = randomGenerator.nextFloat();
				if(this.getAlienShip().getShipId() == 80) {
					if(chance <= 0.25f) {
						if(player.boxHash == "" && player.alienID == 0) {
							player.sendPacketToBoundSessions("0|A|STD|Indoctrine-Oil sandığı düşürdün!");
					    	String hash = generateHash();    	
					    	player.sendPacketToBoundSessions("0|c|" + hash + "|40|" + this.getCurrentPositionX() + "|" + this.getCurrentPositionY());
					    	player.boxHash = hash;
					    	player.alienID = this.getAlienShip().getShipId();
						}
					}
				}
        	}
        }
        
    }
    /**
     Note: sets at least {@link Alien#ALIEN_DAMAGE_MIN}

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
        this.getLaserAttack()
            .attack();
        this.damageEntity();
        this.checkPlayerShootingOnSecureZone();
    }

    public void checkPlayerShootingOnSecureZone() {
    	if(this.getAlienShip().getShipId() != 80 && this.getAlienShip().getShipId() != 81) {
        	for(MovableMapEntity inRangeEntity : this.getInRangeMovableMapEntities()) {
        		if(inRangeEntity != null) {
        			if(inRangeEntity instanceof Player) {
        				Player player = (Player) inRangeEntity;
        				if(player.inInSecureZone && (!player.getLaserAttack().isAttackInProgress() && player.getLockedTarget() != this)) {
                        	this.setLockedTarget(null);
                        	((AlienAI) this.mArtificialInteligence).setAIOption(AIOption.SEARCH_FOR_ENEMIES);
        				}
        			}
        		}
        	}
    	}
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
}
