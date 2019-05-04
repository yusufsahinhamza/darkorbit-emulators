package mysql;

import org.json.JSONObject;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Collection;

import simulator.map_entities.movable.Player;
import simulator.map_entities.stationary.stations.BattleStation;
import simulator.system.SpaceMap;
import simulator.system.clans.Clan;
import simulator.system.clans.Diplomacy;
import simulator.system.ships.StorageShip;
import simulator.users.Account;
import simulator.users.Drone;
import storage.ClanStorage;
import storage.ShipStorage;
import storage.SpaceMapStorage;
import utils.Log;

/**
 Açıklama: Oyunculara ve oyuna ait tüm bilgileri yükleme.
 */
public class QueryManager {

    private static Connection mConnection;

    /**
     Description: Loads maps
     */
    public static void loadMaps() {

        final String query =
                "SELECT m.*, (SELECT GROUP_CONCAT(c.cbsID) FROM clans_Battlestations c WHERE c.mapID=m.mapID) AS cbsIDs FROM maps m;";
        try {

            final ResultSet result = MySQLManager.query(mConnection, query);

            int mapsLoaded = 0;
            while (result.next()) {
                final short spaceMapID = result.getShort("mapID");
                final short factionID = result.getShort("factionID");
                final String name = result.getString("name");
                final boolean starterMap = result.getBoolean("isStartedMap");
                final boolean pvpMap = result.getBoolean("isPVP");
                final String portalsJSON = result.getString("portals");
                final String stationsJSON = result.getString("stations");
                final String aliensJSON = result.getString("npcs");
                final String collectablesJSON = result.getString("collectables");
                final String bonusBoxesJSON = result.getString("bonusboxes");
                final String galaxyGatesJSON = result.getString("galaxyGates");
                final String cbsIDsString = result.getString("cbsIDs");
                String[] cbss = new String[]{};
                try {
                    cbss = cbsIDsString.split(",");
                } catch (NullPointerException e) {
                    //no battleStations on this map}
                }
                short[] cbsIDs = new short[cbss.length];
                for (short i = 0; i < cbss.length; i++) {
                    cbsIDs[i] = Short.valueOf(cbss[i]);
                }
                final SpaceMap map =
                        new SpaceMap(spaceMapID, factionID, name, starterMap, pvpMap, portalsJSON, stationsJSON, cbsIDs,
                                     aliensJSON, collectablesJSON, bonusBoxesJSON, galaxyGatesJSON);
                SpaceMapStorage.addSpaceMap(map);
                mapsLoaded++;
            }
            Log.pt("Maps loaded: " + mapsLoaded);
        } catch (SQLException e) {
            Log.pt("Couldn't load maps!");
            Log.pt(e.getMessage());
            System.exit(0);
        }
    }

    /**
    Description: Loads ships
    */   
    public static void loadShips() {
        final String query = "SELECT * FROM server.ships";
        try {
            final ResultSet result = MySQLManager.query(getConnection(), query);

            int shipsLoaded = 0;
            while (result.next()) {

                final int pShipId = result.getInt("shipID");
                final String pShipName = result.getString("name");
                final int pBaseHP = result.getInt("health");
                final int pBaseSpeed = result.getInt("speed");
                final JSONObject pRewardJSON = new JSONObject(result.getString("reward"));

                final int pBaseShieldPoints = result.getInt("shield");
                final int pBaseDamage = result.getInt("damage");
                final int pBaseShieldAbsorption = result.getInt("shdAbs");
                final boolean pAggressive = result.getBoolean("ia");

                final String pShipLootId = result.getString("lootID");
                final int pBaseCargoCapacity = result.getInt("cargo");
                final int pBaseLasersCount = result.getInt("lasers");
                final int pBaseHeavyGunsCount = result.getInt("heavy_guns");
                final int pBaseGeneratorsCount = result.getInt("generators");
                final int pBaseExtrasCount = result.getInt("extras");
                final int pShopPrice = result.getInt("costs");
                final boolean pElite = result.getBoolean("elite");
                final StorageShip ship =
                        new StorageShip(pShipId, pShipName, pBaseHP, pBaseSpeed, pRewardJSON, pBaseShieldPoints,
                                        pBaseDamage, pBaseShieldAbsorption, pAggressive, pShipLootId,
                                        pBaseCargoCapacity, pBaseLasersCount, pBaseHeavyGunsCount, pBaseGeneratorsCount,
                                        pBaseExtrasCount, pShopPrice, pElite);
                ShipStorage.addStorageShip(ship);

                shipsLoaded++;
            }

            Log.pt("Ships loaded: " + shipsLoaded);
        } catch (SQLException e) {
            Log.pt("Couldn't load ships!");
            Log.pt(e.getMessage());
            System.exit(0);
        }

    }
    
