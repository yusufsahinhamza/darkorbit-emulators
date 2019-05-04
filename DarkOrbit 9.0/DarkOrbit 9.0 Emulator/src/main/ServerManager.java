package main;

import net.AbstractServer;
import net.chat_server.ChatServer;
import net.game_server.CommandHandler;
import net.game_server.CommandLookup;
import net.game_server.GameServer;
import net.game_server.GameSession;
import net.policy_server.PolicyServer;
import net.socket_server.SocketServer;
import mysql.MySQLManager;
import mysql.QueryManager;
import simulator.GameManager;
import simulator.map_entities.movable.Player;
import simulator.netty.ServerCommand;
import simulator.users.Account;
import storage.ClanStorage;
import storage.SpaceMapStorage;
import utils.Log;

public class ServerManager
        implements AbstractServer.ServerStartedListener {

    // Servers
    private final GameServer   mGameServer;
    private final PolicyServer mPolicyServer;
    private final ChatServer   mChatServer;
    private final SocketServer mSocketServer;
    private final GameManager  mGameManager;

    // Properties manager
    private final PropertiesManager mPropertiesManager;

    // Server started booleans
    private boolean mGameServerStarted;
    private boolean mPolicyServerStarted;
    private boolean mChatServerStarted;
    private boolean mSocketServerStarted;
    private boolean mAllServersStarted;

    // ==============================================================

    public ServerManager(final PropertiesManager pPropertiesManager) {

        this.mPropertiesManager = pPropertiesManager;

        this.mGameServer = new GameServer();
        this.mPolicyServer = new PolicyServer();
        this.mChatServer = new ChatServer();
        this.mSocketServer = new SocketServer();

        this.mGameManager = GameManager.getInstance();

    }

    public void begin() {
        Log.p(Log.LINE_MINUS);

        this.connectToDB();

        Log.p(Log.LINE_MINUS);

        this.loadDataFromDB();

        Log.p(Log.LINE_MINUS);

        this.startSimulation();

        Log.p(Log.LINE_MINUS);

        this.startServers();
        
    }

    private void connectToDB() {
        final MySQLManager mySQLManager =
                new MySQLManager(this.mPropertiesManager.getDatabaseHost(), this.mPropertiesManager.getDatabasePort(),
                                 this.mPropertiesManager.getDatabaseUsername(),
                                 this.mPropertiesManager.getDatabasePassword(),
                                 this.mPropertiesManager.getDatabaseName());
        QueryManager.setConnection(mySQLManager.getConnection());
    }


    // loads data from db
    private void loadDataFromDB() {

        Log.p("Loading data from the database...");


        //        QueryManager.loadShop(connection);
        //        QueryManager.loadClanBattleStations(connection);// STRICTLY before maps atm
        QueryManager.loadClans();
        QueryManager.loadShips();
        QueryManager.loadMaps();
    }

    // starts world simulation
    private void startSimulation() {

        Log.p("Creating world...");

        // 1. add space maps
        this.mGameManager.addSpaceMaps(SpaceMapStorage.getSpaceMapCollection());
        // 2. add clans
        this.mGameManager.addClans(ClanStorage.getClanCollection());
        // 3. init commands lookup
        CommandLookup.initLookup();
        // 4. init commands handler
        CommandHandler.initHandler();
        Log.p("World created and simulation started");

    }

    // start all servers
    private void startServers() {

        Log.p("Starting servers...");

        this.mGameServer.startServer(this.mPropertiesManager.getGameServerPort(), this);
        this.mPolicyServer.startServer(this);
        this.mSocketServer.startServer(this.mPropertiesManager.getSocketServerPort(), this);
        this.mChatServer.startServer(this.mPropertiesManager.getChatServerPort(), this);
        
    }

    // ==============================================================

    public void shutdown(final int pSeconds) {
        for (int i = pSeconds; i > 0; i--) {
            try {

                this.mGameManager.sendServerDisconnectMessage(i);
                Thread.sleep(1000);   
                
            } catch (InterruptedException e) {
                Log.p("Server shutdown interrupted", e.getMessage());
            }
        }
        this.mGameManager.shutdown();
        
        //Bilgileri kaydet
        this.forceUpdateDatabase();  
        
        Log.p("<======================================","<Shutdown succeeded>","======================================>");
        System.exit(0);

    }
        
    public void forceUpdateDatabase() {
    	try {
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
    	} catch (Exception e){
            Log.pt("Kullanıcılar kaydedilemedi, kapatılıyor...");
            Log.pt(e.getMessage());
            System.exit(0);
        }
    }

	// ==============================================================

    // Send a packet to a single user if he's online
    public void sendPacketToUID(final int pUserId, final String pPacket) {
        this.mGameManager.sendPacketToUID(pUserId, pPacket);
    }

    // Send a command to a single user if he's online
    public void sendCommandToUID(final int pUserId, final ServerCommand pCommand) {
        this.mGameManager.sendCommandToUID(pUserId, pCommand);
    }

    // Send a packet to every user online
    public void broadcastPacket(final String pPacketData) {
        this.mGameManager.broadcastPacket(pPacketData);
    }

    // ==============================================================

    @Override
    public void onServerStarted(final AbstractServer pServer) {

        if (pServer instanceof GameServer) {
            this.mGameServerStarted = true;
        }
        if (pServer instanceof PolicyServer) {
            this.mPolicyServerStarted = true;
        }
        if (pServer instanceof ChatServer) {
            this.mChatServerStarted = true;
        }
        if (pServer instanceof SocketServer) {
            this.mSocketServerStarted = true;
        }

        if (this.mPolicyServerStarted && this.mChatServerStarted && this.mSocketServerStarted &&
            this.mGameServerStarted) {
            this.mAllServersStarted = true;
        }

        if (this.mAllServersStarted) {
            // TODO block all servers' connections until this block is called
        	Log.p("All servers started. Waiting for users connections...");
        	Log.p(Log.LINE_MINUS);
        }
    }

}
