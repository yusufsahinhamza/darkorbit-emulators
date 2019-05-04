package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class QualitySettingsModule
        implements ServerCommand {

    public static int     ID                  = 6715;
    public        boolean mNotSet             = false;
    public        int     mQualityAttack      = 0;
    public        int     mQualityBackground  = 0;
    public        int     mQualityCollectable = 0;
    public        boolean mQualityCustomized  = false;
    public        int     mQualityEffect      = 0;
    public        int     mQualityEngine      = 0;
    public        int     mQualityExplosion   = 0;
    public        int     mQualityPoizone     = 0;
    public        int     mQualityPresetting  = 0;
    public        int     mQualityShip        = 0;

    public QualitySettingsModule(boolean pNotSet, int pQualityAttack, int pQualityBackground, int pQualityPresetting,
                                 boolean pQualityCustomized, int pQualityPoizone, int pQualityShip, int pQualityEngine,
                                 int pQualityExplosion, int pQualityCollectable, int pQualityEffect) {
        this.mNotSet = pNotSet;
        this.mQualityAttack = pQualityAttack;
        this.mQualityBackground = pQualityBackground;
        this.mQualityPresetting = pQualityPresetting;
        this.mQualityCustomized = pQualityCustomized;
        this.mQualityPoizone = pQualityPoizone;
        this.mQualityShip = pQualityShip;
        this.mQualityEngine = pQualityEngine;
        this.mQualityExplosion = pQualityExplosion;
        this.mQualityCollectable = pQualityCollectable;
        this.mQualityEffect = pQualityEffect;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(this.mQualityPoizone);
            param1.writeBoolean(this.mQualityCustomized);
            param1.writeShort(5012);
            param1.writeShort(this.mQualityPresetting);
            param1.writeShort(this.mQualityAttack);
            param1.writeShort(this.mQualityEngine);
            param1.writeShort(this.mQualityCollectable);
            param1.writeBoolean(this.mNotSet);
            param1.writeShort(this.mQualityExplosion);
            param1.writeShort(this.mQualityEffect);
            param1.writeShort(this.mQualityShip);
            param1.writeShort(this.mQualityBackground);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}