    /**
    Açıklama: hesapları yükleme
    */
    public static Account loadAccount(final int pUserID) {
        try {
            final PreparedStatement statement =
                    getConnection().prepareStatement("SELECT * FROM players_accounts WHERE userID = ? LIMIT 1");
            statement.setInt(1, pUserID);
            final ResultSet resultSet = statement.executeQuery();
                      
            if (resultSet.next()) {
                        return new Account(
                        		resultSet.getInt("userID"), resultSet.getInt("globalID"), 
                        		resultSet.getInt("serverID"), resultSet.getShort("factionID"), 
                        		resultSet.getInt("shipID"), resultSet.getInt("clanID"), 
                        		resultSet.getShort("rankID"), resultSet.getBoolean("is_admin"),
                        		resultSet.getBoolean("is_cm"), resultSet.getBoolean("havePet"), 
                        		resultSet.getString("sessionID"), resultSet.getString("username"), 
                        		resultSet.getString("ShipUsername"), resultSet.getString("title"),
                        		resultSet.getShort("mapID"), resultSet.getInt("positionX"), 
                        		resultSet.getInt("positionY"), resultSet.getLong("experience"), 
                        		resultSet.getLong("honor"), resultSet.getLong("credits"), 
                        		resultSet.getLong("uridium"), resultSet.getLong("killedGoliath"), 
                        		resultSet.getLong("killedVengeance"), resultSet.getLong("OwnKilled"), 
                        		resultSet.getDouble("jackpot"), resultSet.getShort("level"), 
                        		resultSet.getBoolean("cloaked"), resultSet.getBoolean("premium"), 
                        		resultSet.getInt("repairUp"), resultSet.getInt("rocketDmgUp"), 
                        		resultSet.getString("avatar"), resultSet.getString("settings"),
                        		resultSet.getString("resources"), resultSet.getString("lastSelectedLaser"), 
                        		resultSet.getString("lastSelectedRocket"), resultSet.getBoolean("online"),
                        		resultSet.getString("petName"), resultSet.getInt("rankPoints"),
                        		resultSet.getInt("ioil"), resultSet.getInt("eco_10"), resultSet.getInt("sar_02"),
                        		resultSet.getString("puzzle_letters")
                        		 );
            }
        } catch (SQLException e) {
            Log.pt("Couldn't load account " + pUserID);
            Log.pt(e.getMessage());
        }
        return null;
    }

    /**
    Açıklama: Ekipmanları yükleme
    */
    public static void loadEquipments(Account account) {
        try {
            //Ekipman Configürasyon 1
            final PreparedStatement statementConfiguration1 =
                    getConnection().prepareStatement("SELECT * FROM players_config WHERE config_id = 1 AND player_id = ? LIMIT 1");         
            statementConfiguration1.setInt(1, account.getUserId());
            final ResultSet resultConfiguration1 = statementConfiguration1.executeQuery();
            //
            
            //Ekipman Configürasyon 2
            final PreparedStatement statementConfiguration2 =
            		getConnection().prepareStatement("SELECT * FROM players_config WHERE config_id = 2 AND player_id = ? LIMIT 1");
            statementConfiguration2.setInt(1, account.getUserId());
            final ResultSet resultConfiguration2 = statementConfiguration2.executeQuery();
            //
            
            if (resultConfiguration1.next()) {
            	account.getEquipmentManager().setConfig1(resultConfiguration1.getInt("max_damage"), resultConfiguration1.getInt("max_shield"),
            			resultConfiguration1.getInt("ship_speed"));
            }
            if (resultConfiguration2.next()) {
            	account.getEquipmentManager().setConfig2(resultConfiguration2.getInt("max_damage"), resultConfiguration2.getInt("max_shield"),
            			resultConfiguration2.getInt("ship_speed"));
            }       	
        } catch (SQLException e) {
            Log.pt("Couldn't load account " + account.getUserId());
            Log.pt(e.getMessage());
        }
    }
    
