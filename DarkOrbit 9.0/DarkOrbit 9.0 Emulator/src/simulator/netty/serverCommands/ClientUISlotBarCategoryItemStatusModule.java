package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ClientUISlotBarCategoryItemStatusModule
        implements ServerCommand {

    public static int   ID         = 24696;
    public static short DEFAULT    = 0;
    public static short BLUE       = 4;
    public static short YELLOW     = 3;
    public static short short_1167 = 5;
    public static short RED        = 1;
    public static short GREEN      = 2;
    public static short short_790  = 6;

    public boolean available       = false;
    public double  maxCounterValue = 0;
    public boolean buyable         = false;
    public boolean selected        = false;
    public double  counterValue    = 0;
    public boolean blocked         = false;
    public String  iconLootId      = "";
    public boolean visible         = false;
    public boolean activatable     = false;
    public short   counterStyle    = 0;
    public String  var_1474        = "";
    public ClientUITooltipsCommand toolTipItemBar;
    public ClientUITooltipsCommand toolTipSlotBar;

    public ClientUISlotBarCategoryItemStatusModule(ClientUITooltipsCommand pToolTipItemBarCommand, boolean pActivatable,
                                                   String pIconLootId, boolean pVisible, short pCounterStyle,
                                                   String param3, double pCounterValue, boolean pBlocked,
                                                   boolean pAvailable, ClientUITooltipsCommand pToolTipSlotBarCommand,
                                                   boolean pBuyable, boolean pSelected, double pMaxCounterValue) {
        this.available = pAvailable;
        this.visible = pVisible;
        this.var_1474 = param3;
        this.toolTipItemBar = pToolTipItemBarCommand;
        this.toolTipSlotBar = pToolTipSlotBarCommand;
        this.buyable = pBuyable;
        this.maxCounterValue = pMaxCounterValue;
        this.counterValue = pCounterValue;
        this.counterStyle = pCounterStyle;
        this.iconLootId = pIconLootId;
        this.activatable = pActivatable;
        this.selected = pSelected;
        this.blocked = pBlocked;
    }


    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 26;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream out) {
        try {
            out.writeBoolean(this.blocked);
            out.writeBoolean(this.buyable);
            out.writeShort(this.counterStyle);
            out.writeBoolean(this.selected);
            this.toolTipItemBar.write(out);
            out.writeUTF(this.var_1474);
            out.writeBoolean(this.available);
            this.toolTipSlotBar.write(out);
            out.writeDouble(this.counterValue);
            out.writeBoolean(this.visible);
            out.writeUTF(this.iconLootId);
            out.writeDouble(this.maxCounterValue);
            out.writeBoolean(this.activatable);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}