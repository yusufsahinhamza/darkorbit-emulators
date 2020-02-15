package main;


import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Scanner;
import java.util.Timer;
import java.util.TimerTask;

import utils.Settings;
import utils.Log;

/**
 Description: This is the main file which contains "public static void main(String[] args)"

 @author Manulaiko
 @date 14/02/2014
 @editor LEJYONER
 @date 03/04/2017
 @file Launcher.java
 @package main
 @project SpaceBattles */
public class Launcher {

    private static final String PROPERTIES_FILE_PATH = "config.properties";
    private static Scanner read;
    /**
     Description: Main method

     @param args:
     Command line arguments
     */
    public static void main(String[] args) {
    //	Log.p("You need enter username and password to start the emulator!");
    //	read = new Scanner(System.in);
    //	System.out.print("Username: ");
    //	String username = read.nextLine();
    //	System.out.print("Password: ");
    //	String password = read.nextLine();

    //	if (username.equals("") && password.equals(""))
    //	{
        Log.br();
        Log.p("<======================================","<DarkOrbit 9.0>","======================================>");

        final PropertiesManager propertiesManager;
        // First read properties
        try {
            propertiesManager = new PropertiesManager(PROPERTIES_FILE_PATH);
        } catch (FileNotFoundException e) {

            Log.p("Properties file not found. Server shutting down");

            System.exit(0);
            return;
        } catch (IllegalArgumentException e) {

            Log.p("Couldn't read properties file. Server shutting down: " + e.getMessage());

            System.exit(0);
            return;
        } catch (IOException e) {

            Log.p("Couldn't read properties file. Server shutting down: " + e.getMessage());

            System.exit(0);
            return;
        }

        Log.p("Current emulator version: " + propertiesManager.getVersion());

        final ServerManager serverManager = new ServerManager(propertiesManager);
        serverManager.begin();

        final CommandLineProcessor cmdProcessor = new CommandLineProcessor(serverManager);
        cmdProcessor.startProcessing();

        new Timer().schedule(new TimerTask() {
            public void run() {
            	cmdProcessor.restartManual(60);
            }
        }, 2430000); // 1 saat = 3645000

    	}
 // }
}
