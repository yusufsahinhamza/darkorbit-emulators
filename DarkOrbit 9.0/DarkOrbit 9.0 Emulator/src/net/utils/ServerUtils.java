package net.utils;

import net.game_server.GameSession;

import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;

import simulator.GameManager;
import simulator.netty.ServerCommand;
import utils.Log;

/**
 Description: Here will be methods that all Socket Server uses such as send packets
 */
public class ServerUtils {

    //TODO
    //    /**
    //     Description: Send a command
    //
    //     @param socket:
    //     the socket that we will use to send the packet
    //     @param sc:
    //     the ServerCommand that we will send
    //     */
    //    public static void sendCommand(Socket socket, ServerCommand sc) {
    //        try {
    //            //byte array to write server command
    //            ByteArrayOutputStream ByteArrayData = new ByteArrayOutputStream();
    //            DataOutputStream osData = new DataOutputStream(ByteArrayData);
    //            //byte array that will be used to write on socket
    //            //this will contain lenght of ByteArrayData and then ByteArrayData
    //            ByteArrayOutputStream ByteArrayOut = new ByteArrayOutputStream();
    //            DataOutputStream osOut = new DataOutputStream(ByteArrayOut);
    //
    //            //write command into osData byte array
    //            sc.write(osData);
    //            //write lenght of command into main byte array
    //            osOut.writeShort(ByteArrayData.size());
    //            //write command into main byte array
    //            ByteArrayData.writeTo(ByteArrayOut);
    //
    //            OutputStream out = socket.getOutputStream();
    //            //write main byte array on socket
    //            byte[] data = ByteArrayOut.toByteArray();
    //            out.write(data, 0, data.length);
    //            out.flush();
    //        } catch (IOException e) {
    //            //socket possible is closed so we dont send the command
    //        }
    //    }

    // TODO
    //  /**
    //     Description: Send a command to all users loaded(in range)
    //
    //     @param pGameServerClientConnection:
    //     the ConnectionManager.loadedUsers
    //     */
    //    public static void sendCommandToRange(GameServerClientConnection pGameServerClientConnection, ServerCommand sc,
    //                                          boolean sendToOwnHero) {
    //        if (sendToOwnHero) {
    //            //send packet to hero
    //            sendCommand(pGameServerClientConnection.getSocket(), sc);
    //        }
    //        //Navigate on ConnectionManager.loadedUsers(Map<Integer, net.game.ConnectionManager>)
    //        for (Entry<Integer, GameServerClientConnection> users : pGameServerClientConnection.loadedUsers.entrySet()) {
    //            //for each user in the dictionary/map send the packet
    //            sendCommand(users.getValue()
    //                             .getSocket(), sc);
    //        }
    //    }

    //TODO

    /**
     Description: Send a command to the a specific map id

     @param mapID:
     the map id to send the packet
     @param sc:
     the ServerCommand that we will send
     */
    public static void sendCommandToAllInMap(int mapID, ServerCommand sc) {
        for (GameSession gameSession : GameManager.getGameSessions()) {
            if (gameSession.getPlayer()
                           .getCurrentSpaceMapId() == mapID) {
                gameSession.getGameServerClientConnection()
                           .sendToSendCommand(sc);
            }
        }
    }

    //TODO
    //    /**
    //     Description: Send a packet trough command(LegacyModule) to specific socket
    //
    //     @param socket:
    //     the socket that we will use to send the packet
    //     @param packet:
    //     the packet that we will send
    //     */
    //    public static void sendPacket(Socket socket, String packet) {
    //        LegacyModule lm = new LegacyModule(packet);
    //        sendCommand(socket, lm);
    //    }

