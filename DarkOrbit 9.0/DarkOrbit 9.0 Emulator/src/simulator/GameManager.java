package simulator;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import java.util.Collection;
import java.util.HashMap;

import simulator.netty.ServerCommand;
import simulator.netty.serverCommands.WordPuzzleWindowInitCommand;
import simulator.users.Account;
import simulator.system.clans.Clan;
import simulator.system.SpaceMap;
import utils.Log;

/**
 This class is used to simulate ALL the GAME LOGIC on server. But since implementing all the logic in one class is very
 fucked idea it will distribute game logic to all kind of managers & other classes
 */
public final class GameManager {

    private static GameManager INSTANCE;

    // =============== GameSession data ===============
    // Stores all connected accounts
    // each GameSession object contains Account, Player etc
    private HashMap<Integer, GameSession> mGameSessionsMap = new HashMap<>(); // UID, GameSession
    // ================================================

    // ================ SpaceMaps data ================
    // Stores all maps added to simulated world
    // each SpaceMap object contains a map and a mechanism that simulates aliens etc TODO better explanation
    private HashMap<Short, SpaceMap> mSpaceMaps = new HashMap<>(); // map ID, SpaceMap
    // ================================================

    // ================== Clans data ==================
    // Stores all clans' data of server
    // each Account object contains all account data and managers to deal with that data
    private HashMap<Integer, Clan> mClans = new HashMap<>(); // clan ID, Clan
    // ================================================

    private GameManager() {
        // For singleton usage
    }

    public static GameManager getInstance() {
        if (INSTANCE == null) {
            INSTANCE = new GameManager();
        }
        return INSTANCE;
    }

    /**
     @param pGameSession
     GameSession to add to online users
     */
    public static void addGameSession(final GameSession pGameSession) {

        final Account account = pGameSession.getAccount();
        // add session to sessions map
        INSTANCE.mGameSessionsMap.put(account.getUserId(), pGameSession);

        final GameServerClientConnection gscc = pGameSession.getGameServerClientConnection();
        
        gscc.sendToSendCommand(account.getClientSettingsManager()
                                .getUserSettingsCommand());
        gscc.sendToSendCommand(account.getClientSettingsManager().getClientUIMenuBarsCommand());
        gscc.sendToSendCommand(account.getClientSettingsManager().getClientUISlotBarsCommand());
        gscc.sendToSendCommand(account.getClientSettingsManager()
                                .getUserKeyBindingsUpdateCommand());

        final short mapID = account.getPlayer()
                                   .getCurrentSpaceMapId();
        final SpaceMap map = INSTANCE.mSpaceMaps.get(mapID);
        if (map == null) {
            Log.p("null spacemap");
            return;
        }
        map.addAndInitGameSession(pGameSession);
    }

    /**
     Note: removes game session if it exists in online sessions, otherwise does nothing

     @param pUserID
     UID of GameSession to remove from online users
     */
    public static void removeGameSession(final int pUserID) {
        INSTANCE.mGameSessionsMap.remove(pUserID);
    }

    /**
     @param pUserID
     user ID of GameSession to get

     @return GameSession for given user ID or null if map with this GameSession isn't online
     */
    public static GameSession getGameSession(final int pUserID) {
        return INSTANCE.mGameSessionsMap.get(pUserID);
    }
      
    /**
     @return all GameSessions that are online
     */
    public static Collection<GameSession> getGameSessions() {
        return INSTANCE.mGameSessionsMap.values();
    }


    /**
     @param pSpaceMap
     adds a map to simulated world
     */
    public void addSpaceMap(final SpaceMap pSpaceMap) {
        this.mSpaceMaps.put(pSpaceMap.getSpaceMapId(), pSpaceMap);
        pSpaceMap.startSimulation();
    }

    public Collection<SpaceMap> getAllSpaceMaps() {
        return this.mSpaceMaps.values();
    }

    /**
     @param pSpaceMaps
     Collection of Clan objects to add to simulated world
     */
    public void addSpaceMaps(final Collection<SpaceMap> pSpaceMaps) {
        for (final SpaceMap spaceMap : pSpaceMaps) {
            this.addSpaceMap(spaceMap);
        }
    }

