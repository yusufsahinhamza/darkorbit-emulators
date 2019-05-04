package net.socket_server;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.Socket;
import mysql.QueryManager;
import net.chat_server.ChatClientConnection;
import net.game_server.GameSession;
import org.json.JSONObject;
import simulator.GameManager;
import simulator.map_entities.movable.Player;
import simulator.system.SpaceMap;
import simulator.system.ships.ShipFactory;
import simulator.users.Account;
import storage.SpaceMapStorage;
import utils.Log;

public class SocketServerClientConnection
        implements Runnable {

    public Socket mSocket;

    private BufferedReader mSocketInputStreamReader;
    private BufferedWriter mSocketInputStreamWriter;
    private Thread mSocketClientConnectionThread;

    public SocketServerClientConnection(Socket pSocket) {
        this.mSocket = pSocket;

        try {
            this.mSocketInputStreamReader = new BufferedReader(new InputStreamReader(pSocket.getInputStream()));
            this.mSocketInputStreamWriter = new BufferedWriter(new OutputStreamWriter(pSocket.getOutputStream()));
            this.mSocketClientConnectionThread = new Thread(this);
            this.mSocketClientConnectionThread.setDaemon(true);
            this.mSocketClientConnectionThread.start();
        } catch (IOException e) {
            Log.pt("SocketServerClientConnection.java'da bir sorun var!");
        }
    }

    public void run() {
        try {
            String line = this.mSocketInputStreamReader.readLine();
            if (line != null) {
                assemblePacket(line);
            }
            this.mSocketInputStreamReader.close();
            this.mSocketInputStreamWriter.close();
            this.mSocket.close();
        } catch (Exception e) {
            Log.pt("Couldn't read packet!");
            Log.pt(e.getMessage());
        }
    }

    public static String byteArrayToHex(byte[] a) {
        String hex = new String();
        for (byte by : a) {
            hex += Byte.valueOf(by)
                       .toString();
            hex += " ";
        }
        hex = hex.substring(0, hex.length() - 1);
        return hex;
    }

    public void assemblePacket(final String str) {
        try {
            JSONObject json = new JSONObject(str);
            String packetID = json.getString("commandID");
        	int userID = json.getInt("userID");                        
            final GameSession gameSession = GameManager.getGameSession(userID);
            if(gameSession != null) {
            final long currentTime = System.currentTimeMillis();
            final Player player = gameSession.getPlayer();
            final Account account = gameSession.getAccount();
            final ChatClientConnection chatClientConnection = gameSession.getChatClientConnection();
            final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(player.getCurrentSpaceMapId());
            String message, name, petName;
            int uridium, honor, shipID, factionID, credits, experience;            
            switch (packetID) {
            		case "isim":
            			name = json.getString("name");                        
                		account.setShipUsername(name);
                    	spaceMap.removeGameSessionOnMap(userID);
                        for (final GameSession gameSession2 : player.getBoundGameSessions()) {
                            spaceMap.addAndInitGameSession(gameSession2);
                        }
                        player.sendCommandToInRange(player.getShipCreateCommand((short)0,false));
                		break;
            		case "petIsmi":
            			petName = json.getString("petName");                        
                		account.setPetName(petName);

                		break;
                    case "bonus_ver":
                        uridium = json.getInt("uridium");  
                        honor = json.getInt("honor");    
                        experience = json.getInt("experience");                        
                    	player.getAccount().changeUridium(uridium);                   	
                    	player.getAccount().changeHonor(honor);                    	
                    	player.getAccount().changeExperience(experience);      	
                    	player.sendPacketToBoundSessions("0|LM|ST|EP|" + experience + "|" + player.getAccount()
                                .getExperience() + "|" +
                                player.getAccount()
						    .getLevel());
                    	player.sendPacketToBoundSessions("0|LM|ST|HON|" + honor + "|" + player.getAccount()
						                                 .getHonor());
                    	player.sendPacketToBoundSessions("0|LM|ST|URI|" + uridium + "|" + player.getAccount()
						.getUridium());
                    	QueryManager.saveAccount(account);
                        break;
                    case "kupon_kullan":
                        uridium = json.getInt("uridium");                        
                    	player.getAccount().changeUridium(uridium);
                    	player.sendPacketToBoundSessions("0|LM|ST|URI|" + uridium + "|" + player.getAccount()
                                .getUridium());
                    	credits = json.getInt("credits"); 
                    	if(credits != 0) {
                    		player.getAccount().changeCredits(credits);
                    		player.sendPacketToBoundSessions("0|LM|ST|CRE|" + credits + "|" + player.getAccount()
                                .getCredits()); 
                    	}
                        break;
                    case "gemi_degistir":
                    	shipID = json.getInt("shipID"); 
                    	if(player.isInEquipZone()) {
                    		if(!player.getLaserAttack().isAttackInProgress()) {
                    			if ((currentTime - player.getLastDamagedTime()) >= 10000) {
                    				player.changePlayerShip(ShipFactory.getPlayerShip(shipID));
                    			}
                    		}
                    	}                	
                        break;
                    case "mesaj":
                    	message = json.getString("message"); 
                    	if(chatClientConnection != null) {
                    		chatClientConnection.sendMessage(message); 
                    	}
                        break;
                    case "sirket_degistir":
                    	factionID = json.getInt("factionID"); 
                    	uridium = json.getInt("uridium"); 
                    	honor = json.getInt("honor"); 
                    	
                    	player.getAccount().setFactionId((short) factionID);
                    	player.getAccount().setUridium(uridium);
                    	player.getAccount().setHonor(honor);

                    	int positionX = 0, positionY = 0, mapID = 0;

                    	if(player.getCurrentSpaceMapId() == 13 || player.getCurrentSpaceMapId() == 14
                         	   || player.getCurrentSpaceMapId() == 15 || player.getCurrentSpaceMapId() == 16) {	        	   
                             	if(account.getFactionId() == 1) {
                             		positionX = 1600;
                             		positionY = 1600;
                             		mapID = 13;
                             	} else if(account.getFactionId() == 2) {                             		
                             		positionX = 19500;
                             		positionY = 1500;
                             		mapID = 14;
                             	} else if(account.getFactionId() == 3) {
                             		positionX = 19500;
                             		positionY = 11600;
                             		mapID = 15;
                             	}	
                         	} else {
                             	if(account.getFactionId() == 1) {
                             		positionX = 2000;
                             		positionY = 6000;
                             		mapID = 20;
                             	} else if(account.getFactionId() == 2) {
                             		positionX = 10000;
                             		positionY = 2000;
                             		mapID = 24;
                             	} else if(account.getFactionId() == 3) {
                             		positionX = 18500;
                             		positionY = 6000;
                             		mapID = 28;
                             	}
                         	}
                        
                        player.jumpPortal((short) mapID, positionX, positionY);
                        
                        QueryManager.saveAccount(account);
                        break;
            		case "ekipman_guncelle":
            	        if(player.isInEquipZone()) {
            	        	QueryManager.loadEquipments(account);
            	        	player.sendCommandToBoundSessions(player.getSetSpeedCommand());
            	        	player.sendCommandToBoundSessions(player.getShieldUpdateCommand());
            	        }
                		break;
            }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
