package net.policy_server;

import net.AbstractServer;

import java.io.IOException;
import java.net.Socket;

import utils.Log;

public class PolicyServer
        extends AbstractServer {

    private static final int SOCKET_SERVER_PORT = 843;

    private static final String POLICY_SERVER_THREAD_NAME = "[PolicyServer Thread]";

    public void startServer(final ServerStartedListener pListener) {
        this.startServer(SOCKET_SERVER_PORT, POLICY_SERVER_THREAD_NAME, pListener);
    }

    @Override
    public void run() {
        super.run();

        // always try to accept new connections
        while (true) {

            try {

                //We wait for a connection (socket) from server
                final Socket serverSocket = this.mServerSocket.accept();
                if (this.checkAccess(serverSocket.getInetAddress()
                                                 .getHostAddress())) {
                    // Create ClientConnection
                    new PolicyClientConnection(serverSocket);
                }

            } catch (IOException e) {
                Log.pt("Connection unresolved. Shutting down : " + e.getMessage());
                System.exit(0);
            }

        }

    }
}