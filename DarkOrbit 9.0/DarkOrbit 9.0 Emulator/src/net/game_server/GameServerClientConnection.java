package net.game_server;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.EOFException;
import java.io.IOException;
import java.io.OutputStream;
import java.net.Socket;

import simulator.GameManager;
import simulator.netty.ClientCommand;
import simulator.netty.ServerCommand;
import simulator.netty.handlers.ICommandHandler;
import simulator.netty.serverCommands.LegacyModule;
import utils.Log;


/**
 Description: This is the thread that will handle each user in the game

 @author Manulaiko & Ordepsousa
 @date 15/02/2014
 @file ConnectionManager.java
 @package net.game.newClient
 @project SpaceBattles */
public class GameServerClientConnection
        implements Runnable {

    //private final ClientPacketProcessor mClientPacketProcessor;

    private Socket          mSocket;
    private       DataInputStream mSocketIncomingDataInputStream;

    private Thread mGameClientConnectionThread;

    private GameSession mGameSession = null;

    /**
     @param pSocket:
     Socket of TCP connection with game client
     */
    public GameServerClientConnection(final Socket pSocket) {

        Log.pt("GameServer ClientConnection created");

        this.mSocket = pSocket;

        try {

            // Get DataInputStream for incoming packets
            this.mSocketIncomingDataInputStream = new DataInputStream(pSocket.getInputStream());

            // Creating a thread for this GameClientConnection
            this.mGameClientConnectionThread = new Thread(this);
            this.mGameClientConnectionThread.start();


        } catch (IOException e) {

            // e.printStackTrace();

            Log.pt("GameServer ClientConnection could not get DataInputStream from given Socket");
            
            if(this.getGameSession() != null && this.getGameSession().getAccount() != null){ 
                this.getGameSession().close();
            }

        }

    }

    /**
     Description: This method will read the netty packet and assemble the packets
     */
    public void run() {

        try {

            while (true) {

                // create packet & redirect to ClientPacketProcessor
                int length = this.mSocketIncomingDataInputStream.readShort();
                if (length > 0) {

                    byte[] byteArray = new byte[length];
                    this.mSocketIncomingDataInputStream.read(byteArray, 0, length);

                    ByteArrayInputStream bais = new ByteArrayInputStream(byteArray, 0, byteArray.length);
                    DataInputStream packet = new DataInputStream(bais);

                    this.processCommand(packet);

                    //                    int id = packet.readShort();
                    //                    this.mClientPacketProcessor.processCommand(id, packet);
                }

            }

        } catch (EOFException e) {

            Log.pt("EOF reached when trying to read DataInputStream length");

            // something wrong at client side
            if(this.getGameSession() != null && this.getGameSession().getAccount() != null){
                this.getGameSession().close();
            }

        } catch (IOException e) {

            Log.pt("IOException when trying to read DataInputStream length");

            // something wrong at client side
            if(this.getGameSession() != null && this.getGameSession().getAccount() != null){
                this.getGameSession().close();
            }

        }

    }

    private void processCommand(final DataInputStream pDataInputStream) {
    	
    		if(!this.getSocket().isClosed()) {
    			if(this.getSocket().isConnected()) {
    				if(this.getSocket().isBound()) {
    					if(this.getSocket() != null) {
        final ClientCommand clientCommand = CommandLookup.getCommand(pDataInputStream);
        if (clientCommand != null) {
        	try{
	            Log.pt("Received new command with ID = " + clientCommand.getID());
	            final ICommandHandler commandHandler = CommandHandler.getCommandHandler(this, clientCommand);
	            if (commandHandler != null) {
	                Log.pt("Executing handler with ID = " + clientCommand.getID());
	                commandHandler.execute();
	            }
        	} catch (Exception e) {
        		//
        	}
        }
    					}
    				}
    			}
    		}
    	
    }

    public void safeClose() {
        this.close();
    }

    public void close() {
            try {
                this.getSocket().close();          	   
                System.out.println("Soket kapatıldı!");
                this.getGameSession().close();
            } catch (IOException ex) {
            	System.out.println("Soket kapatılamadı!");
            }
    }

    public Socket getSocket() {
        return this.mSocket;
    }

    // synchronized for thread safety, null-ok
    public void sendCommand(final ServerCommand pServerCommand) {
        try {
    	if(pServerCommand != null) {
    		if(!this.getSocket().isClosed()) {
    			if(this.getSocket().isConnected()) {
    				if(this.getSocket().isBound()) {
    					if(this.getSocket() != null) {
    						if(this.mGameClientConnectionThread != null) {
    							if(this.mGameClientConnectionThread.getName() != "") {
    	    				            //byte array to write server command
	    				            ByteArrayOutputStream baosData = new ByteArrayOutputStream();
	    				            DataOutputStream osData = new DataOutputStream(baosData);
	    				            //byte array that will be used to write on socket
	    				            //this will contain length of ByteArrayData and then ByteArrayData
	    				            ByteArrayOutputStream baosOut = new ByteArrayOutputStream();
	    				            DataOutputStream osOut = new DataOutputStream(baosOut);

	    				            //write command into osData byte array
	    				            pServerCommand.write(osData);
	    				            //write length of command into main byte array
	    				            osOut.writeShort(baosData.size());
	    				            //write command into main byte array
	    				            baosData.writeTo(baosOut);

	    				            OutputStream out = this.getSocket()
	    				                                   .getOutputStream();
	    				            //write main byte array on socket
	    				            byte[] data = baosOut.toByteArray();
	    				            out.write(data, 0, data.length);
	    				            out.flush();
    							}
    						}
    					}
    				}
    			}
    		}
    	}
        } catch (IOException e) {
           // System.out.println("Exception when sending command: " + e.getMessage());
        }
    }

    public void sendToSendCommand(final ServerCommand pServerCommand) {
        try {
	    	if(pServerCommand != null) {
	    		if(!this.getSocket().isClosed()) {
	    			if(this.getSocket().isConnected()) {
	    				if(this.getSocket().isBound()) {
	    					if(this.getSocket() != null) {
	    						if(this.mGameClientConnectionThread != null) {
	    							if(this.mGameClientConnectionThread.getName() != "") {
	    								this.sendCommand(pServerCommand);
	    							}
	    						}
	    					}
	    				}
	    			}
	    		}
	    	}
    	} catch (Exception e) {
    		System.out.println(e);
    	}
    }
    
    public void sendPacket(String packet) {
        final LegacyModule legacyModule = new LegacyModule(packet);
        this.sendToSendCommand(legacyModule);
    }

    public GameSession getGameSession() {
        return mGameSession;
    }

    public void setGameSession(final GameSession pGameSession) {
        this.mGameClientConnectionThread.setName("[GS:CC] UID:" + pGameSession.getAccount()
                                                                              .getUserId());
        mGameSession = pGameSession;
    }
}