    //  TODO
    //  /**
    //     Description: Send a packet trough command(LegacyModule) to users that are locking hero
    //
    //     @param user:
    //     the ConnectionManager
    //     @param packet:
    //     the packet that we will send
    //     */
    //    public static void sendPacketToUsersLockingHero(GameServerClientConnection user, String packet,
    //                                                    boolean sendToOwnHero) {
    //        LegacyModule lm = new LegacyModule(packet);
    //        if (sendToOwnHero) {
    //            //send packet to hero
    //            sendCommand(user.getSocket(), lm);
    //        }
    //        //Navigate on ConnectionManager.loadedUsers(Map<Integer, net.game.ConnectionManager>)
    //        for (Entry<Integer, GameServerClientConnection> users : user.loadedUsers.entrySet()) {
    //            GameServerClientConnection User = users.getValue();
    //            if (User.mAccount.getSelID() == user.userID) {
    //                //Send the packet
    //                sendCommand(users.getValue()
    //                                 .getSocket(), lm);
    //            }
    //        }
    //    }

    //   TODO
    //    /**
    //     Description: Send a packet trough command(LegacyModule) to users loaded
    //
    //     @param pGameSession
    //     GameSession to use as range center
    //     @param packet:
    //     the packet that we will send
    //     */
    //    public static void sendPacketToRange(GameSession pGameSession, String packet, boolean sendToOwnHero) {
    //        LegacyModule lm = new LegacyModule(packet);
    //        if (sendToOwnHero) {
    //            //send packet to hero
    //            sendCommand(pGameSession.getConnectionSocket(), lm);
    //        }
    //
    //
    //
    //        //Navigate on ConnectionManager.loadedUsers(Map<Integer, net.game.ConnectionManager>)
    //        for (Entry<Integer, GameServerClientConnection> users : user.loadedUsers.entrySet()) {
    //            //for each user in the dictionary/map send the packet
    //            sendCommand(users.getValue()
    //                             .getSocket(), lm);
    //        }
    //    }

    /**
     Description: Send a packet, nothing else

     @param socket:
     the socket that we will use to send the packet
     @param packet:
     the packet that we will send
     */
    public static void sendChatAndUpdaterPackets(final Socket socket, final String packet) {
        try {

            //First get the PrintWriter object from the socket
            PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
            //And then print the packet to the PrintWriter object
            out.print((packet) + (char) 0x00);
            //Flush it and you sent your packet!
            out.flush();

        } catch (IOException e) {
            //We couldn't sent this packet! that's bad...
            Log.pt("Couldn't send packet!");
            Log.pt(e.getMessage());
        }
    }

    /**
     Description: Send a packet to every user in new server

     @param packet:
     the packet to send
     */
    public static void sendPacketToAllUsers(final String packet) {
        for (final GameSession gameSession : GameManager.getGameSessions()) {
            gameSession.getGameServerClientConnection()
                       .sendPacket(packet);
        }
    }

    /**
     Description: Send a packet to every user in the map of new server

     @param pMapID:
     the map to send the packet
     @param pPacket:
     the packet to send
     */
    public static void sendPacketAllUsersOnMap(final int pMapID, final String pPacket) {
        for (final GameSession gameSession : GameManager.getGameSessions()) {
            if (gameSession.getPlayer()
                           .getCurrentSpaceMapId() == pMapID) {
                gameSession.getGameServerClientConnection()
                           .sendPacket(pPacket);
            }
        }
    }


    /**
     Description: Sends the policy

     @param socket:
     the socket that we will use to send the policy
     */
    public static void sendPolicy(Socket socket) {
        String policy = "<?xml version=\"1.0\"?>\r\n" +
                        "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                        "<cross-domain-policy>\r\n" +
                        "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                        "</cross-domain-policy>";
        sendChatAndUpdaterPackets(socket, policy);
    }

    //TODO
    //    /**
    //     Description: Sends an error
    //
    //     @param socket:
    //     the socket that we will use to send the error
    //     @param errorCode:
    //     error code
    //     */
    //    public static void sendError(Socket socket, int errorCode) {
    //        sendPacket(socket, "ERR|" + errorCode);
    //    }

}
