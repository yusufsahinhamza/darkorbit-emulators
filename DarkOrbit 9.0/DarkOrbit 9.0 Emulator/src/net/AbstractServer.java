package net;

import java.io.IOException;
import java.net.ServerSocket;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import utils.Log;

public abstract class AbstractServer
        implements Runnable {

    // ServerSocket will be listening to connections
     protected ServerSocket mServerSocket;

    // Server will be running in its own thread
     protected Thread mServerThread;

    // Server port
     protected int mServerPort;

    // Server started listener
     protected ServerStartedListener mServerStartedListener;

    // To start server: create 'startServer' method that calls one of 'startServerInternal' methods
    public void startServer(final int pPort, final String pThreadName, final ServerStartedListener pListener) {

        this.mServerStartedListener = pListener;

        try {
        	if (this.getSocket() == null) {
	            this.mServerPort = pPort;
	
	            this.mServerSocket = new ServerSocket(pPort);
	
	            this.mServerThread = new Thread(this);
	            if (pThreadName != null) {
	                this.mServerThread.setName(pThreadName);
	            }
	            this.mServerThread.start();
        	}
        } catch (IOException e) {
            Log.pt("Couldn't listen on port " + pPort);
            Log.pt(e.getMessage());
        }

    }

    public ServerSocket getSocket() {
    	return this.mServerSocket;
    }
    
    public int getServerPort() {
        return this.mServerPort;
    }

    @Override
    public void run() {

        Log.pt("Listening on port " + this.getServerPort());

        this.mServerStartedListener.onServerStarted(this);

    }

    private ConcurrentHashMap<String, Long> mConnectedIps = new ConcurrentHashMap<>();
    private List<String>                    mBlockedIps   = Collections.synchronizedList(new ArrayList<String>());

    private static final int ALLOWED_TIME_BETWEEN_CONNECTIONS = 1000;

    public boolean checkAccess(final String pIP) {
        if (!pIP.equalsIgnoreCase("127.0.0.1")) {

            final Object lastConnectedTimeObject = mConnectedIps.get(pIP);
            final long currentTime = System.currentTimeMillis();
       //     Log.p("received IP = " + pIP + " on " + currentTime);
            mConnectedIps.put(pIP, currentTime);
            if (lastConnectedTimeObject != null) {
                final long lastConnectedTime = (long) lastConnectedTimeObject;
                if ((currentTime - lastConnectedTime) < ALLOWED_TIME_BETWEEN_CONNECTIONS) {
               //     Log.p(pIP + "Is making DDOS!");
                    //this IP is making ddos!!
                    if (mBlockedIps.contains(pIP)) {
                        return false;
                    } else {
                        mBlockedIps.add(pIP);
                    }
                }
            }
        }
        return true;
    }

    public interface ServerStartedListener {

        public void onServerStarted(final AbstractServer pServer);
    }

}