    /**
    Açıklama: Gemi kontrol etme
    */
    public static boolean checkShip(final int pUserID, final String shipName) {
        try {
            String sql = "SELECT * FROM server.players_accounts WHERE userID = ?";
           	
            PreparedStatement check = getConnection().prepareStatement(sql);

            check.setInt(1, pUserID);
           
            final ResultSet result = check.executeQuery();
           
            while (result.next()) {
            	final boolean surgeon = result.getBoolean("surgeon");
            	final boolean surgeonCicada = result.getBoolean("surgeonCicada");
            	final boolean surgeonLocust = result.getBoolean("surgeonLocust");
            	final boolean cyborg = result.getBoolean("cyborg");

            	if(shipName == "surgeon") {
            		return surgeon;
            	} else if(shipName == "surgeonCicada") {
            		return surgeonCicada;
            	} else if(shipName == "surgeonLocust") {
            		return surgeonLocust;
            	} else if(shipName == "cyborg") {
            		return cyborg;
            	}
            }  	
        } catch (SQLException e) {
            Log.pt("Couldn't check ship " + pUserID);
            Log.pt(e.getMessage());
        }
		return false;
    }
    
    /**
    Açıklama: hesapları kaydetme
    */    
       public static void saveAccount(Account account) {
            try {
            	
            	Player player = account.getPlayer();
            	final long uridium = account.getUridium();
            	final long honor = account.getHonor();
            	final long experience = account.getExperience();
            	final long credits = account.getCredits();
            	final long killedGoliath = account.getKilledGoliath();
            	final long killedVengeance = account.getKilledVengeance();
            	final long ownKilled = account.getOwnKilled();
            	final double jackpot = account.getJackpot();
            	final short level = account.getLevel();
            	final boolean cloaked = account.isCloaked();
            	final boolean premium = account.isPremiumAccount();
            	final String settings = account.getClientSettingsManager().packToJSON();
            	final boolean inEquipZone = account.getPlayer().isInEquipZone();
            	final String lastSelectedLaser = account.getClientSettingsManager().getSelectedLaserItem();
            	final String lastSelectedRocket = account.getClientSettingsManager().getSelectedRocketItem();
            	final String resources = account.getResourcesManager().packToJSON();
            	final String letters = account.lettersToJSON();
            	
               	String sql = "UPDATE players_accounts SET userID = ?,  "
                        + "title = ?, positionX = ?, positionY = ?, "
                        + "experience = ?, honor = ?, credits = ?, uridium = ?, killedGoliath = ?, "
                        + "killedVengeance = ?, OwnKilled = ?, jackpot = ?, "
                        + "level = ?, cloaked = ?, premium = ?, settings = ?, "
                        + "inEquipZone = ?, lastSelectedLaser = ?, lastSelectedRocket = ?, "
                        + "resources = ?, online = ?, rankPoints = ?, factionID = ?, ioil = ?,"
                        + "eco_10 = ?, sar_02 = ?, puzzle_letters = ? WHERE userID = ?";
               	
                PreparedStatement update = getConnection().prepareStatement(sql);
    
               update.setInt(1, account.getUserId());
               update.setString(2, account.getTitle());
               update.setInt(3, player.getCurrentPositionX());
               update.setInt(4, player.getCurrentPositionY());
               update.setLong(5, experience);
               update.setLong(6, honor); 
               update.setLong(7, credits);
               update.setLong(8, uridium);
               update.setLong(9, killedGoliath);
               update.setLong(10, killedVengeance);
               update.setLong(11, ownKilled);
               update.setDouble(12, jackpot);
               update.setShort(13, level);
               update.setBoolean(14, cloaked);
               update.setBoolean(15, premium);
               update.setString(16, settings);
               update.setBoolean(17, inEquipZone);
               update.setString(18, lastSelectedLaser);
               update.setString(19, lastSelectedRocket);
               update.setString(20, resources);
               update.setBoolean(21, account.getOnline());
               update.setInt(22, account.getRankPoints());
               update.setInt(23, account.getFactionId());
               update.setInt(24, account.getIOil());
               update.setInt(25, account.getECO_10());
               update.setInt(26, account.getSAR_02());
               update.setString(27, letters);
               
               update.setInt(28, account.getUserId());
               
               update.execute();
               
           } catch (SQLException e) {
                Log.pt("Couldn't save account " + account.getUserId());
                Log.pt(e.getMessage());
            }
        }
       
