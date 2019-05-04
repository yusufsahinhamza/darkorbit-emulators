package main;

import mysql.QueryManager;
import net.game_server.GameSession;

import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.NoSuchElementException;
import java.util.Scanner;
import java.util.Set;

import simulator.GameManager;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.utils.DefaultAssignings;
import utils.Settings;
import utils.Log;

/**
 This class is used to work with command line
 */
public class CommandLineProcessor
        implements Runnable {

    private static final String CMD_PROCESSOR_THREAD_NAME = "[CommandLineProcessor Thread]";
    
    private static final String CMD_SEND_UID       = "send_uid";
    private static final String CMD_RESTART        = "restart";
    private static final String CMD_RESTART_MANUEL = "restart_manual";
    private static final String CMD_HELP           = "help";
    private static final String CMD_YARDIM         = "yardim";
    private static final String CMD_THREAD_COUNT   = "tc";
    private static final String CMD_GIVE_URIDIUM   = "uridium";
    private static final String CMD_GIVE_TITLE     = "baslik";
    private static final String CMD_GIVE_CREDITS   = "credits";
    private static final String CMD_GIVE_PREMIUM   = "premium";
    private static final String CMD_GIVE_DAMAGE    = "hasar";
    private static final String CMD_GIVE_SPEED     = "hiz";
    private static final String CMD_CHANGE_MAP     = "tasi";
    private static final String CMD_CHAT_UNBAN     = "unban";
    private static final String CMD_SPAWN_NPC      = "npc";
    
    private final Scanner mConsoleScanner;

    private final Thread mCmdThread;

    private final ServerManager mServerManager;

    public CommandLineProcessor(ServerManager pServerManager) {

        this.mConsoleScanner = new Scanner(System.in);

        this.mCmdThread = new Thread(this);
        this.mCmdThread.setName(CMD_PROCESSOR_THREAD_NAME);
        this.mCmdThread.setDaemon(true);

        this.mServerManager = pServerManager;

    }

    public void startProcessing() {
        this.mCmdThread.start();
    }

    public void run() {

        while (true) {

            commandProcessingBlock:
            {

                String command;

                //Everything is running and ok, know an infinite loop to keep it up
                //Read input
                try {
                    command = this.mConsoleScanner.nextLine();
                } catch (NoSuchElementException e) {
                    // console is not accessible, stop reading it
                    this.stopProcessing();
                    return;
                }

                String[] commandSplit = command.split(" ");

                if (commandSplit.length == 0) {
                    // no command
                    break commandProcessingBlock;

                } else if (commandSplit.length == 1) {

                    // We have one word command

                    switch (commandSplit[0]) {

                        case CMD_YARDIM:
                        case CMD_HELP:
                            this.printHelp();
                            break;

                        case CMD_RESTART:
                            this.executeClose();
                            break;
                        	
                        case CMD_THREAD_COUNT:
                            Set<Thread> runningThreadSet = Thread.getAllStackTraces()
                                                                 .keySet();
                            Log.pt("Thread count = " + runningThreadSet.size());
                            for (Thread thread : runningThreadSet) {
                                Log.pt("Thread id = " + thread.getId() + ", name = " + thread.getName());
                            }
                            break;

                        default:
                            Log.pt("Command \"" + commandSplit[0] + "\" not found. Possibly re-check spelling");
                            break;

                    }

                } else if (commandSplit.length > 1) {

                    // We have multiple word command

                    switch (commandSplit[0]) {

                        case CMD_SEND_UID:

                            // Send a packet to a single user

                            if (commandSplit.length < 3) {
                                Log.pt("Invalid command syntax. Not enough arguments");
                                break;
                            }

                            int userId;

                            try {
                                userId = Integer.parseInt(commandSplit[1]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument usedId should be an integer");
                                break;
                            }

                            this.executeSendToUser(userId, commandSplit[2]);

                            break;

                        case CMD_RESTART_MANUEL:
                            int saniye = 0;
                            try {
                            	saniye = Integer.parseInt(commandSplit[1]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument saniye should be a number.");
                            }
                        	this.restartManual(saniye);
                        	break;
                        	
                        case CMD_GIVE_CREDITS:
                            String username = "";
                            try {
                            	username = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            int credits = 0;
                            try {
                            	credits = Integer.parseInt(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument credits should be a number.");
                            }
                            for(GameSession account : GameManager.getGameSessions()){ 
                            final Player player = account.getPlayer();
                            if (account != null) {
                            	if(player != null) {
                            		if(player.getAccount().getShipUsername().equals(username)) {
                            		player.getAccount().changeCredits(credits);
                            		player.sendPacketToBoundSessions("0|LM|ST|CRE|" + credits + "|" + player.getAccount()
                                        .getCredits());
                            	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+credits+" credits verildi!");
                            	QueryManager.saveAccount(player.getAccount());
                             }
                           }
                         }
                       }
                            break;
                            
                        case CMD_GIVE_URIDIUM:
                        	String username2 = "";
                            try {
                            	username2 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            int uridium = 0;
                            try {
                                uridium = Integer.parseInt(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument uridium should be a number.");
                            }
                            for(GameSession account2 : GameManager.getGameSessions()){
                            final Player player2 = account2.getPlayer();
                            if (account2 != null) {
                            	if(player2 != null) {
                            		if(player2.getAccount().getShipUsername().equals(username2)) {
                            		player2.getAccount().changeUridium(uridium);
                            		player2.sendPacketToBoundSessions("0|LM|ST|URI|" + uridium + "|" + player2.getAccount()
                                        .getUridium());
                            	Log.pt("'"+player2.getAccount().getUserId()+"' ID'li ve '"+player2.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+uridium+" uridium verildi!");
                            	QueryManager.saveAccount(player2.getAccount());
                            }
                          }
                        }
                      }
                            break;
                            
                        case CMD_GIVE_TITLE:
                        	String username3 = "";
                            try {
                            	username3 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            String title = "";
                            try {
                            	title = commandSplit[2];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument title should be a number.");
                            }
                            for(GameSession account3 : GameManager.getGameSessions()){
                            final Player player3 = account3.getPlayer();
                            if (account3 != null) {
                            	if(player3 != null) {
                            		if(player3.getAccount().getShipUsername().equals(username3)) {
                            		player3.getAccount().setTitle(title);
                            	
                            		player3.getAccount().getPlayer().
                            	sendPacketToBoundSessions("0|n|t|" + player3.getAccount()
                                		.getUserId() + "|0|" + player3.getAccount()
  	                                  .getTitle());
                            	
                            		player3
                                .sendPacketToInRange("0|n|t|" + player3.getAccount()
                                		.getUserId() + "|0|" + player3.getAccount()
    	                                  .getTitle());
                            		
                                	Log.pt("'"+player3.getAccount().getUserId()+"' ID'li ve '"+player3.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+title+" başlığı verildi!");
                                	QueryManager.saveAccount(player3.getAccount());
                            }
                          }
                        }
                      }
                            break;
                            
                        case CMD_GIVE_PREMIUM:
                        	String username4 = "";
                            try {
                            	username4 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            int premium = 0;
                            try {
                            	premium = Integer.parseInt(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument premium should be a number.");
                            }
                            for(GameSession account4 : GameManager.getGameSessions()){
                            final Player player4 = account4.getPlayer();
                            if (account4 != null) {
                            	if(player4 != null) {
                            		if(player4.getAccount().getShipUsername().equals(username4)) {
                            	if(premium == 1)
                            	{
                            		if(player4.getAccount().isPremiumAccount())
                            		{
                            			Log.pt("'"+player4.getAccount().getUserId()+"' ID'li ve '"+player4.getAccount().getUsername()+"' kullanıcı adlı oyuncunun premium avantajları zaten aktif!");
                            		}
                            		else
                            		{
                            			player4.getAccount().setPremiumAccount(true);
                            			player4.sendPacketToBoundSessions("0|A|STD|Premium avantajlarınız eklendi!");
                            			player4.sendPacketToBoundSessions("0|A|STD|2. Slot çubuğunuzu görmek için oyunu yeniden başlatın!");
                            		Log.pt("'"+player4.getAccount().getUserId()+"' ID'li ve '"+player4.getAccount().getUsername()+"' kullanıcı adlı oyuncuya premium avantajları eklendi!");
                            		}
                            		
                                	QueryManager.saveAccount(player4.getAccount());
                            	}
                            	else
                            	{
                            		player4.getAccount().setPremiumAccount(false);
                            		player4.sendPacketToBoundSessions("0|A|STD|Premium avantajlarınız kaldırıldı!");
                            		Log.pt("'"+player4.getAccount().getUserId()+"' ID'li ve '"+player4.getAccount().getUsername()+"' kullanıcı adlı oyuncudan premium avantajları alındı!");
                            		QueryManager.saveAccount(player4.getAccount());
                            	}
                              }
                            }
                          }
                        }
                            break;

                        case CMD_GIVE_DAMAGE:
                        	String username5 = "";
                            try {
                            	username5 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            int hasar = 0;
                            try {
                            	hasar = Integer.parseInt(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument uridium should be a number.");
                            }
                            for(GameSession account5 : GameManager.getGameSessions()){
                            final Player player5 = account5.getPlayer();
                            if (account5 != null) {
                            	if(player5 != null) {
                            		if(player5.getAccount().getShipUsername().equals(username5)) {
                            		player5.getAccount().getEquipmentManager().setDamageConfig1(hasar);
                            		player5.getAccount().getEquipmentManager().setDamageConfig2(hasar);
                            		player5.sendPacketToBoundSessions("0|A|STD|Geminize "+hasar+" hasar eklendi!");
                            		player5.sendPacketToBoundSessions("0|A|STD|Konfigürasyonunuzu değiştiriniz!");
                            	Log.pt("'"+player5.getAccount().getUserId()+"' ID'li ve '"+player5.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+hasar+" hasar eklendi!");
                            	QueryManager.saveAccount(player5.getAccount());
                            }
                          }
                        }
                      }
                            break;
                            
                        case CMD_GIVE_SPEED:
                            String username6 = "";
                            try {
                            	username6 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            int hiz = 0;
                            try {
                            	hiz = Integer.parseInt(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument uridium should be a number.");
                            }
                            for(GameSession account6 : GameManager.getGameSessions()){
                            final Player player6 = account6.getPlayer();
                            if (account6 != null) {
                            	if(player6 != null) {
                            		if(player6.getAccount().getShipUsername().equals(username6)) {
                            		player6.getAccount().getEquipmentManager().setSpeedConfig1(hiz);
                            		player6.getAccount().getEquipmentManager().setSpeedConfig2(hiz);
                            		player6.sendCommandToBoundSessions(player6.getSetSpeedCommand());
                            		player6.sendPacketToBoundSessions("0|A|STD|Geminize "+hiz+" hız eklendi!");
                            	Log.pt("'"+player6.getAccount().getUserId()+"' ID'li ve '"+player6.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+hiz+" hız eklendi!");
                            	QueryManager.saveAccount(player6.getAccount());

                            }
                          }
                        }
                      }
                            break;
                            
                        case CMD_CHANGE_MAP:
                        	String username7 = "";
                            try {
                            	username7 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            short mapID = 0;
                            try {
                            	mapID = Short.parseShort(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument uridium should be a number.");
                            }
                            for(GameSession account7 : GameManager.getGameSessions()){
                            final Player player7 = account7.getPlayer();
                            if (account7 != null) {
                            	if(player7 != null) {
                            		if(player7.getAccount().getShipUsername().equals(username7)) {                                        
                                        player7.jumpPortal(mapID, 0, 0);                                        
		                            	Log.pt("'"+player7.getAccount().getUserId()+"' ID'li ve '"+player7.getAccount().getUsername()+"' kullanıcı adlı oyuncu '"+mapID+"' ID'li haritaya taşındı!");
		                            	QueryManager.saveAccount(player7.getAccount());
		                            }
		                          }
		                        }
		                      }
                            break;
                            
                        case CMD_CHAT_UNBAN:
                        	String username8 = "";
                            try {
                            	username8 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            for(GameSession account8 : GameManager.getGameSessions()){
                            final Player player8 = account8.getPlayer();
                            if (account8 != null) {
                            	if(player8 != null) {
                            		if(player8.getAccount().getShipUsername().equals(username8)) {
                            		player8.sendPacketToBoundSessions("0|A|STD|Banınız kaldırıldı, oyununuzu yenileyiniz!");
                            		
                            		Log.pt("'"+player8.getAccount().getUserId()+"' ID'li ve '"+player8.getAccount().getUsername()+"' kullanıcı adlı oyuncunun chat banı kaldırıldı!");
                            	QueryManager.saveAccount(player8.getAccount());
                            }
                          }
                        }
                      }
                            break;
                            
                        case CMD_SPAWN_NPC:
                        	String username9 = "";
                            try {
                            	username9 = commandSplit[1];
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                        	int shipID = 0;
                            try {
                            	shipID = Integer.parseInt(commandSplit[2]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            int amount = 0;
                            try {
                            	amount = Integer.parseInt(commandSplit[3]);
                            } catch (NumberFormatException e) {
                                Log.pt("Invalid command syntax. Second argument userID should be a number.");
                            }
                            for(GameSession account9 : GameManager.getGameSessions()){
                            final Player player9 = account9.getPlayer();
                            if (account9 != null) {
                            	if(player9 != null) {
                            		if(player9.getAccount().getShipUsername().equals(username9)) {
                         //           for (int i = amount; i > 0; i--) {
                         //               final Alien alien = new Alien(shipID, player9.getCurrentSpaceMapId(),
                         //               		player9);
                        //                player9.getCurrentSpacemap()
    					//				.addAlien(alien);
                       //         } 
                              }
                            }
                          }
                        }
                            break;
                            
                        default:
                            Log.pt("Command \"" + commandSplit[0] + "\" not found. Possibly re-check spelling");
                            break;
                    }

                }

            }

        }

    }

    private void stopProcessing() {
        if (this.mCmdThread != null && this.mCmdThread.isAlive()) {
            this.mCmdThread.interrupt();
        }
    }

    private void executeSendToUser(final int pUserId, final String pPacketData) {
        this.mServerManager.sendPacketToUID(pUserId, pPacketData);
    }

    public void executeClose() {
        this.mServerManager.shutdown(30);
    }

    public void restartManual(final int pSeconds) {
        this.mServerManager.shutdown(pSeconds);
    }

    public void printHelp() {
        Log.pt("<=============================<Emulator Komut Listesi>=============================>");
        Log.pt("Belirli oyuncuya uridium vermek için: 'uridium (userID) (uridium)'");
        Log.pt("Belirli oyuncuya credi vermek için: 'credits (userID) (credits)'");
        Log.pt("Belirli oyuncuya başlık vermek için: 'baslik (userID) (başlık loot id)'");
        Log.pt("Belirli oyuncuya premium vermek için: 'premium (userID) (1)'");
        Log.pt("Belirli oyuncudan premium almak için: 'premium (userID) (0)'");
        Log.pt("Belirli oyuncunun hasarını artırmak için: 'hasar (userID) (hasar)'");
        Log.pt("Belirli oyuncunun hızını artırmak için: 'hiz (userID) (hız)'");
        Log.pt("Belirli saniye restart atmak için: 'restart_manual (saniye)'");
        Log.pt("Belirli oyuncunun haritasına değiştirmek için: 'tasi (userID) (harita ID)'");
        Log.pt("30 saniyelik restart atmak için: 'restart'");
        Log.pt("<==================================================================================>");
    }

}
