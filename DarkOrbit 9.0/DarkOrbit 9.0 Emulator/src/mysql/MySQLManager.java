package mysql;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import utils.Log;

/**
 This class will manage the MySQL connection
 */
public class MySQLManager {

    private Connection mConnection;

    public MySQLManager(final String pHost, final int pPort, final String pUsername, final String pPassword,
                        final String pDatabaseName) {

        Log.p("Connecting to MySQL DB on " + pHost);

        try {
            //Create a new MySQL connection, the DSN for a MySQL connection in java is jdbc:mysql://host
            //We send the connection DSN, the username and the password as parameters
            this.mConnection = DriverManager.getConnection(
                    "jdbc:mysql://" + pHost + ":" + pPort + "/" + pDatabaseName + "?autoReconnect=true&characterEncoding=UTF-8", pUsername,
                    pPassword);

            //add automatically ";" at the end of queries
            this.mConnection.setAutoCommit(true);
            Log.pt("Connection succesful");
            Log.p("Connected to MySQL DB on " + pHost);
        } catch (SQLException e) {

            Log.pt("Couldn't connect to MySQL DB");
            Log.pt(e.getMessage());
            System.exit(0);

        }

    }

    public static synchronized ResultSet query(final Connection pConnection, final String pQuery)
            throws SQLException {

        //We create a new statment to execute the query
        final Statement statement = pConnection.createStatement();

        //Wait a bit (If the query is too long)
        statement.setQueryTimeout(300);

        //And return the result
        return statement.executeQuery(pQuery);
    }

    public static synchronized void update(final Connection pConnection, final String pQuery)
            throws SQLException {
        //We create a new statment to execute the query
        Statement statement = pConnection.createStatement();

        //Wait a bit (If the query is too long...
        statement.setQueryTimeout(300);

        //Execute the query
        statement.executeUpdate(pQuery);
    }

    public void end() {

        Log.pt("Closing MySQL DB connection");

        if (this.mConnection != null) {

            try {

                if (!this.mConnection.isClosed()) {
                    this.mConnection.close();
                }

                Log.pt("MySQL DB connection closed");

            } catch (SQLException e) {

                Log.pt("Couldn't close MySQL DB connection");
                Log.pt(e.getMessage());

            }

            this.mConnection = null;

        }

    }

    public Connection getConnection() {
        return this.mConnection;
    }

}