       /**
       Açıklama: cephane kaydetme
       */    
          public static void saveAmmo(Account account) {
               try {
               	
                String sql = "UPDATE players_accounts SET eco_10 = ?, sar_02 = ? WHERE userID = ?";
                  	
                PreparedStatement update = getConnection().prepareStatement(sql);
       
                update.setInt(1, account.getECO_10());
                update.setInt(2, account.getSAR_02());
                  
                update.setInt(3, account.getUserId());                  
                update.execute();
                  
              } catch (SQLException e) {
                   Log.pt("Couldn't save ammo " + account.getUserId());
                   Log.pt(e.getMessage());
               }
           }
          
          /**
          Açıklama: gemi kaydetme
          */    
             public static void saveShip(Account account) {
                  try {
                  	
                   String sql = "UPDATE players_accounts SET shipID = ? WHERE userID = ?";
                     	
                   PreparedStatement update = getConnection().prepareStatement(sql);
          
                   update.setInt(1, account.getPlayer().getPlayerShip().getShipId());
                     
                   update.setInt(2, account.getUserId());                  
                   update.execute();
                     
                 } catch (SQLException e) {
                      Log.pt("Couldn't save ship " + account.getUserId());
                      Log.pt(e.getMessage());
                  }
              }
             
       /**
       Açıklama: Havoc ve hercules ekleme
       */    
          public static void addDroneDesign(Account account, String droneDesign) {
               try {
            	   
            	   String sql = "";
            	   if(droneDesign == "drone_designs_havoc") {
	                 sql = "UPDATE players_drones SET Config1DroneDesign = ? WHERE userID = ? LIMIT 1";
            	   } else if(droneDesign == "drone_designs_hercules") {
  	                 sql = "UPDATE players_drones SET Config2DroneDesign = ? WHERE userID = ? LIMIT 1";
            	   }           	   
                 	
	                 PreparedStatement update = getConnection().prepareStatement(sql);
	       
	                 update.setString(1, droneDesign);                  
	                 update.setInt(2, account.getUserId());
	                 
	                 update.execute();
              } catch (SQLException e) {
                   Log.pt("Couldn't save account " + account.getUserId());
                   Log.pt(e.getMessage());
               }
           }
          
