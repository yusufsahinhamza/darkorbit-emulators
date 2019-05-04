package simulator.users;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import mysql.QueryManager;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.DroneFormationChangeCommand;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;
import utils.Tools;

/**
 Used to manage drones ??? huh?
 @author Shock & Pedro 
 Edited & Fixed by LEJYONER.
 */
public class DroneManager
        extends AbstractAccountInternalManager {

    public static final String DEFAULT_FORMATION      = "drone_formation_default";
    public static final String TURTLE_FORMATION       = "drone_formation_f-01-tu";
    public static final String ARROW_FORMATION        = "drone_formation_f-02-ar";
    public static final String LANCE_FORMATION        = "drone_formation_f-03-la";
    public static final String STAR_FORMATION         = "drone_formation_f-04-st";
    public static final String PINCER_FORMATION       = "drone_formation_f-05-pi";
    public static final String DOUBLE_ARROW_FORMATION = "drone_formation_f-06-da";
    public static final String DIAMOND_FORMATION      = "drone_formation_f-07-di";
    public static final String CHEVRON_FORMATION      = "drone_formation_f-08-ch";
    public static final String MOTH_FORMATION         = "drone_formation_f-09-mo";
    public static final String CRAB_FORMATION         = "drone_formation_f-10-cr";
    public static final String HEART_FORMATION        = "drone_formation_f-11-he";
    public static final String BARRAGE_FORMATION      = "drone_formation_f-12-ba";
    public static final String BAT_FORMATION          = "drone_formation_f-13-bt";


    public static final List<String> droneCategory =
            Arrays.asList(DEFAULT_FORMATION, TURTLE_FORMATION, ARROW_FORMATION, /**LANCE_FORMATION,*/ /**STAR_FORMATION,*/
                          PINCER_FORMATION, /**DOUBLE_ARROW_FORMATION,*/ DIAMOND_FORMATION, CHEVRON_FORMATION,
                          MOTH_FORMATION, CRAB_FORMATION, HEART_FORMATION /**BARRAGE_FORMATION, BAT_FORMATION*/);

    private static final int    DIAMOND_REGENERATE_DELAY        = 1000;
    private static final double DIAMOND_REGENERATION_PERCENTAGE = 0.01;
    private static final int    DIAMOND_MAX_REGENERATION        = 5000;
    private static final int    MOTH_WEAKEN_DELAY               = 1000;
    private static final double MOTH_WEAKEN_PERCENTAGE          = 0.01;
    private static final int    DRONE_CHANGE_COOLDOWN_TIME      = 3000;
    
    private long mLastDiamondRegenerationTime = 0L;
    private long mLastMothWeakenTime          = 0L;

    private ConcurrentHashMap<Integer, Drone> mDrones = new ConcurrentHashMap<>();

    private String mParsedDronesConfig1 = "";
    private String mParsedDronesConfig2 = "";
    private String mSelectedFormation   = DEFAULT_FORMATION;
    
    private static final String DRONE_FLAX = "drone_flax";
    private static final String DRONE_IRIS = "drone_iris";
    private static final String DRONE_APIS = "drone_apis";
    private static final String DRONE_ZEUS = "drone_zeus";

    private static final String DRONE_DESIGN_HAVOC    = "drone_designs_havoc";
    private static final String DRONE_DESIGN_HERCULES = "drone_designs_hercules";
    
    private long mDroneCooldownEndTime = 0L;
    
    public DroneManager(final Account pAccount) {
        super(pAccount);
        this.addDrones(QueryManager.loadDrones(pAccount.getUserId()));
    }

    @Override
    public void setFromJSON(final String pDroneJSON) {

    }

    @Override
    public void setNewAccount() {

    }

    @Override
    public String packToJSON() {
        return null;
    }

    public void onTickCheckMethods() {
        this.checkDiamondRegeneration();
        this.checkMothWeaken();
    }

    public void addDrones(final Collection<Drone> collection) {
        for (final Drone drone : collection) {
            this.mDrones.put(drone.getDroneId(), drone);
        }
        this.parseDronesString();
    }
   
    public void addDesignToDrone(final int pDroneId, final int pConfigId, final String pDesignLootId) {
        final Drone drone = this.mDrones.get(pDroneId);
        if (drone != null) {
            if (pConfigId == 1) {
                drone.setDesignConfig1LootId(pDesignLootId);
            } else {
                drone.setDesignConfig2LootId(pDesignLootId);
            }
            this.parseDronesString();
            if (this.getAccount()
                    .getPlayer()
                    .getCurrentConfiguration() == pConfigId) {
                this.getAccount()
                    .getPlayer()
                    .sendPacketToBoundSessions(this.getDronesPacket());
                this.getAccount()
                    .getPlayer()
                    .sendPacketToInRange(this.getDronesPacket());
            }
        }
    }

    public void removeDesignOnDrone(final int pDroneId, final int pConfigId) {
        final Drone drone = this.mDrones.get(pDroneId);
        if (drone != null) {
            if (drone != null) {
                if (pConfigId == 1) {
                    drone.setDesignConfig1LootId("");
                } else {
                    drone.setDesignConfig2LootId("");
                }
                this.parseDronesString();
            }
        }
    }

    public void parseDronesString() {
        String parsedDronesConfig1 = "0|n|d|" + this.getAccount()
                                                    .getUserId();
        for (final Drone drone : this.mDrones.values()) {
            parsedDronesConfig1 += "|" + getDroneType(drone.getDroneLootId()) + "|" + drone.getDroneLevel() + "|" +
                                   getDesignType(drone.getDesignConfig1LootId());
        }
        String parsedDronesConfig2 = "0|n|d|" + this.getAccount()
                                                    .getUserId();
        for (final Drone drone : this.mDrones.values()) {
            parsedDronesConfig2 += "|" + getDroneType(drone.getDroneLootId()) + "|" + drone.getDroneLevel() + "|" +
                                   getDesignType(drone.getDesignConfig2LootId());
        }
        this.setParsedDronesConfig1(parsedDronesConfig1);
        this.setParsedDronesConfig2(parsedDronesConfig2);
    }

    public String getDronesPacket() {
        if (this.getAccount()
                .getPlayer()
                .getCurrentConfiguration() == 1) {
            return this.mParsedDronesConfig1;
        }
        return this.mParsedDronesConfig2;
    }

    private static int getDroneType(final String pLootId) {
        switch (pLootId) {
            case DRONE_FLAX:
                return 1;
            case DRONE_IRIS:
                return 2;
            case DRONE_APIS:
                return 3;
            case DRONE_ZEUS:
                return 4;
            default:
                return 0;
        }
    }

    private static int getDesignType(final String pDesignLootId) {
        switch (pDesignLootId) {
            case DRONE_DESIGN_HAVOC:
                return 1;
            case DRONE_DESIGN_HERCULES:
                return 2;
            default:
                return 0;
        }
    }

    public short getSelectedFormation() {
        switch (this.mSelectedFormation) {
            case DEFAULT_FORMATION:
                return 0;
            case TURTLE_FORMATION:
                return 1;
            case ARROW_FORMATION:
                return 2;
            case LANCE_FORMATION:
                return 3;
            case STAR_FORMATION:
                return 4;
            case PINCER_FORMATION:
                return 5;
            case DOUBLE_ARROW_FORMATION:
                return 6;
            case DIAMOND_FORMATION:
                return 7;
            case CHEVRON_FORMATION:
                return 8;
            case MOTH_FORMATION:
                return 9;
            case CRAB_FORMATION:
                return 10;
            case HEART_FORMATION:
                return 11;
            case BARRAGE_FORMATION:
                return 12;
            case BAT_FORMATION:
                return 13;
            default:
                return 0;
        }
    }
    
    public void setSelectedFormation(final String pSelectedFormation) {
    	 final long currentTime = System.currentTimeMillis();
        if (currentTime - this.getDroneCooldownEndTime() >= 0) {
            final Player player = this.getAccount()
                                      .getPlayer();
            if (this.mSelectedFormation != pSelectedFormation) {

                this.mSelectedFormation = pSelectedFormation;
                
                this.sendFormationsEffect(pSelectedFormation);

                final DroneFormationChangeCommand droneFormationChangeCommand = new DroneFormationChangeCommand(
                        this.getAccount()
                            .getPlayer()
                            .getMapEntityId(), this.getSelectedFormation());

                this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(droneFormationChangeCommand);
                this.getAccount()
                .getPlayer()
                .sendCommandToInRange(droneFormationChangeCommand);
                
                this.setDroneCooldownEndTime(currentTime + DRONE_CHANGE_COOLDOWN_TIME);
                player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                        player.getAccount().getAmmunitionManager().getCooldownType(DEFAULT_FORMATION),
                        player.getAccount().getAmmunitionManager().getItemTimerState(DEFAULT_FORMATION), DRONE_CHANGE_COOLDOWN_TIME,
                        DRONE_CHANGE_COOLDOWN_TIME));
            }
        }
    }

    private void sendFormationsEffect(final String pNewSelectedFormation) {
        final Player player = this.getAccount()
                                  .getPlayer();
        switch (pNewSelectedFormation) {
            case DOUBLE_ARROW_FORMATION:
                if (player.getCurrentShieldPoints() > player.getMaximumShieldPoints()) {
                    player.setCurrentShieldPoints(player.getMaximumShieldPoints());
                }
                break;
            case DIAMOND_FORMATION:
                if (player.getCurrentHitPoints() > player.getMaximumHitPoints()) {
                    player.setCurrentHitPoints(player.getMaximumHitPoints());
                }
                break;
            case CHEVRON_FORMATION:
                if (player.getCurrentHitPoints() > player.getMaximumHitPoints()) {
                    player.setCurrentHitPoints(player.getMaximumHitPoints());
                }
                break;
        }
        player.sendCommandToBoundSessions(player.getShieldUpdateCommand());
        player.sendCommandToBoundSessions(player.getHitpointsUpdateCommand());
        player.sendCommandToBoundSessions(player.getSetSpeedCommand());
    }

    public int getSpeedBoost(final int pSpeed) {
        switch (this.mSelectedFormation) {
            case CRAB_FORMATION:
                return (int) Tools.getBoost(pSpeed, -20D);
            case BAT_FORMATION:
                return (int) Tools.getBoost(pSpeed, -15D);
            default:
                return pSpeed;
        }
    }
    
    public int getHitpointsBoost(final int pHitpoints) {
        switch (this.mSelectedFormation) {
            case DIAMOND_FORMATION:
                return (int) Tools.getBoost(pHitpoints, -30D);
            case CHEVRON_FORMATION:
                return (int) Tools.getBoost(pHitpoints, -20D);
            case MOTH_FORMATION:
                return (int) Tools.getBoost(pHitpoints, +20D);
            case HEART_FORMATION:
                return (int) Tools.getBoost(pHitpoints, +20D);
            default:
                return pHitpoints;
        }
    }

    public int getShieldPointsBoost(final int pShieldPoints) {
        switch (this.mSelectedFormation) {
            case TURTLE_FORMATION:
                return (int) Tools.getBoost(pShieldPoints, +10D);
            case DOUBLE_ARROW_FORMATION:
                return (int) Tools.getBoost(pShieldPoints, -20D);
            case HEART_FORMATION:
                return (int) Tools.getBoost(pShieldPoints, +10D);
            default:
                return pShieldPoints;
        }
    }

    public int getShieldAbsorbBoost(final int pShieldAbsorb) {
        switch (this.mSelectedFormation) {
            case CRAB_FORMATION:
                return (int) Tools.getBoost(pShieldAbsorb, +20D);
            case BARRAGE_FORMATION:
                return (int) Tools.getBoost(pShieldAbsorb, -15D);
            default:
                return pShieldAbsorb;
        }
    }

    public double getTargetShieldAbsorbBoost(final double pShieldAbsorb) {
        switch (this.mSelectedFormation) {
            case PINCER_FORMATION:
                return (int) Tools.getBoost(pShieldAbsorb, +10D);
            case DOUBLE_ARROW_FORMATION:
                return (int) Tools.getBoost(pShieldAbsorb, -10D);
            case MOTH_FORMATION:
                return (int) Tools.getBoost(pShieldAbsorb, -20D);
            default:
                return pShieldAbsorb;
        }
    }

    public int getLaserDamageBoost(final int pDamage, final boolean isNpc) {
        switch (this.mSelectedFormation) {
            case TURTLE_FORMATION:
                return (int) Tools.getBoost(pDamage, -7.5D);
            case ARROW_FORMATION:
                return (int) Tools.getBoost(pDamage, -3D);
            case PINCER_FORMATION:
                return (int) Tools.getBoost(pDamage, +3D);
            case HEART_FORMATION:
                return (int) Tools.getBoost(pDamage, -5D);
            case BARRAGE_FORMATION:
                return (int) Tools.getBoost(pDamage, isNpc ? +5D : 0D);
            case BAT_FORMATION:
                return (int) Tools.getBoost(pDamage, isNpc ? +8D : 0D);
            default:
                return pDamage;
        }
    }

    public int getRocketDamageBoost(final int pDamage, final boolean isNpc) {
        switch (this.mSelectedFormation) {
            case TURTLE_FORMATION:
                return (int) Tools.getBoost(pDamage, -7.5D);
            case ARROW_FORMATION:
                return (int) Tools.getBoost(pDamage, +20D);
            case STAR_FORMATION:
                return (int) Tools.getBoost(pDamage, +25D);
            case CHEVRON_FORMATION:
                return (int) Tools.getBoost(pDamage, +50D); //normal = (pDamage, +50D);
            case BAT_FORMATION:
                return (int) Tools.getBoost(pDamage, isNpc ? +8D : 0D);
            default:
                return pDamage;
        }
    }

    public int getMineDamageBoost(final int pDamage) {
        switch (this.mSelectedFormation) {
            case LANCE_FORMATION:
                return (int) Tools.getBoost(pDamage, +50D);
            default:
                return pDamage;
        }
    }

    public int getRocketLauncherReloadTimeBoost(final int pDefaultTime) {
        switch (this.mSelectedFormation) {
            case STAR_FORMATION:
                return (int) Tools.getBoost(pDefaultTime, +33D);
            default:
                return pDefaultTime;
        }
    }

    public int getHonorPointsBoost(final int pHonorPoints) {
        switch (this.mSelectedFormation) {
            case PINCER_FORMATION:
                return (int) Tools.getBoost(pHonorPoints, +5D);
            default:
                return pHonorPoints;
        }
    }

    public int getExperienceBoost(final int pExperience, final boolean isNpc) {
        switch (this.mSelectedFormation) {
            case BARRAGE_FORMATION:
                return (int) Tools.getBoost(pExperience, isNpc ? +5D : 0D);
            case BAT_FORMATION:
                return (int) Tools.getBoost(pExperience, isNpc ? +8D : 0D);
            default:
                return pExperience;
        }
    }

    public void checkDiamondRegeneration() {
        if (this.mSelectedFormation.equalsIgnoreCase(DIAMOND_FORMATION)) {
            final long currentTime = System.currentTimeMillis();
            if ((currentTime - this.getLastDiamondRegenerationTime()) > DIAMOND_REGENERATE_DELAY) {

                final Player player = this.getAccount()
                                          .getPlayer();
                final int maximumShieldPoints = player.getMaximumShieldPoints();

                int regeneration = (int) (maximumShieldPoints * DIAMOND_REGENERATION_PERCENTAGE);
                if (regeneration > DIAMOND_MAX_REGENERATION) {
                    regeneration = DIAMOND_MAX_REGENERATION;
                }

                final int currentShieldPoints = player.getCurrentShieldPoints();
                if (currentShieldPoints + regeneration > maximumShieldPoints) {
                    regeneration = maximumShieldPoints - currentShieldPoints;
                }

                player.changeCurrentShieldPoints(+regeneration);
                player.sendCommandToBoundSessions(player.getShieldUpdateCommand());

                this.setLastDiamondRegenerationTime(currentTime);
            }
        }
    }

    public void checkMothWeaken() {
        if (this.mSelectedFormation.equalsIgnoreCase(MOTH_FORMATION)) {
            final long currentTime = System.currentTimeMillis();
            if ((currentTime - this.getLastMothWeakenTime()) > MOTH_WEAKEN_DELAY) {

                final Player player = this.getAccount()
                                          .getPlayer();
                final int maximumShieldPoints = player.getMaximumShieldPoints();

                int weaken = (int) (maximumShieldPoints * MOTH_WEAKEN_PERCENTAGE);

                final int currentShieldPoints = player.getCurrentShieldPoints();
                if ((currentShieldPoints - weaken) < 0) {
                    weaken = currentShieldPoints;
                }

                player.changeCurrentShieldPoints(-weaken);
                player.sendCommandToBoundSessions(player.getShieldUpdateCommand());

                this.setLastMothWeakenTime(currentTime);
            }
        }
    }

    public long getDroneCooldownEndTime() {
        return mDroneCooldownEndTime;
    }

    public void setDroneCooldownEndTime(final long pDroneCooldownEndTime) {
    	mDroneCooldownEndTime = pDroneCooldownEndTime;
    }
    
    public long getLastDiamondRegenerationTime() {
        return mLastDiamondRegenerationTime;
    }

    public void setLastDiamondRegenerationTime(final long pLastDiamondRegenerationTime) {
        mLastDiamondRegenerationTime = pLastDiamondRegenerationTime;
    }

    public long getLastMothWeakenTime() {
        return mLastMothWeakenTime;
    }

    public void setLastMothWeakenTime(final long pLastMothWeakenTime) {
        mLastMothWeakenTime = pLastMothWeakenTime;
    }

    public String getParsedDronesConfig1() {
        return mParsedDronesConfig1;
    }

    public void setParsedDronesConfig1(final String pParsedDronesConfig1) {
        mParsedDronesConfig1 = pParsedDronesConfig1;
    }

    public String getParsedDronesConfig2() {
        return mParsedDronesConfig2;
    }

    public void setParsedDronesConfig2(final String pParsedDronesConfig2) {
        mParsedDronesConfig2 = pParsedDronesConfig2;
    }
}
