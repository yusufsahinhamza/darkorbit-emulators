package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class QualitySettingsRequest
        extends ClientCommand {

    public static final int     ID                 = 32602;
    public              int     qualityEngine      = 0;
    public              int     qualityEffect      = 0;
    public              boolean qualityCustomized  = false;
    public              int     qualityCollectable = 0;
    public              int     qualityPoizone     = 0;
    public              int     qualityPresetting  = 0;
    public              int     qualityBackground  = 0;
    public              int     qualityAttack      = 0;
    public              int     qualityExplosion   = 0;
    public              int     qualityShip        = 0;

    public QualitySettingsRequest(DataInputStream pIn) {
        super(pIn, ID);
    }

    public void read() {
        try {
            this.qualityBackground = in.readShort();
            this.qualityAttack = in.readShort();
            this.qualityPoizone = in.readShort();
            this.qualityCustomized = in.readBoolean();
            this.qualityEngine = in.readShort();
            this.qualityPresetting = in.readShort();
            this.qualityEffect = in.readShort();
            this.qualityCollectable = in.readShort();
            this.qualityShip = in.readShort();
            this.qualityExplosion = in.readShort();
            in.readShort();
        } catch (IOException e) {
        }
    }
}