package net.chat_server;

import mysql.QueryManager;
import net.chat_server.assemblies.LoginAssembly;
import net.game_server.GameSession;
import net.utils.ServerUtils;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.Socket;
import java.text.SimpleDateFormat;
import java.util.Collection;
import java.util.Date;
import java.util.Locale;
import java.util.Vector;
import simulator.GameManager;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.map_entities.movable.Spaceball;
import simulator.netty.serverCommands.ClientUITooltipTextFormatModule;
import simulator.netty.serverCommands.DestructionTypeModule;
import simulator.netty.serverCommands.KillScreenOptionModule;
import simulator.netty.serverCommands.KillScreenOptionTypeModule;
import simulator.netty.serverCommands.KillScreenPostCommand;
import simulator.netty.serverCommands.MessageLocalizedWildcardCommand;
import simulator.netty.serverCommands.MessageWildcardReplacementModule;
import simulator.netty.serverCommands.PriceModule;
import simulator.netty.serverCommands.ShipDeselectionCommand;
import simulator.netty.serverCommands.ShipDestroyCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.system.SpaceMap;
import simulator.system.ships.ShipFactory;
import simulator.users.Account;
import simulator.users.ClientSettingsManager;
import simulator.utils.DefaultAssignings;
import storage.SpaceMapStorage;
import utils.Settings;
import utils.Log;

/**
 Description: This is the thread that will handle each user in the chat

 @author Manulaiko
 @date 22/02/2014
 @file ConnectionManager.java
 @package net.game.newClient
 @project SpaceBattles 
 Edited by LEJYONER.
 */
