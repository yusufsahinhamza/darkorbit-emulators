package net.chat_server.assemblies;

import net.chat_server.ChatClientConnection;
import net.chat_server.Constants;
import net.game_server.GameSession;
import net.utils.ServerUtils;

import java.net.Socket;

import simulator.GameManager;
import simulator.users.Account;
//import java.io.*;

/**
 Description: This class will assemble a login request

 @author Manulaiko
 @date 07/04/2014
 @file LoginAssembly.java
 @package net.chat.assemblies
 @project SpaceBattles Emulator */
public class LoginAssembly
        extends ServerUtils {

    public int                  userID;
    public String               sessionID;
    public Socket               socket;
    public ChatClientConnection user;
    public Account              mAccount;

    /**
     Constructor

     @param socket:
     the socket we will use
     */
    public LoginAssembly(Socket socket, ChatClientConnection user) {
        this.socket = socket;
        this.user = user;
    }

    /**
     Description: Assemble a login request and check if the user can login

     @param paket:
     the packet to assemble

     @return true if the user can login, false if not
     */
    public boolean assembleLoginRequest(String paket) {
        //Split packet
        String[] packet = paket.split(Constants.PARAM_SEPERATOR);

        //String username  = packet[0];
        int userID = Integer.parseInt(packet[1]);
        this.sessionID = packet[2];
        /*int pid          = Integer.parseInt(packet[3]);
        String lang      = packet[4];
		String clan      = packet[5];
		String version   = packet[6];
		*/
        //int unknow       = Integer.parseInt(packet[7]);
        //int unknow       = Integer.parseInt(packet[8]);
        if (userID == 0) {
            return false;
        }
        final GameSession gameSession = GameManager.getGameSession(userID);
        if (gameSession != null) {
            this.mAccount = gameSession.getAccount();
            //The user exits!!
            //Now we must check the sessionID
            if (this.mAccount.getSessionId()
                             .equals(sessionID)) {
                //SessionID is correct!
                //Now see if it's online
                if (!GameManager.isUserInGame(userID)) {
                    System.out.println("User " + this.mAccount.getUserId() + " isn't ingame!");

                    return false;
                } else {
                    //Check if user is already in chat
                    if (gameSession.getChatClientConnection() == null) {
                        //user isn't connected
                        gameSession.setChatClientConnection(this.user);
                        sendData();
                        return true;
                    } else {
                        //user is already in chat, relog it
                        this.user = gameSession.getChatClientConnection();
                        sendData();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /**
     Description: Sends login packets
     */
    public void sendData() {
        sendChatAndUpdaterPackets(this.socket, "bv%" + this.user.userID + "#");
        //TODO send custom rooms
        sendChatAndUpdaterPackets(this.socket,
                                  "by%1|Global|0|-1|0|0}2|MMO|1|1|0|0}3|EIC|2|2|0|0}4|VRU|3|3|0|0}5|Clan Search|5|-1|0|0#");
    }
}