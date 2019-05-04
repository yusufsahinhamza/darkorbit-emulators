package net.game_server;

import net.AbstractServer;

import java.io.IOException;
import java.net.Socket;

import utils.Log;

public class GameServer
        extends AbstractServer {

    public static final String GAME_SERVER_THREAD_NAME = "[GameServer Thread]";

    public void startServer(final int pPort, final ServerStartedListener pListener) {
        super.startServer(pPort, GAME_SERVER_THREAD_NAME, pListener);
    }

    @Override
    public void run() {
        super.run();

        // always try to accept new connections
        while (true) {
            try {
                //We wait for a connection (socket) from server
                final Socket serverSocket = this.mServerSocket.accept();
                new GameServerClientConnection(serverSocket);
            } catch (IOException e) {
                Log.pt("Connection unresolved. Shutting down : " + e.getMessage());
                // System.exit(0); // should we exit if a connection fails o_O?
            }

        }

    }

}