public class ChatClientConnection
        implements Runnable {

    public Socket         socket;
    public BufferedReader in;
    public Thread         thread;

    public int userID = 0, room = -1;
    public String sessionID = "";
    public Account user;

    public LoginAssembly loginAssembly;
    private GameManager  mGameManager;
    
    /**
     Constructor

     @param socket:
     The current client socket
     */
    public ChatClientConnection(Socket socket) {
        try {
            //Same as net.game.Server()
            if(Settings.TEXTS_ENABLED) { System.out.println("Received connection!"); }
            this.socket = socket;
            in = new BufferedReader(new InputStreamReader(socket.getInputStream()));

            this.loginAssembly = new LoginAssembly(this.socket, this);
            this.mGameManager = GameManager.getInstance();
            
            thread = new Thread(this);
            thread.setDaemon(true);
            thread.start();
        } catch (IOException e) {
            System.out.println("Couldn't process connection!");
            System.out.println(e.getMessage());
        }
    }

    /**
     Description: This method will read the packet char by char
     */
    public void run() {
        try {
            //Same as net.game.ConnectionManager()
            String packet = "";
            char[] packetChar = new char[1];

            while (in.read(packetChar, 0, 1) != -1) {
                if (packetChar[0] != '\u0000' && packetChar[0] != '\n' && packetChar[0] != '\r') {
                    packet += packetChar[0];
                } else if (!packet.isEmpty()) {

                	if(Settings.TEXTS_ENABLED) { System.out.println("Chat received: " + packet); }

                    packet = new String(packet.getBytes(), "UTF-8");
                    assemblePacket(packet);
                    packet = "";
                }
            }
        } catch (IOException e) {

        }
    }

    /**
     Description: This method will assemble the packets

     @param paket:
     The packet to assemble
     */
    public void assemblePacket(String paket) {
        if (paket.equals("<policy-file-request/>")) {
            ServerUtils.sendPolicy(socket);
            if(Settings.TEXTS_ENABLED) { System.out.println("Policy file sent"); }
        } else {
            //split the packet
            String[] packet = paket.split(Constants.MSG_SEPERATOR);

            switch (packet[0]) {
                //packet[0] = "bu"
                case Constants.CMD_USER_LOGIN:
                    if (this.loginAssembly.assembleLoginRequest(packet[2])) {
               //         //TODO
                        //set ConnectionManager data
                    		this.userID = this.loginAssembly.userID;
                        	this.sessionID = this.loginAssembly.sessionID;
                	         this.user = this.loginAssembly.mAccount;
                	       }
                    break;

                //packet[0] = "bx"
                case Constants.CMD_GET_USER_ROOMLIST:
                    this.room = Integer.parseInt(packet[1]);
                    break;

                //packet[0] = "bz"
                case Constants.CMD_USER_JOIN:
                    this.room = Integer.parseInt(packet[2].substring(0, packet[2].length() - 1));
                    
                    String packet2 = "dq%Duel atmak için: /duel [isim]#";
                    String packet3 = "dq%Duel kabul etmek için: /a#";
                    String packet4 = "dq%Duel reddetmek için: /r#";
                    final GameSession pGameSession = GameManager.getGameSession(this.user.getUserId());
                        //Check if user is in same room
                        ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                        if (chatClientConnection != null) {
                            if (chatClientConnection.room == this.room) {
                                //Send packet
                                ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet2 + "#");
                                ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet3 + "#");
                                ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet4 + "#");
                            }
                        }
                    
                    break;

                //packet[0] = "a"
                case Constants.CMD_USER_MSG:
                    sendMessage(packet[2]);
                    break;
            }
        }
    }

    /**
     Description: This method will send a message in the chat

     @param message:
     The message
     */
    
    public void restartGame(final int pSeconds) {   	
        for (int i = pSeconds; i > 0; i--) {
            try {
                this.mGameManager.sendServerDisconnectMessage(i);
                Thread.sleep(1000);   
                
            } catch (InterruptedException e) {
                Log.p("Server shutdown interrupted", e.getMessage());
            }
        }
		int playerCount = 0;
        for (GameSession gameSessionAll : GameManager.getGameSessions()) {
        	final Account account = gameSessionAll.getAccount();             	
            if (account != null) {
                final Player player = account.getPlayer();
                if (player != null) {               	
                    //kaydetme içeriği başlangıç
                	account.setOnline(false);
                	player.setInEquipZone(true);
                    QueryManager.saveAccount(player.getAccount());
                        playerCount++;
                    //kaydetme içeriği son	
                }
            }
        }
        Log.p(playerCount + " kullanıcı kaydedildi!");
        this.mGameManager.shutdown();       
        Log.p("<======================================","<Shutdown succeeded>","======================================>");
        System.exit(0);
    }
    
    public void sendMessage(String message) {
    	try{
        if (!message.startsWith("/")) {
            String packet = null;
            
            if (this.user.isAdmin()) {
                packet = "j%" + room + "@" + this.user.getShipUsername() + "@" + message.substring(0, message.length() - 1);
            } else if (this.user.isCm()) {
                packet = "j%" + room + "@" + "[CM] "+this.user.getShipUsername()+"" + "@" + message.substring(0, message.length() - 1) + "@" + "3"; 
            } else if (QueryManager.checkChatBanned(this.user.getUserId())) {          	
          		    String packet2 = "dq%You have been banned from the chat.#";
                final GameSession pGameSession = GameManager.getGameSession(this.user.getUserId());
                if(pGameSession != null){
                    ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                    if (chatClientConnection != null) {
                        if (chatClientConnection.room == this.room) {
                            ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet2 + "#");
                        }
                    }
                }
                
            } else {
                //Normal user
                packet = "a%" + room + "@" + this.user.getShipUsername() + "@" + message.substring(0, message.length() - 1);
            }
            
            //Check if user is in a clan
            if (!this.user.getClanTag()
                          .equals("") && !this.user.isAdmin() && !this.user.isCm()) {
                //User is in a clan, let's add the clan tag to the packet
                packet += "@" + user.getClanTag();
            }
            
            for (final GameSession pGameSession : GameManager.getGameSessions()) {
                //Check if user is in same room
                ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                if (chatClientConnection != null) {
                    if (chatClientConnection.room == this.room) {
                        //Send packet
                        ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                    }
                }
            }             
        	
        } else {
            message = message.substring(0, message.length() - 1);
            try {
                if (message.startsWith("/ban") && (this.user.isAdmin() || this.user.isCm())) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final String sebep = message.substring(4 + username.length(), message.length());
                    
                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                            if(player.getAccount().getShipUsername().equals(username)) { 
                            	if(player.getAccount().getUserId() != this.user.getUserId()) {
                        	player.sendPacketToBoundSessions("0|A|STD|Chatiniz Administrator tarafından yasaklandı!");
                            String packet = "at%#";                        
                            final GameSession pGameSession = GameManager.getGameSession(player.getAccount().getUserId());
                            if(pGameSession != null){
                                ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                                if (chatClientConnection != null) {
                                    if (chatClientConnection.room == this.room) {
                                        ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                                    }
                                }
                            }
                            Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncu chatten banlandı!");
                            QueryManager.saveAccount(player.getAccount());
                            
                        	SimpleDateFormat tarihFormati = new SimpleDateFormat("dd MMMM yyyy / HH:mm", new Locale("tr"));
                            String tarih = tarihFormati.format(new Date());
                            final String Tarih = ""+tarih+"";
                            QueryManager.saveBanned(Tarih, player, this.user.getPlayer(), sebep, false);
                        }
                      }
                    }
                  }
                }
                    
                    String packet = "dq%"+ username +" yasaklandı.#";                           
                    for(GameSession pGameSession : GameManager.getGameSessions()) {
                    if(pGameSession != null){
                        ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                        if (chatClientConnection != null) {
                            if (chatClientConnection.room == this.room) {
                                ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                            }
                        }
                    }
                 }
                    
              } else if (message.startsWith("/unban") && (this.user.isAdmin() || this.user.isCm())) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    
                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        		if(player.getAccount().getUserId() != this.user.getUserId()) {                         
                            player.sendPacketToBoundSessions("0|A|STD|Chat banınız Administrator tarafından kaldırıldı!");                            
                            Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncunun chat banı kaldırıldı!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                }
              } else if (message.startsWith("/kick") && (this.user.isAdmin() || this.user.isCm())) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    
                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        		if(player.getAccount().getUserId() != this.user.getUserId()) {
                            String packet = "as%#"; 
                            final GameSession pGameSession = GameManager.getGameSession(player.getAccount().getUserId());
                            if(pGameSession != null){
                                ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                                if (chatClientConnection != null) {
                                    if (chatClientConnection.room == this.room) {
                                        ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                                    }
                                }
                            }
                            Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncu chatten kicklendi!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                }
                    
                    String packet = "dq%"+ username +" atıldı.#";                           
                    for(GameSession pGameSession : GameManager.getGameSessions()) {
                    if(pGameSession != null){
                        ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                        if (chatClientConnection != null) {
                            if (chatClientConnection.room == this.room) {
                                ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                            }
                        }
                    }
                 }
                    
              } else if (message.startsWith("/w") && !QueryManager.checkChatBanned(this.user.getUserId())) {
                	
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    
                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        		if(player.getAccount().getUserId() != this.user.getUserId()) {                       		
                            final GameSession pGameSession = GameManager.getGameSession(this.user.getUserId());
                            if(pGameSession != null){
                                ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                                if (chatClientConnection != null) {
                                    if (chatClientConnection.room == this.room) {
                                        ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, "cw%" + username + "@ " + message.substring(4 + username.length(), message.length()) + "#");
                                    }
                                }
                            }  
                            final GameSession pGameSession2 = GameManager.getGameSession(player.getAccount().getUserId());
                            if(pGameSession2 != null){
                                ChatClientConnection chatClientConnection = pGameSession2.getChatClientConnection();
                                if (chatClientConnection != null) {
                                    if (chatClientConnection.room == this.room) {
                                        ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, "cv%" + this.user.getShipUsername() + "@ " + message.substring(4 + username.length(), message.length()) + "#");
                                    }
                                 }
                              }   
                           }
                        }
                     }
                  }
               } 	
            } else if (message.startsWith("/başlık") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final String titleID = strings[2];

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        		this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+"'a "+titleID+" başlığı verildi!");
                        	player.getAccount().setTitle(titleID);
                        	final String packet = "0|n|t|" + player.getAccount().getUserId() + "|0|" + titleID;
                            player.sendPacketToBoundSessions(packet);
                            player.sendPacketToInRange(packet);
                            Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+titleID+" başlığı verildi!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                }
                else if(message.startsWith("/credits") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final int credits = Integer.parseInt(strings[2]);

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        	player.getAccount().changeCredits(credits);
                        	player.sendPacketToBoundSessions("0|LM|ST|CRE|" + credits + "|" + player.getAccount()
                                    .getCredits());
                        	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+"'a "+credits+" credits verildi!");
                        	
                        	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+credits+" credits verildi!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                } else if(message.startsWith("/uridium") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final int uridium = Integer.parseInt(strings[2]);

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        	player.getAccount().changeUridium(uridium);
                        	player.sendPacketToBoundSessions("0|LM|ST|URI|" + uridium + "|" + player.getAccount()
                                    .getUridium());
                        	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+"'a "+uridium+" uridium verildi!");
                        	
                        	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+uridium+" uridium verildi!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                } else if(message.startsWith("/premium") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final int premium = Integer.parseInt(strings[2]);

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        	if(premium == 1)
                        	{
                        		if(player.getAccount().isPremiumAccount())
                        		{
                        			Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncunun premium avantajları zaten aktif!");
                        		}
                        		else
                        		{
                        			player.getAccount().setPremiumAccount(true);
                        			player.sendPacketToBoundSessions("0|A|STD|Premium avantajlarınız eklendi!");
                        		Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya premium avantajları eklendi!");
                        		}
                        		
                            	QueryManager.saveAccount(player.getAccount());
                        	}
                        	else
                        	{
                        		player.getAccount().setPremiumAccount(false);
                        		player.sendPacketToBoundSessions("0|A|STD|Premium avantajlarınız kaldırıldı!");
                        		Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncudan premium avantajları alındı!");
                        		QueryManager.saveAccount(player.getAccount());
                        	}
                        }
                      }
                    }
                  }
                } else if(message.startsWith("/hasar") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final int hasar = Integer.parseInt(strings[2]);

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        	player.getAccount().getEquipmentManager().setDamageConfig1(hasar);
                        	player.getAccount().getEquipmentManager().setDamageConfig2(hasar);
                        	player.sendPacketToBoundSessions("0|A|STD|Geminize "+hasar+" hasar eklendi!");
                        	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+hasar+" hasar eklendi!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                } else if(message.startsWith("/hız") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final int hiz = Integer.parseInt(strings[2]);

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        	player.getAccount().getEquipmentManager().setSpeedConfig1(hiz);
                        	player.getAccount().getEquipmentManager().setSpeedConfig2(hiz);
                        	player.sendCommandToBoundSessions(player.getSetSpeedCommand());
                        	player.sendPacketToBoundSessions("0|A|STD|Geminize "+hiz+" hız eklendi!");
                        	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncuya "+hiz+" hız eklendi!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                } else if(message.startsWith("/taşı") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final short mapID = Short.parseShort(strings[2]);

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        		this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+"' "+mapID+"' ID'li haritaya taşındı!");                               
                                player.jumpPortal(mapID, 0, 0);
	                        	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncu '"+mapID+"' ID'li haritaya taşındı!");
	                            QueryManager.saveAccount(player.getAccount());
	                        }
	                      }
	                    }
	                  }
                } else if(message.startsWith("/jpbattle") && this.user.isAdmin()) {
                    for (int i = 10; i > 0; i--) {
                        try {
                        	ServerUtils.sendPacketToAllUsers("0|A|STD|Jackpot Battle {"+ i +"} saniye sonra başlayacaktır!");
                            Thread.sleep(1000);                                          
                        } catch (InterruptedException e) {
                            Log.p("Server shutdown interrupted", e.getMessage());
                        }
                    }
                    
                    for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                     	final Account account = gameSessionAll.getAccount(); 
                        if (account != null) {
                            final Player player = account.getPlayer();
                            if (player != null) {
                                int posX = 0, posY = 0;
                                if(player.getAccount().getFactionId() == 1) {
                                	posX = 1000;
                                	posY = 1000;
                                } else if (player.getAccount().getFactionId() == 2) {
                                	posX = 19700;
                                	posY = 1000;
                                } else if (player.getAccount().getFactionId() == 3) {
                                	posX = 10000;
                                	posY = 11300;
                                }
                                player.jumpPortal((short) 42, posX, posY);
                                //event içeriği son	
                                    
                            	Log.pt("Event başlatıldı!");
                                QueryManager.saveAccount(player.getAccount());
                            }
                        }
                    }                                 
                } else if(message.startsWith("/patlat") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];

                    for(GameSession account : GameManager.getGameSessions()){ 
                    if (account != null) {
                        final Player player = account.getPlayer();
                        if (player != null) {
                        	if(player.getAccount().getShipUsername().equals(username)) {
                        		
                        		this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+" patlatıldı!");
                        		player.setDestroyed(true);
                        		player.getLaserAttack().setAttackInProgress(false);
                        		player.setLockedTarget(null);

                                final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap((short) 42);
                                if(player.getCurrentSpaceMapId() == 42) {
                                final int kalanOyuncu = (spaceMap.getAllPlayers().size()) - 1;
                                final String geriSayim = "0|LM|ST|SLE|"+ kalanOyuncu;
                                ServerUtils.sendPacketToAllUsers(geriSayim);
                                }
                                
                                final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(player.getMapEntityId(), 1);
                                player.sendCommandToBoundSessions(shipDestroyCommand);
                                player.sendCommandToInRange(shipDestroyCommand);
                                
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
                                        new KillScreenPostCommand(this.user.getShipUsername(), "http://localhost/indexInternal.es?action=internalDock",
                                                                  "MISC", new DestructionTypeModule(DestructionTypeModule.PLAYER),
                                                                  killScreenOptionModules);
                                player.sendCommandToBoundSessions(killScreenPostCommand);
                                player.setInEquipZone(true);                             
                                QueryManager.saveAccount(player.getAccount());
                                
                        	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncu admin tarafından patlatıldı!");
                            QueryManager.saveAccount(player.getAccount());
                        }
                      }
                    }
                  }
                } else if(message.startsWith("/stta") && this.user.isAdmin()) {
                	
                ServerUtils.sendPacketToAllUsers("0|A|STD|"+ message.substring(5, message.length()) +"");
                    
                } else if(message.startsWith("/restart") && this.user.isAdmin()) {
                    final int saniye = 30;
                    
                    this.restartGame(saniye);
                    
                }  else if(message.startsWith("/m_restart") && this.user.isAdmin()) {
                	String[] strings = message.split(" ");
                	final int saniye = Integer.parseInt(strings[1]);  
                    
                    this.restartGame(saniye);
                    
                } else if(message.startsWith("/ship") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final String username = strings[1];
                    final int shipID = Integer.parseInt(strings[2]);                    
                    for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                    	final Account account = gameSessionAll.getAccount();             	
                        if (account != null) {
                            final Player player = account.getPlayer();
                            if (player != null) {
                            	if(player.getAccount().getShipUsername().equals(username)) {
                            	 player.changePlayerShip(ShipFactory.getPlayerShip(shipID));
                            	}
                            }
                        }
                    }
                } else if(message.startsWith("/npc") && this.user.isAdmin()) {
                    String[] strings = message.split(" ");
                    final int shipID = Integer.parseInt(strings[1]);
                    final int amount = Integer.parseInt(strings[2]);
                                      
                    final GameSession gameSession = GameManager.getGameSession(this.user.getUserId());
                    	final Account account = gameSession.getAccount();             	
                        if (account != null) {
                            final Player player = account.getPlayer();
                            if (player != null) { 
                                for (int i = amount; i > 0; i--) {
                                    final Alien alien = new Alien(shipID, player.getCurrentSpaceMap());
									player.getCurrentSpaceMap()
									.addAlien(alien);
                              }                                
                        } 
                     }
                  
               } else if (message.startsWith("/gameban") && this.user.isAdmin()) {
                   String[] strings = message.split(" ");
                   final String username = strings[1];
                   final String sebep = message.substring(4 + username.length(), message.length());
                   
                   for(GameSession account : GameManager.getGameSessions()){ 
                   if (account != null) {
                       final Player player = account.getPlayer();
                       if (player != null) {
                           if(player.getAccount().getShipUsername().equals(username)) { 
                           	if(player.getAccount().getUserId() != this.user.getUserId()) {
                           		this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+" oyundan yasaklandı!");
                       	   player.sendPacketToBoundSessions("0|A|STD|Hesabınız Administrator tarafından yasaklandı!");
                               String packet = "at%#" + room + "@" + this.user.getShipUsername() + "@" + message.substring(0, message.length() - 1);
	                   		player.setDestroyed(true);
	                   		player.getLaserAttack().setAttackInProgress(false);
	                   		player.setLockedTarget(null);

                           for(MovableMapEntity inRangeEntity : player.getInRangeMovableMapEntities()) {
                           	if(inRangeEntity instanceof Player) {
                           		final ShipRemoveCommand shipRemoveCommand = new ShipRemoveCommand(player.getMapEntityId());
                           		final Player otherPlayer = (Player) inRangeEntity;
                           		otherPlayer.sendCommandToBoundSessions(shipRemoveCommand);
                           		otherPlayer.removeInRangeMapEntity(player);
                           	}
                           }
                           player.removeAllInRangeMapIntities();
                           final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap((short) 42);
                           if(player.getCurrentSpaceMapId() == 42) {
                           final int kalanOyuncu = (spaceMap.getAllPlayers().size()) - 1;
                           final String geriSayim = "0|LM|ST|SLE|"+ kalanOyuncu;
                           ServerUtils.sendPacketToAllUsers(geriSayim);
                           }
                           
                           final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(player.getMapEntityId(), 1);
                           player.sendCommandToBoundSessions(shipDestroyCommand);
                           player.sendCommandToInRange(shipDestroyCommand);
                           
                           final GameSession gameSession = GameManager.getGameSession(player.getAccount().getUserId());
                           gameSession.close();
                           
                           player.getAccount().setOnline(false);
                           player.setInEquipZone(true);                             
                           QueryManager.saveAccount(player.getAccount());
                           
                           final GameSession pGameSession = GameManager.getGameSession(player.getAccount().getUserId());
                           if(pGameSession != null){
                               ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                               if (chatClientConnection != null) {
                                   if (chatClientConnection.room == this.room) {
                                       //Send packet
                                       ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                                   }
                               }
                           }
                           Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncu oyundan banlandı!");
                           QueryManager.saveAccount(player.getAccount());
                           
                       	SimpleDateFormat tarihFormati = new SimpleDateFormat("dd MMMM yyyy / HH:mm", new Locale("tr"));
                           String tarih = tarihFormati.format(new Date());
                           final String Tarih = ""+tarih+"";
                           QueryManager.saveBanned(Tarih, player, this.user.getPlayer(), sebep, true);
                       }
                     }
                   }
                 }
               }
             } else if(message.startsWith("/enable_shoot") && this.user.isAdmin()) { 
                 for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                 	final Account account = gameSessionAll.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                         Settings.SHOOT_ENABLED = true;
                         player.sendPacketToBoundSessions("0|A|STD|Ateş etme etkinleştirildi!");
                         }
                     }
                 }                 
            } else if(message.startsWith("/disable_shoot") && this.user.isAdmin()) { 
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                 	final Account account = gameSessionAll.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                        	 Settings.SHOOT_ENABLED = false;
                        	 player.sendPacketToBoundSessions("0|A|STD|Ateş etme kapatıldı!");
                         }
                     }
                }                       	 
            }  else if(message.startsWith("/enable_friend-shoot") && this.user.isAdmin()) { 
                 for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                 	final Account account = gameSessionAll.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                         Settings.FRIEND_SHOOT_ENABLED = true;
                         player.sendPacketToBoundSessions("0|A|STD|Dost ateşi etkinleştirildi!");
                         }
                     }
                 }                 
            } else if(message.startsWith("/disable_friend-shoot") && this.user.isAdmin()) { 
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                 	final Account account = gameSessionAll.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                        	 Settings.FRIEND_SHOOT_ENABLED = false;
                        	 player.sendPacketToBoundSessions("0|A|STD|Dost ateşi kapatıldı!");
                         }
                     }
                }                       	 
            } else if(message.startsWith("/enable_x2") && this.user.isAdmin()) { 
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                 	final Account account = gameSessionAll.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
              	         Settings.REWARD_DOUBLER_ENABLED = true;
              	         player.sendPacketToBoundSessions("0|A|STD|x2 Uridium etkinleştirildi!");
                         }
                     }
                }
            } else if(message.startsWith("/disable_x2") && this.user.isAdmin()) {
                for (GameSession gameSessionAll : GameManager.getGameSessions()) {
                 	final Account account = gameSessionAll.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
              	         Settings.REWARD_DOUBLER_ENABLED = false;
              	       player.sendPacketToBoundSessions("0|A|STD|x2 Uridium kapatıldı!");
                         }
                     }
                }
            } else if(message.startsWith("/start_spaceball") && this.user.isAdmin()) { 
            	 				ServerUtils.sendPacketToAllUsers("0|A|STD|Spaceball started!");
            	                Settings.SPACEBALL_ENABLED = true;            	                
                                final Spaceball spaceball = new Spaceball(Settings.SPACEBALL_TYPE, SpaceMapStorage.getSpaceMap((short) 16));
                                spaceball.getCurrentSpaceMap()
								.addSpaceball(spaceball);                                   
                                
                                for (GameSession gameSession : GameManager.getGameSessions()) {
                                 	final Account account = gameSession.getAccount(); 
                                 	final ClientSettingsManager clientSettingsManager = account.getClientSettingsManager();
                                     if (account != null) {
                                         final Player player = account.getPlayer();
                                         if (player != null) {
                                        	 player.sendCommandToBoundSessions(clientSettingsManager.getClientUIMenuBarsCommand());
                                         }
                                     }
                                }
                                                                                             
            } else if(message.startsWith("/stop_spaceball") && this.user.isAdmin()) { 
            	ServerUtils.sendPacketToAllUsers("0|A|STD|Spaceball is over!");
            	Settings.SPACEBALL_ENABLED = false;
                SpaceMap spaceMap = SpaceMapStorage.getSpaceMap((short) 16);
                if (spaceMap != null) {
                    final Collection<MovableMapEntity> allMapEntities = spaceMap.getAllMovableMapEntities()
                            .values();
                      for (final MovableMapEntity thisMapEntity : allMapEntities) {
                    	  if(thisMapEntity instanceof Spaceball) {                    		  
                    		  final Spaceball spaceball = (Spaceball) thisMapEntity;                    		  
                              for (final MovableMapEntity thisMapEntity2 : spaceball.getInRangeMovableMapEntities()) {
                              	if(thisMapEntity2 instanceof Player) {
                              		final Player player = (Player) thisMapEntity2;
                              		if(player.getLockedTarget() == spaceball) {
                              		player.getLaserAttack().setAttackInProgress(false);
                              		player.setLockedTarget(null);
                              		player.sendCommandToBoundSessions(new ShipDeselectionCommand());
                              		}
                              	}

                              }
                              spaceball.setPositionXY(21000, 13200);
                              spaceball.setSelectedFactionID(0);
                              spaceball.setMMODamage(0);
                              spaceball.setEICDamage(0);
                              spaceball.setVRUDamage(0);
                              spaceball.getCurrentSpaceMap().removeSpaceball(spaceball.getMapEntityId());
                              spaceball.getCurrentSpaceMap().removeSpaceballForAllInRangeMapIntities(spaceball.getMapEntityId());
                              final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(spaceball.getMapEntityId(), 1);
                              spaceball.sendCommandToInRange(shipDestroyCommand);
                    	  }
                      }
                }    
                for (GameSession gameSession : GameManager.getGameSessions()) {
                 	final Account account = gameSession.getAccount(); 
                 	final ClientSettingsManager clientSettingsManager = account.getClientSettingsManager();
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                        	 player.sendCommandToBoundSessions(clientSettingsManager.getClientUIMenuBarsCommand());
                         }
                     }
                }
            } else if(message.startsWith("/komutlar") && this.user.isAdmin()) { 
                	GameSession gameSession = GameManager.getGameSession(this.user.getUserId());               
                 	final Account account = gameSession.getAccount();             	
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                        	 String packet   = "dq%/ban [kullanıcı adı].#";
                        	 String packet2  = "dq%/unban [kullanıcı adı].#";
                        	 String packet3  = "dq%/kick [kullanıcı adı].#";
                        	 String packet4  = "dq%/gameban [kullanıcı adı].#";
                        	 String packet5  = "dq%/stta [yazı].#";
                        	 String packet6  = "dq%/taşı [kullanıcı adı] [map id].#";
                        	 String packet7  = "dq%/uridium [kullanıcı adı] [miktar].#";
                        	 String packet8  = "dq%/credits [kullanıcı adı] [miktar].#";
                        	 String packet9  = "dq%/başlık [kullanıcı adı] [başlık id].#";
                        	 String packet10 = "dq%/hasar [kullanıcı adı] [hasar].#";
                        	 String packet11 = "dq%/hız [kullanıcı adı] [hız].#";
                        	 String packet12 = "dq%/start_spaceball.#";
                        	 String packet13 = "dq%/stop_spaceball.#";
                        	 String packet14 = "dq%/x2_enable.#";
                        	 String packet15 = "dq%/x2_disable.#";
                        	 String packet16 = "dq%/friendshootenable.#";
                        	 String packet17 = "dq%/friendshootdisable.#";
                        	 String packet18 = "dq%/npc [npc id] [miktar].#";
                        	 String packet19 = "dq%/patlat [kullanıcı adı].#";
                        	 String packet20 = "dq%/restart.#";
                        	 String packet21 = "dq%/premium [kullanıcı adı] [1 veya 0].#";
                             final GameSession pGameSession = GameManager.getGameSession(player.getAccount().getUserId());
                             if(pGameSession != null){
                                 ChatClientConnection chatClientConnection = pGameSession.getChatClientConnection();
                                 if (chatClientConnection != null) {
                                     if (chatClientConnection.room == this.room) {
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet2 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet3 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet4 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet5 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet6 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet7 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet8 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet9 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet10 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet11 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet12 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet13 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet14 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet15 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet16 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet17 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet18 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet19 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet20 + "#");
                                         ServerUtils.sendChatAndUpdaterPackets(chatClientConnection.socket, packet21 + "#");
                                     }
                                 }
                             }
                         }
                     }                      	 
            } else if(message.startsWith("/danom") && this.user.isAdmin()) {
                final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(this.user.getPlayer().getCurrentSpaceMapId());
                
                for (Alien aliens : spaceMap.getAllAliens()) {
                	final ShipDestroyCommand shipDestroyCommand = new ShipDestroyCommand(aliens.getMapEntityId(), 1);
                	aliens.setLockedTarget(null);
                	aliens.getLaserAttack().setAttackInProgress(false);
                    spaceMap.removeAlien(aliens.getAlienId());
                    
                    for (MovableMapEntity movableMapEntity : aliens.getInRangeMovableMapEntities()) {
                    	if(movableMapEntity != null) {
                    		if(movableMapEntity instanceof Player) {
                    			((Player) movableMapEntity).sendCommandToBoundSessions(shipDestroyCommand);
                    		}
                    	}
                    }
                                   
            }
            } else if(message.startsWith("/h_taşı") && this.user.isAdmin()) {
                String[] strings = message.split(" ");
                final short mapID = Short.parseShort(strings[1]);
                
                for (GameSession gameSession : GameManager.getGameSessions()) {
                 	final Account account = gameSession.getAccount(); 
                     if (account != null) {
                         final Player player = account.getPlayer();
                         if (player != null) {
                             int posX = 0, posY = 0;
                             if(player.getAccount().getFactionId() == 1) {
                             	posX = 1000;
                             	posY = 1000;
                             } else if (player.getAccount().getFactionId() == 2) {
                             	posX = 19700;
                             	posY = 1000;
                             } else if (player.getAccount().getFactionId() == 3) {
                             	posX = 10000;
                             	posY = 11300;
                             }
                             player.jumpPortal(mapID, posX, posY);
                         }
                     }
                }
            } else if(message.startsWith("/p_ban") && this.user.isAdmin()) {
                String[] strings = message.split(" ");
                final String username = strings[1];
                final int yuzde = Integer.parseInt(strings[2]);

                for(GameSession account : GameManager.getGameSessions()){ 
                if (account != null) {
                    final Player player = account.getPlayer();
                    if (player != null) {
                    	if(player.getAccount().getShipUsername().equals(username)) {                  		
                    		if(yuzde == 2 || yuzde == 4) {
                    			
                    	final long uridium = player.getAccount().getUridium() - player.getAccount().getUridium() / yuzde;
                    	final long honor = player.getAccount().getHonor() - player.getAccount().getHonor() / yuzde;
                    	final long killedGoliath = honor / 512;
                    	player.getAccount().setKilledGoliath(killedGoliath);
                        player.getAccount().setHonor(honor);
                        player.sendPacketToBoundSessions("0|LM|ST|HON|-" + honor + "|" + player.getAccount() .getHonor());
                    	player.getAccount().setUridium(uridium);
                    	player.sendPacketToBoundSessions("0|LM|ST|URI|-" + uridium + "|" + player.getAccount().getUridium());
                    	
                    	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+" kesintileri yapıldı!");
                    	
                    	Log.pt("'"+player.getAccount().getUserId()+"' ID'li ve '"+player.getAccount().getUsername()+"' kullanıcı adlı oyuncunun puanları "+yuzde+"'ye bölündü!");
                        QueryManager.saveAccount(player.getAccount());
                    }
                    	}
                  }
                }
              }
            } else if(message.startsWith("/id") && this.user.isAdmin()) {
                String[] strings = message.split(" ");
                final String username = strings[1];

                for(GameSession account : GameManager.getGameSessions()){ 
                if (account != null) {
                    final Player player = account.getPlayer();
                    if (player != null) {
                    	if(player.getAccount().getShipUsername().equals(username)) {                  		                   				
                    	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+username+" adlı oyuncunun ID'si: "+player.getAccount().getUserId()+"");
                    }
                  }
                }
              }
            } else if(message.startsWith("/duel")) {
                String[] strings = message.split(" ");
                final String username = strings[1];

                if(this.user.getPlayer().getDuelUser() == null) {
                for(GameSession account : GameManager.getGameSessions()){ 
                if (account != null) {
                    final Player player = account.getPlayer();
                    if (player != null) {
                    	if(player.getAccount().getShipUsername().equals(username)) {                  		
                    	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|"+player.getAccount().getShipUsername()+" adlı oyuncuya meydan okudun!");
                    	player.sendPacketToBoundSessions("0|A|STD|"+this.user.getShipUsername()+" sana meydan okuyor!");
                    	this.user.getPlayer().setDuelUser(player);
                    	player.setDuelUser(this.user.getPlayer());
                    }
                  }
                }
              }
                } else {
                	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|Zaten bir meydan okuman bulunuyor!");
                }
            } else if(message.startsWith("/a")) {

                if(this.user.getPlayer().getDuelUser() != null) {
                for(GameSession account : GameManager.getGameSessions()){ 
                if (account != null) {
                    final Player player = account.getPlayer();
                    if (player != null) {
                    	if(player.getAccount().getShipUsername().equals(this.user.getPlayer().getDuelUser().getAccount().getShipUsername())) {                  		                   				
                    		
                    		if(this.user.getPlayer().isInSecureZone() && player.isInSecureZone()) {                    		
                    		
                    		player.jumpPortal((short) Settings.DUEL_MAP, 800, 3100);
                    		
                    		
                            final Player thisPlayer = this.user.getPlayer();
                            
                            thisPlayer.jumpPortal((short) Settings.DUEL_MAP, 9700, 3100);
                            
                    		}
                    }
                  }
                }
              }
                } else {
                	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|Bir meydan okuman yok!");
                }
            } else if(message.startsWith("/r")) {

                if(this.user.getPlayer().getDuelUser() != null) {
                for(GameSession account : GameManager.getGameSessions()){ 
                if (account != null) {
                    final Player player = account.getPlayer();
                    if (player != null) {
                    		this.user.getPlayer().getDuelUser().sendPacketToBoundSessions("0|A|STD|"+this.user.getShipUsername()+" meydan okumanı reddetti!");
                    		this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|Meydan okumayı reddettin!");
                    		this.user.getPlayer().setDuelUser(null);
                    }
                }
                }
                } else {
                	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|Bir meydan okuman yok!");
                }
            } else if(message.startsWith("/harita_menzil") && this.user.isAdmin()) {
                String[] strings = message.split(" ");
                final int tip = Integer.parseInt(strings[1]);
                
                if(tip == 0) {
                	SpaceMap.VISIBILITY_RANGE = 2000;
                	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|Harita menzilleri normal olarak ayarlandı!");
                } else if (tip == 1) {
                	SpaceMap.VISIBILITY_RANGE = 99999;
                	this.user.getPlayer().sendPacketToBoundSessions("0|A|STD|Harita menzilleri sınırsız olarak ayarlandı!");
                }

            }
            } catch (Exception e) {           	
            }
        }
    }  catch(Exception e) {
    }
    }
}
