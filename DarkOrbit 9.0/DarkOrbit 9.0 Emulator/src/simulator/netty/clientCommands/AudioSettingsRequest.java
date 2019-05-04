package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the audio settings request

 @author Ordepsousa
 @date 22/07/2014
 @file AudioSettingsRequest.java
 @package game.objects.netty.commands */
public class AudioSettingsRequest
        extends ClientCommand {

    public static final int     ID              = 3057;
    public              int     voice           = 0;
    public              int     music           = 0;
    public              boolean playCombatMusic = false;
    public              int     sound           = 0;

    /**
     Constructor
     */
    public AudioSettingsRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.voice = in.readInt();
            this.voice = this.voice >>> 8 | this.voice << 24;
            this.playCombatMusic = in.readBoolean();
            this.music = in.readInt();
            this.music = this.music << 7 | this.music >>> 25;
            this.sound = in.readInt();
            this.sound = this.sound >>> 6 | this.sound << 26;
        } catch (IOException e) {
        }
    }
}