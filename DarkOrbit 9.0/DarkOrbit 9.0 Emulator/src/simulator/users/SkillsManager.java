package simulator.users;

import java.util.Arrays;
import java.util.List;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.AttackHitCommand;
import simulator.netty.serverCommands.AttackTypeModule;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;
import simulator.system.clans.Diplomacy;
import utils.Settings;

/**
 Created by LEJYONER on 10/05/2017.
 */

public class SkillsManager
        extends AbstractAccountInternalManager {

    public static final String SENTINEL_ABILITY            = "ability_sentinel";
    public static final String DIMINISHER_ABILITY          = "ability_diminisher";
    public static final String VENOM_ABILITY               = "ability_venom";
    public static final String SPECTRUM_ABILITY            = "ability_spectrum";
    public static final String SOLACE_ABILITY              = "ability_solace";
    public static final String AEGIS_HP_REPAIR             = "ability_aegis_hp-repair";
    public static final String AEGIS_REPAIR_POD            = "ability_aegis_repair-pod";
    public static final String AEGIS_SHIELD_REPAIR         = "ability_aegis_shield-repair";
    public static final String CITADEL_DRAW_FIRE           = "ability_citadel_draw-fire";
    public static final String CITADEL_FORTIFY             = "ability_citadel_fortify";
    public static final String CITADEL_PROTECTION          = "ability_citadel_protection";
    public static final String CITADEL_TRAVEL              = "ability_citadel_travel";
    public static final String SPEARHEAD_DOUBLE_MINIMAP    = "ability_spearhead_double-minimap";
    public static final String SPEARHEAD_JAM_X             = "ability_spearhead_jam-x";
    public static final String SPEARHEAD_TARGET_MARKER     = "ability_spearhead_target-marker";
    public static final String SPEARHEAD_ULTIMATE_CLOAK    = "ability_spearhead_ultimate-cloak";
    
    public static final List<String> abilityCategory = Arrays.asList(SENTINEL_ABILITY, DIMINISHER_ABILITY, VENOM_ABILITY, SPECTRUM_ABILITY, SOLACE_ABILITY);

    private static final int    VENOM_ABILITY_DURATION_TIME         = 30000;
    private static final int    DIMINISHER_ABILITY_DURATION_TIME    = 30000;
    private static final int    SPECTRUM_ABILITY_DURATION_TIME      = 30000;
    private static final int    SENTINEL_ABILITY_DURATION_TIME      = 30000;
    
    private static final int    SOLACE_ABILITY_COOLDOWN_TIME        = 300000;
    private static final int    VENOM_ABILITY_COOLDOWN_TIME         = 300000;
    private static final int    DIMINISHER_ABILITY_COOLDOWN_TIME    = 300000;
    private static final int    SPECTRUM_ABILITY_COOLDOWN_TIME      = 300000;
    private static final int    SENTINEL_ABILITY_COOLDOWN_TIME      = 300000;
    private static final int    AEGIS_HP_REPAIR_COOLDOWN_TIME       = 300000;
    private static final int    AEGIS_SHIELD_REPAIR_COOLDOWN_TIME   = 300000;
    
    private long mSolaceAbilityCooldownEndTime              = 0L;
    private long mSentinelAbilityCooldownEndTime            = 0L;
    private long mVenomAbilityCooldownEndTime               = 0L;
    private long mDiminisherAbilityCooldownEndTime          = 0L;
    private long mSpectrumAbilityCooldownEndTime            = 0L;
    private long mAegisHpRepairAbilityCooldownEndTime       = 0L;
    private long mAegisHpRepairAbilityLastRepairTime        = 0L;
    private long mAegisShieldRepairAbilityCooldownEndTime   = 0L;
    private long mAegisShieldRepairAbilityLastRepairTime    = 0L;
    private long mVenomAbilityLastDamageTime                = 0L;
    private int  mVenomAbilityLastDamage                    = 1500;
    private int  mCyborgAbilityLastDamage                   = 6800;
    
    private long mSentinelAbilityEffectFinishTime             = 0L;
    private long mDiminisherAbilityEffectFinishTime           = 0L;
    private long mVenomAbilityEffectFinishTime                = 0L;
    private long mSpectrumAbilityEffectFinishTime             = 0L;
    
    private boolean mSpectrumAbilityEffectActivated            = false;
    private boolean mAegisHpRepairAbilityEffectActivated       = false;
    private boolean mAegisShieldRepairAbilityEffectActivated   = false;
    private boolean mVenomAbilityEffectActivated               = false;
    private boolean mDiminisherAbilityEffectActivated          = false;
    private boolean mSentinelAbilityEffectActivated            = false;
    
    private String mSkillsJSON;
    
    public SkillsManager(final Account pAccount) {
        super(pAccount);
    }

    public void onTickCheckMethods() {
        this.checkSpectrumAbility();
        this.checkAegisHpRepairAbility();
        this.checkAegisShieldRepairAbility();
        this.checkVenomDamageAbility();
        this.checkDiminisherAbility();
        this.checkSentinelAbility();
    }
    
    public boolean isSentinelAbilityActivated() {
        return (System.currentTimeMillis() - this.getSentinelAbilityEffectFinishTime()) < 0;
    }
    
    private void checkSentinelAbility() {
        final Player player = this.getAccount()
                                  .getPlayer();
        if (this.isSentinelAbilityActivated()) {
                //    player.setCurrentShieldAbsorb(50);
        } else if (this.isSentinelAbilityEffectActivated()) {
            final String packet = "0|SD|D|R|4|" + player.getAccount().getUserId() + "";
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);            
           // player.setCurrentShieldAbsorb(80);     
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(SENTINEL_ABILITY),
            		player.getAccount().getAmmunitionManager().getItemTimerState(SENTINEL_ABILITY), SENTINEL_ABILITY_COOLDOWN_TIME,
            		SENTINEL_ABILITY_COOLDOWN_TIME));
            this.setSentinelAbilityEffectActivated(false);
        }
    }
    
    public boolean isDiminisherAbilityActivated() {
        return (System.currentTimeMillis() - this.getDiminisherAbilityEffectFinishTime()) < 0;
    }
    
    private void checkDiminisherAbility() {
        final Player player = this.getAccount()
                                  .getPlayer();
        if (this.isDiminisherAbilityActivated()) {
      /**      for (final MovableMapEntity thisMapEntity : player.getInRangeMovableMapEntities()) {
                if(thisMapEntity instanceof Player)
                {               	                    
                    final Player otherPlayer = (Player)thisMapEntity;

                    if(player.getLockedTarget() == otherPlayer) {
                    otherPlayer.setCurrentShieldAbsorb(120);
                    player.setCurrentShieldAbsorb(90);
                    } else {
                    	otherPlayer.setCurrentShieldAbsorb(80);
                    }
                }                             	
            } */
        } else if (this.isDiminisherAbilityEffectActivated()) {
            final String packet = "0|SD|D|R|2|" + player.getAccount().getUserId() + "";
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);            
            
       /**     for (final MovableMapEntity thisMapEntity : player.getInRangeMovableMapEntities()) {
                if(thisMapEntity instanceof Player)
                {                   
                    final Player otherPlayer = (Player)thisMapEntity;
                    otherPlayer.setCurrentShieldAbsorb(80);
                }                             	
            }      
        	if(player.getLockedTarget() instanceof Player) {
        	final Player playerlocked = (Player) player.getLockedTarget();
            if(playerlocked != null) {
            final String deactivePacketForLockedPlayer = "0|SD|D|R|2|" + playerlocked.getAccount().getUserId() + "";
            player.sendPacketToInRange(deactivePacketForLockedPlayer);
            player.sendPacketToBoundSessions(deactivePacketForLockedPlayer);
              }
        	} */
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(DIMINISHER_ABILITY),
            		player.getAccount().getAmmunitionManager().getItemTimerState(DIMINISHER_ABILITY), DIMINISHER_ABILITY_COOLDOWN_TIME,
            		DIMINISHER_ABILITY_COOLDOWN_TIME));
            this.setDiminisherAbilityEffectActivated(false);
        }
    }
    
    public void sendAegisShieldRepairAbility() // aegis kalkan basma yeteneği
    {
        final Player player = this.getAccount()
                .getPlayer();
    	final long currentTime = System.currentTimeMillis();
    	if (currentTime - this.getAegisShieldRepairAbilityCooldownEndTime() >= 0) {
            if(player.getPlayerShipId() == 49 || player.getAccount().isAdmin()) {
            	this.setAegisShieldRepairAbilityEffectActivated(true);
                this.setAegisShieldRepairAbilityCooldownEndTime(currentTime + AEGIS_SHIELD_REPAIR_COOLDOWN_TIME);
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                		player.getAccount().getAmmunitionManager().getCooldownType(AEGIS_SHIELD_REPAIR),
                		player.getAccount().getAmmunitionManager().getItemTimerState(AEGIS_SHIELD_REPAIR), AEGIS_SHIELD_REPAIR_COOLDOWN_TIME,
                		AEGIS_SHIELD_REPAIR_COOLDOWN_TIME));
            }
            else {
            	final String wrongAbilityPacket = "0|A|STD|You need be Aegis for use this ability!";
                player.sendPacketToBoundSessions(wrongAbilityPacket);
            }
    	}
    }
    
    public boolean isVenomAbilityActivated() {
        return (System.currentTimeMillis() - this.getVenomAbilityEffectFinishTime()) < 0;
    }

    private void checkVenomDamageAbility() {
        final Player player = this.getAccount()
                                  .getPlayer();
        final long currentTime = System.currentTimeMillis();
        if (this.isVenomAbilityActivated()) {
            if ((currentTime - this.getVenomAbilityLastDamageTime()) >= 1000) {

            	if(player.getPlayerShipId() == 445 || player.getPlayerShipId() == 452) {
            		
                    int damage = this.getCyborgAbilityLastDamage();
                    
                    if(player.getLockedTarget() != null){
                    if(player.getLockedTarget() instanceof Player) {
                    	final Player playerlocked = (Player) player.getLockedTarget();
                    	if(playerlocked.canBeShoot()) {                  		
                    		
                    		playerlocked.changeCurrentHitPoints(-damage);
                            final AttackHitCommand attackHitCommand =
                                    new AttackHitCommand(new AttackTypeModule(AttackTypeModule.LASER), 0,//attackerID
                                    		playerlocked.getMapEntityId(), playerlocked.getCurrentHitPoints(),
                                    		playerlocked.getCurrentShieldPoints(), playerlocked.getCurrentNanoHull(),
                                                         damage, false);
                            
                            player.sendCommandToInRange(attackHitCommand);
                            playerlocked.sendCommandToInRange(attackHitCommand);
                    		
                            playerlocked.setLastCheckedDamageTime(currentTime);
                            playerlocked.setLastDamagedTime(currentTime);
                            
                    	}
                    	damage = damage + 300;
                    	if(playerlocked.getCurrentHitPoints() <= 0 || damage >= playerlocked.getCurrentHitPoints()) {
                    		if(!playerlocked.isDestroyed()) {
                    		playerlocked.destroy(player);
                    		}
                    	}
                    } else if(player.getLockedTarget() instanceof Alien) {
                    	final Alien alien = (Alien) player.getLockedTarget();
                    	
                    	alien.changeCurrentHitPoints(-damage);
                            final AttackHitCommand attackHitCommand =
                                    new AttackHitCommand(new AttackTypeModule(AttackTypeModule.LASER), 0,//attackerID
                                    		alien.getMapEntityId(), alien.getCurrentHitPoints(),
                                    		alien.getCurrentShieldPoints(), alien.getCurrentNanoHull(),
                                                         damage, false);
                            
                            player.sendCommandToInRange(attackHitCommand);
                            alien.sendCommandToInRange(attackHitCommand);
                    		
                            alien.setLastCheckedDamageTime(currentTime);
                            alien.setLastDamagedTime(currentTime);
                            
                    	damage = damage + 300;
                    	if(alien.getCurrentHitPoints() <= 0 || damage >= alien.getCurrentHitPoints()) {
                    		alien.destroy(player);
                    	}                        
                        
                    }
                    
                    }
                    this.setVenomAbilityLastDamageTime(currentTime);
                    this.setCyborgAbilityLastDamage(this.getCyborgAbilityLastDamage() + 300);
                    
            	} else {
            	
                int damage = this.getVenomAbilityLastDamage();
                
                if(player.getLockedTarget() != null){
                if(player.getLockedTarget() instanceof Player) {
                	final Player playerlocked = (Player) player.getLockedTarget();
                	if(playerlocked.canBeShoot()) {                  		
                		
                		playerlocked.changeCurrentHitPoints(-damage);
                        final AttackHitCommand attackHitCommand =
                                new AttackHitCommand(new AttackTypeModule(AttackTypeModule.LASER), 0,//attackerID
                                		playerlocked.getMapEntityId(), playerlocked.getCurrentHitPoints(),
                                		playerlocked.getCurrentShieldPoints(), playerlocked.getCurrentNanoHull(),
                                                     damage, false);
                        
                        player.sendCommandToInRange(attackHitCommand);
                        playerlocked.sendCommandToInRange(attackHitCommand);
                		
                        playerlocked.setLastCheckedDamageTime(currentTime);
                        playerlocked.setLastDamagedTime(currentTime);
                        
                	}
                	damage = damage + 200;
                	if(playerlocked.getCurrentHitPoints() <= 0 || damage >= playerlocked.getCurrentHitPoints()) {
                		if(!playerlocked.isDestroyed()) {
                		playerlocked.destroy(player);
                		}
                	}
                } else if(player.getLockedTarget() instanceof Alien) {
                	final Alien alien = (Alien) player.getLockedTarget();
                	
                	alien.changeCurrentHitPoints(-damage);
                        final AttackHitCommand attackHitCommand =
                                new AttackHitCommand(new AttackTypeModule(AttackTypeModule.LASER), 0,//attackerID
                                		alien.getMapEntityId(), alien.getCurrentHitPoints(),
                                		alien.getCurrentShieldPoints(), alien.getCurrentNanoHull(),
                                                     damage, false);
                        
                        player.sendCommandToInRange(attackHitCommand);
                        alien.sendCommandToInRange(attackHitCommand);
                		
                        alien.setLastCheckedDamageTime(currentTime);
                        alien.setLastDamagedTime(currentTime);
                        
                	damage = damage + 200;
                	if(alien.getCurrentHitPoints() <= 0 || damage >= alien.getCurrentHitPoints()) {
                		alien.destroy(player);
                	}                        
                    
                }
                }
                this.setVenomAbilityLastDamageTime(currentTime);
                this.setVenomAbilityLastDamage(this.getVenomAbilityLastDamage() + 200);
            	}
                
            }
        } else if (this.isVenomAbilityEffectActivated()) {
        	this.setVenomAbilityLastDamage(0);
        	if(player.getLockedTarget() instanceof Player) {
	        	final Player playerlocked = (Player) player.getLockedTarget();
	        	
	            final String deactivePacket = "0|SD|D|R|5|" + player.getAccount().getUserId() + "";
	            player.sendPacketToInRange(deactivePacket);
	            player.sendPacketToBoundSessions(deactivePacket);
	            
	            if(playerlocked != null) {
		            final String deactivePacketForLockedPlayer = "0|SD|D|R|5|" + playerlocked.getAccount().getUserId() + "";
		            player.sendPacketToInRange(deactivePacketForLockedPlayer);
		            player.sendPacketToBoundSessions(deactivePacketForLockedPlayer);
	            }
        	} else if(player.getLockedTarget() instanceof Alien) {
	        	final Alien alien = (Alien) player.getLockedTarget();
	        	
	            final String deactivePacket = "0|SD|D|R|5|" + player.getAccount().getUserId() + "";
	            player.sendPacketToInRange(deactivePacket);
	            player.sendPacketToBoundSessions(deactivePacket);
	            
	            if(alien != null) {
		            final String deactivePacketForLockedPlayer = "0|SD|D|R|5|" + alien.getMapEntityId() + "";
		            player.sendPacketToInRange(deactivePacketForLockedPlayer);
		            player.sendPacketToBoundSessions(deactivePacketForLockedPlayer);
	            }
        	}
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(VENOM_ABILITY),
            		player.getAccount().getAmmunitionManager().getItemTimerState(VENOM_ABILITY), VENOM_ABILITY_COOLDOWN_TIME,
            		VENOM_ABILITY_COOLDOWN_TIME));
            this.setVenomAbilityEffectActivated(false);
        }
    }
    
    private boolean isAegisShieldRepairAbilityActivated() {
        return (System.currentTimeMillis() - this.getAegisShieldRepairAbilityCooldownEndTime()) < 0;
    }

    private void checkAegisShieldRepairAbility() {
        final Player player = this.getAccount()
                                  .getPlayer();
        final long currentTime = System.currentTimeMillis();
        if (this.isAegisShieldRepairAbilityActivated()) {
            if ((currentTime - this.getAegisHpRepairAbilityLastRepairTime()) >= 1000) {

                int shield = 15000;
                int shieldforlockedplayer = 25000;
                
                player.healEntity(shield, player.HEAL_SHIELD);
                if(player.getLockedTarget() != null){
                if(player.getLockedTarget() instanceof Player) {
                	final Player playerlocked = (Player) player.getLockedTarget();
                	playerlocked.healEntity(shieldforlockedplayer, player.HEAL_SHIELD);
                }
                }
                this.setAegisHpRepairAbilityLastRepairTime(currentTime);
            }
        } else if (this.isAegisHpRepairAbilityEffectActivated()) {

        }
    }
    
    public void sendAegisHpRepairAbility() // aegis can basma yeteneği
    {
        final Player player = this.getAccount()
                .getPlayer();
    	final long currentTime = System.currentTimeMillis();
    	if (currentTime - this.getAegisHpRepairAbilityCooldownEndTime() >= 0) {
            if(player.getPlayerShipId() == 49 || player.getAccount().isAdmin()) {
            	this.setAegisHpRepairAbilityEffectActivated(true);
                this.setAegisHpRepairAbilityCooldownEndTime(currentTime + AEGIS_HP_REPAIR_COOLDOWN_TIME);
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                		player.getAccount().getAmmunitionManager().getCooldownType(AEGIS_HP_REPAIR),
                		player.getAccount().getAmmunitionManager().getItemTimerState(AEGIS_HP_REPAIR), AEGIS_HP_REPAIR_COOLDOWN_TIME,
                		AEGIS_HP_REPAIR_COOLDOWN_TIME));
            }
            else {
            	final String wrongAbilityPacket = "0|A|STD|You need be Aegis for use this ability!";
                player.sendPacketToBoundSessions(wrongAbilityPacket);
            }
    	}
    }
    
    private boolean isAegisHpRepairAbilityActivated() {
        return (System.currentTimeMillis() - this.getAegisHpRepairAbilityCooldownEndTime()) < 0;
    }

    private void checkAegisHpRepairAbility() {
        final Player player = this.getAccount()
                                  .getPlayer();
        final long currentTime = System.currentTimeMillis();
        if (this.isAegisHpRepairAbilityActivated()) {
            if ((currentTime - this.getAegisHpRepairAbilityLastRepairTime()) >= 1000) {

                int heal = 20000;
                int healforlockedplayer = 40000;
                
                player.healEntity(heal, player.HEAL_HITPOINTS);
                if(player.getLockedTarget() != null){
                if(player.getLockedTarget() instanceof Player) {
                	final Player playerlocked = (Player) player.getLockedTarget();
                	playerlocked.healEntity(healforlockedplayer, player.HEAL_HITPOINTS);
                }
                }
                this.setAegisHpRepairAbilityLastRepairTime(currentTime);
            }
        } else if (this.isAegisHpRepairAbilityEffectActivated()) {

        }
    }
    
    public void sendSpectrumAbility() // spectrum yeteneği
    { 	
    	final long currentTime = System.currentTimeMillis();
        final Player player = this.getAccount()
                .getPlayer();
        if (currentTime - this.getSpectrumAbilityCooldownEndTime() >= 0) {
        if(player.getPlayerShipId() == 65 || player.getPlayerShipId() == 447 || player.getPlayerShipId() == 450 || player.getAccount().isAdmin()) {
        	final String spectrumPacket = "0|SD|A|R|3|" + player.getAccount().getUserId() + "";
        	
        	/**

			AEGİS TAMİR TÜPÜ
			
            player.sendCommandToBoundSessions(new AssetCreateCommand(new AssetTypeModule(AssetTypeModule.HEALING_POD), "", 1, "", 1212, 0,
                    0, player.getCurrentPositionX(), player.getCurrentPositionY(), 0, true, true,
                    true, true, new ClanRelationModule(ClanRelationModule.NONE),
                    new Vector<VisualModifierCommand>()));
		*/
            
            
            player.sendPacketToBoundSessions(spectrumPacket);
            player.sendPacketToInRange(spectrumPacket);
            this.setSpectrumAbilityEffectActivated(true);
            this.setSpectrumAbilityEffectFinishTime(currentTime + SPECTRUM_ABILITY_DURATION_TIME);
            this.setSpectrumAbilityCooldownEndTime(currentTime + SPECTRUM_ABILITY_DURATION_TIME + SPECTRUM_ABILITY_COOLDOWN_TIME);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(SPECTRUM_ABILITY),
            		player.getAccount().getAmmunitionManager().getItemTimerState(""), SPECTRUM_ABILITY_DURATION_TIME,
            		SPECTRUM_ABILITY_DURATION_TIME));
        }
        }
    }
    
    public boolean isSpectrumAbilityActivated() {
        return (System.currentTimeMillis() - this.getSpectrumAbilityEffectFinishTime()) < 0;
    }

    private void checkSpectrumAbility() {
        final Player player = this.getAccount()
                                  .getPlayer();
        if (this.isSpectrumAbilityActivated()) {
                	
        } else if (this.isSpectrumAbilityEffectActivated()) {
            final String packet = "0|SD|D|R|3|" + player.getAccount().getUserId() + "";
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(SPECTRUM_ABILITY),
            		player.getAccount().getAmmunitionManager().getItemTimerState(SPECTRUM_ABILITY), SPECTRUM_ABILITY_COOLDOWN_TIME,
            		SPECTRUM_ABILITY_COOLDOWN_TIME));
            this.setSpectrumAbilityEffectActivated(false);
        }
    }
    
    public void sendSentinelAbility() //sentinel yeteneği
    {
    	final long currentTime = System.currentTimeMillis();
        final Player player = this.getAccount()
                .getPlayer();
        if (currentTime - this.getSentinelAbilityCooldownEndTime() >= 0) {
        if(player.getPlayerShipId() == 66 || player.getPlayerShipId() == 448 || player.getPlayerShipId() == 449 ||  player.getAccount().isAdmin()) {        	
    	final String sentinelPacket = "0|SD|A|R|4|" + player.getAccount().getUserId() + "";
        player.sendPacketToBoundSessions(sentinelPacket);
        player.sendPacketToInRange(sentinelPacket);
        this.setSentinelAbilityEffectActivated(true);
        this.setSentinelAbilityEffectFinishTime(currentTime + SENTINEL_ABILITY_DURATION_TIME);
        this.setSentinelAbilityCooldownEndTime(currentTime + SENTINEL_ABILITY_DURATION_TIME + SENTINEL_ABILITY_COOLDOWN_TIME);
        player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
        		player.getAccount().getAmmunitionManager().getCooldownType(SENTINEL_ABILITY),
        		player.getAccount().getAmmunitionManager().getItemTimerState(""), SENTINEL_ABILITY_DURATION_TIME,
        		SENTINEL_ABILITY_DURATION_TIME));
        }
        }
    }
    
    public void sendSolaceAbility() // solace yeteneği
    {
    	final long currentTime = System.currentTimeMillis();
        final Player player = this.getAccount()
                .getPlayer();
        if (currentTime - this.getSolaceAbilityCooldownEndTime() >= 0) {
        if(player.getPlayerShipId() == 63 || player.getAccount().isAdmin()) {  
    	final String solacePacket = "0|SD|A|R|1|" + player.getAccount().getUserId() + "";
        player.sendPacketToBoundSessions(solacePacket);
        player.sendPacketToInRange(solacePacket);
        
        int repair = (int) (player.getMaximumHitPoints() / 100) * 50;
        player.healEntity(repair, player.HEAL_HITPOINTS);
        this.setSolaceAbilityCooldownEndTime(currentTime + SOLACE_ABILITY_COOLDOWN_TIME);
        player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
        		player.getAccount().getAmmunitionManager().getCooldownType(SOLACE_ABILITY),
        		player.getAccount().getAmmunitionManager().getItemTimerState(SOLACE_ABILITY), SOLACE_ABILITY_COOLDOWN_TIME,
                SOLACE_ABILITY_COOLDOWN_TIME));
        }
        }
    }
    
    public void sendDiminisherAbility() // diminisher yeteneği
    {
        final Player player = this.getAccount()
                .getPlayer();
        final long currentTime = System.currentTimeMillis();
        if (currentTime - this.getDiminisherAbilityCooldownEndTime() >= 0) {
        if(player.getPlayerShipId() == 64 || player.getAccount().isAdmin()) {
        	if(player.getLockedTarget() == null)
        	{
            	final String lockedTargetNull = "0|A|STD|You need locked enemy's for start Diminisher ability!";
                player.sendPacketToBoundSessions(lockedTargetNull);
        	} else if(player.getLockedTarget() instanceof Alien) {
            	final String lockedTargetAlien = "0|A|STD|Now just for players!";
                player.sendPacketToBoundSessions(lockedTargetAlien);
        	} else {
    	final String diminisherPacket = "0|SD|A|R|2|" + player.getAccount().getUserId() + "";
    	final String diminisherPacketToSelectedUser = "0|SD|A|R|2|" + player.getLockedTarget().getMapEntityId() + "";
    	player.setDiminisherliDusman((Player) player.getLockedTarget());
        player.sendPacketToBoundSessions(diminisherPacket);
        player.sendPacketToInRange(diminisherPacket);
        player.sendPacketToBoundSessions(diminisherPacketToSelectedUser);
        player.sendPacketToInRange(diminisherPacketToSelectedUser);
        this.setDiminisherAbilityEffectActivated(true);
        this.setDiminisherAbilityEffectFinishTime(currentTime + DIMINISHER_ABILITY_DURATION_TIME);
        this.setDiminisherAbilityCooldownEndTime(currentTime + DIMINISHER_ABILITY_DURATION_TIME + DIMINISHER_ABILITY_COOLDOWN_TIME);
        player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
        		player.getAccount().getAmmunitionManager().getCooldownType(DIMINISHER_ABILITY),
        		player.getAccount().getAmmunitionManager().getItemTimerState(""), DIMINISHER_ABILITY_DURATION_TIME,
        		DIMINISHER_ABILITY_DURATION_TIME));
        	}
        }
        }
    }
    
    public void sendVenomAbility() // venom yeteneği
    {
        final Player player = this.getAccount().getPlayer();
        final long currentTime = System.currentTimeMillis();
        if(player.getLockedTarget() instanceof Player) {
        	final Player lockedplayer = (Player) this.getAccount().getPlayer().getLockedTarget();	        
	        if (currentTime - this.getVenomAbilityCooldownEndTime() >= 0) {
	        	if(lockedplayer != null) {
	        	
	        if(player.getPlayerShipId() == 67 || player.getAccount().isAdmin() || player.getPlayerShipId() == 445 || player.getPlayerShipId() == 452) {
	        	if(player.getLockedTarget() == null) {
	            	final String lockedTargetNull = "0|A|STD|You need locked enemy's for start Venom ability!";
	                player.sendPacketToBoundSessions(lockedTargetNull);
	        	} else if(player.getLockedTarget() instanceof Alien) {
	            	final String lockedTargetAlien = "0|A|STD|Now just for players!";
	                player.sendPacketToBoundSessions(lockedTargetAlien);
	        	} else {
	        		if(!player.getLockedTarget().isInSecureZone()) {
					
					
	        			if(!Settings.FRIEND_SHOOT_ENABLED) {
					        final Player targetPlayer = (Player) player.getLockedTarget();						
							boolean isWar;
	                        if(player.getAccount().getFactionId() == targetPlayer.getAccount().getFactionId()){
	                            isWar = false;
	                            for(Diplomacy dip : player.getAccount().getClan().getDiplomacies()){
	                                if(dip.relationType == 3 && (dip.clanID1 == targetPlayer.getAccount().getClanId() || dip.clanID2 == targetPlayer.getAccount().getClanId())){
	                                    isWar = true;
	                                }
	                            }
	                            
	                            if(!isWar) {
	                            	return;
	                            }
	                            
	                        }                        
	        			}
					
					
	    	final String venomPacket = "0|SD|A|R|5|" + player.getAccount().getUserId() + "";
	    	final String venomPacketToSelectedUser = "0|SD|A|R|5|" + player.getLockedTarget().getMapEntityId() + "";
	    	player.setVenomluDusman((Player) player.getLockedTarget());
	        player.sendPacketToBoundSessions(venomPacket);
	        player.sendPacketToInRange(venomPacket);
	        player.sendPacketToBoundSessions(venomPacketToSelectedUser);
	        player.sendPacketToInRange(venomPacketToSelectedUser);
	        this.setVenomAbilityEffectActivated(true);
	        this.setVenomAbilityEffectFinishTime(currentTime + VENOM_ABILITY_DURATION_TIME);
	        this.setVenomAbilityCooldownEndTime(currentTime + VENOM_ABILITY_DURATION_TIME + VENOM_ABILITY_COOLDOWN_TIME);
	        player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
	        		player.getAccount().getAmmunitionManager().getCooldownType(VENOM_ABILITY),
	        		player.getAccount().getAmmunitionManager().getItemTimerState(""), VENOM_ABILITY_DURATION_TIME,
	        		VENOM_ABILITY_DURATION_TIME));
					
					
					
					
	        	}
	        	}
	        	}
	        }
	        }
        } else if(player.getLockedTarget() instanceof Alien) {	        
	        if (currentTime - this.getVenomAbilityCooldownEndTime() >= 0) {
	        if(player.getPlayerShipId() == 67 || player.getAccount().isAdmin() || player.getPlayerShipId() == 445 || player.getPlayerShipId() == 452) {
	        	final Alien alien = (Alien) player.getLockedTarget();
	        	
	        	player.setVenomluDusman((Alien) player.getLockedTarget());
	        	final String spectrumPacket = "0|SD|A|R|5|" + player.getAccount().getUserId() + "";
	            player.sendPacketToBoundSessions(spectrumPacket);
	            player.sendPacketToInRange(spectrumPacket);
	            
		    	final String venomPacketToSelectedUser = "0|SD|A|R|5|" + alien.getMapEntityId() + "";
		        alien.sendPacketToInRange(venomPacketToSelectedUser);
		        
		        this.setVenomAbilityEffectActivated(true);
		        this.setVenomAbilityEffectFinishTime(currentTime + VENOM_ABILITY_DURATION_TIME);
		        this.setVenomAbilityCooldownEndTime(currentTime + VENOM_ABILITY_DURATION_TIME + VENOM_ABILITY_COOLDOWN_TIME);
		        player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
		        		player.getAccount().getAmmunitionManager().getCooldownType(VENOM_ABILITY),
		        		player.getAccount().getAmmunitionManager().getItemTimerState(""), VENOM_ABILITY_DURATION_TIME,
		        		VENOM_ABILITY_DURATION_TIME));
	        }
        }
        }
    }
    
    public long getSpectrumAbilityEffectFinishTime() {
        return mSpectrumAbilityEffectFinishTime;
    }

    public void setSpectrumAbilityEffectFinishTime(final long pSpectrumAbilityEffectFinishTime) {
    	mSpectrumAbilityEffectFinishTime = pSpectrumAbilityEffectFinishTime;
    }
    
    public long getVenomAbilityEffectFinishTime() {
        return mVenomAbilityEffectFinishTime;
    }

    public void setVenomAbilityEffectFinishTime(final long pVenomAbilityEffectFinishTime) {
    	mVenomAbilityEffectFinishTime = pVenomAbilityEffectFinishTime;
    }
    
    public long getDiminisherAbilityEffectFinishTime() {
        return mDiminisherAbilityEffectFinishTime;
    }

    public void setDiminisherAbilityEffectFinishTime(final long pDiminisherAbilityEffectFinishTime) {
    	mDiminisherAbilityEffectFinishTime = pDiminisherAbilityEffectFinishTime;
    }
    
    public long getSentinelAbilityEffectFinishTime() {
        return mSentinelAbilityEffectFinishTime;
    }

    public void setSentinelAbilityEffectFinishTime(final long pSentinelAbilityEffectFinishTime) {
        mSentinelAbilityEffectFinishTime = pSentinelAbilityEffectFinishTime;
    }
    
    public int getVenomAbilityLastDamage() {
        return mVenomAbilityLastDamage;
    }

    public void setVenomAbilityLastDamage(final int pVenomAbilityLastDamage) {
    	mVenomAbilityLastDamage = pVenomAbilityLastDamage;
    }
    
    public int getCyborgAbilityLastDamage() {
        return mCyborgAbilityLastDamage;
    }

    public void setCyborgAbilityLastDamage(final int pCyborgAbilityLastDamage) {
    	mCyborgAbilityLastDamage = pCyborgAbilityLastDamage;
    }
    
    public long getVenomAbilityLastDamageTime() {
        return mVenomAbilityLastDamageTime;
    }

    public void setVenomAbilityLastDamageTime(final long pVenomAbilityLastDamageTime) {
    	mVenomAbilityLastDamageTime = pVenomAbilityLastDamageTime;
    }
    
    public boolean isVenomAbilityEffectActivated() {
        return mVenomAbilityEffectActivated;
    }

    public void setVenomAbilityEffectActivated(final boolean pVenomAbilityEffectActivated) {
    	mVenomAbilityEffectActivated = pVenomAbilityEffectActivated;
    }
    
    public boolean isSentinelAbilityEffectActivated() {
        return mSentinelAbilityEffectActivated;
    }

    public void setSentinelAbilityEffectActivated(final boolean pSentinelAbilityEffectActivated) {
    	mSentinelAbilityEffectActivated = pSentinelAbilityEffectActivated;
    }
    
    public boolean isDiminisherAbilityEffectActivated() {
        return mDiminisherAbilityEffectActivated;
    }

    public void setDiminisherAbilityEffectActivated(final boolean pDiminisherAbilityEffectActivated) {
    	mDiminisherAbilityEffectActivated = pDiminisherAbilityEffectActivated;
    }
    
    public long getAegisShieldRepairAbilityLastRepairTime() {
        return mAegisShieldRepairAbilityLastRepairTime;
    }

    public void setAegisShieldRepairAbilityLastRepairTime(final long pAegisShieldRepairAbilityLastRepairTime) {
        mAegisShieldRepairAbilityLastRepairTime = pAegisShieldRepairAbilityLastRepairTime;
    }
    
    public boolean isAegisShieldRepairAbilityEffectActivated() {
        return mAegisShieldRepairAbilityEffectActivated;
    }

    public void setAegisShieldRepairAbilityEffectActivated(final boolean pAegisShieldRepairAbilityEffectActivated) {
    	mAegisShieldRepairAbilityEffectActivated = pAegisShieldRepairAbilityEffectActivated;
    }
    
    public long getAegisHpRepairAbilityLastRepairTime() {
        return mAegisHpRepairAbilityLastRepairTime;
    }

    public void setAegisHpRepairAbilityLastRepairTime(final long pAegisHpRepairAbilityLastRepairTime) {
        mAegisHpRepairAbilityLastRepairTime = pAegisHpRepairAbilityLastRepairTime;
    }
    
    public boolean isAegisHpRepairAbilityEffectActivated() {
        return mAegisHpRepairAbilityEffectActivated;
    }

    public void setAegisHpRepairAbilityEffectActivated(final boolean pAegisHpRepairAbilityEffectActivated) {
    	mAegisHpRepairAbilityEffectActivated = pAegisHpRepairAbilityEffectActivated;
    }
    
    public boolean isSpectrumAbilityEffectActivated() {
        return mSpectrumAbilityEffectActivated;
    }

    public void setSpectrumAbilityEffectActivated(final boolean pSpectrumAbilityEffectActivated) {
        mSpectrumAbilityEffectActivated = pSpectrumAbilityEffectActivated;
    }
    
    public long getVenomAbilityCooldownEndTime() {
        return mVenomAbilityCooldownEndTime;
    }
    
    public void setVenomAbilityCooldownEndTime(final long pVenomAbilityCooldownEndTime) {
    	mVenomAbilityCooldownEndTime = pVenomAbilityCooldownEndTime;
    }
    
    public long getDiminisherAbilityCooldownEndTime() {
        return mDiminisherAbilityCooldownEndTime;
    }
    
    public void setDiminisherAbilityCooldownEndTime(final long pDiminisherAbilityCooldownEndTime) {
    	mDiminisherAbilityCooldownEndTime = pDiminisherAbilityCooldownEndTime;
    }
    
    public long getSolaceAbilityCooldownEndTime() {
        return mSolaceAbilityCooldownEndTime;
    }
    
    public void setSolaceAbilityCooldownEndTime(final long pSolaceAbilityCooldownEndTime) {
    	mSolaceAbilityCooldownEndTime = pSolaceAbilityCooldownEndTime;
    }
    
    public long getSentinelAbilityCooldownEndTime() {
        return mSentinelAbilityCooldownEndTime;
    }
    
    public void setSentinelAbilityCooldownEndTime(final long pSentinelAbilityCooldownEndTime) {
    	mSentinelAbilityCooldownEndTime = pSentinelAbilityCooldownEndTime;
    }
    
    public long getSpectrumAbilityCooldownEndTime() {
        return mSpectrumAbilityCooldownEndTime;
    }
    
    public void setSpectrumAbilityCooldownEndTime(final long pSpectrumAbilityCooldownEndTime) {
    	mSpectrumAbilityCooldownEndTime = pSpectrumAbilityCooldownEndTime;
    }
    
    public long getAegisHpRepairAbilityCooldownEndTime() {
        return mAegisHpRepairAbilityCooldownEndTime;
    }
    
    public void setAegisHpRepairAbilityCooldownEndTime(final long pAegisHpRepairAbilityCooldownEndTime) {
    	mAegisHpRepairAbilityCooldownEndTime = pAegisHpRepairAbilityCooldownEndTime;
    }
    
    public long getAegisShieldRepairAbilityCooldownEndTime() {
        return mAegisShieldRepairAbilityCooldownEndTime;
    }
    
    public void setAegisShieldRepairAbilityCooldownEndTime(final long pAegisShieldRepairAbilityCooldownEndTime) {
    	mAegisShieldRepairAbilityCooldownEndTime = pAegisShieldRepairAbilityCooldownEndTime;
    }
    
	@Override
	public void setFromJSON(final String pSkillsJSON) {
		this.mSkillsJSON = pSkillsJSON;
	}

	@Override
	public void setNewAccount() {
	}

	@Override
	public String packToJSON() {
		return this.mSkillsJSON;
	}
}