       /**
       Açıklama: öldürülenleri kaydetme
       */    
          public static void saveKiller(final String tarih, final String saat, final Player kesilen, final Player kesen) {
               try {    
            	   
            	final Player kesilenOyuncu = kesilen;
            	final Player kesenOyuncu = kesen;
            	   
            	final String Tarih = tarih;
            	final String Saat = saat;
               	final int kesilenID = kesilenOyuncu.getAccount().getUserId();
               	final String kesilenKullaniciAdi = kesilenOyuncu.getAccount().getUsername();
               	final String kesilenNick = kesilenOyuncu.getAccount().getShipUsername();
               	final int kesenID = kesenOyuncu.getAccount().getUserId();
               	final String kesenKullaniciAdi = kesenOyuncu.getAccount().getUsername();
               	final String kesenNick = kesenOyuncu.getAccount().getShipUsername();
               	
                   String sql = "INSERT INTO server_killed_players (tarih,saat,kesilenID,kesilenKullaniciAdi,kesilenNick,kesenID,kesenKullaniciAdi,kesenNick) VALUES(?,?,?,?,?,?,?,?)";
                  	
                   PreparedStatement update = getConnection().prepareStatement(sql);
       
                  update.setString(1, Tarih);
                  update.setString(2, Saat);
                  update.setInt(3, kesilenID);
                  update.setString(4, kesilenKullaniciAdi);
                  update.setString(5, kesilenNick);
                  update.setInt(6, kesenID);
                  update.setString(7, kesenKullaniciAdi);
                  update.setString(8, kesenNick);
                  
                  update.execute();
                  
              } catch (SQLException e) {
                   Log.pt("Öldürme kayıt edilemedi!");
                   Log.pt(e.getMessage());
               }
           }
          
          /**
          Açıklama: banlama kaydetme
          */    
             public static void saveBanned(final String tarih, final Player banlanan, final Player banlayan, final String sebep, final boolean oyunBani) {
                  try {    
               	   
               	final Player banlananOyuncu = banlanan;
               	final Player banlayanOyuncu = banlayan;
               	   
               		final String Tarih = tarih;
                  	final int banlananID = banlananOyuncu.getAccount().getUserId();
                  	final String banlananKullaniciAdi = banlananOyuncu.getAccount().getUsername();
                  	final String banlananNick = banlananOyuncu.getAccount().getShipUsername();
                  	final int banlayanID = banlayanOyuncu.getAccount().getUserId();
                  	final String banlayanKullaniciAdi = banlayanOyuncu.getAccount().getUsername();
                  	final String banlayanNick = banlayanOyuncu.getAccount().getShipUsername();
                  	final String Sebep = sebep;
                  	
                      String sql = "INSERT INTO server_banned_players (banlamaTarihi,banlananID,banlananKullaniciAdi,banlananNick,banlayanID,banlayanKullaniciAdi,banlayanNick,banlamaSebebi,banTürü) VALUES(?,?,?,?,?,?,?,?,?)";
                     	
                      PreparedStatement update = getConnection().prepareStatement(sql);
          
                     update.setString(1, Tarih);
                     update.setInt(2, banlananID);
                     update.setString(3, banlananKullaniciAdi);
                     update.setString(4, banlananNick);
                     update.setInt(5, banlayanID);
                     update.setString(6, banlayanKullaniciAdi);
                     update.setString(7, banlayanNick);
                     update.setString(8, Sebep);
                     update.setString(9, oyunBani == true ? "oyunBanı" : "chatBanı");
                     
                     update.execute();
                     
                 } catch (SQLException e) {
                      Log.pt("Banlama kayıt edilemedi!");
                      Log.pt(e.getMessage());
                  }
              }

             /**
             Açıklama: chat banı kontrol etme
             * @return 
             */    
                public static boolean checkChatBanned(final int userID) {
                     try {    
                         String sql = "SELECT * FROM server.server_banned_players WHERE banlananID = ? AND banTürü = 'chatBanı'";
                        	
                         PreparedStatement check = getConnection().prepareStatement(sql);
             
                         check.setInt(1, userID);
                        
                         final ResultSet result = check.executeQuery();
                        
                         while (result.next()) {
                        	 return true;
                         }
                    } catch (SQLException e) {
                         Log.pt("Ban kontrol edilemedi!");
                         Log.pt(e.getMessage());
                     }
					return false;
                 }

                /**
                Açıklama: chat banı kontrol etme
                * @return 
                */    
                   public static boolean checkGameBanned(final int userID) {
                        try {    
                            String sql = "SELECT * FROM server.server_banned_players WHERE banlananID = ? AND banTürü = 'oyunBanı'";
                           	
                            PreparedStatement check = getConnection().prepareStatement(sql);
                
                            check.setInt(1, userID);
                           
                            final ResultSet result = check.executeQuery();
                           
                            while (result.next()) {
                            	return true;
                            }
                       } catch (SQLException e) {
                            Log.pt("Ban kontrol edilemedi!");
                            Log.pt(e.getMessage());
                        }
   					return false;
                    }
                   
