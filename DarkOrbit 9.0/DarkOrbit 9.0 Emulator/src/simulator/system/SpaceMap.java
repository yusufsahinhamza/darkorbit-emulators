package simulator.system;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import net.utils.ServerUtils;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Vector;
import java.util.concurrent.ConcurrentHashMap;

import simulator.GameManager;
import simulator.map_entities.Lockable;
import simulator.map_entities.collectables.BonusBox;
import simulator.map_entities.collectables.Resource;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.map_entities.movable.Spaceball;
import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.map_entities.stationary.ClanBattleStation;
import simulator.map_entities.stationary.Portal;
import simulator.map_entities.stationary.StationaryMapEntity;
import simulator.map_entities.stationary.stations.BattleStation;
import simulator.map_entities.stationary.stations.HomeStation;
import simulator.netty.serverCommands.AddOreCommand;
import simulator.netty.serverCommands.AssetTypeModule;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.ClientUITooltipModule;
import simulator.netty.serverCommands.ClientUITooltipsCommand;
import simulator.netty.serverCommands.DroneFormationChangeCommand;
import simulator.netty.serverCommands.EquipReadyCommand;
import simulator.netty.serverCommands.FactionModule;
import simulator.netty.serverCommands.MapAssetActionAvailableCommand;
import simulator.netty.serverCommands.OreTypeModuleCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.netty.serverCommands.SpaceMapStationModule;
import simulator.netty.serverCommands.SpaceMapStationsCommand;
import simulator.netty.serverCommands.SpacemapWindowUpdate;
import simulator.netty.serverCommands.class_436;
import simulator.netty.serverCommands.class_456;
import simulator.netty.serverCommands.class_561;
import simulator.netty.serverCommands.class_580;
import simulator.netty.serverCommands.class_667;
import simulator.netty.serverCommands.class_761;
import simulator.netty.serverCommands.class_802;
import simulator.system.clans.Diplomacy;
import simulator.users.Account;
import simulator.users.ClientSettingsManager;
import simulator.utils.DefaultAssignings;
import storage.ClanBattleStationsStorage;
import utils.MathUtils;
import utils.Settings;

/**
 This class is responsible for all on-map logic on a specific map
 */
