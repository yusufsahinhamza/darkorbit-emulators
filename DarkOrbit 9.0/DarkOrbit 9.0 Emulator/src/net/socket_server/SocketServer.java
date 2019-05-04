package net.socket_server;

import net.AbstractServer;
import java.net.Socket;
import utils.Log;

public class SocketServer
        extends AbstractServer {

    private static final String SOCKET_SERVER_THREAD_NAME = "[SocketServer Thread]";

    public void startServer(final int pPort, final ServerStartedListener pListener) {
        this.startServer(pPort, SOCKET_SERVER_THREAD_NAME, pListener);
    }

    public void run() {
        super.run();
        while (true) {
            try {
                final Socket socketConnection = this.getSocket().accept();
                new SocketServerClientConnection(socketConnection);
            } catch (Exception e) {
                Log.pt("[SocketServer](start()) ERROR!: " + e.getMessage());
                e.printStackTrace();
            }
        }
    }
}
