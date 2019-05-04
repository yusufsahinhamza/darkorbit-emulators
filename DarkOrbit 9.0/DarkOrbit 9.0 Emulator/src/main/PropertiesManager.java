package main;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 Created by bpdev on 29/01/2015.
 */
public class PropertiesManager {

    // value names in config.properties
    private static final String SERVER_ID_PROP                   = "server_ID";
    private static final String DB_HOST_PROP                     = "host";
    private static final String DB_USERNAME_PROP                 = "username";
    private static final String DB_PASSWORD_PROP                 = "password";
    private static final String DB_NAME_PROP                     = "database";
    private static final String DB_PORT_PROP                     = "port";
    private static final String SOCKET_SERVER_IP_PROP            = "ip";
    private static final String GAME_SERVER_PORT_PROP            = "gameServerPort";
    private static final String CHAT_SERVER_PORT_PROP            = "chatPort";
    private static final String SOCKET_SERVER_PORT_PROP          = "updaterPort";
    private static final String PRINT_NEW_CLIENT_PACKETS_PROP    = "newClientPackets";
    private static final String PRINT_OLD_CLIENT_PACKETS_PROP    = "oldClientPackets";
    private static final String PRINT_SOCKET_SERVER_PACKETS_PROP = "updaterPackets";
    private static final String PRINT_CHAT_SERVER_PACKETS_PROP   = "chatPackets";
    private static final String CHECK_TRADE_STATUS_PROP          = "trade";
    private static final String DB_AUTO_UPDATE_PROP              = "autoSave";
    private static final String DB_AUTO_UPDATE_MINUTES_PROP      = "saveTime";
    private static final String DEBUG_MODE_PROP                  = "debug";
    private static final String VERSION_PROP                     = "version";

    // default values to set (if a value is not present in config.properties)
    private static final String SERVER_ID_PROP_DEFAULT                   = "1";
    private static final String DB_HOST_PROP_DEFAULT                     = "localhost";
    private static final String DB_USERNAME_PROP_DEFAULT                 = "root";
    private static final String DB_PASSWORD_PROP_DEFAULT                 = "";
    private static final String DB_NAME_PROP_DEFAULT                     = "server";
    private static final String DB_PORT_PROP_DEFAULT                     = "3307";
    private static final String SOCKET_SERVER_IP_PROP_DEFAULT            = "127.0.0.1";
    private static final String GAME_SERVER_PORT_PROP_DEFAULT            = "8080";
    private static final String CHAT_SERVER_PORT_PROP_DEFAULT            = "9338";
    private static final String SOCKET_SERVER_PORT_PROP_DEFAULT          = "4301";
    private static final String PRINT_NEW_CLIENT_PACKETS_PROP_DEFAULT    = "false";
    private static final String PRINT_OLD_CLIENT_PACKETS_PROP_DEFAULT    = "false";
    private static final String PRINT_SOCKET_SERVER_PACKETS_PROP_DEFAULT = "false";
    private static final String PRINT_CHAT_SERVER_PACKETS_PROP_DEFAULT   = "false";
    private static final String CHECK_TRADE_STATUS_PROP_DEFAULT          = "false";
    private static final String DB_AUTO_UPDATE_PROP_DEFAULT              = "true";
    private static final String DB_AUTO_UPDATE_MINUTES_PROP_DEFAULT      = "60";
    private static final String DEBUG_MODE_PROP_DEFAULT                  = "false";
    private static final String VERSION_PROP_DEFAULT                     = "";


    // our properties values to use, reachable via getters
    private int     mServerId;
    private String  mDatabaseHost;
    private String  mDatabaseUsername;
    private String  mDatabasePassword;
    private String  mDatabaseName;
    private int     mDatabasePort;
    private String  mSocketServerIp;
    private int     mGameServerPort;
    private int     mChatServerPort;
    private int     mSocketServerPort;
    private boolean mPrintNewClientPackets;
    private boolean mPrintOldClientPackets;
    private boolean mPrintSocketServerPackets;
    private boolean mPrintChatServerPackets;
    private boolean mCheckTradeStatus;
    private boolean mDatabaseAutosave;
    private int     mDatabaseAutosaveIntervalMinutes;
    private boolean mDebugMode;
    private String  mVersion;

    // Properties object, used to extract properties from file
    private final Properties mProperties;

    /**
     This class serves to hold properties loaded from a file

     @param pFilePath
     file path of a file to read properties from

     @throws FileNotFoundException
     if file cannot be found on that filepath
     @throws IOException
     if any I/O error occurs
     @throws IllegalArgumentException
     if values in file are of incorrectly formed
     */
    public PropertiesManager(final String pFilePath)
            throws IOException, IllegalArgumentException {

        // initializing Properties object
        this.mProperties = new Properties();

        // loads initial properties
        this.load(pFilePath);

    }