    /**
     @param pMapID
     map ID of SpaceMap to get

     @return SpaceMap with given ID or null if map with this ID isn't added to world
     */
    public static SpaceMap getSpaceMapById(final short pMapID) {
        return INSTANCE.mSpaceMaps.get(pMapID);
    }

    /**
     @param pClan
     Clan to add to simulated world
     */
    public void addClan(final Clan pClan) {
        this.mClans.put(pClan.getClanId(), pClan);
    }

    /**
     @param pClans
     Collection of Clan objects to add to simulated world
     */
    public void addClans(final Collection<Clan> pClans) {
        for (final Clan clan : pClans) {
            this.addClan(clan);
        }
    }

    /**
     @param pClanID
     clan ID of clan to get

     @return Clan with given ID or null if no Clan with this ID was added to world
     */
    public static Clan getClanById(final int pClanID) {
        final Clan clan = getInstance().mClans.get(pClanID);
        Log.p(String.valueOf(clan == null));
        return clan;
    }


    /**
     @param pUserID
     user ID to check

     @return true if user is online (has a GameSession in GameManager)
     */
    public static boolean isUserInGame(final int pUserID) {
        return INSTANCE.mGameSessionsMap.containsKey(pUserID);
    }

    /**
     Broadcasts server disconnect message to all online users

     @param pSeconds
     seconds to set in message
     */
    public void sendServerDisconnectMessage(final int pSeconds) {
    	try{
	        final Collection<GameSession> sessions = this.mGameSessionsMap.values();
	        for (GameSession session : sessions) {
	            session.getGameServerClientConnection()
	                   .sendPacket("0|A|STM|server_restart_n_seconds|" + pSeconds);
	        }
	        Log.pt("Sent disconnect message: " + pSeconds + " seconds left");
    	} catch (Exception e) {
    		//
    	}
    }

    public void broadcastPacket(final String pPacketData) {

        final Collection<GameSession> gameSessions = this.mGameSessionsMap.values();
        for (GameSession session : gameSessions) {
            session.getGameServerClientConnection()
                   .sendPacket(pPacketData);
        }

        Log.pt("Packet broadcast successful ;)");

    }

    public void sendPacketToUID(final int pUserId, final String pPacket) {

        if (!isUserInGame(pUserId)) {

            Log.pt("User with UID = " + pUserId + " is not connected. Packet not sent");

            return;
        }

        GameManager.getGameSession(pUserId)
                   .getGameServerClientConnection()
                   .sendPacket(pPacket);

        Log.pt("Packet sent to UID: " + pUserId);

    }

    public void sendCommandToUID(final int pUserId, final ServerCommand pCommand) {

        if (!isUserInGame(pUserId)) {

            Log.pt("User with UID = " + pUserId + " is not connected. Command not sent");

            return;
        }

        GameManager.getGameSession(pUserId)
                   .getGameServerClientConnection()
                   .sendToSendCommand(pCommand);

        Log.pt("Command sent to UID: " + pUserId);

    }

    public static void closeGameSession(final int pUserID) {

        // TODO conditionally remove from map(time not passed, in battle etc)

        final GameSession session = GameManager.getGameSession(pUserID);

        if (session != null) {

            session.close();

            removeGameSession(pUserID);

            Log.pt("GameSession for UID = " + pUserID + " has been closed & removed from online sessions");

        } else {
            Log.pt("GameSession for UID = " + pUserID + " doesn't exist");
        }

    }
    
    public static  void tryJump(final int pUserID, final short map){
        final GameSession session = GameManager.getGameSession(pUserID);

        if (session != null) {
            session.tryJump(map);

            //Log.pt("GameSession for UID = " + pUserID + " has been closed & removed from online sessions");

        } else {
            Log.pt("GameSession for UID = " + pUserID + " doesn't exist");
        }
    }
    
    public void shutdown() {
        //TODO
    }

}
