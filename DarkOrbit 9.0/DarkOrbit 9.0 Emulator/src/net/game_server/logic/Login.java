package net.game_server.logic;

import net.game_server.GameServerClientConnection;
import net.game_server.GameSession;

import mysql.QueryManager;
import simulator.GameManager;
import simulator.users.Account;
import utils.Log;

/**
 Created by bpdev on 02/02/2015.
 */
public class Login {

    public static GameSession login(final GameServerClientConnection pGameServerClientConnection, final int userID,
                                    final String pSessionID) {
    	QueryManager.loadClans();
        final GameSession previousGameSession = GameManager.getGameSession(userID);
        if (previousGameSession != null) {
            // Game session exists already
      //      Log.p("Bu oyuncu zaten oyunda!");
      //      Log.p("Yeni sessiondan giriş yapıldı!");
            previousGameSession.close();
        }

        final Account account = QueryManager.loadAccount(userID);
        QueryManager.loadEquipments(account);
        
        if (account == null) {
            Log.pt("Bu id ye ait hesap yüklenemedi " + userID);
            return null;
        }
    	
        if (QueryManager.checkGameBanned(account.getUserId())) {
        	Log.pt("Banlı oyuncu girişi iptal edildi: " + account.getShipUsername());
            return null;
        }
        
        // Create session and add to online sessions
        final GameSession session = new GameSession(pGameServerClientConnection, account);
        // => all OK, proceed
   //     Log.pt("Login complete for user with UID = " + userID);
   //     System.out.println("Giriş tamamlandı: Kullanıcı ID = "+ account.getUserId() +" // Kullanıcı adı = "+ account.getUsername() +" // Oyun adı = "+ account.getShipUsername() +" // "
   //     		+ "Rütbe = "+ account.getRankName() +" Klan Tag: "+ account.getClanTag() +" // "
   //     		+ "Premium = "+ account.getPremiumYesOrNo() +" // Gemi adı = "+ account.getPlayer().getPlayerShip().getShipName() +" ");
    //    Log.p(Log.LINE_MINUS); ddos
        account.setOnline(true);
        QueryManager.saveAccount(account);
        return session;
    }

}