public class SpaceMap
        implements DefaultAssignings, Runnable {

    private static final String SPACEMAP_LOGIC_THREAD_NAME = "[ServerManager Thread]";

    private static final double TICKS_PER_SECOND = 5.0D;
    private static final double TICK_INTERVAL_NS = 1000000000 / TICKS_PER_SECOND;

    public static int VISIBILITY_RANGE = 2000; // normal == 2000
    private static final int ACTIVATED_RANGE  = 300;

    private final short mSpaceMapId;
    private final short mFactionId;
    
    private final String mSpaceMapName;

    private Player mPlayer;
    
    private final boolean mStarterMap;
    private final boolean mPvpMap;

    // FACTION BASES
    private final ArrayList<HomeStation>                           mHomeStations        = new ArrayList<>();
    //STATIONARY MAP ENTITIES
    public final HashMap<Integer, ActivatableStationaryMapEntity> mActivatableEntities = new HashMap<>();
    // ALIENS
    private final ConcurrentHashMap<Integer, Alien>                mAliensMap           = new ConcurrentHashMap<>();
    // PLAYERS
    private final ConcurrentHashMap<Integer, Player>               mPlayersMap          = new ConcurrentHashMap<>();
    private final ArrayList<Integer>                               mPlayersToRemove     = new ArrayList<>();

    private final ConcurrentHashMap<Integer, Spaceball>            mSpaceballMap        = new ConcurrentHashMap<>();
    
    // RESOURCES
    private final ConcurrentHashMap<String, Resource>   mResourceMap    = new ConcurrentHashMap<>();
    // BONUS BOXES
    private final ConcurrentHashMap<String, BonusBox>   mBonusBoxesMap  = new ConcurrentHashMap<>();

    private long mLastAliensLaserAttackTime = 0L;

    // thread that is responsible for game logic simulation
    private Thread  mSimulationThread;
    private boolean mQuitRequested;

    public ActivatableStationaryMapEntity getActivatableMapEntity(final int pAssetId) {
        try {
            return this.mActivatableEntities.get(pAssetId);
        } catch (IndexOutOfBoundsException e) {
        }
        return null;
    }

    // TODO add FIREWORKS
    // TODO add SPACE BALL

    public SpaceMap(final short pSpaceMapId, final short pFactionId, final String pSpaceMapName,
                    final boolean pStarterMap, final boolean pPvpMap, final String pPortalsJSON,
                    final String pStationsJSON, final short[] pClanBattleStationIDs, final String pAliensJSON,
                    final String pCollectablesJSON, final String pBonusBoxesJSON,//
                    final String pGalaxyGatesJSON // TODO use GG
    ) {

        this.mSpaceMapId = pSpaceMapId;
        this.mFactionId = pFactionId;

        this.mSpaceMapName = pSpaceMapName;

        this.mStarterMap = pStarterMap;
        this.mPvpMap = pPvpMap;

        // PORTALS
        final JSONArray portalsJSON = new JSONArray(pPortalsJSON);
        for (int i = 0; i < portalsJSON.length(); i++) {

            final JSONObject portalJSON = portalsJSON.getJSONObject(i);

            final int pPositionX = portalJSON.getJSONArray("position")
                                             .getInt(0);
            final int positionY = portalJSON.getJSONArray("position")
                                            .getInt(1);
            final short targetSpaceMapID = (short) portalJSON.getInt("toMapID");
            final int targetPositionX = portalJSON.getJSONArray("toPosition")
                                                  .getInt(0);
            final int targetPositionY = portalJSON.getJSONArray("toPosition")
                                                  .getInt(1);
            final short levelRequirement = (short) portalJSON.getInt("level");
            final short graphicsId = (short) portalJSON.getInt("gfxID");
            final short factionIconID = (short) portalJSON.getInt("factionScrap");
            
            
            final boolean visible = portalJSON.getBoolean("visible");
            final boolean working = portalJSON.getBoolean("working");

            final Portal portal =
                    new Portal(this, pPositionX, positionY, targetSpaceMapID, targetPositionX, targetPositionY,
                               levelRequirement, graphicsId, factionIconID, visible, working);

            this.mActivatableEntities.put(portal.getMapEntityId(), portal);

        }

        // STATIONS
        final JSONArray stationsJSON = new JSONArray(pStationsJSON);
        for (int i = 0; i < stationsJSON.length(); i++) {

            final JSONObject station = stationsJSON.getJSONObject(i);

            final short stationTypeId = (short) station.getInt("type");
            final short factionId = (short) station.getInt("factionID");
            final int positionX = station.getInt("x");
            final int positionY = station.getInt("y");

            switch (stationTypeId) {
                case AssetTypeModule.BASE_COMPANY:
                    this.mHomeStations.add(new HomeStation(this, factionId, positionX, positionY));
                    break;
                case AssetTypeModule.ASTEROID:
                    final BattleStation battleStation = new BattleStation(this, positionX, positionY);
                    this.mActivatableEntities.put(battleStation.getMapEntityId(), battleStation);
                    break;
                default:
                    break;
            }
        }

        // CLAN BATTLE STATIONS
        if (pClanBattleStationIDs != null) {
            for (short cbsID : pClanBattleStationIDs) {
                final ClanBattleStation cbs = ClanBattleStationsStorage.getClanBattleStation(cbsID);
                //                this.mClanBattleStations.put(cbs.getClanBattleStationId(), cbs);
            }
        }

        // ALIENS
        final JSONArray aliensJSON = new JSONArray(pAliensJSON);
        for (int i = 0; i < aliensJSON.length(); i++) {

            final JSONObject alienJSON = aliensJSON.getJSONObject(i);

            final int aliensAmount = alienJSON.getInt("amount");
            final int alienShipID = alienJSON.getInt("shipID");

            this.addAliens(aliensAmount, alienShipID);

        }

        //CUBİKONLAR
        if(this.getSpaceMapId() == 18 || this.getSpaceMapId() == 22 || this.getSpaceMapId() == 26) {
	        this.addCubikon(14000, 4000); //SAĞ ÜST
	        this.addCubikon(14000, 9000); // SAĞ ALT
	        this.addCubikon(6000, 4000); // SOL ÜST
	        this.addCubikon(6000, 9000); // SOL ALT
        }
        
        
        // COLLECTABLES: RESOURCES, BONUS BOXES, PIRATE BOXES
        final JSONArray collectablesJSON = new JSONArray(pCollectablesJSON);

        for (int i = 0; i < collectablesJSON.length(); i++) {

            final JSONObject collectable = collectablesJSON.getJSONObject(i);

            final int amount = collectable.getInt("amount");

            final int top = collectable.getJSONArray("topLeft")
                                       .getInt(0);
            final int left = collectable.getJSONArray("topLeft")
                                        .getInt(1);
            final int bottom = collectable.getJSONArray("bottomRight")
                                          .getInt(0);
            final int right = collectable.getJSONArray("bottomRight")
                                         .getInt(1);

            final short collectableID = (short) collectable.getInt("collectableID");
            this.addResource(collectableID, top, left, bottom, right, amount);
        }

        // COLLECTABLES: RESOURCES, BONUS BOXES, PIRATE BOXES
        final JSONArray bonusboxesJSON = new JSONArray(pBonusBoxesJSON);

        for (int i = 0; i < bonusboxesJSON.length(); i++) {

            final JSONObject bonusbox = bonusboxesJSON.getJSONObject(i);

            final int amount = bonusbox.getInt("amount");

            final int top = bonusbox.getJSONArray("topLeft")
                                       .getInt(0);
            final int left = bonusbox.getJSONArray("topLeft")
                                        .getInt(1);
            final int bottom = bonusbox.getJSONArray("bottomRight")
                                          .getInt(0);
            final int right = bonusbox.getJSONArray("bottomRight")
                                         .getInt(1);

            final short bonusboxID = (short) bonusbox.getInt("collectableID");
            this.addBonusBox(bonusboxID, top, left, bottom, right, amount);
        }
        
        this.mSimulationThread = new Thread(this);
    }

    public void startSimulation() {
        this.prepareAllStations();

        this.mSimulationThread = new Thread(this);
        this.mSimulationThread.setName(SPACEMAP_LOGIC_THREAD_NAME);
        this.mSimulationThread.start();
    }

    public void run() {
        try {
            while (!this.mQuitRequested) {

                this.tick();
                Thread.sleep(100);

            }
        } catch (InterruptedException e) {
        }
        this.quit();
    }

    public void requestQuit() {
        // TODO use
        this.mQuitRequested = true;
    }

    private void quit() {
        //TODO end simulation properly
    }

    private void addAliens(final int pAmount, final int pShipId) {
        for (int i = 0; i < pAmount; i++) {
            this.addAlien(new Alien(pShipId, this));
        }
    }

    public void addCubikon(final int positionX, final int positionY) {
    	final Alien alien = new Alien(80, this, positionX, positionY);
    	alien.constantPositionX = positionX;
    	alien.constantPositionY = positionY;
    	this.addAlien(alien);
    }
    
    public void addSpaceball(final Spaceball pSpaceball) {
        this.mSpaceballMap.put(pSpaceball.getSpaceballId(), pSpaceball);
    }
    
    public void addAlien(final Alien pAlien) {
        this.mAliensMap.put(pAlien.getAlienId(), pAlien);
    }
    
    public void addResource(final short pCollectableID, final int pTop, final int pLeft, final int pBottom,
                            final int pRight, final int pAmount) {
        for (int i = 0; i < pAmount; i++) {
            final Resource resource = new Resource(pCollectableID, pTop, pLeft, pBottom, pRight);
            this.mResourceMap.put(resource.getHash(), resource);
        }
    }
    
    public void addBonusBox(final short pCollectableID, final int pTop, final int pLeft, final int pBottom,
            final int pRight, final int pAmount) {
        for (int i = 0; i < pAmount; i++) {
            final BonusBox bonusBox = new BonusBox(pCollectableID, pTop, pLeft, pBottom, pRight);
            this.mBonusBoxesMap.put(bonusBox.getHash(), bonusBox);
        }
    }
    
    private void prepareAllStations() {
        for (final HomeStation homeStation : this.mHomeStations) {
            homeStation.prepareStations();
        }
    }

    public void tick() {
        this.checkMovableMapEntitiesRange();
    }

    private synchronized void checkMovableMapEntitiesRange() {
        final Collection<MovableMapEntity> allMapEntities = this.getAllMovableMapEntities()
                                                                .values();
        for (final MovableMapEntity thisMapEntity : allMapEntities) {
        	
			if(thisMapEntity instanceof Player) {
				if(thisMapEntity.isDestroyed()) {
					this.removePlayer(thisMapEntity.getMapEntityId());
				}
			}
			if(thisMapEntity instanceof Alien) {
				if(thisMapEntity.isDestroyed()) {
					this.removeAlien(thisMapEntity.getMapEntityId());
				}
			}
			
            thisMapEntity.doTick();
            final int thisPositionX = thisMapEntity.getCurrentPositionX();
            final int thisPositionY = thisMapEntity.getCurrentPositionY();
            final Lockable thisLockedTarget = thisMapEntity.getLockedTarget();

            for (final MovableMapEntity otherMapEntity : allMapEntities) {
                //ignore thisMapEntity
                if (!thisMapEntity.equals(otherMapEntity)) {
                    final int otherPositionX = otherMapEntity.getCurrentPositionX();
                    final int otherPositionY = otherMapEntity.getCurrentPositionY();
                    final Lockable otherLockedTarget = otherMapEntity.getLockedTarget();

                    if ((Math.abs(thisPositionX - otherPositionX) <= VISIBILITY_RANGE) &&
                        (Math.abs(thisPositionY - otherPositionY) <= VISIBILITY_RANGE)) {
                        //both map entities are in range of each other

                        if (!thisMapEntity.hasMapEntityInRange(otherMapEntity)) {
                            //add otherMapEntity to thisMapEntity range
                            thisMapEntity.addInRangeMapEntity(otherMapEntity);

                            if (thisMapEntity instanceof Player) {
                                final Player thisPlayer = (Player) thisMapEntity;
                                short relationType = 0;
                                boolean sameClan = false;
                                
                                if(otherMapEntity instanceof Player) {
                                	final Player otherPlayer = (Player) otherMapEntity;     
                                	
                                    if(thisPlayer.getAccount().getClan() != null && otherPlayer.getAccount().getClan() != null) {
                                        if(otherPlayer.getAccount().getClanId() == thisPlayer.getAccount().getClanId()) {
                                        	sameClan = true;
                                        } else {
	                                        final List<Diplomacy> dips = thisPlayer.getAccount().getClan().getDiplomacies();
	                                        int otherPlayerClanID = otherPlayer.getAccount().getClanId();
	                                        for(Diplomacy dip : dips){
	                                            if(dip.clanID1 == otherPlayerClanID || dip.clanID2 == otherPlayerClanID){
	                                            	relationType = (short) dip.relationType;
	                                            }
	                                        }
                                        }
                                    }
                                    
                                    if(thisPlayer.getCurrentSpaceMapId() == Settings.DUEL_MAP) {                                	
                                    	if(otherPlayer.getDuelUser() == thisPlayer && thisPlayer.getDuelUser() == otherPlayer) {
                                    		thisPlayer.sendCommandToBoundSessions(otherPlayer.getShipCreateCommand(relationType, sameClan));
                                    	}                        	
                                    } else if(thisPlayer.getCurrentSpaceMapId() == 42) {
                                    	thisPlayer.sendCommandToBoundSessions(otherPlayer.getJPBShipCreateCommand(relationType));
                                    } else {
                                    	thisPlayer.sendCommandToBoundSessions(otherPlayer.getShipCreateCommand(relationType, sameClan));
                                    }
                                    
                                    if(otherPlayer.getAccount().getPetManager().isActivated()) {
                                    	thisPlayer.sendCommandToBoundSessions(otherPlayer.getPetCreateCommand(relationType));
                                    }                
                                    
                                    thisPlayer.sendPacketToBoundSessions(otherPlayer.getAccount()
                                            										.getDroneManager()
                                            										.getDronesPacket());
                                    
                                    thisPlayer.sendCommandToBoundSessions(new DroneFormationChangeCommand(otherPlayer.getMapEntityId(), otherPlayer.getAccount().getDroneManager().getSelectedFormation()));
                                    
                                    if(thisPlayer.getCurrentSpaceMapId() != 42) {
                                    	thisPlayer.sendPacketToBoundSessions("0|n|t|" + otherPlayer.getAccount()
                                        														   .getUserId() + 
                                        									 "|0|" + otherPlayer.getAccount()
                                        									 					.getTitle());
                                    }
                                    
                                } else {
                                	thisPlayer.sendCommandToBoundSessions(otherMapEntity.getShipCreateCommand(relationType, sameClan));
                                }
                                
                                if (thisMapEntity.getMovement()
                                                 .isMovementInProgress()) {
                                	thisPlayer.sendCommandToBoundSessions(otherMapEntity.getMovement()
                                                                                        .getMoveCommand());
                                }
                            }
                        }
                        if (!otherMapEntity.hasMapEntityInRange(thisMapEntity)) {
                            //add thisMapEntity to otherMapEntity range
                            otherMapEntity.addInRangeMapEntity(thisMapEntity);

                            if (otherMapEntity instanceof Player) {
                                final Player otherPlayer = (Player) otherMapEntity;
                                short relationType = 0;
                                boolean sameClan = false;
                                
                                if(thisMapEntity instanceof Player) {
                                	final Player thisPlayer = (Player) thisMapEntity;     
                                	
                                    if(thisPlayer.getAccount().getClan() != null && otherPlayer.getAccount().getClan() != null) {
                                        if(otherPlayer.getAccount().getClanId() == thisPlayer.getAccount().getClanId()) {
                                        	sameClan = true;
                                        } else {
	                                        final List<Diplomacy> dips = otherPlayer.getAccount().getClan().getDiplomacies();
	                                        int thisPlayerClanID = thisPlayer.getAccount().getClanId();
	                                        for(Diplomacy dip : dips){
	                                            if(dip.clanID1 == thisPlayerClanID || dip.clanID2 == thisPlayerClanID){
	                                            	relationType = (short) dip.relationType;
	                                            }
	                                        }
                                        }
                                    }
                                    
                                    if(otherPlayer.getCurrentSpaceMapId() == Settings.DUEL_MAP) {                                	
                                    	if(otherPlayer.getDuelUser() == thisPlayer && thisPlayer.getDuelUser() == otherPlayer) {
                                    		otherPlayer.sendCommandToBoundSessions(thisPlayer.getShipCreateCommand(relationType, sameClan));
                                    	}                        	
                                    } else if(otherPlayer.getCurrentSpaceMapId() == 42) {
                                    	otherPlayer.sendCommandToBoundSessions(thisPlayer.getJPBShipCreateCommand(relationType));
                                    } else {
                                    	otherPlayer.sendCommandToBoundSessions(thisPlayer.getShipCreateCommand(relationType, sameClan));
                                    }
                                    
                                    if(thisPlayer.getAccount().getPetManager().isActivated()) {
                                    	otherPlayer.sendCommandToBoundSessions(thisPlayer.getPetCreateCommand(relationType));
                                    }
                                    
                                    otherPlayer.sendPacketToBoundSessions(thisPlayer.getAccount()
                                            										.getDroneManager()
                                            										.getDronesPacket());
                                    
                                    otherPlayer.sendCommandToBoundSessions(new DroneFormationChangeCommand(thisPlayer.getMapEntityId(), thisPlayer.getAccount().getDroneManager().getSelectedFormation()));
                                    
                                    if(otherPlayer.getCurrentSpaceMapId() != 42) {
                                    	otherPlayer.sendPacketToBoundSessions("0|n|t|" + thisPlayer.getAccount()
                                        														   .getUserId() + 
                                        									 "|0|" + thisPlayer.getAccount()
                                        									 					.getTitle());
                                    }
                                    
                                } else {
                                	otherPlayer.sendCommandToBoundSessions(thisMapEntity.getShipCreateCommand(relationType, sameClan));
                                }
                                
                                if (thisMapEntity.getMovement()
                                                 .isMovementInProgress()) {
                                    otherPlayer.sendCommandToBoundSessions(thisMapEntity.getMovement()
                                                                                        .getMoveCommand());
                                }
                            }       
                        }


                    } else if ((thisMapEntity instanceof Alien && thisLockedTarget instanceof Player) ||
                            ((thisLockedTarget == null || !thisLockedTarget.equals(otherMapEntity)) &&
                             (otherLockedTarget == null || !otherLockedTarget.equals(thisMapEntity)))) {
                     //map entities are not in range of each other and any of them is locking the other

                     if (thisMapEntity.hasMapEntityInRange(otherMapEntity)) {
                         final ShipRemoveCommand shipDestroyCommand =
                                 thisMapEntity.removeInRangeMapEntity(otherMapEntity);
                         if (thisMapEntity instanceof Player) {
                             ((Player) thisMapEntity).sendCommandToBoundSessions(shipDestroyCommand);
                         }
                     }
                     if (otherMapEntity.hasMapEntityInRange(thisMapEntity)) {
                         final ShipRemoveCommand shipDestroyCommand =
                                 otherMapEntity.removeInRangeMapEntity(thisMapEntity);
                         if (otherMapEntity instanceof Player) {
                             ((Player) otherMapEntity).sendCommandToBoundSessions(shipDestroyCommand);
                         }
                     }
                 }
             }
         }
     }
 }
    
    private synchronized void checkPlayersToRemove() {
        for (final int playerId : this.mPlayersToRemove) {
            final Player player = this.mPlayersMap.remove(playerId);
            for (final MovableMapEntity movableMapEntity : player.getInRangeMovableMapEntities()) {
                final ShipRemoveCommand shipRemoveCommand = movableMapEntity.removeInRangeMapEntity(player);
                if (movableMapEntity instanceof Player) {
                    ((Player) movableMapEntity).sendCommandToBoundSessions(shipRemoveCommand);
                }
            }
            player.removeAllInRangeMapIntities();
        }
        this.mPlayersToRemove.clear();
    }

    public void onPlayerMovement(final Player pPlayer) {
        final boolean inRadiationChanged = this.checkRadiation(pPlayer);
        final boolean assetsChanged = this.checkActivatables(pPlayer);
        if (inRadiationChanged || assetsChanged) {
            pPlayer.sendCommandToBoundSessions(pPlayer.getBeaconCommand());
        }
    }

    private boolean checkRadiation(final Player pPlayer) {
        final int positionX = pPlayer.getCurrentPositionX();
        final int positionY = pPlayer.getCurrentPositionY();

        boolean inRadiationZone = false;
        if (this.getSpaceMapId() == 16) {
            inRadiationZone = positionX < 0 || positionX > 41800 || positionY < 0 || positionY > 26000;
        } else {
            inRadiationZone = positionX < 0 || positionX > 20900 || positionY < 0 || positionY > 13000;
        }
        if (pPlayer.isInRadiationZone() != inRadiationZone) {
            pPlayer.setInRadiationZone(inRadiationZone);
            return true;
        }
    	 return false;
    }
    
    /**
     Description: Send assets(portals, stations, ...) button ON/OFF

     @param pPlayer
     GameSession to activate/deactivate buttons for & sets demilitarized zone status
     */
    private boolean checkActivatables(final Player pPlayer) {
        final ArrayList<ActivatableStationaryMapEntity> activatables = new ArrayList<>();
        activatables.addAll(this.mActivatableEntities.values());
        for (final HomeStation homeStation : this.mHomeStations) {
            activatables.add(homeStation);
            activatables.addAll(homeStation.getActivatables());
        }

        boolean isInSecureZone = false;
        boolean inEquipZone = false;

        for (final ActivatableStationaryMapEntity entity : activatables) {

            final boolean inRange = MathUtils.hypotenuse(entity.getCurrentPositionX() - pPlayer.getCurrentPositionX(),
                                                         entity.getCurrentPositionY() -
                                                         pPlayer.getCurrentPositionY()) <= entity.getActivatedRange();

            final short status = inRange ? MapAssetActionAvailableCommand.ON : MapAssetActionAvailableCommand.OFF;

            if (entity instanceof HomeStation) {
                switch (entity.getAssetType()) {
                    case AssetTypeModule.BASE_COMPANY:
                        if (MathUtils.hypotenuse(pPlayer.getCurrentPositionX() - entity.getCurrentPositionX(),
                                                 pPlayer.getCurrentPositionY() - entity.getCurrentPositionY()) <=
                            HomeStation.SECURE_ZONE_RANGE) {            	
                        	if(((HomeStation) entity).getFactionId() == pPlayer.getAccount().getFactionId()) {                     		
                        		final long currentTime = System.currentTimeMillis();
                        		if ((currentTime - pPlayer.getLastDamagedTime()) >= 5000) {
                        			if((currentTime - pPlayer.getLaserAttack().getLastShotTime()) >= 1000) {
                        				if(!pPlayer.getLaserAttack().isAttackInProgress()) {
                                    		inEquipZone = true;
                                    		isInSecureZone = true;
                        				}
                        			}
                        		}
                        	}
                        	pPlayer.inInSecureZone = true;
                        } else {
                        	pPlayer.inInSecureZone = false;
                        }
                        break;
                    default:
                        // do nothing if base type not defined
                        break;
                }
            } else if (entity instanceof Portal) {
                if (MathUtils.hypotenuse(pPlayer.getCurrentPositionX() - entity.getCurrentPositionX(),
                                         pPlayer.getCurrentPositionY() - entity.getCurrentPositionY()) <=
                    Portal.SECURE_ZONE_RANGE) {
                	if(pPlayer.getAccount().getFactionId() == this.getFactionId()) {
                		final long currentTime = System.currentTimeMillis();
                		if ((currentTime - pPlayer.getLastDamagedTime()) >= 5000) {
                			if((currentTime - pPlayer.getLaserAttack().getLastShotTime()) >= 1000) {
                				if(!pPlayer.getLaserAttack().isAttackInProgress()) {
                					isInSecureZone = true;
                				}
                			}
                		}
                	}
                    pPlayer.inInSecureZone = true;
                } else {
                	pPlayer.inInSecureZone = false;
                }
            }

            final boolean activateButton = pPlayer.updateActivatable(entity, inRange);

            if (activateButton) {

                final MapAssetActionAvailableCommand assetAction =
                        new MapAssetActionAvailableCommand(entity.getMapEntityId(),// asset id
                                                           status,// action ON/OFF
                                                           inRange, // can be activated
                                                           new ClientUITooltipsCommand(
                                                                   new Vector<ClientUITooltipModule>()), //
                                                           new class_580() //
                        );
                pPlayer.sendCommandToBoundSessions(assetAction);
            }

        }

        if (pPlayer.isInEquipZone() != inEquipZone) {
            pPlayer.setInEquipZone(inEquipZone);
            pPlayer.sendCommandToBoundSessions(new EquipReadyCommand(inEquipZone));
            if(pPlayer.isInEquipZone()) {
                pPlayer.sendPacketToBoundSessions("0|A|STM|msg_equip_ready");
                QueryManager.saveAccount(pPlayer.getAccount());
            } else {
            	pPlayer.sendPacketToBoundSessions("0|A|STM|msg_equip_not_ready");
            	QueryManager.saveAccount(pPlayer.getAccount());
            }
        }
        if (pPlayer.isInSecureZone() != isInSecureZone) {
            pPlayer.setInSecureZone(isInSecureZone);
            return true;
        }
    	return false;
    }
    
    public void removeGameSession(final GameSession pGameSession) {
        final Account account = pGameSession.getAccount();
        account.getPlayer()
               .removeAllInRangeMapIntities();
        account.getPlayer()
               .setCurrentInRangePortalId(INVALID_ID);
        this.mPlayersMap.remove(pGameSession.getPlayer());
        
        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();
        final ClientSettingsManager clientSettingsManager = account.getClientSettingsManager();

        // 1. send UI (menu bars: top right & top left)
        gscc.sendToSendCommand(clientSettingsManager.getClientUIMenuBarsCommand());
        //2. wtf is this?
        gscc.sendToSendCommand(new class_761(new Vector<class_436>()));
        // 3. send Clan BattleStations
        this.sendSpacemapWindowsBattleStations(pGameSession);
        // 4. windows
        this.sendWindows(pGameSession);
        // 5. ship init command
        gscc.sendToSendCommand(account.getPlayer()
                                .getShipInitializationCommand());
        gscc.sendToSendCommand(new SpacemapWindowUpdate(true, true));
        //Received Legacy JCPU|GET
    }
    
    public void addAndInitGameSession(final GameSession pGameSession) {
    	try{
        final Account account = pGameSession.getAccount();
        account.getPlayer().removeAllInRangeMapIntities();
        account.getPlayer().setCurrentInRangePortalId(INVALID_ID);
        this.mPlayersMap.put(account.getUserId(), pGameSession.getPlayer());

        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();
        final ClientSettingsManager clientSettingsManager = account.getClientSettingsManager();

		gscc.sendToSendCommand(account.getClientSettingsManager().getUserSettingsCommand());
		gscc.sendToSendCommand(account.getClientSettingsManager().getClientUIMenuBarsCommand());
		gscc.sendToSendCommand(account.getClientSettingsManager().getClientUISlotBarsCommand());
		gscc.sendToSendCommand(account.getClientSettingsManager() .getUserKeyBindingsUpdateCommand());
        gscc.sendToSendCommand(clientSettingsManager.getClientUIMenuBarsCommand());
        gscc.sendToSendCommand(new class_761(new Vector<class_436>()));
        this.sendWindows(pGameSession);
        gscc.sendToSendCommand(account.getPlayer()
                                .getShipInitializationCommand());        
        gscc.sendToSendCommand(new SpacemapWindowUpdate(true, true));
        
        
    	for(final MovableMapEntity movableMapEntity : this.getAllPlayers()) {
    		final Player otherPlayer = (Player) movableMapEntity;
    		final Player thisPlayer = account.getPlayer();
    		
    		short relationType = 0;
    		boolean sameClan = false;
    		if(thisPlayer.getAccount().getClan() != null && otherPlayer.getAccount().getClan() != null) {
                if(otherPlayer.getAccount().getClanId() == thisPlayer.getAccount().getClanId()) {
                	sameClan = true;
                } else {
                    final List<Diplomacy> dips = otherPlayer.getAccount().getClan().getDiplomacies();
                    int thisPlayerClanID = thisPlayer.getAccount().getClanId();
                    for(Diplomacy dip : dips){
                        if(dip.clanID1 == thisPlayerClanID || dip.clanID2 == thisPlayerClanID){
                        	relationType = (short) dip.relationType;
                        }
                    }
                }
            }
	
    		if(account.getPlayer().getCurrentSpaceMapId() == Settings.DUEL_MAP) {
    			if(otherPlayer.getDuelUser() == thisPlayer && thisPlayer.getDuelUser() == otherPlayer) {
    				otherPlayer.sendCommandToBoundSessions(account.getPlayer().getShipCreateCommand(relationType, sameClan));
    			}
    		} else {
    			otherPlayer.sendCommandToBoundSessions(account.getPlayer().getShipCreateCommand(relationType, sameClan));
    		}

            
            if(thisPlayer.getAccount().getPetManager().isActivated()) {
            	otherPlayer.sendCommandToBoundSessions(thisPlayer.getPetCreateCommand(relationType));
            }
            
            otherPlayer.sendPacketToBoundSessions(thisPlayer.getAccount()
                    										.getDroneManager()
                    										.getDronesPacket());
            
            otherPlayer.sendCommandToBoundSessions(new DroneFormationChangeCommand(thisPlayer.getMapEntityId(), thisPlayer.getAccount().getDroneManager().getSelectedFormation()));
            
            if(otherPlayer.getCurrentSpaceMapId() != 42) {
            	otherPlayer.sendPacketToBoundSessions("0|n|t|" + thisPlayer.getAccount()
                														   .getUserId() + 
                									 "|0|" + thisPlayer.getAccount()
                									 					.getTitle());
            }

    	}

    	}catch(Exception e) {   		
    	}
        //Received Legacy JCPU|GET
    }
    
    private void sendWindows(final GameSession pGameSession) {
        Vector<class_667> windows = new Vector<>();
        windows.add(new class_667(class_667.JUMP_GATES));
        windows.add(new class_667(class_667.ATTACK));
        windows.add(new class_667(class_667.EXTRA_CPU));
        windows.add(new class_667(class_667.TRAINING_GROUNDS));
        windows.add(new class_667(class_667.TECH_FACTORY));
        windows.add(new class_667(class_667.THE_SHOP));
        windows.add(new class_667(class_667.CHANGING_SHIPS));
        windows.add(new class_667(class_667.JUMP_DEVICE));
        windows.add(new class_667(class_667.GALAXY_GATE));
        windows.add(new class_667(class_667.SECOND_CONFIGURATION));
        windows.add(new class_667(class_667.AUCTION_HOUSE));
        windows.add(new class_667(class_667.PREPARE_BATTLE));
        windows.add(new class_667(class_667.SKYLAB));
        windows.add(new class_667(class_667.SHIP_REPAIR));
        windows.add(new class_667(class_667.POLICY_CHANGES));
        windows.add(new class_667(class_667.INSTALLING_NEW_EQUIPMENT));
        windows.add(new class_667(class_667.FULL_CARGO));
        windows.add(new class_667(class_667.ITEM_UPGRADE));
        windows.add(new class_667(class_667.BOOST_YOUR_EQUIP));
        windows.add(new class_667(class_667.ORE_TRANSFER));
        windows.add(new class_667(class_667.CLAN_BATTLE_STATION));
        windows.add(new class_667(class_667.HOW_TO_FLY));
        windows.add(new class_667(class_667.SELL_RESOURCE));
        windows.add(new class_667(class_667.LOOKING_FOR_GROUPS));
        windows.add(new class_667(class_667.SHIP_DESIGN));
        windows.add(new class_667(class_667.CONTACT_LIST));
        windows.add(new class_667(class_667.UNKOWN_DANGERS));
        windows.add(new class_667(class_667.WEALTHY_FAMOUS));
        windows.add(new class_667(class_667.short_955));
        windows.add(new class_667(class_667.GET_MORE_AMMO));
        windows.add(new class_667(class_667.REQUEST_MISSION));
        windows.add(new class_667(class_667.ROCKET_LAUNCHER));
        windows.add(new class_667(class_667.WELCOME));
        windows.add(new class_667(class_667.PALLADIUM));
        windows.add(new class_667(class_667.EQUIP_YOUR_ROCKETS));
        windows.add(new class_667(class_667.PVP_WARNING));
        windows.add(new class_667(class_667.SKILL_TREE));
        class_802 c802 = new class_802(windows);
        pGameSession.getGameServerClientConnection()
                    .sendToSendCommand(c802);
    }

    public void sendBaseStations(final GameSession pGameSession) {
        ArrayList<Integer> questGiversIds = new ArrayList<>();
        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();
        for (final HomeStation homeStation : this.mHomeStations) {
            final Collection<StationaryMapEntity> stations = homeStation.getAllStations();
            stations.add(homeStation);
            for (final StationaryMapEntity stationaryEntities : stations) {
                gscc.sendToSendCommand(stationaryEntities.getAssetCreateCommand());
            }
            questGiversIds.addAll(homeStation.getQuestGivers());
        }
        //give missions
        Vector<class_561> vc561 = new Vector<>();
        for (final Integer questGiverEntityId : questGiversIds) {
            vc561.add(new class_561(questGiverEntityId, questGiverEntityId,
                                    new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>())));
        }
        class_456 c456 = new class_456(vc561);
        gscc.sendToSendCommand(c456);

    }

    public void sendActivatableMapEntities(final GameServerClientConnection pGameServerClientConnection) {
        for (final ActivatableStationaryMapEntity activatableStationaryMapEntity : this.mActivatableEntities.values()) {
            pGameServerClientConnection.sendToSendCommand(activatableStationaryMapEntity.getAssetCreateCommand());
            if (activatableStationaryMapEntity instanceof BattleStation) {
                activatableStationaryMapEntity.handleClick(pGameServerClientConnection.getGameSession());
            }
        }
    }

    public void sendResources(final GameSession pGameSession) {
        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();

        for (final Resource resource : this.mResourceMap.values()) {
            gscc.sendToSendCommand(
                    new AddOreCommand(resource.getHash(), new OreTypeModuleCommand(resource.getCollectableId()),
                                      resource.getCurrentPositionX(), resource.getCurrentPositionY()));
        }
    }

    public void sendBonusBoxes(final GameSession pGameSession) {
        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();

        for (final BonusBox bonusbox : this.mBonusBoxesMap.values()) {           
            gscc.sendPacket("0|c|"+bonusbox.getHash()+"|2|" + bonusbox.getCurrentPositionX() + "|" + bonusbox.getCurrentPositionY());
        }
    }
    
    private void sendSpacemapWindowsBattleStations(final GameSession pGameSession) {
        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();

        Vector<SpaceMapStationModule> pStations = new Vector<>();
        for (final ActivatableStationaryMapEntity activatableStationaryMapEntity : this.mActivatableEntities.values()) {

            if (activatableStationaryMapEntity instanceof BattleStation) {
                final BattleStation cbs = (BattleStation) activatableStationaryMapEntity;
                SpaceMapStationModule pStation =
                        new SpaceMapStationModule(this.getSpaceMapId(), cbs.getCurrentPositionX(),
                                                  cbs.getCurrentPositionY(), cbs.getStatus(), //
                                                  0.0D,//
                                                  "",//clan name
                                                  cbs.getAssetName(),// CBS Name
                                                  new FactionModule((short) 1)// faction id
                        );
                pStations.add(pStation);
            }
        }
        SpaceMapStationsCommand stations = new SpaceMapStationsCommand(90000.0D, pStations);
        gscc.sendToSendCommand(stations);
    }

    //    /**
    //     Description: Creates BonusBox
    //
    //     @param count:
    //     amount of bonusbox to create
    //     */
    //    public void createBonusBox(int count, int gfxID, Point topLeft, Point bottomRight) {
    //        for (int i = 0; i < count; i++) {
    //            int x = Tools.getRandom(topLeft.getX(), bottomRight.getX());
    //            int y = Tools.getRandom(topLeft.getY(), bottomRight.getY());
    //            BonusBox bb = new BonusBox("bb" + mBonusBoxes.size(), gfxID, x, y);
    //            mBonusBoxes.put(bb.collectableID, bb);
    //        }
    //    }
    //
    //    /**
    //     Description: Creates PirateBox
    //
    //     @param count:
    //     amount of piratebox to create
    //     */
    //    public void createPirateBox(int count, int gfxID, Point topLeft, Point bottomRight) {
    //        for (int i = 0; i < count; i++) {
    //            int x = Tools.getRandom(topLeft.getX(), bottomRight.getX());
    //            int y = Tools.getRandom(topLeft.getY(), bottomRight.getY());
    //            PirateBox pb = new PirateBox("pb" + mPirateBoxes.size(), gfxID, x, y);
    //            mPirateBoxes.put(pb.collectableID, pb);
    //        }
    //    }
    //
    //    /**
    //     Description: Creates prometium
    //
    //     @param count:
    //     amount of prometium to create
    //     */
    //    public void createPrometium(int count, Point topLeft, Point bottomRight) {
    //        for (int i = 0; i < count; i++) {
    //            int x = Tools.getRandom(topLeft.getX(), bottomRight.getX());
    //            int y = Tools.getRandom(topLeft.getY(), bottomRight.getY());
    //            Resource resource = new Resource("pr" + mResources.size(), 1, x, y);
    //            mResources.put(resource.collectableID, resource);
    //        }
    //    }
    //
    //    /**
    //     Description: Creates endurium
    //
    //     @param count:
    //     amount of prometium to create
    //     */
    //    public void createEndurium(int count, Point topLeft, Point bottomRight) {
    //        for (int i = 0; i < count; i++) {
    //            int x = Tools.getRandom(topLeft.getX(), bottomRight.getX());
    //            int y = Tools.getRandom(topLeft.getY(), bottomRight.getY());
    //            Resource resource = new Resource("en" + mResources.size(), 2, x, y);
    //            mResources.put(resource.collectableID, resource);
    //        }
    //    }
    //
    //    /**
    //     Description: Creates terbium
    //
    //     @param count:
    //     amount of terbium to create
    //     */
    //    public void createTerbium(int count, Point topLeft, Point bottomRight) {
    //        for (int i = 0; i < count; i++) {
    //            int x = Tools.getRandom(topLeft.getX(), bottomRight.getX());
    //            int y = Tools.getRandom(topLeft.getY(), bottomRight.getY());
    //            Resource resource = new Resource("te" + mResources.size(), 3, x, y);
    //            mResources.put(resource.collectableID, resource);
    //        }
    //    }
    //
    //    /**
    //     Description: Creates palladium
    //
    //     @param count:
    //     amount of palladium to create
    //     */
    //    public void createPalladium(int count, Point topLeft, Point bottomRight) {
    //        for (int i = 0; i < count; i++) {
    //            int x = Tools.getRandom(topLeft.getX(), bottomRight.getX());
    //            int y = Tools.getRandom(topLeft.getY(), bottomRight.getY());
    //            Resource resource = new Resource("pa" + mResources.size(), 15, x, y);
    //            mResources.put(resource.collectableID, resource);
    //        }
    //    }


    public boolean isStarterMap() {
        return this.mStarterMap;
    }

    public boolean isPvpMap() {
        return this.mPvpMap;
    }
    
    public Collection<BonusBox> getBonusBoxes() {
        return this.mBonusBoxesMap.values();
    }
    
    public short getFactionId() {
        return this.mFactionId;
    }

    public short getSpaceMapId() {
        return this.mSpaceMapId;
    }

    public String getSpaceMapName() {
        return this.mSpaceMapName;
    }

    public Collection<Player> getAllPlayers() {
        return this.mPlayersMap.values();
    }

    public Collection<Alien> getAllAliens() {
        return this.mAliensMap.values();
    }
    
    public HashMap<Integer, MovableMapEntity> getAllMovableMapEntities() {
        HashMap<Integer, MovableMapEntity> allMovableMapEntities = new HashMap<>();
        allMovableMapEntities.putAll(this.mAliensMap);
        allMovableMapEntities.putAll(this.mPlayersMap);
        allMovableMapEntities.putAll(this.mSpaceballMap);
        return allMovableMapEntities;
    }

    public void removeGameSessionOnMap(final int userID) {
        this.mPlayersMap.remove(userID);
    }

    public long getLastAliensLaserAttackTime() {
        return mLastAliensLaserAttackTime;
    }

    public void setLastAliensLaserAttackTime(final long pLastAliensLaserAttackTime) {
        mLastAliensLaserAttackTime = pLastAliensLaserAttackTime;
    }

    public void addPlayerToRemoveList(final int pPlayerId) {
        this.mPlayersToRemove.add(pPlayerId);
    }
    
    public void removePlayer(final int pPlayerId) {
    	this.mPlayersMap.remove(pPlayerId);
    }
    
    public void removeAlien(final int pAlienId) {
        this.mAliensMap.remove(pAlienId);
    }
   
	public void removeSpaceball(int pSpaceballId) {
		this.mSpaceballMap.remove(pSpaceballId);
	}
	
	public void removeBonusBox(final BonusBox pBonusBox) {
		this.mBonusBoxesMap.remove(pBonusBox);
	}
	
    public void removeSpaceballForAllInRangeMapIntities(final int pSpaceballId) {
    	final Spaceball spaceball = mSpaceballMap.remove(pSpaceballId);
        if(spaceball != null)  {
            for (final MovableMapEntity movableMapEntity : spaceball.getInRangeMovableMapEntities()) {
                final ShipRemoveCommand shipRemoveCommand = movableMapEntity.removeInRangeMapEntity(spaceball);
                if (movableMapEntity instanceof Player) {
                    ((Player) movableMapEntity).sendCommandToBoundSessions(shipRemoveCommand);
                }
            }
            spaceball.removeAllInRangeMapIntities();
        }
    }
}

