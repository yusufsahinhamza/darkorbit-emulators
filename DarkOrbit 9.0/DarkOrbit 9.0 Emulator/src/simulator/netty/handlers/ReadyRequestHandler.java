package simulator.netty.handlers;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import java.util.Vector;

import simulator.map_entities.movable.Player;
import simulator.map_entities.stationary.stations.BattleStation;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.ReadyRequest;
import simulator.netty.serverCommands.ArenaStatusCommand;
import simulator.netty.serverCommands.AssetCreateCommand;
import simulator.netty.serverCommands.AssetTypeModule;
import simulator.netty.serverCommands.ClanRelationModule;
import simulator.netty.serverCommands.ContactsListUpdateCommand;
import simulator.netty.serverCommands.LootModule;
import simulator.netty.serverCommands.PetInitializationCommand;
import simulator.netty.serverCommands.QuestCaseModule;
import simulator.netty.serverCommands.QuestConditionModule;
import simulator.netty.serverCommands.QuestConditionStateModule;
import simulator.netty.serverCommands.QuestDefinitionModule;
import simulator.netty.serverCommands.QuestElementModule;
import simulator.netty.serverCommands.QuestIconModule;
import simulator.netty.serverCommands.QuestInitializationCommand;
import simulator.netty.serverCommands.QuestTypeModule;
import simulator.netty.serverCommands.VisualModifierCommand;
import simulator.netty.serverCommands.class_470;
import simulator.netty.serverCommands.class_488;
import simulator.netty.serverCommands.class_502;
import simulator.netty.serverCommands.class_762;
import simulator.netty.serverCommands.class_873;
import simulator.netty.serverCommands.class_875;
import simulator.netty.serverCommands.class_917;
import simulator.netty.serverCommands.class_922;
import simulator.users.Account;
import utils.Log;
import utils.Settings;

/**
 Created by Pedro on 30-03-2015.
 Edited by LEJYONER.
 */

