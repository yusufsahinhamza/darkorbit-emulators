package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class GameplaySettingsRequest
        extends ClientCommand {

    public static final int     ID                            = 13714;
    public              boolean doubleclickAttackEnabled      = false;
    public              boolean autochangeAmmo                = false;
    public              boolean autoStartEnabled              = false;
    public              boolean autoRefinement                = false;
    public              boolean autoBoost                     = false;
    public              boolean autoBuyBootyKeys              = false;
    public              boolean quickSlotStopAttack           = false;
    public              boolean displayBattlerayNotifications = false;

    public GameplaySettingsRequest(DataInputStream pIn) {
        super(pIn, ID);
    }

    public void read() {
        try {
            this.autoBuyBootyKeys = in.readBoolean();
            this.autoRefinement = in.readBoolean();
            this.autochangeAmmo = in.readBoolean();
            this.quickSlotStopAttack = in.readBoolean();
            this.autoStartEnabled = in.readBoolean();
            this.displayBattlerayNotifications = in.readBoolean();
            this.doubleclickAttackEnabled = in.readBoolean();
            this.autoBoost = in.readBoolean();
        } catch (IOException e) {
        }
    }
}