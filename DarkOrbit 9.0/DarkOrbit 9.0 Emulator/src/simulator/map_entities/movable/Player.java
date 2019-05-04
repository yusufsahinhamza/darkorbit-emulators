package simulator.map_entities.movable;

import mysql.QueryManager;
import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;
import net.utils.ServerUtils;

import org.json.JSONArray;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.HashSet;
import java.util.Locale;
import java.util.Timer;
import java.util.TimerTask;
import java.util.Vector;
import java.util.concurrent.ConcurrentHashMap;

import simulator.GameManager;
import simulator.map_entities.AttackableMapEntity;
import simulator.map_entities.Lockable;
import simulator.map_entities.MapEntity;
import simulator.map_entities.stationary.ActivatableStationaryMapEntity;
import simulator.map_entities.stationary.Portal;
import simulator.netty.ServerCommand;
import simulator.netty.serverCommands.AttackHitCommand;
import simulator.netty.serverCommands.AttackTypeModule;
import simulator.netty.serverCommands.AttributeHitpointUpdateCommand;
import simulator.netty.serverCommands.AttributeShieldUpdateCommand;
import simulator.netty.serverCommands.BeaconCommand;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.LegacyModule;
import simulator.netty.serverCommands.PetActivationCommand;
import simulator.netty.serverCommands.SetSpeedCommand;
import simulator.netty.serverCommands.ShipCreateCommand;
import simulator.netty.serverCommands.ShipDestroyCommand;
import simulator.netty.serverCommands.ShipInitializationCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.netty.serverCommands.ShipSelectionCommand;
import simulator.netty.serverCommands.ShipWarpModule;
import simulator.netty.serverCommands.ShipWarpWindowCommand;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.netty.serverCommands.WordPuzzleLetterAchievedCommand;
import simulator.netty.serverCommands.WordPuzzleLetterModule;
import simulator.netty.serverCommands.WordPuzzleWindowInitCommand;
import simulator.netty.serverCommands.class_365;
import simulator.system.SpaceMap;
import simulator.system.ships.PlayerShip;
import simulator.system.ships.ShipsConstants;
import simulator.users.Account;
import simulator.users.CpusManager;
import storage.ShipStorage;
import storage.SpaceMapStorage;
import utils.Settings;
import utils.Tools;
import simulator.netty.serverCommands.ClientUITooltipTextFormatModule;
import simulator.netty.serverCommands.DestructionTypeModule;
import simulator.netty.serverCommands.KillScreenOptionModule;
import simulator.netty.serverCommands.KillScreenOptionTypeModule;
import simulator.netty.serverCommands.KillScreenPostCommand;
import simulator.netty.serverCommands.MessageLocalizedWildcardCommand;
import simulator.netty.serverCommands.MessageWildcardReplacementModule;
import simulator.netty.serverCommands.PriceModule;

/**
 Player class is responsible for holding player on-map state
 Edited,added etc... fucking sooo much time by LEJYONER.
 */