public class ReadyRequestHandler
        implements ICommandHandler {

    private final GameServerClientConnection mGameServerClientConnection;
    private final ReadyRequest               mReadyRequest;

    public ReadyRequestHandler(final GameServerClientConnection pGameServerClientConnection,
                               final ClientCommand pReadyRequest) {
        this.mGameServerClientConnection = pGameServerClientConnection;
        this.mReadyRequest = (ReadyRequest) pReadyRequest;
    }

    @Override
    public void execute() {
        Log.pt("READY REQUEST HANDLER");
        final GameSession gameSession = this.mGameServerClientConnection.getGameSession();
        final Account account = gameSession.getAccount();
        final Player player = account.getPlayer();
        if (gameSession != null) {
            switch (this.mReadyRequest.readyType) {
                case ReadyRequest.MAP_LOADED:
                	
                	/**
                 	final BattleStation station = new BattleStation (player.getCurrentSpaceMap(),player.getCurrentPositionX(),player.getCurrentPositionY());
                 	player.getCurrentSpaceMap().mActivatableEntities.put(station.getMapEntityId(), station);
                 	player.sendCommandToBoundSessions(station.getAssetCreateCommand());
                 	
                 	player.sendCommandToBoundSessions(new AssetCreateCommand(new AssetTypeModule((short)37), "T1",
                 			1, "", 14242424, 5, 9990,
                 			station.getCurrentPositionX() - 413, station.getCurrentPositionY() + 97 , 9991, false, true, true, true,
                            new ClanRelationModule(ClanRelationModule.NONE),
                            new Vector<VisualModifierCommand>()));
            */
                	
                	if(account.isAdmin()) {
                		
                		/**
                		//GÖREV
                		final Vector<LootModule> currens = new Vector<LootModule>();
                		currens.add(new LootModule("currency_uridium",10000));
                		currens.add(new LootModule("currency_experience",10000));
                		currens.add(new LootModule("currency_credits",10000));
                		
                		final QuestDefinitionModule quests = new QuestDefinitionModule(
                				(short) 1, //görev sayısı
                				new Vector<QuestTypeModule>(QuestTypeModule.MISSION),
                				QuestDefinitionModule.b1,                    				                				                				
                				new QuestCaseModule(
                						11800101,
                						true,
                						true,
                						true,
                						1,
                						new Vector<QuestElementModule>()
                						)                      
                		,
                		currens,
                		new Vector<QuestIconModule>()			
                				);
                		
                		
                		player.sendCommandToBoundSessions(new QuestInitializationCommand(quests));
                		//GÖREV
                		*/
                		
                		//JACKPOT BEKLEME EKRANI
                    	player.sendCommandToBoundSessions(new ArenaStatusCommand(ArenaStatusCommand.JACKPOT,ArenaStatusCommand.WAITING_FOR_PLAYERS,1,10,10,"CAN",0,0,120,60));
                    	//JACKPOT BEKLEME EKRANI
                    	
                    	////JACKPOT MAÇ SONUCU
                    	//JackpotArenaMatchResultModule winnerData = new JackpotArenaMatchResultModule(90, 90, "", 90, 90, 90, 90, 90);
                    	//JackpotArenaMatchResultModule loserData = new JackpotArenaMatchResultModule(90, 90, "", 90, 90, 90, 90, 90);                  	
                    	//player.sendCommandToBoundSessions(new JackpotArenaMatchResultCommand(true, winnerData, loserData, new MessageLocalizedCommand("TEST")));
                    	//JACKPOT MAÇ SONUCU
                    	
                    	
                    	//ADMİN ŞEKİLLERİ
                    	/*
                    	player.sendCommandToBoundSessions(new VisualModifierCommand(player.getAccount().getUserId(), VisualModifierCommand.BLUE_SIGNAL, 0, "", 1, true));
                    	player.sendCommandToBoundSessions(new VisualModifierCommand(player.getAccount().getUserId(), VisualModifierCommand.DAMAGE_ICON, 0, "", 1, true));
                    	player.sendCommandToBoundSessions(new VisualModifierCommand(player.getAccount().getUserId(), VisualModifierCommand.JPA_CAMERA, 0, "", 1, true));
                    	player.sendCommandToBoundSessions(new VisualModifierCommand(player.getAccount().getUserId(), VisualModifierCommand.LEONOV_EFFECT, 0, "", 1, true));
                    	player.sendCommandToBoundSessions(new VisualModifierCommand(player.getAccount().getUserId(), VisualModifierCommand.DIVERSE, 0, "", 1, true));
                    	player.sendCommandToBoundSessions(new VisualModifierCommand(player.getAccount().getUserId(), VisualModifierCommand.RED_SIGNAL, 0, "", 1, true));
                    	*/
                    	//ADMİN ŞEKİLLERİ
                    	 
                	}
                	//ŞİRKET HİYERARŞİSİ FALANI FİLANI SIRALAMASI
                	
                	/**
                	final Vector<CompanyHierarchyRankingModule> mmo = new Vector<CompanyHierarchyRankingModule>();
                	for(final Clan clan : ClanStorage.getClanCollection()) {
                		if(clan.getFactionId() == 1) {
	                		
	                			mmo.add(new CompanyHierarchyRankingModule(clan.getClanId(),1,clan.getClanName(),QueryManager.getLeaderName(clan.getClanId()),"",clan.getRankPoints()));
	                		
                		}
                	}
                	final Vector<CompanyHierarchyRankingModule> eic = new Vector<CompanyHierarchyRankingModule>();
                	for(final Clan clan : ClanStorage.getClanCollection()) {
                		if(clan.getFactionId() == 2) {
	                		
	                			eic.add(new CompanyHierarchyRankingModule(clan.getClanId(),1,clan.getClanName(),QueryManager.getLeaderName(clan.getClanId()),"",clan.getRankPoints()));
	                		
                		}
                	}
                	final Vector<CompanyHierarchyRankingModule> vru = new Vector<CompanyHierarchyRankingModule>();
                	for(final Clan clan : ClanStorage.getClanCollection()) {
                		if(clan.getFactionId() == 3) {
	                		
	                			vru.add(new CompanyHierarchyRankingModule(clan.getClanId(),1,clan.getClanName(),QueryManager.getLeaderName(clan.getClanId()),"",clan.getRankPoints()));
	                		
                		}
                	}
                	
                	final CompanyHierarchyRankingModule my = new CompanyHierarchyRankingModule(account.getClanId(),1,account.getClan().getClanName(),account.getShipUsername(),"",1);
                	
                	player.sendCommandToBoundSessions(new CompanyHierarchyInitializationCommand(mmo, eic, vru, my, new FactionModule(account.getFactionId())));
                	
                	*/
                	
                    //  	Vector<BoosterTypesModule> boosterTypes = new Vector<>();
                    // 	boosterTypes.add(new BoosterTypesModule(BoosterTypesModule.DMG_B01));
                     	
                  //   	player.sendCommandToBoundSessions(new BoosterUpdateCommand(new BoosterAttributeType(BoosterAttributeType.DAMAGE), (short) 999999999, boosterTypes));
                     	
                  //       gameSession.getGameServerClientConnection()
                  //                  .sendToSendCommand(new GroupInvitationStateCommand(true));
                	
                	/////////////////////////////////////////////////////////
                	
                	//YAPBOZ
		        	int activeLetterCount = 0;
		            for(boolean active : account.puzzleLetters.values()){      			               
		                if(active) {
		                	activeLetterCount++;			                	
		                }        
		    		}
                	player.sendCommandToBoundSessions(player.getWordPuzzleLetterAchievedCommand(activeLetterCount == Settings.harfSayisi ? true : false));
                	//YAPBOZ
                	
                    if(player.getConfigurationChanged()) {
                    	player.sendPacketToBoundSessions("0|S|CFG|" + player.getCurrentConfiguration());
                    	player.sendPacketToBoundSessions("0|A|CC|" + player.getCurrentConfiguration());
                    } else {
                    	player.sendPacketToBoundSessions("0|A|CC|2");
                    }
                    
                    player.sendPacketToBoundSessions(account
                                                      .getDroneManager()
                                                      .getDronesPacket());
                    player.getCurrentSpaceMap()
                               .sendBaseStations(gameSession);
                    player.getCurrentSpaceMap()
                               .sendActivatableMapEntities(gameSession.getGameServerClientConnection());
                    player.getCurrentSpaceMap()
                               .sendResources(gameSession);
                    player.getCurrentSpaceMap()
                                .sendBonusBoxes(gameSession);                    
                    player.getCurrentSpaceMap()
                               .onPlayerMovement(player);
                  
                    player //spaceball skorlarını gönderme
                    .sendPacketToBoundSessions("0|n|ssi|" + 0 + "|" + 0 + "|" + 0);
                    player //istila skorlarını gönderme
                    .sendPacketToBoundSessions("0|n|isi|" + 0 + "|" + 0 + "|" + 0);

                    player.sendPacketToBoundSessions("0|n|t|" + player.getAccount()
                    		.getUserId() + "|0|" + player.getAccount()
                              .getTitle());
                    
                    if(account.isAdmin() || account.havePet()) { 
                    	player.sendCommandToBoundSessions(new PetInitializationCommand(true, true, true));
                    }
                    
                    Vector<class_917> vc917 = new Vector<>();
                    Vector<class_470> vc470 = new Vector<>();
                    vc470.add(new class_470());
                    Vector<class_873> vc873 = new Vector<>();
                    vc873.add(new class_873(class_873.const_1766));
                    vc917.add(new class_917(new class_502(1, vc470), new class_488(1, vc873)));
                    player.sendCommandToBoundSessions(new ContactsListUpdateCommand(new class_762(vc917),
                                                                          new class_922(false, false, false, false),
                                                                          new class_875(false)));
                    
                    player.sendCommandToBoundSessions(player
                                                       .getSetSpeedCommand());
                    player.sendCommandToBoundSessions(account
                                                       .getClientSettingsManager()
                                                       .getClientUISlotBarsCommand());

                    break;
                case ReadyRequest.HERO_LOADED:

                    break;
            }
        }
    }
}
