package net.chat_server;

import net.AbstractServer;

import java.io.IOException;
import java.net.Socket;
import java.util.ArrayList;

import utils.Log;

public class ChatServer
        extends AbstractServer {

    private static final String CHAT_SERVER_THREAD_NAME = "[ChatServer Thread]";

    private ArrayList<ChatClientConnection> mChatClientConnections = new ArrayList<ChatClientConnection>();

    public void startServer(final int pPort, final ServerStartedListener pListener) {
        this.startServer(pPort, CHAT_SERVER_THREAD_NAME, pListener);
    }

    // Same as GameServer
    @Override
    public void run() {
        super.run();

        while (true) {
            try {
                final Socket socketConnection = this.mServerSocket.accept();
                if (this.checkAccess(socketConnection.getInetAddress()
                                                     .getHostAddress())) {
                    this.mChatClientConnections.add(new ChatClientConnection(socketConnection));
                }
            } catch (IOException e) {
                Log.pt("Connection unresolved. Shutting down : " + e.getMessage());
                // System.exit(0); // should we exit if a connection fails o_O?
            }

        }

    }

}