public class Player
        extends AttackableMapEntity {

    private static final float PLAYER_CARGO_CAPACITY_MULTIPLIER_MIN = 1.0f;

    private final Account mAccount;
//    private final GameManager  mGameManager;
    private PlayerShip mPlayerShip;
    private Player mDuelUser;
    // ==============================================================

    public HashSet<ActivatableStationaryMapEntity> mInRangeAssets = new HashSet<>();

    private int mCurrentInRangePortalId = INVALID_ID;
    
    private boolean mInSecureZone    = false;
    private boolean mInRadiationZone = false;
    private boolean mInEquipZone     = false;
    public boolean mCloaked; //kamuflaj var veya yok
    private boolean mIsJumping = false;

    // -> Ship:equippedItems something like that
    public int currentRocketLauncher = 1;

    public long mRadiationZoneLastDamageTime = 0L;
    public long mLastSendKillScreenTime = 0L;
    public  long  mLastTimeConnected = 0;
    private long mConfigurationCooldownEndTime = 0;
    private int  mCurrentCargoCapacity;

    // Game sessions that show this player
    public ArrayList<GameSession> mGameSessions = new ArrayList<>();

    public int mAliensCount = 0;
    // ==============================================================

    private short mCurrentConfiguration;
    
    private int mMaximumHitPoints;
    private int mCurrentHitPoints;
    private int mMaximumNanoHull;
    private int mCurrentNanoHull;
    private int mMaximumShieldPointsConfig1;
    private int mCurrentShieldPointsConfig1;
    private int mMaximumShieldPointsConfig2;
    private int mCurrentShieldPointsConfig2;
    private int mCurrentShieldAbsorb = 80;

    private boolean mIsDestroyed = false;
    private boolean mConfigurationChanged;
    public boolean inInSecureZone = true;
    
    public String boxHash = "";
    public int alienID = 0;

    private final ConcurrentHashMap<Integer, MovableMapEntity> mRedMarks = new ConcurrentHashMap<>();
    
    public Collection<MovableMapEntity> getRedMarks() {
        return this.mRedMarks.values();
    }
    
    public void addRedMark(final MovableMapEntity pObject) {
    	this.mRedMarks.put(pObject.getMapEntityId(), pObject);
    }
    
    public void removeRedMark(final MovableMapEntity pObject) {
    	this.mRedMarks.remove(pObject);
    }
    
    public Player(final Account pAccount, final SpaceMap pCurrentSpaceMapId, final PlayerShip pPlayerShip) {
        super(pCurrentSpaceMapId, pAccount.getUserId());

        this.setPlayerShip(pPlayerShip);
        this.setMaximumHitPoints(pPlayerShip.getBaseHitPoints());
        this.setCurrentHitPoints(pPlayerShip.getBaseHitPoints());

        this.mAccount = pAccount;
        this.init(pAccount);
        if(this.mAccount.isCloaked()) {
        	this.mAccount.getCpusManager().getSelectedCpus().add(CpusManager.CLK_XL);
        }
     /**   if(this.getAccount().isAdmin()) {
        //ZETA KAPISI
     	final Portal portal = new Portal (this.getCurrentSpaceMap(), 15500, 5000, (short) 71, 10450, 6500, (short) 1, (short) 54, (short) 1, true, true);
     	this.getCurrentSpaceMap().mActivatableEntities.put(portal.getMapEntityId(), portal);
     	this.sendCommandToBoundSessions(portal.getAssetCreateCommand());
        }
        */
    }

    // ==============================================================

    private void init(final Account pAccount) {
        //TODO set all needed values from managers in Account
    	this.setPositionXY(pAccount.getPositionX(), pAccount.getPositionY());
    }

    public ShipWarpWindowCommand getShipWarpWindowCommand() {
    	Vector<ShipWarpModule> ships = new Vector<ShipWarpModule>();
    	if(QueryManager.checkShip(this.getAccount().getUserId(), "cyborg"))  { ships.add(new ShipWarpModule(ShipsConstants.CYBORG,ShipStorage.getShipLootID(ShipsConstants.CYBORG),"Cyborg",0,0,1,"")); }
    	if(QueryManager.checkShip(this.getAccount().getUserId(), "surgeon")) { ships.add(new ShipWarpModule(ShipsConstants.SURGEON,ShipStorage.getShipLootID(ShipsConstants.SURGEON),"Surgeon",0,0,2,"")); }
    	if(QueryManager.checkShip(this.getAccount().getUserId(), "surgeonCicada")) { ships.add(new ShipWarpModule(ShipsConstants.SURGEON_CICADA,ShipStorage.getShipLootID(ShipsConstants.SURGEON_CICADA),"Surgeon Cicada",0,0,3,"")); }
    	if(QueryManager.checkShip(this.getAccount().getUserId(), "surgeonLocust")) { ships.add(new ShipWarpModule(ShipsConstants.SURGEON_LOCUST,ShipStorage.getShipLootID(ShipsConstants.SURGEON_LOCUST),"Surgeon Locust",0,0,4,"")); } 
    	
    	
    	ships.add(new ShipWarpModule(ShipsConstants.G_CHAMPION_LEGEND,ShipStorage.getShipLootID(ShipsConstants.G_CHAMPION_LEGEND),"★ G-Champion Legend",0,0,5,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_GOAL,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_GOAL),"★ G-Goal",0,0,6,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_INDEPENDENCE,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_INDEPENDENCE),"★ G-Independence",0,0,7,""));
    	
    	
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_RAZER,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_RAZER),"G-Razer",0,0,8,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_KICK,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_KICK),"G-Kick",0,0,9,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH,ShipStorage.getShipLootID(ShipsConstants.GOLIATH),"Goliath",0,0,10,""));
     	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_ENFORCER,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_ENFORCER),"G-Enforcer",0,0,11,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_BASTION,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_BASTION),"G-Bastion",0,0,12,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_DIMINISHER,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_DIMINISHER),"G-Diminisher",0,0,13,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_REFEREE,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_REFEREE),"G-Referee",0,0,14,""));
    	
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SATURN,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SATURN),"G-Saturn",0,0,15,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SENTINEL,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SENTINEL),"G-Sentinel",0,0,16,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SENTINEL_FROST,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SENTINEL_FROST),"G-Sentinel Frost",0,0,17,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SENTINEL_LEGEND,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SENTINEL_LEGEND),"G-Sentinel Legend",0,0,18,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SOLACE,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SOLACE),"G-Solace",0,0,19,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SPECTRUM,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SPECTRUM),"G-Spectrum",0,0,20,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SPECTRUM_FROST,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SPECTRUM_FROST),"G-Spectrum Frost",0,0,21,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_SPECTRUM_LEGEND,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_SPECTRUM_LEGEND),"G-Spectrum Legend",0,0,22,""));
    	ships.add(new ShipWarpModule(ShipsConstants.GOLIATH_VENOM,ShipStorage.getShipLootID(ShipsConstants.GOLIATH_VENOM),"G-Venom",0,0,23,""));
       /**	
        * VERİTABANINDAKİ TÜM GEMİLERİ EKLEME
        * 
        * for (final StorageShip ship : ShipStorage.getStorageShipCollection()) {
    		int i = 1;
    		i++;
    		if(ship.getShipId() != player.getPlayerShip().getShipId()) {
    			ships.add(new ShipWarpModule(ship.getShipId(),ship.getShipLootId(),ship.getShipName(),0,0,i,""));
    		}
    	}
       	*/
       	
    	return new ShipWarpWindowCommand(0,0,this.inInSecureZone,ships);
    }
    
    public WordPuzzleLetterAchievedCommand getWordPuzzleLetterAchievedCommand(boolean pCompleted) {
    	this.sendCommandToBoundSessions(new WordPuzzleWindowInitCommand(Settings.harfSayisi));
    	Vector<WordPuzzleLetterModule> letters = new Vector<WordPuzzleLetterModule>();
    	letters.add(new WordPuzzleLetterModule(Settings.harf1, this.getAccount().puzzleLetters.get("puzzleLetter1") == true ? 0: -1));
    	letters.add(new WordPuzzleLetterModule(Settings.harf2, this.getAccount().puzzleLetters.get("puzzleLetter2") == true ? 1: -1));
    	letters.add(new WordPuzzleLetterModule(Settings.harf3, this.getAccount().puzzleLetters.get("puzzleLetter3") == true ? 2: -1));
    	letters.add(new WordPuzzleLetterModule(Settings.harf4, this.getAccount().puzzleLetters.get("puzzleLetter4") == true ? 3: -1));    
    	letters.add(new WordPuzzleLetterModule(Settings.harf5, this.getAccount().puzzleLetters.get("puzzleLetter5") == true ? 4: -1)); 
    	letters.add(new WordPuzzleLetterModule(Settings.harf6, this.getAccount().puzzleLetters.get("puzzleLetter6") == true ? 5: -1)); 
    	letters.add(new WordPuzzleLetterModule(Settings.harf7, this.getAccount().puzzleLetters.get("puzzleLetter7") == true ? 6: -1)); 
    	letters.add(new WordPuzzleLetterModule(Settings.harf8, this.getAccount().puzzleLetters.get("puzzleLetter8") == true ? 7: -1)); 
    	letters.add(new WordPuzzleLetterModule(Settings.harf9, this.getAccount().puzzleLetters.get("puzzleLetter9") == true ? 8: -1)); 
    	letters.add(new WordPuzzleLetterModule(Settings.harf10, this.getAccount().puzzleLetters.get("puzzleLetter10") == true ? 9: -1)); 
    	return new WordPuzzleLetterAchievedCommand(letters, pCompleted ? true : false);
    }
    
    public Player getDuelUser() {
    	return this.mDuelUser;
    }
    
    public void setDuelUser(final Player pDuelUser) {
    	this.mDuelUser = pDuelUser;
    }
    
    // ==============================================================

    public Account getAccount() {
        return this.mAccount;
    }

    // ==============================================================

    /**
     @return current UserShip of this player
     */
    public PlayerShip getPlayerShip() {
        return this.mPlayerShip;
    }

    /**
     @param pPlayerShip
     new UserShip to assign
     */
    private void setPlayerShip(final PlayerShip pPlayerShip) {
        this.mPlayerShip = pPlayerShip;
    }

    public void changePlayerShip(final PlayerShip pPlayerShip) {
    	final Player player = this;
    	final GameSession gameSession = GameManager.getGameSession(player.getMapEntityId());
    	final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(player.getCurrentSpaceMapId());
    	player.setPlayerShip(pPlayerShip);
    	spaceMap.removePlayer(player.getMapEntityId());
		spaceMap.addAndInitGameSession(gameSession);
        QueryManager.saveAccount(gameSession.getAccount());
    }
    
    public void setLoadAccount() {
        final Account loadAccount = QueryManager.loadAccount(this.getAccount().getUserId());
    	final long uridium = loadAccount.getUridium();
    	final long credits = loadAccount.getUridium();
    	final long experience = loadAccount.getExperience();
    	final long honor = loadAccount.getHonor();
    	this.getAccount().setUridium(uridium);
    	this.getAccount().setCredits(credits);
    	this.getAccount().setExperience(experience);
    	this.getAccount().setHonor(honor);        
    	this.getAccount().setOnline(false);
    	this.setInEquipZone(true);  	    	   	
        QueryManager.saveAccount(this.getAccount());
    }
    
    public ShipCreateCommand getShipCreateCommand(short relationType, boolean sameClan) {
    	final Vector<VisualModifierCommand> vmc = new Vector<VisualModifierCommand>();
    	if(this.getAccount().isAdmin()) {
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.BLUE_SIGNAL, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.DAMAGE_ICON, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.JPA_CAMERA, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.LEONOV_EFFECT, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.RED_SIGNAL, 0, "", 1, true));
    	}
        return new ShipCreateCommand(this.getMapEntityId(), // user id gibi bir şey
        		                     this.getShipLootId(), // gemi loot id
        		                     this.getAccount()
                                         .getExpansionStage(), // gemi kargo alanı
                                     this.getAccount()
                                         .getClanTag(), // klan tag ismi
                                     this.getAccount()
                                         .getShipUsername(), // kullanıcı ismi
                                     this.getCurrentPositionX(), // oyuncu x pozisyonu
                                     this.getCurrentPositionY(), // oyuncu y pozisyonu
                                     this.getAccount()
                                         .getFactionId(), 
                                     this.getAccount()
                                         .getKilledGoliath() >= 100 ? 100 : 63, // yüzük sayısı (100 = taç / 63 = 6 halka)
                                     this.getAccount()
                                         .getRankId(), //rütbe id
                                     this.getAccount()
                                         .getRankId() == 21 ? true : false, // haritadaki noktası (admin ise büyük)
                                     new ClanRelationModule(sameClan ? ClanRelationModule.ALLIED : relationType), // clan durumu (1= ittifak / 2= nap / 3= savaş)
                                     0, // bilmiyorum
                                     false, // icon can change ship (base)
                                     false, // not an NPC
                                     this.getAccount()
                                         .isCloaked(), // kamufle var veya yok
                                     sameClan ? ClanRelationModule.NON_AGGRESSION_PACT : ClanRelationModule.NONE, // oyun içi isim rengi (aynı klan yeşil)
                                     sameClan ? ClanRelationModule.ALLIED : relationType, // oyuncunun mini haritadaki rengi
                                     vmc, // visual modifiers
                                     new class_365(class_365.DEFAULT) // clan relation type on minimap
        );
    }
  
    public ShipCreateCommand getJPBShipCreateCommand(short relationType) {
    	final Vector<VisualModifierCommand> vmc = new Vector<VisualModifierCommand>();
    	if(this.getAccount().isAdmin()) {
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.BLUE_SIGNAL, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.DAMAGE_ICON, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.JPA_CAMERA, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.LEONOV_EFFECT, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.RED_SIGNAL, 0, "", 1, true));
    	}
        return new ShipCreateCommand(this.getMapEntityId(), // user id gibi bir şey
        		                     this.getShipLootId(), // gemi loot id
        		                     this.getAccount()
                                         .getExpansionStage(), // gemi kargo alanı
                                     "", // klan tag ismi
                                     "*****", // kullanıcı ismi
                                     this.getCurrentPositionX(), // oyuncu x pozisyonu
                                     this.getCurrentPositionY(), // oyuncu y pozisyonu
                                     this.getAccount()
                                         .getFactionId(), 
                                     this.getAccount()
                                         .getKilledGoliath() >= 100 ? 100 : 63, // yüzük sayısı (100 = taç / 63 = 6 halka)
                                //     this.getAccount()
                                //         .getGalaxyGatesManager()
                                //         .getRingsCount(),//rings
                                     this.getAccount()
                                         .getRankId(), //rütbe id
                                     this.getAccount()
                                         .getRankId() == 21 ? true : false, // haritadaki noktası (admin ise büyük)
                                     new ClanRelationModule(ClanRelationModule.NONE), // clan durumu (1= ittifak / 2= nap / 3= savaş)
                                     0, // bilmiyorum
                                     false, // icon can change ship (base)
                                     false, // not an NPC
                                     this.getAccount()
                                         .isCloaked(), // kamufle var veya yok
                                     ClanRelationModule.NONE, // oyun içi isim rengi (aynı klan yeşil)
                                     ClanRelationModule.NONE, // oyuncunun mini haritadaki rengi
                                     vmc, // visual modifiers
                                     new class_365(class_365.DEFAULT) // clan relation type on minimap
        );
    }
    
    public PetActivationCommand getPetCreateCommand(short relationType) {
        return new PetActivationCommand(this.getAccount() // kullanıcı id
        									.getUserId(), 
						        		this.getAccount() // pet id
						        			.getPetManager()
						        			.getPetID(), 
						        		22, // ship id
						        		3, //ambar
                                        this.getAccount() // pet ismi
                                        	.getPetName() != null ? this.getAccount().getPetName() : "PET.10", 
						        		this.getAccount() // pet şirket id
						        			.getFactionId(), 
						        		this.getAccount() // pet clan id
						        			.getClanId(), 
						        		15, // pet seviye
						        		this.getAccount() // pet clan tag
						        			.getClanTag(), 
						        		new ClanRelationModule(relationType), // pet clan durumu (1= ittifak / 2= nap / 3= savaş)
						        		this.getCurrentPositionX(), // pet x
						        		this.getCurrentPositionY(), // pet y
						        		this.getSpeed(), // pet hız
						        		false, 
						        		true, 
						        		new class_365(class_365.DEFAULT)
        );
    }
    
    @Override
    public void doTick() {
        super.doTick();
        this.getMovement()
            .move();
        this.getAccount()
        	.getAmmunitionManager()
        	.onTickCheckMethods();
        this.getLaserAttack()
            .attack();
        this.getRocketAttack()
            .checkRocketAttackSystem();
        this.getRocketLauncherAttack()
        	.checkRocketLauncherAttackSystem();
        this.getAccount()
            .getSkillsManager()
            .onTickCheckMethods();
        this.getAccount()
            .getPetManager()
            .onTickCheckMethods();
        this.getAccount()
            .getDroneManager()
            .onTickCheckMethods();
        this.getAccount()
            .getTechsManager()
            .onTickCheckMethods();
        this.getAccount()
        	.getResourcesManager()
        	.doTick();
        this.checkUserInRadiationZone();
        this.damageEntity();
        this.checkAbilities();
        this.checkUserMovement();
        this.checkSelectedFormation();
        this.checkMyShip();
        if(!this.getAccount().isAdmin()) {
        	if(this.getSpeed() > 500) {
        		this.sendPacketToBoundSessions("0|A|STD|Hız sınırını aşıyorsunuz ve bu bir ban sebebidir, dikkatli olunuz!");
        		final GameSession gameSession = GameManager.getGameSession(this.getAccount().getUserId());
        		gameSession.close();
        	}
        }
     this.checkUserInGalaxyGates();
    }
    
    public int mDalga = 0;
    public int mRewardGived = 0;
    
    private long mSaniyeCooldown         = 0L;
    public int mSaniye = 16;
    
    public void checkUserInGalaxyGates() {
    	final Player player = this;
    	
    	if(this.getCurrentSpaceMapId() == 71) { //ZETA
    		final long currentTime = System.currentTimeMillis();
            for (int i = 15; i > 0; i--) {
            	if(mSaniye != 0) {
	            	if((currentTime - mSaniyeCooldown) >= 0) {
	            		mSaniye--;
	            		mSaniyeCooldown = currentTime + 1000;
	            		player.sendPacketToBoundSessions("0|A|STD|-=- "+mSaniye+" -=-");
	            	}
            	}
            }
            
            if(mSaniye == 0) {
	    		if(mDalga == 0) {
	    			if(this.getCurrentSpaceMap().getAllAliens().size() == 0) {
	    				this.createAlien(100, 3);
	    				mDalga = 1;	
	    				player.sendPacketToBoundSessions("0|A|STD|Dalga: "+mDalga+"");
	    			}
	    		} else if(mDalga == 1) {
	    			if(this.getCurrentSpaceMap().getAllAliens().size() == 0) {
	    				this.createAlien(100, 4);
	    				mDalga = 2;	
	    				player.sendPacketToBoundSessions("0|A|STD|Dalga: "+mDalga+"");
	    			}	
	    		} else if(mDalga == 2) {
	    			if(this.getCurrentSpaceMap().getAllAliens().size() == 0) {
	    				this.createAlien(100, 3);
	    				mDalga = 3;	
	    				player.sendPacketToBoundSessions("0|A|STD|Dalga: "+mDalga+"");
	    			}	
	    		} else if(mDalga == 3) {
	    			if(this.getCurrentSpaceMap().getAllAliens().size() == 0) {
	    				this.createAlien(100, 4);
	    				mDalga = 4;	
	    				player.sendPacketToBoundSessions("0|A|STD|Dalga: "+mDalga+"");
	    			}
	    		} else if(mDalga == 4) {
	    			if(this.getCurrentSpaceMap().getAllAliens().size() == 0) {
	        			this.createAlien(100, 5);
	    				mDalga = 5;	
	    				player.sendPacketToBoundSessions("0|A|STD|Dalga: "+mDalga+"");
	    			}
	    		}  else if(mDalga == 5) {
	    			if(this.getCurrentSpaceMap().getAllAliens().size() == 0) {
	        			this.createAlien(99, 3);
	    				mDalga = 6;	
	    				player.sendPacketToBoundSessions("0|A|STD|Dalga: "+mDalga+"");
	    			}
	    		} else if(this.getCurrentSpaceMap().getAllAliens().size() == 0 && mDalga == 6) {
	    			
	    			if(mRewardGived == 0) {
		    			int posX = 0, posY = 0;
		    			short mapID = 0;
		    			
		            	if(player.getAccount().getFactionId() == 1) {
		            		posX = 2000;
		            		posY = 6000;
		            		mapID = 20;
		            	} else if(player.getAccount().getFactionId() == 2) {
		            		posX = 10000;
		            		posY = 2000;
		            		mapID = 24;
		            	} else if(player.getAccount().getFactionId() == 3) {
		            		posX = 18500;
		            		posY = 6000;
		            		mapID = 28;
		            	}
		            	try {
							Thread.sleep(5000);
						} catch (InterruptedException e) {
							e.printStackTrace();
						}
		    			this.giveReward(35000,6000000,200000);
		    			player.getAccount().getDroneManager().addDrones(QueryManager.loadDrones(this.getAccount().getUserId()));;
		    			this.jumpPortal(mapID, posX, posY);
	    		  }
	    			
	    		}
    	}
    	}
    }
    
    public void createAlien(int pShipID, int pAmount) {
    	final Player player = this;
        for (int i = pAmount; i > 0; i--) {
            final Alien alien = new Alien(pShipID, player.getCurrentSpaceMap(), player);
			player.getCurrentSpaceMap()
			.addAlien(alien);
        }
    }
    
    public void giveReward(int pUridium, int pExperience, int pHonor) {
    	if(mRewardGived == 0) {
    		mRewardGived = 1;
	    	final Player player = this;
	    	QueryManager.addDroneDesign(player.getAccount(), "drone_designs_havoc");
	    	player.getAccount()
	                    .changeExperience(+pExperience);
	    	player.getAccount()
	                    .changeHonor(+pHonor);
	    	player.getAccount()
	                    .changeUridium(+pUridium);
	        QueryManager.saveAccount(player.getAccount());
	        
	        player.sendPacketToBoundSessions("0|LM|ST|EP|" + pExperience + "|" + player.getAccount()
	                                                                                       .getExperience() + "|" +
	                                                                                       player.getAccount()
	                                                           .getLevel());
	        player.sendPacketToBoundSessions("0|LM|ST|HON|" + pHonor + "|" + player.getAccount()
	                                                                                        .getHonor());
	        player.sendPacketToBoundSessions("0|LM|ST|URI|" + pUridium + "|" + player.getAccount()
	                .getUridium()); 
    	}
    }
    
    public void jumpPortal(final short mapID, final int posX, final int posY) {
    	final Player player = this;
        if (!player.isJumping()) {
            player.setJumping(true);
            
            new Timer().schedule(new TimerTask() {

                @Override
                public void run() {    
                	
                    boolean petWillOpen = false;                    
                    if(player.getAccount().getPetManager().isActivated()) {
                    	player.getAccount().getPetManager().Deactivate(); 
                    	petWillOpen = true;
                    }
                    
                    for(Alien al: player.getCurrentSpaceMap().getAllAliens()){
                    	if(al != null) {
	                        for (final MovableMapEntity movableMapEntity : al.getInRangeMovableMapEntities()) {
	                            if (movableMapEntity instanceof Player) {
	                                Player pl = ((Player) movableMapEntity);
	                                if(pl.getAccount().getUserId() == player.getAccount().getUserId()){
	                                    al.setLockedTarget(null);
	                                }
	                            }
	                        }
                    	}
                    }
                    
                    final ShipRemoveCommand shipRemoveCommand = new ShipRemoveCommand(player.getAccount().getUserId());
                    for (MovableMapEntity inRangeEntity : player.getCurrentSpaceMap().getAllPlayers()) {
                    	if(inRangeEntity != null) {
                    		if(inRangeEntity instanceof Player) {
                            	Player inRangePlayers = (Player) inRangeEntity;
                            	inRangePlayers.sendCommandToBoundSessions(shipRemoveCommand);
                    		}
                    	}
                    }
                    
                    player.getCurrentSpaceMap()
                	      .removePlayer(player.getMapEntityId());
                    player.getMovement()
                	      .setMovementInProgress(false);
                    player.setPositionXY(posX, posY);
                    player.setCurrentSpaceMap(mapID);
                    player.setCurrentInRangePortalId(INVALID_ID);
                    player.setJumping(false);
                    try {
						Thread.sleep(Portal.JUMP_DELAY_NOW);
					} catch (InterruptedException e) {
					}
                    GameManager.tryJump(player.getMapEntityId(), mapID);                   
                    player.sendPacketToBoundSessions("0|A|STD|Atlama tamamlandı!");
                    
                    if(petWillOpen) {
                    	player.getAccount().getPetManager().Activate(); 
                    	petWillOpen = false;
                    }
                }
            }, Portal.JUMP_DELAY);
        }
    }
    
    public void loadAccount() {
        	final Account loadAccount = QueryManager.loadAccount(this.getAccount().getUserId());
        	final long uridium = loadAccount.getUridium();
        	final int rocketDmgUp = loadAccount.getRocketDmgUp();
        	final int repairUp = loadAccount.getRepairUp();
        	if(uridium != this.getAccount().getUridium()) {
        	this.sendPacketToBoundSessions("0|A|C|U|" + uridium + "|" + this.getAccount()
                    .getUridium());
        	this.getAccount().setUridium(uridium);
        	}
        	if(rocketDmgUp != this.getAccount().getRocketDmgUp()) {
        		this.getAccount().setRocketDmgUp(rocketDmgUp);
        	}
        	if(repairUp != this.getAccount().getRepairUp()) {
        		this.getAccount().setRepairUp(repairUp);
        	}
    }
    
    public void checkMyShip() {
    	final GameSession gameSession = GameManager.getGameSession(this.getAccount().getUserId());  
    	if(gameSession == null || gameSession.getGameServerClientConnection() == null || this.isDestroyed()) {
    			final ShipRemoveCommand shipRemoveCommand = new ShipRemoveCommand(this.getAccount().getUserId());
    			ServerUtils.sendCommandToAllInMap(this.getCurrentSpaceMapId(), shipRemoveCommand);
    	}
    }
    
    public void checkUserInRadiationZone() {
    	final Player player = this;
        final long currentTime = System.currentTimeMillis();

        if (player.isInRadiationZone()) {
            if ((currentTime - this.getRadiationZoneLastDamageTime()) >= 1000) {

                int damage = 20000;

                if(!player.canBeShoot()) return;
                
            	this.changeCurrentHitPoints(-damage);
                final AttackHitCommand attackHitCommand =
                        new AttackHitCommand(new AttackTypeModule(AttackTypeModule.RADIATION), 0,//attackerID
                        		this.getMapEntityId(), this.getCurrentHitPoints(),
                        		this.getCurrentShieldPoints(), this.getCurrentNanoHull(),
                                             damage, false);
                
                player.sendCommandToBoundSessions(attackHitCommand);
                this.sendCommandToInRange(attackHitCommand);
        		
                this.setLastCheckedDamageTime(currentTime);
                this.setLastDamagedTime(currentTime);
                
                this.setRadiationZoneLastDamageTime(currentTime);
                
                if(damage > player.getCurrentHitPoints() || player.getCurrentHitPoints() <= 0)
                {
                	for(final MovableMapEntity inRangeEntity : this.getInRangeMovableMapEntities()) {
                		if(inRangeEntity != null) {
	                		if(inRangeEntity instanceof Player) {
	                			final Player otherPlayer = (Player) inRangeEntity;
	                			if(otherPlayer.getLaserAttack().isAttackInProgress() && otherPlayer.getLockedTarget() == this) {
	                				this.destroy(otherPlayer);
	                			} else {
	                				this.sendRadiationZoneKillScreen();
	                			}
	                		} else if(inRangeEntity instanceof Alien) {	                			
	                			final Alien alien = (Alien) inRangeEntity;
	                			if(alien.getLaserAttack().isAttackInProgress() && alien.getLockedTarget() == this) {
	                				this.destroy(alien);
	                			} else {
	                				this.sendRadiationZoneKillScreen();
	                			}	                			
	                		}
                		} else {
                			this.sendRadiationZoneKillScreen();
                		}
                	}
                }
            }
        }
    }
     
    public void sendRadiationZoneKillScreen() {
    	
    	this.setDestroyed(true);
    	this.getCurrentSpaceMap().removeGameSessionOnMap(this.getAccount().getUserId());
		this.getLaserAttack().setAttackInProgress(false);
		this.setLockedTarget(null);
        
    	final long currentTime = System.currentTimeMillis();
    	if ((currentTime - this.getLastSendKillScreenTime()) >= 0) {
    		this.setLastSendKillScreenTime(currentTime + 999999999);
        final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap((short) 42);
        if(this.getCurrentSpaceMapId() == 42) {
        final int kalanOyuncu = (spaceMap.getAllPlayers().size()) - 1;
        final String geriSayim = "0|LM|ST|SLE|"+ kalanOyuncu;
        ServerUtils.sendPacketAllUsersOnMap(42, geriSayim);
        }
        
        final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(this.getMapEntityId(), 1);
        this.sendCommandToInRange(shipDestroyCommand);
        this.sendCommandToBoundSessions(shipDestroyCommand);

        final Vector<KillScreenOptionModule> killScreenOptionModules = new Vector<>();
        final KillScreenOptionModule killScreenOptionModule =
                new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_REPAIR),
                                           new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                           new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                               new ClientUITooltipTextFormatModule(
                                                                                       ClientUITooltipTextFormatModule.LOCALIZED),
                                                                               new Vector<MessageWildcardReplacementModule>()),
                                           new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                               new ClientUITooltipTextFormatModule(
                                                                                       ClientUITooltipTextFormatModule.LOCALIZED),
                                                                               new Vector<MessageWildcardReplacementModule>()),
                                           new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                               new ClientUITooltipTextFormatModule(
                                                                                       ClientUITooltipTextFormatModule.LOCALIZED),
                                                                               new Vector<MessageWildcardReplacementModule>()),
                                           new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                               new ClientUITooltipTextFormatModule(
                                                                                       ClientUITooltipTextFormatModule.LOCALIZED),
                                                                               new Vector<MessageWildcardReplacementModule>()));
        killScreenOptionModules.add(killScreenOptionModule);
        final KillScreenPostCommand killScreenPostCommand =
                new KillScreenPostCommand("", "http://localhost/indexInternal.es?action=internalDock",
                                          "MISC", new DestructionTypeModule(DestructionTypeModule.RADITATION),
                                          killScreenOptionModules);
        this.sendCommandToBoundSessions(killScreenPostCommand);
        this.setInEquipZone(true);
        QueryManager.saveAccount(this.getAccount()); 
    	}
    }
    
    public void checkUserMovement() {
        if(this.getMovement().isMovementInProgress()) {
        	if (this.checkLogoutProcessRunning()) {
        		this.stopLogoutProcess();
        	}
        }
        for (final MovableMapEntity movableMapEntity : this.getInRangeMovableMapEntities()) {
            if (movableMapEntity instanceof Player) {
            	if(movableMapEntity.getLockedTarget() == this && movableMapEntity.getLaserAttack().isAttackInProgress()) {
            		if (this.checkLogoutProcessRunning()) {
            			this.stopLogoutProcess();
            		}
            	}
            }
        }
    }
    
    public void checkSelectedFormation() {
        if(this.getAccount().getDroneManager().getSelectedFormation() == 9) {
            for (final MovableMapEntity thisMapEntity : this.getInRangeMovableMapEntities()) {
                if(thisMapEntity instanceof Player)
                {               	                    
                    final Player otherPlayer = (Player) thisMapEntity;

                    if(this.getLockedTarget() == otherPlayer && this.getLaserAttack().isAttackInProgress()) {
                        otherPlayer.setCurrentShieldAbsorb(70);
                    } else {
                    	otherPlayer.setCurrentShieldAbsorb(80);
                    }
                }                             	
            }
        } else if(this.getAccount().getDroneManager().getSelectedFormation() == 10) {
            for (final MovableMapEntity thisMapEntity : this.getInRangeMovableMapEntities()) {
                if(thisMapEntity instanceof Player)
                {               	                    
                    final Player otherPlayer = (Player) thisMapEntity;

                    if(otherPlayer.getLaserAttack().isAttackInProgress() && otherPlayer.getLockedTarget() == this) {
                        this.setCurrentShieldAbsorb(80);
                    } else {
                    	this.setCurrentShieldAbsorb(100);
                    }
                }                             	
            }
        }
    }
    
    public void checkAbilities() {
    	final long currentTime = System.currentTimeMillis();     	    	   	
    	if(this.getAccount().isCloaked()) {
            final String deactiveDiminisher = "0|SD|D|R|2|" + this.getAccount().getUserId() + "";
            this.sendPacketToInRange(deactiveDiminisher);
            final String deactiveSpectrum = "0|SD|D|R|3|" + this.getAccount().getUserId() + "";
            this.sendPacketToInRange(deactiveSpectrum);
            final String deactiveSentinel = "0|SD|D|R|4|" + this.getAccount().getUserId() + "";
            this.sendPacketToInRange(deactiveSentinel);
            final String deactiveVenom = "0|SD|D|R|5|" + this.getAccount().getUserId() + "";
            this.sendPacketToInRange(deactiveVenom);
    	} else {
    		if(this.getAccount().getSkillsManager().isSpectrumAbilityActivated()) {
            	final String spectrumPacket = "0|SD|A|R|3|" + this.getAccount().getUserId() + "";
                this.sendPacketToInRange(spectrumPacket);
    		} 
    		if (this.getAccount().getSkillsManager().isSentinelAbilityActivated()) {
    	    	final String sentinelPacket = "0|SD|A|R|4|" + this.getAccount().getUserId() + "";
    	    	this.sendPacketToInRange(sentinelPacket);
    		} 
    		if (this.getAccount().getSkillsManager().isVenomAbilityActivated()) {
    			final String venomPacket = "0|SD|A|R|5|" + this.getAccount().getUserId() + "";
    			this.sendPacketToInRange(venomPacket);
    		} 
    		if (this.getAccount().getSkillsManager().isDiminisherAbilityActivated()) {
    	    	final String diminisherPacket = "0|SD|A|R|2|" + this.getAccount().getUserId() + "";
    	    	this.sendPacketToInRange(diminisherPacket);  			
    		}
    	}    	    	   	
    	if(this.getAccount().getSkillsManager().isDiminisherAbilityActivated()) {
           if(this.getLockedTarget() != this.alDiminisherliDusman() || this.getLockedTarget() == null) {
            final String deactivePacket = "0|SD|D|R|2|" + this.getAccount().getUserId() + "";
            this.sendPacketToInRange(deactivePacket);
            this.sendPacketToBoundSessions(deactivePacket);
            
            final String deactivePacketForDiminisherli = "0|SD|D|R|2|" + this.alDiminisherliDusman().getAccount().getUserId() + "";
            this.sendPacketToInRange(deactivePacketForDiminisherli);
            this.sendPacketToBoundSessions(deactivePacketForDiminisherli);
            
            this.getAccount().getSkillsManager().setDiminisherAbilityCooldownEndTime(currentTime + 300000);
            this.getAccount().getSkillsManager().setDiminisherAbilityEffectFinishTime(currentTime);
            this.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		this.getAccount().getAmmunitionManager().getCooldownType("ability_diminisher"),
            		this.getAccount().getAmmunitionManager().getItemTimerState("ability_diminisher"), 300000,
            		300000));
    	}
    	}
    	if(this.getAccount().getSkillsManager().isVenomAbilityActivated()) {
    		if(this.getLockedTarget() != this.alVenomluDusman() || this.getLockedTarget() == null) {
    			if(this.alVenomluDusman() instanceof Player) {		            
		            final String deactivePacketForVenomlu = "0|SD|D|R|5|" + ((Player) this.alVenomluDusman()).getAccount().getUserId() + "";
		            this.sendPacketToInRange(deactivePacketForVenomlu);
		            this.sendPacketToBoundSessions(deactivePacketForVenomlu);
    			} if(this.alVenomluDusman() instanceof Alien) {
		            final String deactivePacketForVenomlu = "0|SD|D|R|5|" + ((Alien) this.alVenomluDusman()).getMapEntityId() + "";
		            this.sendPacketToInRange(deactivePacketForVenomlu);
		            this.sendPacketToBoundSessions(deactivePacketForVenomlu);
    			}
    			
	            this.getAccount().getSkillsManager().setVenomAbilityCooldownEndTime(currentTime + 300000);
	            this.getAccount().getSkillsManager().setVenomAbilityEffectFinishTime(currentTime);
	    		this.getAccount().getSkillsManager().setVenomAbilityLastDamage(0);
	            final String deactivePacket = "0|SD|D|R|5|" + this.getAccount().getUserId() + "";
	            this.sendPacketToInRange(deactivePacket);
	            this.sendPacketToBoundSessions(deactivePacket);
	            this.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
	            		this.getAccount().getAmmunitionManager().getCooldownType("ability_venom"),
	            		this.getAccount().getAmmunitionManager().getItemTimerState("ability_venom"), 300000,
	            		300000));
    			
    	}
    	}
    }
    
    public MovableMapEntity mVenomluDusman;
    
    public MovableMapEntity alVenomluDusman() {
		return mVenomluDusman;   	
    }
    
    public MovableMapEntity setVenomluDusman(final MovableMapEntity pVenomluDusman) {
    	return this.mVenomluDusman = pVenomluDusman;
    }
    
    public Player mDiminisherliDusman;
    
    public Player alDiminisherliDusman() {
		return mDiminisherliDusman;   	
    }
    
    public Player setDiminisherliDusman(final Player pDiminisherliDusman) {
    	return this.mDiminisherliDusman = pDiminisherliDusman;
    }

    public int checkUserMapId() {
        if(this.getAccount().getFactionId() == 1) {
        	this.setCurrentSpaceMap((short) 13);
        	return 13;
        } else if(this.getAccount().getFactionId() == 2) {
        	this.setCurrentSpaceMap((short) 14);
        	return 14;
        } else if(this.getAccount().getFactionId() == 3) {
        	this.setCurrentSpaceMap((short) 15);
        	return 15;
        } else {
        	this.setCurrentSpaceMap((short) 1);
        }
		return 0;
    }
    
    public void checkUserXY() {
        if(this.getAccount().getFactionId() == 1) {
        	this.setPositionXY(1600, 1600);
        } else if(this.getAccount().getFactionId() == 2) {
        	this.setPositionXY(19500, 1500);
        } else if(this.getAccount().getFactionId() == 3) {
        	this.setPositionXY(19500, 11600);
        } else {
        	this.setPositionXY(this.getCurrentPositionX(), this.getCurrentPositionY());
        }
    }
    
    public ShipInitializationCommand getShipInitializationCommand() {
    	final Vector<VisualModifierCommand> vmc = new Vector<VisualModifierCommand>();
    	/*
    	if(this.getAccount().isAdmin()) {
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.BLUE_SIGNAL, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.DAMAGE_ICON, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.JPA_CAMERA, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.LEONOV_EFFECT, 0, "", 1, true));
    		vmc.add(new VisualModifierCommand(this.getAccount().getUserId(), VisualModifierCommand.RED_SIGNAL, 0, "", 1, true));
    	}
    	*/
        return new ShipInitializationCommand(this.getMapEntityId(),//UID
                                             this.getAccount()
                                                 .getShipUsername(),//username
                                             this.getPlayerShip()
                                                 .getShipLootId(),//ship itemID
                                             this.getSpeed(),//speed
                                             this.getCurrentShieldPoints(),//shield
                                             this.getMaximumShieldPoints(),//max shield
                                             this.getCurrentHitPoints(),//hitpoints
                                             this.getMaximumHitPoints(),//max hitpoints
                                             this.getAccount()
                                                 .getResourcesManager()
                                                 .getResourcesCountInCargo(),
                                             // resources in cargo (not including xenomit)
                                             this.getCurrentCargoCapacity(),//cargo
                                             this.getCurrentNanoHull(),//nanohull
                                             this.getMaximumNanoHull(),//max nanohull
                                             this.getCurrentPositionX(),//x
                                             this.getCurrentPositionY(),//y
                                             this.getCurrentSpaceMapId(),//map id
                                             this.getAccount()
                                                 .getFactionId(),//faction id
                                             this.getAccount()
                                                 .getClanId(),//
                                             this.getAccount()
                                                 .getExpansionStage(),//
                                             true,//premium
                                             this.getAccount()
                                                 .getExperience(),//experience
                                             this.getAccount()
                                                 .getHonor(),//honor
                                             this.getAccount()
                                                 .getLevel(),//level
                                             this.getAccount()
                                                 .getCredits(),//credits
                                             this.getAccount()
                                                 .getUridium(),//uridium
                                             this.getAccount()
                                                 .getJackpot(),//jackpot
                                             this.getAccount()
                                                 .getRankId(),//rank id
                                             this.getAccount()
                                                 .getClanTag(),//clan tag
                                             this.getAccount()
                                             .getKilledGoliath() >= 100 ? 100 : 63, // yüzük sayısı (100 = taç / 63 = 6 halka)
                                      //       this.getAccount()
                                      //           .getGalaxyGatesManager()
                                      //           .getRingsCount(),//rings
                                             true,//
                                             this.getAccount()
                                                 .isCloaked(),//kamuflaj var veya yok
                                             true,//
                                             vmc//modifiers

        );
    }
    
    public int getSpeed() {
        return this.getAccount()
                   .getResourcesManager()
                   .getSpeedBoost(this.getAccount()
                                      .getDroneManager()
                                      .getSpeedBoost(this.getBaseSpeed() + this.getAccount()
                                                                               .getEquipmentManager()
                                                                               .getSpeed(
                                                                                       this.getCurrentConfiguration())));
    }
    
    public SetSpeedCommand getSetSpeedCommand() {
        final int speed = this.getSpeed();
        return new SetSpeedCommand(speed, speed);
    }

    // ==============================================================


    @Override
    public void receivedAttack(final MovableMapEntity pMovableMapEntity) {

    }

    /**
     @return base HP of this Player's ship
     */
    public int getBaseHitPoints() {
        return this.getPlayerShip()
                   .getBaseHitPoints();
    }

    /**
     @return base SPD of this Player's ship
     */
    public int getBaseSpeed() {
        return this.getPlayerShip()
                   .getBaseSpeed();
    }

    /**
     @return base cargo capacity of this Player's ship
     */
    public int getBaseCargoCapacity() {
        return this.getPlayerShip()
                   .getBaseCargoCapacity();
    }

    /**
     @return base lasers count of this Player's ship
     */
    public int getBaseLasersCount() {
        return this.getPlayerShip()
                   .getBaseLasersCount();
    }

    /**
     @return base generators count of this Player's ship
     */
    public int getBaseGeneratorsCount() {
        return this.getPlayerShip()
                   .getBaseGeneratorsCount();
    }

    /**
     @return base extras count of this Player's ship
     */
    public int getBaseExtrasCount() {
        return this.getPlayerShip()
                   .getBaseExtrasCount();
    }

    /**
     @return loot ID(String ship identifier) of this Player's ship
     */
    public String getShipLootId() {
        return this.getPlayerShip()
                   .getShipLootId();
    }


    // ==============================================================

    /**
     @return reward EXP for destroying this Player
     */
    public int getRewardExperience() {
        return this.getPlayerShip()
                   .getRewardExperience();
    }

    /**
     @return reward HON for destroying this Player
     */
    public int getRewardHonor() {
        return this.getPlayerShip()
                   .getRewardHonor();
    }

    /**
     @return reward credits for destroying this Player
     */
    public int getRewardCredits() {
        return this.getPlayerShip()
                   .getRewardCredits();
    }

    /**
     @return reward URI for destroying this Player
     */
    public int getRewardUridium() {
    	if(Settings.REWARD_DOUBLER_ENABLED) {
        return this.getPlayerShip()
                   .getRewardUridium() * 2;
    	} else {
        return this.getPlayerShip()
                    .getRewardUridium();
    	}
    }

    /**
     @return reward resources array for destroying this Player
     */
    public JSONArray getRewardResources() {
        return this.getPlayerShip()
                   .getRewardResources();
    }

    // ==============================================================

    /**
     @return current Player DMG
     */
    public int getCurrentDamage() {
    	if(this.getLockedTarget() instanceof Player) {
    	final Player lockedPlayer = (Player) this.getLockedTarget();
        if (this.getCurrentConfiguration() == 1) {
            return this.getPlayerShip()
                       .getLaserDamageBoost(this.getAccount()
                                                .getResourcesManager()
                                                .getLaserDamageBoost(this.getAccount()
                                                                         .getEquipmentManager()
                                                                         .getDamageConfig1(), 35), lockedPlayer.getAccount()
                                                                                                               .getFactionId());
        }
        return this.getPlayerShip()
                   .getLaserDamageBoost(this.getAccount()
                                            .getResourcesManager()
                                            .getLaserDamageBoost(this.getAccount()
                                                                     .getEquipmentManager()
                                                                     .getDamageConfig2(), 35), lockedPlayer.getAccount()
    	                                                                                                    .getFactionId());
    } else {
        if (this.getCurrentConfiguration() == 1) {
            return this.getPlayerShip()
                       .getLaserDamageBoost(this.getAccount()
                                                .getResourcesManager()
                                                .getLaserDamageBoost(this.getAccount()
                                                                         .getEquipmentManager()
                                                                         .getDamageConfig1(), 35), 1);
        }
        return this.getPlayerShip()
                   .getLaserDamageBoost(this.getAccount()
                                            .getResourcesManager()
                                            .getLaserDamageBoost(this.getAccount()
                                                                     .getEquipmentManager()
                                                                     .getDamageConfig2(), 35), 1);
    }
    }

    /**
    @return current Player DMG
    */
   public int getCurrentRocketDamage() {
           return this.getSkillRocketDamageBoost(this.getAccount()
        		      .getDroneManager()
        		      .getRocketDamageBoost(
        		      this.getAccount()
                      .getResourcesManager()
                      .getRocketDamageBoost(this.getRocketAttack()
                    		                    .getRocketDamage(), 1), false), false);    
   }
   
   public int getSkillRocketDamageBoost(final int pDamage, final boolean isNpc) {
       switch (this.getAccount().getRocketDmgUp()) {
           case 5:
               return (int) Tools.getBoost(pDamage, +15D);
           default:
               return pDamage;
       }
   }
   
    //    /**
    //     Note: sets at least {@link Player#PLAYER_DAMAGE_MIN}
    //
    //     @param pCurrentDamage
    //     Player DMG new value
    //     */
    //    public void setCurrentDamage(final int pCurrentDamage) {
    //        this.mCurrentDamage = Math.max(PLAYER_DAMAGE_MIN, pCurrentDamage);
    //    }


    // TODO
    public void bindGameSession(final GameSession pGameSession) {
        if (pGameSession != null) {
            this.mGameSessions.add(pGameSession);
        }
    }

    public void unbindGameSession(final GameSession pGameSession) {
        this.mGameSessions.remove(pGameSession);
    }

    public void unbindClosedGameSessions() {
    	this.mGameSessions.remove(this.getBoundGameSessions());
    }

    public ArrayList<GameSession> getBoundGameSessions() {
        return this.mGameSessions;
    }

    //    public int getMaximumShieldPoints() {
    //        return 10; //TODO
    //    }

    //    public int getMaximumHitPoints() {
    //        return this.getPlayerShip()
    //                   .getBaseHitPoints(); //TODO
    //    }

    //    public int getMaximumNanohull() {
    //        return this.getBaseHitPoints();
    //    }

    public int getCurrentCargoCapacity() {
        return this.mCurrentCargoCapacity;
    }

    public void setCurrentCargoCapacityMultiplier(final float pCargoCapacityMultiplier) {
        this.mCurrentCargoCapacity = (int) (this.getBaseCargoCapacity() *
                                            Math.max(PLAYER_CARGO_CAPACITY_MULTIPLIER_MIN, pCargoCapacityMultiplier));
    }

    public int getPlayerShipId() {
        return this.mPlayerShip == null ? INVALID_ID : this.mPlayerShip.getShipId();
    }

    public void initiateMovement(final int pInitialPositionX, final int pInitialPositionY, final int pTargetPositionX,
                                 final int pTargetPositionY) {
        this.getMovement()
            .initiate(pInitialPositionX, pInitialPositionY, pTargetPositionX, pTargetPositionY, this.getSpeed());   
    }

    public void initiateAttack() {
        this.getLaserAttack()
            .initiate(this.getLockedTarget());
    }

    public void selectShip(final int pTargetId) {

    	/**
	   	 PET SEÇİMİ YAPILACAKKEN pTargetId İLE GAMESESSİON VE 
	   	 GAMESESSİON İLE PLAYER ÇEKİLECEK
	   	 PLAYERDEN ACCOUNTA ULAŞILACAK VE ACCOUNTTAN PETMANAGERE
	   	 BÖYLECE PETMANAGERDEN PETİN BİLGİLERİ ALINABİLECEK
	   	 */
    	
        SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(this.getCurrentSpaceMapId());
        if (spaceMap != null) {
            final MovableMapEntity movableMapEntity = spaceMap.getAllMovableMapEntities()
                                                              .get(pTargetId);
            if (movableMapEntity != null) {
                if (movableMapEntity instanceof Player) {
                    Player player = (Player) movableMapEntity;
                    if (player.canBeTargeted()) {
                        this.setLockedTarget(player);
                        this.sendCommandToBoundSessions(new ShipSelectionCommand(player.getAccount()
                                                                                       .getUserId(), 3,
                                                                                 player.getCurrentShieldPoints(),
                                                                                 player.getMaximumShieldPoints(),
                                                                                 player.getCurrentHitPoints(),
                                                                                 player.getMaximumHitPoints(),
                                                                                 player.getCurrentNanoHull(),
                                                                                 player.getMaximumNanoHull(), true));
                    }
                } else if (movableMapEntity instanceof Alien) {
                    Alien alien = (Alien) movableMapEntity;
                    this.setLockedTarget(alien);
                    this.sendCommandToBoundSessions(
                            new ShipSelectionCommand(alien.getMapEntityId(), 3, alien.getCurrentShieldPoints(),
                                                     alien.getBaseShieldPoints(), alien.getCurrentHitPoints(),
                                                     alien.getBaseHitPoints(), alien.getCurrentNanoHull(),
                                                     alien.getMaximumNanoHull(), false));
                } else if (movableMapEntity instanceof Spaceball) {
                	Spaceball spaceball = (Spaceball) movableMapEntity;
                    this.setLockedTarget(spaceball);
                    this.sendCommandToBoundSessions(
                            new ShipSelectionCommand(spaceball.getMapEntityId(), 3, spaceball.getCurrentShieldPoints(),
                            		                 spaceball.getBaseShieldPoints(), spaceball.getCurrentHitPoints(),
                            		                 spaceball.getBaseHitPoints(), spaceball.getCurrentNanoHull(),
                            		                 spaceball.getMaximumNanoHull(), false));
                }
            }
        }
    }

    public void sendCommandToBoundSessions(final ServerCommand pServerCommand) {
        for (final GameSession gameSession : this.mGameSessions) {
            gameSession.getGameServerClientConnection()
                       .sendToSendCommand(pServerCommand);
        }
    }

    public void sendPacketToBoundSessions(final String pPacket) {
        this.sendCommandToBoundSessions(new LegacyModule(pPacket));
    }

    public boolean updateActivatable(final ActivatableStationaryMapEntity pEntity, final boolean pInRange) {

        if (this.mInRangeAssets.contains(pEntity)) {

            if (pInRange) {
                // in range, no need to send command
                return false;
            } else {
                // not in range, need to send command
                if (pEntity instanceof Portal) {
                	this.setCurrentInRangePortalId(INVALID_ID);
                }
                this.mInRangeAssets.remove(pEntity);
                return true;
            }

        } else {

            if (pInRange) {
                // in range, need to send command
                if (pEntity instanceof Portal) {
                	this.setCurrentInRangePortalId(pEntity.getMapEntityId());
                }
                this.mInRangeAssets.add(pEntity);
                return true;
            } else {
                // not in range, no need to send command
                return false;
            }

        }

    }

    public int getCurrentInRangePortalId() {
        return this.mCurrentInRangePortalId;
    }

    public void setCurrentInRangePortalId(int pInRangePortalId) {
        this.mCurrentInRangePortalId = pInRangePortalId;
    }

    public HashSet<ActivatableStationaryMapEntity> getInRangeAssets() {
        return mInRangeAssets;
    }

    public void changeDestroyingGoliathAndVengeanceValue(final MapEntity pKillerMapEntity)
    {
    	final Player killerPlayer = (Player) pKillerMapEntity;
        final Player playerlocked = (Player) killerPlayer.getLaserAttack().mLockedTarget;
        if(playerlocked != null) {
        if(playerlocked.getPlayerShipId() == 10 || playerlocked.getPlayerShipId() == 52
        		|| playerlocked.getPlayerShipId() == 53 || playerlocked.getPlayerShipId() == 54
        		 || playerlocked.getPlayerShipId() == 56
        		 || playerlocked.getPlayerShipId() == 57 || playerlocked.getPlayerShipId() == 59
        		 || playerlocked.getPlayerShipId() == 61 || playerlocked.getPlayerShipId() == 62
        		 || playerlocked.getPlayerShipId() == 63 || playerlocked.getPlayerShipId() == 64
        		 || playerlocked.getPlayerShipId() == 65 || playerlocked.getPlayerShipId() == 66
        		 || playerlocked.getPlayerShipId() == 67 || playerlocked.getPlayerShipId() == 68
        		 || playerlocked.getPlayerShipId() == 86 || playerlocked.getPlayerShipId() == 87
        		 || playerlocked.getPlayerShipId() == 109 || playerlocked.getPlayerShipId() == 110
        		 || playerlocked.getPlayerShipId() == 120 || playerlocked.getPlayerShipId() == 140
        		 || playerlocked.getPlayerShipId() == 141 || playerlocked.getPlayerShipId() == 142
        		 || playerlocked.getPlayerShipId() == 153 || playerlocked.getPlayerShipId() == 155)
        {
            killerPlayer.getAccount()
                        .changeKilledGoliath(+1);
        }
        else if(playerlocked.getPlayerShipId() == 8 || playerlocked.getPlayerShipId() == 16
        		 || playerlocked.getPlayerShipId() == 17 || playerlocked.getPlayerShipId() == 18
        		 || playerlocked.getPlayerShipId() == 58 && playerlocked.getPlayerShipId() == 60)
        {
        	killerPlayer.getAccount()
                        .changeKilledVengeance(+1);
        }
        }
    }
    
    public void changeOwnKilledValue(final MapEntity pKillerMapEntity)
    {
        final Player killerPlayer = (Player) pKillerMapEntity;
        if (killerPlayer.getLaserAttack().mLockedTarget instanceof Player) {
            final Player playerlocked = (Player) killerPlayer.getLaserAttack().mLockedTarget;

            playerlocked.getAccount().changeOwnKilled(+1);
            QueryManager.saveAccount(playerlocked.getAccount());
          }
    }
    
    public void destroy(final MapEntity pKillerMapEntity) {
        if (pKillerMapEntity instanceof Player) {
        	
            final Player killerPlayer = (Player) pKillerMapEntity;
            if(this.getCurrentSpaceMapId() != Settings.DUEL_MAP || killerPlayer.getCurrentSpaceMapId() != Settings.DUEL_MAP) {
            final int exp = killerPlayer.getPlayerShip().getExperienceBoost(this.getRewardExperience());
            final int hon = killerPlayer.getPlayerShip().getHonorBoost(this.getRewardHonor());
            final int uri = this.getRewardUridium();
            killerPlayer.getAccount()
                        .changeExperience(+exp);
            killerPlayer.getAccount()
                        .changeHonor(+hon);
            killerPlayer.getAccount()
                        .setClanHonor(hon);
            killerPlayer.getAccount()
                        .changeUridium(+uri);
            this.changeDestroyingGoliathAndVengeanceValue(pKillerMapEntity);
            QueryManager.saveAccount(killerPlayer.getAccount());
            
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|EP|" + exp + "|" + killerPlayer.getAccount()
                                                                                           .getExperience() + "|" +
                                                   killerPlayer.getAccount()
                                                               .getLevel());
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|HON|" + hon + "|" + killerPlayer.getAccount()
                                                                                            .getHonor());
            killerPlayer.sendPacketToBoundSessions("0|LM|ST|URI|" + uri + "|" + killerPlayer.getAccount()
                    .getUridium());                             
            QueryManager.saveAccount(killerPlayer.getAccount());
            }
            String killerPlayerUserName = killerPlayer.getAccount().getShipUsername();
            String killedPlayerUserName = this.getAccount().getShipUsername();
            if(Settings.TEXTS_ENABLED) { System.out.println("" + killedPlayerUserName+ " Oyun adli oyuncu "+ killerPlayerUserName +" Oyun adli oyuncu tarafından imha edildi!"); }
            final String wasKilledPacket = "0|A|STD|" + killedPlayerUserName + " was killed by " + killerPlayerUserName + "";
            
            this.sendPacketToBoundSessions(wasKilledPacket);
            this.sendPacketToInRange(wasKilledPacket);
            
            //oyuncular jackpot haritasında ise çalıştırılan kodlar başlangıç
            final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap((short) 42);
            if(this.getCurrentSpaceMapId() == 42 || killerPlayer.getCurrentSpaceMapId() == 42) {
            	           
            // her yok etmede geri kalan oyuncu sayısını yazdırma başlangıç
            if(spaceMap.getAllPlayers().size() > 2) {
            final int kalanOyuncu = (spaceMap.getAllPlayers().size()) - 1;
            final String geriSayim = "0|LM|ST|SLE|"+ kalanOyuncu;
            ServerUtils.sendPacketAllUsersOnMap(42, geriSayim);
            }
            // her yok etmede geri kalan oyuncu sayısını yazdırma son
            
            // son iki oyuncu kaldığında öldüren kişiye ödüllerini verme ve ana haritaya döndürme kodu başlangıç
        	if(spaceMap.getAllPlayers().size() == 2) {
        		
                final String geriSayim = "0|LM|ST|SLE|"+ 1;
                ServerUtils.sendPacketAllUsersOnMap(42, geriSayim);
                
            	QueryManager.saveAccount(killerPlayer.getAccount());
            	killerPlayer.getAccount().setTitle("jackpot_battle_winner");
            	final long uridium = 10000;
            	final long honor = 10000;
            	killerPlayer.getAccount().changeUridium(uridium);
            	killerPlayer.getAccount().changeHonor(honor);
            	killerPlayer.sendPacketToBoundSessions("0|LM|ST|URI|" + uridium + "|" + killerPlayer.getAccount()
                        .getUridium());
                killerPlayer.sendPacketToBoundSessions("0|LM|ST|HON|" + honor + "|" + killerPlayer.getAccount()
                        .getHonor());
            	killerPlayer.sendPacketToBoundSessions("0|n|t|" + killerPlayer.getAccount()
                		.getUserId() + "|0|" + killerPlayer.getAccount()
                          .getTitle());
            	killerPlayer.sendPacketToInRange("0|n|t|" + killerPlayer.getAccount()
                		.getUserId() + "|0|" + killerPlayer.getAccount()
                          .getTitle());
            	killerPlayer.sendPacketToBoundSessions("0|A|STD|İkramiye savaşını kazanan: "+ killerPlayerUserName +"");
            	killerPlayer.sendPacketToInRange("0|A|STD|İkramiye savaşını kazanan: "+ killerPlayerUserName +"");

            	short mapID = 1;
            	int posX = 0;
            	int posY = 0;
                if(killerPlayer.getAccount().getFactionId() == 1) {
                	killerPlayer.setPositionXY(1600, 1600);
                	posX = 1600;
                	posY = 1600;
                } else if(killerPlayer.getAccount().getFactionId() == 2) {
                	killerPlayer.setPositionXY(19500, 1500);
                	posX = 19500;
                	posY = 1500;
                } else if(killerPlayer.getAccount().getFactionId() == 3) {
                	killerPlayer.setPositionXY(19500, 11600);
                	posX = 19500;
                	posY = 11600;
                } else {
                	killerPlayer.setPositionXY(killerPlayer.getCurrentPositionX(), killerPlayer.getCurrentPositionY());
                }
                if(killerPlayer.getAccount().getFactionId() == 1) {
                	mapID = 13;
                } else if(killerPlayer.getAccount().getFactionId() == 2) {
                	mapID = 14;
                } else if(killerPlayer.getAccount().getFactionId() == 3) {
                	mapID = 15;
                } else {
                	mapID = 1;
                }
                this.jumpPortal(mapID, posX, posY);                                
	}     
        	  // son iki oyuncu kaldığında öldüren kişiye ödüllerini verme ve ana haritaya döndürme kodu son
            }
           //oyuncular jackpot haritasında ise çalıştırılan kodlar son
            
            if(this.getCurrentSpaceMapId() == Settings.DUEL_MAP || killerPlayer.getCurrentSpaceMapId() == Settings.DUEL_MAP) {

                	int positionX = 0, positionY = 0, mapID = 0;
                	if(killerPlayer.getCurrentSpaceMapId() == 13 || killerPlayer.getCurrentSpaceMapId() == 14
                      	   || killerPlayer.getCurrentSpaceMapId() == 15 || killerPlayer.getCurrentSpaceMapId() == 16) {	        	   
                          	if(killerPlayer.getAccount().getFactionId() == 1) {
                          		positionX = 1600;
                          		positionY = 1600;
                          		mapID = 13;
                          	} else if(killerPlayer.getAccount().getFactionId() == 2) {                             		
                          		positionX = 19500;
                          		positionY = 1500;
                          		mapID = 14;
                          	} else if(killerPlayer.getAccount().getFactionId() == 3) {
                          		positionX = 19500;
                          		positionY = 11600;
                          		mapID = 15;
                          	}	
                      	} else {
                          	if(killerPlayer.getAccount().getFactionId() == 1) {
                          		positionX = 2000;
                          		positionY = 6000;
                          		mapID = 20;
                          	} else if(killerPlayer.getAccount().getFactionId() == 2) {
                          		positionX = 10000;
                          		positionY = 2000;
                          		mapID = 24;
                          	} else if(killerPlayer.getAccount().getFactionId() == 3) {
                          		positionX = 18500;
                          		positionY = 6000;
                          		mapID = 28;
                          	}
                      	}
                    killerPlayer.jumpPortal((short) mapID, positionX, positionY);
          	
            }
            
            //Öldürme kaydetme başlangıç
        	SimpleDateFormat tarihFormati = new SimpleDateFormat("HH:mm", new Locale("tr"));
            String tarih1 = tarihFormati.format(new Date());
            
        	SimpleDateFormat tarihFormati2 = new SimpleDateFormat("dd MMMM yyyy", new Locale("tr"));
            String tarih2 = tarihFormati2.format(new Date());
            
            final String saat = ""+tarih1+"";
            final String tarih = ""+tarih2+"";
            QueryManager.saveKiller(tarih, saat, this, killerPlayer);
            //Öldürme kaydetme son
            
    //        if(killerPlayer.getAccount().getKilledGoliath() == 500)
    //        {
    //            final String Goliath = "0|A|STD|500 GOLIATH OLDURDUN";
    //            killerPlayer.sendPacketToBoundSessions(Goliath);
    //            killerPlayer.sendPacketToInRange(Goliath);
    //        }
            QueryManager.saveAccount(killerPlayer.getAccount());
        }
        
        if (!this.isDestroyed()) {
        	/**
        	if(this.getAccount().getTitle() != "jackpot_battle_winner") {
        		final Player killerPlayer = (Player) pKillerMapEntity;
        		
        		killerPlayer.sendPacketToBoundSessions("0|n|t|" + killerPlayer.getAccount()
                		.getUserId() + "|0|" + this.getAccount()
                          .getTitle());
        		killerPlayer.sendPacketToInRange("0|n|t|" + killerPlayer.getAccount()
                		.getUserId() + "|0|" + this.getAccount()
                          .getTitle());
        		
        		this.sendPacketToBoundSessions("0|n|t|" + this.getAccount()
                		.getUserId() + "|0|");
        		this.sendPacketToInRange("0|n|t|" + this.getAccount()
                		.getUserId() + "|0|");
        		
        		killerPlayer.getAccount().setTitle(this.getAccount().getTitle());
        		this.getAccount().setTitle("");
        		QueryManager.saveAccount(killerPlayer.getAccount());
        		QueryManager.saveAccount(this.getAccount());
        	}
        */
        	if(this.getAccount().getPetManager().isActivated()) {
        		this.getAccount().getPetManager().Deactivate();
        	}
        	
        	this.setDestroyed(true);
        	
            final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(this.getMapEntityId(), 1);
            this.sendCommandToInRange(shipDestroyCommand);
            this.sendCommandToBoundSessions(shipDestroyCommand);

    		this.getLaserAttack().setAttackInProgress(false);
    		this.setLockedTarget(null);
    		
    		if(this.getCurrentSpaceMapId() == Settings.DUEL_MAP) {
    			this.setDuelUser(null);
    		}
            
            final Vector<KillScreenOptionModule> killScreenOptionModules = new Vector<>();
            final KillScreenOptionModule killScreenOptionModule =
                    new KillScreenOptionModule(new KillScreenOptionTypeModule(KillScreenOptionTypeModule.BASIC_REPAIR),
                                               new PriceModule(PriceModule.URIDIUM, 0), true, 0,
                                               new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                   new ClientUITooltipTextFormatModule(
                                                                                           ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                   new Vector<MessageWildcardReplacementModule>()),
                                               new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                   new ClientUITooltipTextFormatModule(
                                                                                           ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                   new Vector<MessageWildcardReplacementModule>()),
                                               new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                   new ClientUITooltipTextFormatModule(
                                                                                           ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                   new Vector<MessageWildcardReplacementModule>()),
                                               new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                                                                                   new ClientUITooltipTextFormatModule(
                                                                                           ClientUITooltipTextFormatModule.LOCALIZED),
                                                                                   new Vector<MessageWildcardReplacementModule>()));
            killScreenOptionModules.add(killScreenOptionModule);
            if(pKillerMapEntity instanceof Player) {
            final Player killerPlayer = (Player) pKillerMapEntity;
            final KillScreenPostCommand killScreenPostCommand =
                    new KillScreenPostCommand(killerPlayer.getAccount().getShipUsername(), "http://localhost/indexInternal.es?action=internalDock",
                                              "MISC", new DestructionTypeModule(DestructionTypeModule.PLAYER),
                                              killScreenOptionModules);         
            this.sendCommandToBoundSessions(killScreenPostCommand);
            } else if(pKillerMapEntity instanceof Alien) {
                final Alien killerAlien = (Alien) pKillerMapEntity;
                final KillScreenPostCommand killScreenPostCommand =
                        new KillScreenPostCommand(killerAlien.getAlienShipName(), "http://localhost/indexInternal.es?action=internalDock",
                                                  "MISC", new DestructionTypeModule(DestructionTypeModule.PLAYER),
                                                  killScreenOptionModules);         
                this.sendCommandToBoundSessions(killScreenPostCommand);
            }
            this.setInEquipZone(true);           
            if (pKillerMapEntity != null) {
                if (pKillerMapEntity instanceof Player) {
                    final Player player = (Player) pKillerMapEntity;
                    player.setLockedTarget(null);
                    player.getLaserAttack()
                          .setAttackInProgress(false);
                    this.changeOwnKilledValue(pKillerMapEntity);
                }
            }          
            QueryManager.saveAccount(this.getAccount());
        }
    }

    public boolean isInSecureZone() {
        return mInSecureZone;
    }

    public void setInSecureZone(final boolean pInSecureZone) {
        this.mInSecureZone = pInSecureZone;
    }

    public boolean isInRadiationZone() {
        return mInRadiationZone;
    }

    public void setInRadiationZone(final boolean pInRadiationZone) {
        mInRadiationZone = pInRadiationZone;
    }

    public boolean isInEquipZone() {
        return mInEquipZone;
    }

    public void setInEquipZone(final boolean pInEquipZone) {
        mInEquipZone = pInEquipZone;
    }

    public AttributeShieldUpdateCommand getShieldUpdateCommand() {
        return new AttributeShieldUpdateCommand(this.getCurrentShieldPoints(), this.getMaximumShieldPoints());
    }

    public AttributeHitpointUpdateCommand getHitpointsUpdateCommand() {
        return new AttributeHitpointUpdateCommand(this.getCurrentHitPoints(), this.getMaximumHitPoints(),
                                                  this.getCurrentNanoHull(), this.getMaximumNanoHull());
    }

    public ServerCommand getBeaconCommand() {
        return new BeaconCommand(1, 1, 1, 1, this.isInSecureZone(), this.isRepairBotActivated(), false,
                                 "equipment_extra_repbot_rep-4", this.isInRadiationZone());
    }

    @Override
    public boolean canBeTargeted() {
        return (System.currentTimeMillis() - this.getAccount()
                                                 .getAmmunitionManager()
                                                 .getEmpEffectEndTime()) >= 0;
    }
   
    public boolean canBeShoot() {
        return (System.currentTimeMillis() - this.getAccount()
                                                 .getAmmunitionManager()
                                                 .getIshEffectEndTime()) >= 0;
    }
    
    public boolean usingSpectrum() {
        return (System.currentTimeMillis() - this.getAccount()
                                                 .getSkillsManager()
                                                 .getSpectrumAbilityCooldownEndTime()) >= 0;
    }
    
    public boolean usingCloak() {
    	return this.getAccount().isCloaked() == true;
    }
    
    @Override
    public int getCurrentHitPoints() {
        return mCurrentHitPoints;
    }

    public void setCurrentHitPoints(final int pCurrentHitPoints) {
        mCurrentHitPoints = pCurrentHitPoints;
    }

    @Override
    public int changeCurrentHitPoints(int pDifferenceHitpoints) {
        if ((mCurrentHitPoints + pDifferenceHitpoints) > this.getMaximumHitPoints()) {
            pDifferenceHitpoints = this.getMaximumHitPoints() - mCurrentHitPoints;
        }
        mCurrentHitPoints += pDifferenceHitpoints;
        return pDifferenceHitpoints;
    }

    @Override
    public int getCurrentNanoHull() {
        return mCurrentNanoHull;
    }

    public void setCurrentNanoHull(final int pCurrentNanoHull) {
        mCurrentNanoHull = pCurrentNanoHull;
    }

    @Override
    public void changeCurrentNanoHull(final int pDifferenceNanoHull) {
        mCurrentNanoHull += pDifferenceNanoHull;
    }

    @Override
    public int getCurrentShieldPoints() {
        if (this.getCurrentConfiguration() == 1) {
            return this.getCurrentShieldPointsConfig1();
        }
        return this.getCurrentShieldPointsConfig2();
    }

    public void setCurrentShieldPoints(final int pCurrentShieldPoints) {
        if (this.getCurrentConfiguration() == 1) {
            mCurrentShieldPointsConfig1 = pCurrentShieldPoints;
        } else {
            mCurrentShieldPointsConfig2 = pCurrentShieldPoints;
        }
    }

    public void setCurrentShieldPointsConfig1(final int pCurrentShieldPointsConfig1) {
        mCurrentShieldPointsConfig1 = pCurrentShieldPointsConfig1;
    }

    public int getCurrentShieldPointsConfig1() {
        return mCurrentShieldPointsConfig1;
    }

    public void setCurrentShieldPointsConfig2(final int pCurrentShieldPointsConfig2) {
        mCurrentShieldPointsConfig2 = pCurrentShieldPointsConfig2;
    }

    public int getCurrentShieldPointsConfig2() {
        return mCurrentShieldPointsConfig2;
    }

    @Override
    public int changeCurrentShieldPoints(int pDifferenceShieldPoints) {
        if (this.getCurrentConfiguration() == 1) {

            if ((mCurrentShieldPointsConfig1 + pDifferenceShieldPoints) > this.getMaximumShieldPoints()) {
                pDifferenceShieldPoints = this.getMaximumShieldPoints() - mCurrentShieldPointsConfig1;
            }
            mCurrentShieldPointsConfig1 += pDifferenceShieldPoints;
            if (mCurrentShieldPointsConfig1 < 0) {
                mCurrentShieldPointsConfig1 = 0;
            }
            return pDifferenceShieldPoints;
        }

        if ((mCurrentShieldPointsConfig2 + pDifferenceShieldPoints) > this.getMaximumShieldPoints()) {
            pDifferenceShieldPoints = this.getMaximumShieldPoints() - mCurrentShieldPointsConfig2;
        }
        mCurrentShieldPointsConfig2 += pDifferenceShieldPoints;
        if (mCurrentShieldPointsConfig2 < 0) {
            mCurrentShieldPointsConfig2 = 0;
        }
        return pDifferenceShieldPoints;
    }

    @Override
    public int getCurrentShieldAbsorb() {
        return this.mCurrentShieldAbsorb;
    }

    public void setCurrentShieldAbsorb(final int pCurrentShieldAbsorb) {
        mCurrentShieldAbsorb = pCurrentShieldAbsorb;
    }

    public ShipRemoveCommand removeInRangeMapEntity(final MovableMapEntity pMovableMapEntity) {
        this.mInRangeMovableMapIntities.remove(pMovableMapEntity);
                if (pMovableMapEntity instanceof Alien && pMovableMapEntity.getLockedTarget() == this) {
                    this.mAliensCount -= 1;
                    pMovableMapEntity.setLockedTarget(null);
               }
        return new ShipRemoveCommand(pMovableMapEntity.getMapEntityId());
    }

        public boolean canBeAttacked() {
            return !this.isInSecureZone() && (System.currentTimeMillis() - this.getAccount()
                                                                               .getAmmunitionManager()
                                                                               .getIshEffectEndTime()) >= 0;
        }
    
    //    @Override
    //    public int getLaserDamageBoost(final int pLaserDamage, final boolean pIsNps) {
    //        //TODO add others boosters
    //        return this.getAccount()
    //                   .getDroneManager()
    //                   .getLaserDamageBoost(pLaserDamage, pIsNps);
    //    }
    //
    //    @Override
    //    public int getRocketDamageBoost(final int pRocketDamage, final boolean pIsNps) {
    //        //TODO add others boosters
    //        return this.getAccount()
    //                   .getDroneManager()
    //                   .getRocketDamageBoost(pRocketDamage, pIsNps);
    //    }

    public short getCurrentConfiguration() {
        return mCurrentConfiguration;
    }

    public boolean getConfigurationChanged() {
    	return mConfigurationChanged;
    }
    
    public void setCurrentConfiguration(final short pCurrentConfiguration) {
        mCurrentConfiguration = pCurrentConfiguration;
        mConfigurationChanged = true;
        final String drones = this.getAccount()
                                  .getDroneManager()
                                  .getDronesPacket();
        this.sendPacketToBoundSessions(drones);
        this.sendPacketToInRange(drones);

        this.sendCommandToBoundSessions(this.getSetSpeedCommand());
        this.sendCommandToBoundSessions(this.getShieldUpdateCommand());
        //this must be send because user can have hercules on any config and we must update max hitpoints
        this.sendCommandToBoundSessions(this.getHitpointsUpdateCommand());
    }

    public int getMaximumHitPoints() {
        return this.getPlayerShip()
                   .getHitPointsBoost(this.getAccount()
                                          .getDroneManager()
                                          .getHitpointsBoost(this.getAccount()
                                                                 .getEquipmentManager()
                                                                 .getHitpointsBoost(this.getCurrentConfiguration(),
                                                                                    mMaximumHitPoints)));
    }

    public void setMaximumHitPoints(final int pMaximumHitPoints) {
        mMaximumHitPoints = pMaximumHitPoints;
    }

    public int getMaximumNanoHull() {
        return mMaximumNanoHull;
    }

    public void setMaximumNanoHull(final int pMaximumNanoHull) {
        mMaximumNanoHull = pMaximumNanoHull;
    }

    public int getMaximumShieldPoints() {
        return this.getPlayerShip()
                   .getShieldPointsBoost(this.getAccount()
                                             .getResourcesManager()
                                             .getShieldPointsBoost(this.getAccount()
                                                                       .getDroneManager()
                                                                       .getShieldPointsBoost(this.getAccount()
                                                                                                 .getEquipmentManager()
                                                                                                 .getShieldPoints(
                                                                                                         this.getCurrentConfiguration()))));
    }

    public void changeConfiguration(final short newConfigID) {
        	
            if (mConfigurationCooldownEndTime == 0 || mConfigurationCooldownEndTime <= System.currentTimeMillis()) {
            	mConfigurationCooldownEndTime = System.currentTimeMillis() + 5000;                
               this.sendPacketToBoundSessions("0|S|CFG|" + newConfigID);
               this.setCurrentConfiguration(newConfigID);
            } else {
                this.sendPacketToBoundSessions("0|A|STM|config_change_failed_time");
            }
    }
    
    private Thread mLogoutThread;
    
    public void startLogoutProcess() { 
    	
    	final Player player = this;
    	final Account account = player.getAccount();
    	
    	player.getLaserAttack().setAttackInProgress(false);
    	player.setLockedTarget(null);
        
        final GameSession gameSession = GameManager.getGameSession(account.getUserId());
        final GameServerClientConnection gscc = gameSession.getGameServerClientConnection();

        if (mLogoutThread == null || !mLogoutThread.isAlive()) {
            mLogoutThread = new Thread() {
                public void run() {
                    try {
                        int i = -1;
                        while (true) {
                            if ((!account.isPremiumAccount() && i >= 5) || (account.isPremiumAccount() && i >= 5)) {
                            	if(account.getPetManager().isActivated()) {
                            		account.getPetManager().Deactivate();
                            	}
                                gameSession.close();
                                break;
                            }
                            Thread.sleep(1000);
                            i++;
                        }
                    } catch (InterruptedException e) {
                        gscc.sendPacket("0|t");
                    }
                }
            };
            this.mLogoutThread.start();
        }
    }
        
    public boolean checkLogoutProcessRunning() {
        if (this.mLogoutThread != null && this.mLogoutThread.isAlive()) {
            return true;
        }
        return false;
    }
    
    public void stopLogoutProcess() {
        if (this.checkLogoutProcessRunning()) {
            this.mLogoutThread.interrupt();
            this.mLogoutThread = null;
        }
    }
    
    public int getMaximumShieldPointsConfig1() {
    	return this.mMaximumShieldPointsConfig1;
    }
    
    public int getMaximumShieldPointsConfig2() {
    	return this.mMaximumShieldPointsConfig2;
    }
    
    public void setMaximumShieldPointsConfig1(final int pMaximumShieldPointsConfig1) {
        this.mMaximumShieldPointsConfig1 = pMaximumShieldPointsConfig1;
    }

    public void setMaximumShieldPointsConfig2(final int pMaximumShieldPointsConfig2) {
    	this.mMaximumShieldPointsConfig2 = pMaximumShieldPointsConfig2;
    }
    
    public void setLockedTarget(final Lockable pLockedTarget) {
        this.getLaserAttack()
            .setAttackInProgress(false);
        this.mLockedTarget = pLockedTarget;
    }

    public Lockable getLockedTarget() {
        return this.mLockedTarget;
    }

    public boolean isDestroyed() {
        return this.mIsDestroyed;
    }

    public void setDestroyed(final boolean pIsDestroyed) {
        this.mIsDestroyed = pIsDestroyed;
    }

    public boolean isJumping() {
        return this.mIsJumping;
    }

    public void setJumping(final boolean pIsJumping) {
        this.mIsJumping = pIsJumping;
    }
        
    public long getRadiationZoneLastDamageTime() {
        return this.mRadiationZoneLastDamageTime;
    }
    
    public void setRadiationZoneLastDamageTime(final long pRadiationZoneLastDamageTime) {
    	this.mRadiationZoneLastDamageTime = pRadiationZoneLastDamageTime;
    }
    
    public long getLastSendKillScreenTime() {
        return this.mLastSendKillScreenTime;
    }
    
    public void setLastSendKillScreenTime(final long pLastSendKillScreenTime) {
    	this.mLastSendKillScreenTime = pLastSendKillScreenTime;
    }
}
