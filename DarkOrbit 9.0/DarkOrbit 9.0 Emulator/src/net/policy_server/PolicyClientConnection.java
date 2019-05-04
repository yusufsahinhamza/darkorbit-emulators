package net.policy_server;


import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.Timer;

import utils.Settings;

/**
 Description: represents connection to policy server
 */
public class PolicyClientConnection
        implements Runnable {

    public Socket         socket;
    public BufferedReader in;
    public Thread         thread;

    public int                    userID    = 0;
    public String                 sessionID = "";
    public PolicyClientConnection target    = null;

    public Timer damage;

    /**
     Constructor

     @param socket:
     The current client socket
     */
    public PolicyClientConnection(Socket socket) {
        try {
        	if(Settings.TEXTS_ENABLED) { System.out.println("Received connection!"); }
            //We save the parameter in the variable socket
            this.socket = socket;
            //We make a new reader (to read packets) with the input stream from the socket
            in = new BufferedReader(new InputStreamReader(socket.getInputStream()));

            //Create a new thread just for the user
            thread = new Thread(this);
            thread.setDaemon(true);
            //And start it
            //look at run() method
            thread.start();
        } catch (IOException e) {
            //We couldn't get the reader object from the socket
            System.out.println("Couldn't process connection!");
            System.out.println(e.getMessage());
            //Close the connection with the user
            try {
                this.socket.close();
            } catch (IOException e1) {
                //Darn... we even couldn't the connection with the user... something is badass
                //I think the best is to close the server
                System.exit(0);
            }
        }
    }

    /**
     Description: This method will read the packet char by char
     */
    public void run() {
        try {
            String packet = "";
            char[] packetChar = new char[1];

            //While the socket input stream (call it packet) char isn't -1 process the packet
            while (in.read(packetChar, 0, 1) != -1) {
                //If the char isn't null, new line or return char
                if (packetChar[0] != '\u0000' && packetChar[0] != '\n' && packetChar[0] != '\r') {
                    //packet increase it's value with the char
                    //Example:
                    // packet = RD, packetChar[0] = Y;
                    // now packet = RDY;
                    packet += packetChar[0];
                } else if (!packet.isEmpty()) {
                    //If the packet isn't "" we have the complete packet

                    //Concat the packet with UTF8
                    packet = new String(packet.getBytes(), "UTF8");
                    //Assemble the packet
                    //Declaration
                    if (packet.equals("<policy-file-request/>")) {
                        //Send the policy
                        sendPolicy(socket);
                        if(Settings.TEXTS_ENABLED) { System.out.println("Policy file sent"); }
                    }
                    //Set the packet again to ""
                    packet = "";
                }
            }
        } catch (IOException e) {
        	if(Settings.TEXTS_ENABLED) { System.out.println("Couldn't read packet!"); }
        	if(Settings.TEXTS_ENABLED) { System.out.println(e.getMessage()); }
        }
    }


    /**
     Description: Send a packet, nothing else

     @param socket:
     the socket that we will use to send the packet
     @param packet:
     the packet that we will send
     */
    public void sendPacket(Socket socket, String packet) {
        try {
            //First get the PrintWriter object from the socket
            PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
            //And then print the packet to the PrintWriter object
            out.print((packet) + (char) 0x00);
            //Flush it and you sent your packet!
            out.flush();
        } catch (IOException e) {
            //We couldn't sent this packet! that's bad...
        	if(Settings.TEXTS_ENABLED) { System.out.println("Couldn't send packet!"); }
        	if(Settings.TEXTS_ENABLED) { System.out.println(e.getMessage()); }
        }
    }

    /**
     Description: Sends the policy

     @param socket:
     the socket that we will use to send the policy
     */
    public void sendPolicy(Socket socket) {
        String policy = "<?xml version=\"1.0\"?>\r\n" +
                        "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                        "<cross-domain-policy>\r\n" +
                        "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                        "</cross-domain-policy>";
        sendPacket(socket, policy);
    }
}