    // method that loads properties from a file
    public void load(final String pFilePath)
            throws IOException, IllegalArgumentException {

        // Read a resource stream from JAR
        InputStream is = this.getClass()
                             .getResourceAsStream("/" + pFilePath);


        if (is == null) {

            // in case of that's not JAR, but a regular launch
            FileReader fileReader = new FileReader(pFilePath);

            // Linking config file to Properties object
            BufferedReader reader = new BufferedReader(fileReader);

            this.mProperties.load(reader);

        } else {
            this.mProperties.load(is);
        }

        // fill values
        this.mServerId = this.stringToInt(this.mProperties.getProperty(SERVER_ID_PROP, SERVER_ID_PROP_DEFAULT));
        this.mDatabaseHost = this.mProperties.getProperty(DB_HOST_PROP, DB_HOST_PROP_DEFAULT);
        this.mDatabaseUsername = this.mProperties.getProperty(DB_USERNAME_PROP, DB_USERNAME_PROP_DEFAULT);
        this.mDatabasePassword = this.mProperties.getProperty(DB_PASSWORD_PROP, DB_PASSWORD_PROP_DEFAULT);
        this.mDatabaseName = this.mProperties.getProperty(DB_NAME_PROP, DB_NAME_PROP_DEFAULT);
        this.mDatabasePort = this.stringToInt(this.mProperties.getProperty(DB_PORT_PROP, DB_PORT_PROP_DEFAULT));
        this.mSocketServerIp = this.mProperties.getProperty(SOCKET_SERVER_IP_PROP, SOCKET_SERVER_IP_PROP_DEFAULT);
        this.mGameServerPort = this.stringToInt(
                this.mProperties.getProperty(GAME_SERVER_PORT_PROP, GAME_SERVER_PORT_PROP_DEFAULT));
        this.mChatServerPort = this.stringToInt(
                this.mProperties.getProperty(CHAT_SERVER_PORT_PROP, CHAT_SERVER_PORT_PROP_DEFAULT));
        this.mSocketServerPort = this.stringToInt(
                this.mProperties.getProperty(SOCKET_SERVER_PORT_PROP, SOCKET_SERVER_PORT_PROP_DEFAULT));
        this.mPrintNewClientPackets = this.stringToBoolean(
                this.mProperties.getProperty(PRINT_NEW_CLIENT_PACKETS_PROP, PRINT_NEW_CLIENT_PACKETS_PROP_DEFAULT));
        this.mPrintOldClientPackets = this.stringToBoolean(
                this.mProperties.getProperty(PRINT_OLD_CLIENT_PACKETS_PROP, PRINT_OLD_CLIENT_PACKETS_PROP_DEFAULT));
        this.mPrintSocketServerPackets = this.stringToBoolean(
                this.mProperties.getProperty(PRINT_SOCKET_SERVER_PACKETS_PROP,
                                             PRINT_SOCKET_SERVER_PACKETS_PROP_DEFAULT));
        this.mPrintChatServerPackets = this.stringToBoolean(
                this.mProperties.getProperty(PRINT_CHAT_SERVER_PACKETS_PROP, PRINT_CHAT_SERVER_PACKETS_PROP_DEFAULT));
        this.mCheckTradeStatus = this.stringToBoolean(
                this.mProperties.getProperty(CHECK_TRADE_STATUS_PROP, CHECK_TRADE_STATUS_PROP_DEFAULT));
        this.mDatabaseAutosave = this.stringToBoolean(
                this.mProperties.getProperty(DB_AUTO_UPDATE_PROP, DB_AUTO_UPDATE_PROP_DEFAULT));
        this.mDatabaseAutosaveIntervalMinutes = this.stringToInt(
                this.mProperties.getProperty(DB_AUTO_UPDATE_MINUTES_PROP, DB_AUTO_UPDATE_MINUTES_PROP_DEFAULT));
        this.mDebugMode =
                this.stringToBoolean(this.mProperties.getProperty(DEBUG_MODE_PROP, DEBUG_MODE_PROP_DEFAULT));
        this.mVersion = this.mProperties.getProperty(VERSION_PROP, VERSION_PROP_DEFAULT);


    }

    public int stringToInt(String pValue) {
        try {
            return Integer.parseInt(pValue);
        } catch (NumberFormatException e) {
            throw new IllegalArgumentException("Can't extract int from given string: " + pValue);
        }
    }

    public boolean stringToBoolean(String pValue) {
        if (pValue.equalsIgnoreCase("true")) {
            return true;
        } else if (pValue.equalsIgnoreCase("false")) {
            return false;
        } else {
            throw new IllegalArgumentException("Can't extract boolean from given string: " + pValue);
        }
    }
    public int getServerId() {
        return this.mServerId;
    }

    public String getDatabaseHost() {
        return this.mDatabaseHost;
    }

    public String getDatabaseUsername() {
        return this.mDatabaseUsername;
    }

    public String getDatabasePassword() {
        return this.mDatabasePassword;
    }

    public String getDatabaseName() {
        return this.mDatabaseName;
    }

    public int getDatabasePort() {
        return this.mDatabasePort;
    }

    public String getSocketServerIp() {
        return this.mSocketServerIp;
    }

    public int getGameServerPort() {
        return this.mGameServerPort;
    }

    public int getChatServerPort() {
        return this.mChatServerPort;
    }

    public int getSocketServerPort() {
        return this.mSocketServerPort;
    }

    public boolean isPrintNewClientPackets() {
        return this.mPrintNewClientPackets;
    }

    public boolean isPrintOldClientPackets() {
        return this.mPrintOldClientPackets;
    }

    public boolean isPrintSocketServerPackets() {
        return this.mPrintSocketServerPackets;
    }

    public boolean isPrintChatServerPackets() {
        return this.mPrintChatServerPackets;
    }

    public boolean isCheckTradeStatus() {
        return this.mCheckTradeStatus;
    }

    public boolean isDatabaseAutosave() {
        return this.mDatabaseAutosave;
    }

    public int getDatabaseAutosaveIntervalMinutes() {
        return this.mDatabaseAutosaveIntervalMinutes;
    }

    public boolean isDebugMode() {
        return this.mDebugMode;
    }

    public String getVersion() {
        return this.mVersion;
    }

}
