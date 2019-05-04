package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 24-03-2015.
 */
public class TechTypeModule {

    public static short ENERGY_LEECH_ARRAY           = 0;
    public static short ENERGY_CHAIN_IMPULSE         = 1;
    public static short ROCKET_PROBABILITY_MAXIMIZER = 2;
    public static short SHIELD_BACKUP                = 3;
    public static short SPEED_LEECH                  = 4;
    public static short BATTLE_REPBOT                = 5;
    public static short CLINGING_IMPULSE_DRONE       = 6;

    public static int ID = 12158;

    public short typeValue = 0;

    public TechTypeModule(short typeValue) {
        this.typeValue = typeValue;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.method_8(param1);
        } catch (IOException e) {
        }
    }

    protected void method_8(DataOutputStream param1) {
        try {
            param1.writeShort(this.typeValue);
            param1.writeShort(-30124);
        } catch (IOException e) {
        }
    }
}