             /**
             Açıklama: diroitleri yükleme
             */
             public static Collection<Drone> loadDrones(final int pUserID) {
          	   final ArrayList<Drone> drones = new ArrayList<>();
                 try {
                     final PreparedStatement preparedStatement =
                             mConnection.prepareStatement("SELECT * FROM server.players_drones WHERE userID = ?");
                     preparedStatement.setInt(1, pUserID);
         
                     final ResultSet result = preparedStatement.executeQuery();
                     
                     while (result.next()) {
                         final int droneID = result.getInt("droneID");
                         final String droneLootID = result.getString("lootID");
                         final int droneLevel = result.getInt("droneLevel");
                         final String Config1DroneDesign = result.getString("Config1DroneDesign");
                         final String Config2DroneDesign = result.getString("Config2DroneDesign");
         
                         drones.add(new Drone(droneID, droneLootID, droneLevel, Config1DroneDesign, Config2DroneDesign));
                     }
                 } catch (SQLException e) {
                     Log.pt("Couldn't load drones");
                     Log.pt(e.getMessage());
                     System.exit(0);
                 }
                 return drones;
             }
             
              /**
               Description: Loads clans
               */      
       public static void loadClans() {

           String query = "SELECT * FROM server.clans";

           try {
               final PreparedStatement preparedStatement =
                       mConnection.prepareStatement(query);

               final ResultSet result = preparedStatement.executeQuery();
               int clansLoaded = 0;
               while (result.next()) {
                   final int clanID = result.getInt("clanID");
                   query = "SELECT * FROM server.clans_diplomacy WHERE clanID1 = " + clanID + " OR clanID2 = "+ clanID;
                   PreparedStatement prep = mConnection.prepareStatement(query);
                   ResultSet result2 = prep.executeQuery();
                   ArrayList<Diplomacy> diplomacies = new ArrayList<Diplomacy>();
                   while(result2.next()){
                       diplomacies.add(new Diplomacy(result2.getInt("clanID1"),
                               result2.getInt("clanID2"),result2.getInt("relationType"),
                               result2.getString("expDate")));
                   }
                   final Clan clan =
                           new Clan(clanID, result.getInt("rankPoints"), result.getString("name"), result.getString("tag"), 
                           		result.getShort("company"), result.getString("members"), 
                           		diplomacies,
                                new ArrayList<BattleStation>());
                   ClanStorage.addClan(clan);
                   clansLoaded++;
               }

               Log.pt("Clans loaded: " + clansLoaded);
           } catch (SQLException e) {
               Log.pt("Couldn't load clans!");
               Log.pt(e.getMessage());
               System.exit(0);
           }

       }

       /**
       Açıklama: Klan lider adını çekme
       */
       public static String getLeaderName(final int pClanID) {
           try {
               String sql = "SELECT * FROM server.clans WHERE clanID = ?";             	
               PreparedStatement check = getConnection().prepareStatement(sql);
               check.setInt(1, pClanID);             
               final ResultSet result = check.executeQuery();
               final int userID = result.getInt("leaderID");
               //
               String sql2 = "SELECT * FROM server.players_accounts WHERE userID = ?";             	
               PreparedStatement check2 = getConnection().prepareStatement(sql2);
               check2.setInt(1, userID);             
               final ResultSet result2 = check2.executeQuery();
               
               while (result.next()) {
            	   return result2.getString("ShipUsername");
               }  	
           } catch (SQLException e) {
               Log.pt("Couldn't get leadername " + pClanID);
               Log.pt(e.getMessage());
           }
   		return "";
       }
       
       public static void setConnection(final Connection pConnection) {
           mConnection = pConnection;
       }

       public static Connection getConnection() {
           return mConnection;
       }

}
