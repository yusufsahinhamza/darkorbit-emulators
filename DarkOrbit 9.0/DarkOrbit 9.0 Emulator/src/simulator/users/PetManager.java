package simulator.users;

import java.util.List;
import java.util.Random;
import java.util.Vector;
import simulator.logic.LaserAttack;
import simulator.logic.PetMovement;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.PetActivationCommand;
import simulator.netty.serverCommands.PetGearRemoveCommand;
import simulator.netty.serverCommands.PetGearTypeModule;
import simulator.netty.serverCommands.AttackLaserRunCommand;
import simulator.netty.serverCommands.MoveCommand;
import simulator.netty.serverCommands.PetGearAddCommand;
import simulator.netty.serverCommands.PetGearSelectCommand;
import simulator.netty.serverCommands.PetHeroActivationCommand;
import simulator.netty.serverCommands.PetStatusCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.netty.serverCommands.class_365;
import simulator.system.SpaceMap;
import simulator.system.clans.Diplomacy;
import simulator.system.ships.PlayerShip;
import simulator.system.ships.ShipFactory;
import storage.SpaceMapStorage;
import utils.MathUtils;
import utils.Tools;

/**
 Created by LEJYONER on 09/01/2018.
 */

public class PetManager
        extends AbstractAccountInternalManager {
	
    private int petID;
    private int currentPositionX = 0;
    private int currentPositionY = 0;    
    private long lastShotTime;
    private boolean isActivated = false;    
    private boolean guardModeActivated = false;
    private boolean kamikazeActivated = false;
    
    private long comboShipRepairCooldownTime    = 0L;
    private long comboShipRepairEffectTime = 0L;
    private long lastComboShipRepairTime = 0L;
    private boolean comboShipRepairEffectActivated    = false;
    
    private int maximumHitPoints;
    private int currentHitPoints;
    private int maximumShieldPoints = 50000;
    private int currentShieldPoints = 50000;
    
    MovableMapEntity lockedObject = null;
    
    public PetManager(final Account pAccount) {
        super(pAccount);
        Random randomGenerator = new Random();
		this.setPetID(1 + randomGenerator.nextInt(999999999));
		
		final PlayerShip pet = ShipFactory.getPlayerShip(22);
        this.setMaximumHitPoints(pet.getBaseHitPoints());
        this.setCurrentHitPoints(pet.getBaseHitPoints());
    }
    
    public void onTickCheckMethods() {
    	if(this.isActivated()) {
    		this.generateNextMovement();
	        this.checkGuardMode();
	        this.checkComboShipRepair();
	       // this.checkKamikaze();
	        //this.getMovement().move();
	        //this.getAccount().getPlayer().sendPacketToBoundSessions("0|A|STD|X: "+this.getCurrentPositionX()+" - Y: "+this.getCurrentPositionY()+"");
    	}
    }            
    
    public void checkKamikaze() {
    	final Player player = this.getAccount().getPlayer();
    	final long currentTime = System.currentTimeMillis();
    	
    	if(this.kamikazeActivated()) {
    		final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(player.getCurrentSpaceMapId());
    		
    		for(final MovableMapEntity inRangeEntity : spaceMap.getAllMovableMapEntities().values()) {
    			if(inRangeEntity != null) {  				
    				if((currentTime - player.getLastDamagedTime() >= 0)) { 					
    					if ((Math.abs(player.getCurrentPositionX() - inRangeEntity.getCurrentPositionX()) <= SpaceMap.VISIBILITY_RANGE) &&
    	                        (Math.abs(player.getCurrentPositionY() - inRangeEntity.getCurrentPositionY()) <= SpaceMap.VISIBILITY_RANGE)) {
					
    						this.getMovement().initiate(this.getCurrentPositionX(), this.getCurrentPositionY(), inRangeEntity.getCurrentPositionX(), inRangeEntity.getCurrentPositionY(), player.getSpeed());
						
	    					if ((Math.abs(this.getCurrentPositionX() - inRangeEntity.getCurrentPositionX()) <= 50) &&
	    	                        (Math.abs(this.getCurrentPositionY() - inRangeEntity.getCurrentPositionY()) <= 50)) {
	    						
	    						player.sendPacketToBoundSessions("0|A|STD|1");
	    						
	    					}    					    					
    					}   					
    				}
    			}
    		}
    	}
    }
    
    public void checkGuardMode() {
    	final Player player = this.getAccount().getPlayer();
    	final long currentTime = System.currentTimeMillis();
    	
    	if(this.guardModeActivated()) {
    		if(player.getLaserAttack().isAttackInProgress() || !player.getRocketAttack().checkAttackTime() || !player.getRocketLauncherAttack().checkAttackTime() || (currentTime - player.getLastDamagedTime() >= 0)) {
    	
    			final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(player.getCurrentSpaceMapId());
    			
    	    	for(final MovableMapEntity inRangeEntity : spaceMap.getAllMovableMapEntities().values()) {
    	    		if(inRangeEntity != null) {
    	    			if(inRangeEntity.getCurrentSpaceMapId() == player.getCurrentSpaceMapId()) {
	    	    			if(player.getLockedTarget() != null) {
	    	    	    		final MovableMapEntity lockedObject = (MovableMapEntity) player.getLockedTarget();
	    	    	    		
	    	    	    		if(lockedObject instanceof Player) {
	    	    	    			if(player.getLaserAttack().isAttackInProgress() || !player.getRocketAttack().checkAttackTime() || !player.getRocketLauncherAttack().checkAttackTime() || (lockedObject.getLaserAttack().isAttackInProgress() && lockedObject.getLockedTarget() == player)) {
		    	    	    			this.setLockedObject(lockedObject);
		    	    	    			this.Attack(lockedObject);	    
	    	    	    			}
	    	    	    		} else {
		    	    	    		if(lockedObject.getLaserAttack().isAttackInProgress() && lockedObject.getLockedTarget() == player) { 
		    	    	    			this.setLockedObject(lockedObject);
		    	    	    			this.Attack(lockedObject);
		    	    	    		}
	    	    	    		}
	    	    	    		
	    	    	    	} else {    			
	    		    			final MovableMapEntity objects = (MovableMapEntity) inRangeEntity;
	    		    				
	    		    	    	if(objects.getLaserAttack().isAttackInProgress() && objects.getLockedTarget() == player) {	
	    		    	    		this.setLockedObject(objects);
	    		    	    		this.Attack(objects);
	    		    	    	}	    	
	    	    	    	}
    	    			}
    	    		}
    	    	}        
    		}
    	}
    }
    
    private boolean comboShipRepairActivated() {
        return (System.currentTimeMillis() - this.comboShipRepairCooldownTime()) < 0;
    }
    
    public void checkComboShipRepair() {
        final Player player = this.getAccount().getPlayer();
		final long currentTime = System.currentTimeMillis();
		
		if(this.comboShipRepairActivated()) {	
			if (currentTime - this.comboShipRepairEffectTime() <= 0) {
				if(!player.getLaserAttack().isAttackInProgress() || player.getRocketAttack().checkAttackTime() || player.getRocketLauncherAttack().checkAttackTime() || !(currentTime - player.getLastDamagedTime() >= 0)) {
					if ((currentTime - this.lastComboShipRepairTime()) >= 1000) {
						int heal = 20000;   
						player.healEntity(heal, player.HEAL_HITPOINTS);
						this.setLastComboShipRepairTime(currentTime);
					}
				}
			}
		} else if (this.comboShipRepairEffectActivated()) {
			player.sendPacketToBoundSessions("0|A|STD|Kombo Gemi Tamiri modülün tekrar yüklenmesi sona erdi.. Modülü tekrar seçebilirsin.");
	    	player.sendCommandToBoundSessions(new PetGearRemoveCommand(new simulator.netty.serverCommands.PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0));
	    	player.sendCommandToBoundSessions(new PetGearAddCommand(new simulator.netty.serverCommands.PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0, true));
			this.setComboShipRepairEffectActivated(false);
		}
    }
    
    public void Activate() { 
		final Player player = this.getAccount().getPlayer();
		
		if(!this.isActivated()) {
			this.setIsActivated(true);
			
			player.sendPacketToBoundSessions("0|A|STM|msg_pet_activated");
			player.sendCommandToBoundSessions(new PetStatusCommand(this.getPetID(), 15, 27000000, 27000000, this.getCurrentHitPoints(), this.getMaxHitPoints(), this.getCurrentShieldPoints(), this.getMaxShieldPoints(), 50000, 50000, player.getSpeed(), this.getAccount().getPetName() != null ? this.getAccount().getPetName() : "PET.10"));
			player.sendCommandToBoundSessions(new PetGearAddCommand(new PetGearTypeModule(PetGearTypeModule.PASSIVE), 0, 0, true));
			player.sendCommandToBoundSessions(new PetGearAddCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), 0, 0, true));  
			//player.sendCommandToBoundSessions(new PetGearAddCommand(new PetGearTypeModule(PetGearTypeModule.KAMIKAZE), 0, 0, true));  
			
			final long currentTime = System.currentTimeMillis();
			if(currentTime - this.comboShipRepairCooldownTime() >= 0) {
				player.sendCommandToBoundSessions(new PetGearAddCommand(new PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0, true));  
			} else {
				player.sendCommandToBoundSessions(new PetGearAddCommand(new PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0, false));
			}
	        player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.PASSIVE), new Vector<Integer>()));
			player.sendCommandToBoundSessions(new PetHeroActivationCommand(this.getAccount().getUserId(), this.getPetID(), (short) 22, 3, this.getAccount().getPetName() != null ? this.getAccount().getPetName() : "PET.10", (short) this.getAccount().getFactionId(), this.getAccount().getClanId(), 15, this.getAccount().getClanTag(), player.getCurrentPositionX(), player.getCurrentPositionY(), player.getSpeed(), new class_365(class_365.DEFAULT)));
			
			for(final MovableMapEntity inRangeEntity : player.getInRangeMovableMapEntities()) {
				if(inRangeEntity != null) {
					if(inRangeEntity instanceof Player) {
						final Player thisPlayer = player;
						final Player otherPlayer = (Player) inRangeEntity;
                        short relationType = 0;
                        
                        if(thisPlayer.getAccount().getClan() != null && otherPlayer.getAccount().getClan() != null) {
                            final List<Diplomacy> dips = otherPlayer.getAccount().getClan().getDiplomacies();
                            int thisPlayerClanID = thisPlayer.getAccount().getClanId();
                            for(Diplomacy dip : dips){
                                if(dip.clanID1 == thisPlayerClanID || dip.clanID2 == thisPlayerClanID){
                                	relationType = (short) dip.relationType;
                                }
                            }                            
                        }  
                        
                        otherPlayer.sendCommandToBoundSessions(new PetActivationCommand(player.getAccount().getUserId(), this.getPetID(), 22, 3, this.getAccount().getPetName() != null ? this.getAccount().getPetName() : "PET.10", this.getAccount().getFactionId(), this.getAccount().getClanId(), 15, this.getAccount().getClanTag(), new ClanRelationModule(relationType), player.getCurrentPositionX(), player.getCurrentPositionY(), player.getSpeed(), false, true, new class_365(class_365.DEFAULT)));
					}
				}				
			}
			
			/**
			player.sendCommandToBoundSessions(new PetGearAddCommand(new PetGearTypeModule(PetGearTypeModule.ENEMY_LOCATOR), 0, 0, true));  
			Vector<Integer> ships = new Vector<Integer>();
			ships.add(1);
			ships.add(2);
			ships.add(3);
			ships.add(4);
			ships.add(5);
			player.sendCommandToBoundSessions(new PetLocatorGearInitializationCommand(new PetGearTypeModule(PetGearTypeModule.ENEMY_LOCATOR), ships));  
			*/
						
		} else {
			this.Deactivate();
		}
        	
    }
    
    public void Deactivate() {
    	final Player player = this.getAccount().getPlayer();

    	if(this.checkAttackTime()) {
    		player.sendPacketToBoundSessions("0|A|STM|msg_pet_deactivated");
    		
	    	player.sendPacketToBoundSessions("0|PET|D");
	    	
	    	this.setIsActivated(false);
	    	this.setGuardModeActivated(false);
	    	
	    	final ShipRemoveCommand shipRemoveCommand = new ShipRemoveCommand(this.getPetID());
	        player.sendCommandToInRange(shipRemoveCommand); 
	        player.sendCommandToBoundSessions(shipRemoveCommand);
    	} else {
    		player.sendPacketToBoundSessions("0|A|STM|msg_pet_in_combat");
    	}
    }
    
    public void PassiveMode() {
    	final Player player = this.getAccount().getPlayer();
    	
    	this.setGuardModeActivated(false);
    	this.setKamikazeActivated(false);
    	player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.PASSIVE), new Vector<Integer>()));
    }
    
    public void GuardMode() {
    	final Player player = this.getAccount().getPlayer();
    	
    	if(this.isActivated()) {
    		this.setGuardModeActivated(true);
    		player.sendPacketToBoundSessions("0|A|STM|msg_pet_guard_mode_activated");
    		player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), new Vector<Integer>()));
    	} else {
    		this.setGuardModeActivated(true);
    		this.Activate();
    		player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), new Vector<Integer>()));
    	}
    }
    
    public void ComboShipRepair() {
    	final Player player = this.getAccount().getPlayer();
    	final long currentTime = System.currentTimeMillis();
    	
    	if(this.isActivated()) {
    		if (currentTime - this.comboShipRepairCooldownTime() >= 0) {
	    		if (player.getCurrentHitPoints() != player.getMaximumHitPoints()) {
		    		if ((currentTime - player.getLastDamagedTime()) >= 10000) {
		            	this.setComboShipRepairEffectActivated(true);
		            	this.setComboShipRepairEffectTime(currentTime + 5000);	            	
		                this.setComboShipRepairCooldownTime(currentTime + 5000 + 15000);
		                player.sendPacketToBoundSessions("0|A|STM|msg_pet_ship_repair_gear_activated");
		                player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), new Vector<Integer>()));
		            	player.sendCommandToBoundSessions(new PetGearRemoveCommand(new simulator.netty.serverCommands.PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0));
		            	player.sendCommandToBoundSessions(new PetGearAddCommand(new simulator.netty.serverCommands.PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0, false));
		    		}
	    		} else {
	    			this.GuardMode();
	    			player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), new Vector<Integer>()));
	    		}
    		} else {
    			this.GuardMode();
    			player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), new Vector<Integer>()));
    		}
    	} else {
    		this.Activate();
    		if (currentTime - this.comboShipRepairCooldownTime() >= 0) {
	    		if (player.getCurrentHitPoints() != player.getMaximumHitPoints()) {
		    		if ((currentTime - player.getLastDamagedTime()) >= 10000) {
		            	this.setComboShipRepairEffectActivated(true);
		            	this.setComboShipRepairEffectTime(currentTime + 5000);	            	
		                this.setComboShipRepairCooldownTime(currentTime + 5000 + 20000);
		                player.sendPacketToBoundSessions("0|A|STM|msg_pet_ship_repair_gear_activated");
		                player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), new Vector<Integer>()));
		            	player.sendCommandToBoundSessions(new PetGearRemoveCommand(new simulator.netty.serverCommands.PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0));
		            	player.sendCommandToBoundSessions(new PetGearAddCommand(new simulator.netty.serverCommands.PetGearTypeModule(PetGearTypeModule.COMBO_SHIP_REPAIR), 0, 0, false));
		    		}
	    		} else {
	    			this.GuardMode();
	    			player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), new Vector<Integer>()));
	    		}
    		} else {
    			this.GuardMode();
    			player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.GUARD), new Vector<Integer>()));
    		}
    	}
    }
    
    public void KamikazeModule() {
    	final Player player = this.getAccount().getPlayer();
    	
    	if(this.isActivated()) {
    		this.setKamikazeActivated(true);
    		player.sendPacketToBoundSessions("0|A|STM|msg_pet_guard_mode_activated");
    		player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.KAMIKAZE), new Vector<Integer>()));
    	} else {
    		this.setKamikazeActivated(true);
    		this.Activate();
    		player.sendCommandToBoundSessions(new PetGearSelectCommand(new PetGearTypeModule(PetGearTypeModule.KAMIKAZE), new Vector<Integer>()));
    	}
    	
    }
    
    /**
         private void generateNextMovement() { 
    	final Player player = this.getAccount().getPlayer();   
    	this.getMovement().initiate(this.getCurrentPositionX(),
                  this.getCurrentPositionY(), player.getCurrentPositionX(),
                  player.getCurrentPositionY(), player.getSpeed());
    }
     */
    
    private void generateNextMovement() { 
    	final Player player = this.getAccount().getPlayer();   
        int petx = player.getCurrentPositionX() + 100;
        int pety = player.getCurrentPositionY() - 100;          
        player.sendCommandToBoundSessions(new MoveCommand(this.getPetID(), petx, pety, this.getDuration()));
        player.sendCommandToInRange(new MoveCommand(this.getPetID(), petx, pety, this.getDuration()));        
    	this.UpdateXy(petx, pety);
    }
     
    private void UpdateXy(int a, int b) {
        int newPosX = a;
        int newPosY = b;
        int oldx = this.getCurrentPositionX();
        int oldy = this.getCurrentPositionY();
        int distance = (int) Math.sqrt(Math.pow(oldx - newPosX, 2.0D) + Math.pow(oldy - newPosY, 2.0D));
        int speed = (int) (300 * 0.37D);
        int dir = (int) (Math.atan2(newPosY - oldy, newPosX - oldx) * 180.0D / (Math.PI));

        if (distance >= speed) {
            oldx += (speed * Math.cos((Math.PI * dir) / 180));
            oldy += (speed * Math.sin((Math.PI * dir) / 180));
            this.setPositionXY(oldx, oldy);
        } else {
            this.setPositionXY(newPosX, newPosY);
        }
    }
    
    private int getDuration() {
        final int distanceX = this.getAccount().getPlayer().getCurrentPositionX() - this.getCurrentPositionX();
        final int distanceY = this.getAccount().getPlayer().getCurrentPositionY() - this.getCurrentPositionY();

        //distance to travel
        final double distance = MathUtils.hypotenuse(distanceX, distanceY);

        return (int) (1000.0D * distance / this.getAccount().getPlayer().getSpeed());
    }
    
    public void Attack(final MovableMapEntity pObject) {
    	final Player player = this.getAccount().getPlayer();
    	
        if (MathUtils.hypotenuse(
                this.getCurrentPositionX() - pObject.getCurrentPositionX(),
                this.getCurrentPositionY() - pObject.getCurrentPositionY()) <=
            LaserAttack.ATTACK_RANGE) {
        	
        	if(this.checkAttackTime()) {
        		if(!pObject.isInSecureZone()) {
	        		this.updateLastShotTime();
	        		
                    final String cloakPacket = "0|n|INV|"+ this.getPetID() +"|0";
                    player.sendPacketToBoundSessions(cloakPacket);
                    player.sendPacketToInRange(cloakPacket);
                    
	    			final AttackLaserRunCommand laserRunCommand = this.createAttackLaserRunCommand(pObject.getMapEntityId());
	    			player.sendCommandToBoundSessions(laserRunCommand);
	    			player.sendCommandToInRange(laserRunCommand);
	    			
	    			if(pObject instanceof Player && !pObject.canBeShoot()) return;
	    			
	    			int damage = Tools.getRandomDamage(player.getLaserAttack().getDamageMultiplier() * 2500);
	    			
	        		if(pObject.getCurrentHitPoints() <= 0 || damage >= pObject.getCurrentHitPoints()) {
	        			if(!pObject.isDestroyed()) {
	        				pObject.destroy(player);
	        			}
	        		}
	            	
	        		pObject.addHitPointsDamage(player, damage);   
        		}
        	}       	
        }
    }
    
    public AttackLaserRunCommand createAttackLaserRunCommand(final int pUserID) {
    	final Player player = this.getAccount().getPlayer();
        return new AttackLaserRunCommand(this.getPetID(), pUserID,
        		                         player.getLaserAttack().getSelectedLaser(), true, false);
    }
    
    public boolean checkAttackTime() {
    	final Player player = this.getAccount().getPlayer();
        final long currentTime = System.currentTimeMillis();
        
        if (player.getLaserAttack().getSelectedLaserItem()
                				   .equalsIgnoreCase(AmmunitionManager.RSB_75)) {
        	return (currentTime - this.getLastShotTime()) >= LaserAttack.RSB_COOLDOWN_TIME;
        }
        return (currentTime - this.getLastShotTime()) >= LaserAttack.TIME_BETWEEN_LASER_SHOTS;
    }
    
    public PetMovement getMovement() {
    	return this.getAccount()
    			   .getMovement();
    }
    
    public void setMaximumHitPoints(final int pHitPoints) {
    	this.maximumHitPoints = pHitPoints;
    }
    
    public void setCurrentHitPoints(final int pHitPoints) {
    	this.currentHitPoints = pHitPoints;
    }
    
    public int getCurrentHitPoints() {
    	return this.currentHitPoints;
    }
    
    public int getMaxHitPoints() {
    	return this.maximumHitPoints;
    }
    
    public int getCurrentShieldPoints() {
    	return this.currentShieldPoints;
    }
    
    public int getMaxShieldPoints() {
    	return this.maximumShieldPoints;
    }
    
    public long comboShipRepairCooldownTime() {
        return this.comboShipRepairCooldownTime;
    }
    
    public void setComboShipRepairCooldownTime(final long pComboShipRepairCooldownTime) {
    	this.comboShipRepairCooldownTime = pComboShipRepairCooldownTime;
    }
    
    public boolean comboShipRepairEffectActivated() {
        return this.comboShipRepairEffectActivated;
    }

    public void setComboShipRepairEffectActivated(final boolean pComboShipRepairEffectActivated) {
    	this.comboShipRepairEffectActivated = pComboShipRepairEffectActivated;
    }
    
    public long comboShipRepairEffectTime() {
        return this.comboShipRepairEffectTime;
    }

    public void setComboShipRepairEffectTime(final long pComboShipRepairEffectTime) {
        this.comboShipRepairEffectTime = pComboShipRepairEffectTime;
    }
    
    public long lastComboShipRepairTime() {
        return this.lastComboShipRepairTime;
    }

    public void setLastComboShipRepairTime(final long pLastComboShipRepairTime) {
    	this.lastComboShipRepairTime = pLastComboShipRepairTime;
    }
    
    public long getLastShotTime() {
        return this.lastShotTime;
    }
    
    private void updateLastShotTime() {
    	final long currentTime = System.currentTimeMillis();
    	this.lastShotTime = currentTime;
    }
    
    public MovableMapEntity getLockedObject() {
    	return this.lockedObject;
    }
    
    public void setLockedObject(final MovableMapEntity pLockedObject) {
    	this.lockedObject = pLockedObject;
    }
    
    public int getPetID() {
    	return this.petID;
    }
    
    public void setPetID(final int pPetID) {
    	this.petID = pPetID;
    }
    
    public int getCurrentPositionX() {
    	return this.currentPositionX;
    }
    
    public int getCurrentPositionY() {
    	return this.currentPositionY;
    }
    
    public void setPositionXY(final int positionX, final int positionY) {
    	this.currentPositionX = positionX;
    	this.currentPositionY = positionY;
    }
    
    public boolean isActivated() {
    	return this.isActivated;
    }
    
    public void setIsActivated(final boolean pIsActivated) {
    	this.isActivated = pIsActivated;
    }
    
    public boolean guardModeActivated() {
    	return this.guardModeActivated;
    }
    
    public void setGuardModeActivated(final boolean pGuardModeActivated) {
    	this.guardModeActivated = pGuardModeActivated;
    }
    
    public boolean kamikazeActivated() {
    	return this.kamikazeActivated;
    }
    
    public void setKamikazeActivated(final boolean pKamikazeActivated) {
    	this.kamikazeActivated = pKamikazeActivated;
    }
    
	public void setFromJSON(final String pSkillsJSON) {		
	}

	public void setNewAccount() {
	}
	public String packToJSON() {
		return "";
	}
}
