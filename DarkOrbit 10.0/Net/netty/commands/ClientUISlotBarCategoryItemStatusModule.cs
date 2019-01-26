using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarCategoryItemStatusModule
    {
        public static short ID = 12754;

        public static short DEFAULT = 0;
        public static short BLUE = 4;
        public static short YELLOW = 3;
        public static short short_1167 = 5;
        public static short RED = 1;
        public static short GREEN = 2;
        public static short short_790 = 6;

        public bool available = false;
        public double maxCounterValue = 0;
        public bool buyable = false;
        public bool selected = false;
        public double counterValue = 0;
        public bool blocked = false;
        public String iconLootId = "";
        public bool visible = false;
        public bool activatable = false;
        public short counterStyle = 0;
        public String var_1474 = "";
        public ClientUITooltipsCommand toolTipItemBar;
        public ClientUITooltipsCommand toolTipSlotBar;

        public ClientUISlotBarCategoryItemStatusModule(ClientUITooltipsCommand pToolTipItemBarCommand, bool pActivatable,
                                                   String pIconLootId, bool pVisible, short pCounterStyle,
                                                   String param3, double pCounterValue, bool pBlocked,
                                                   bool pAvailable, ClientUITooltipsCommand pToolTipSlotBarCommand,
                                                   bool pBuyable, bool pSelected, double pMaxCounterValue)
        {
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

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.buyable);
            param1.writeDouble(this.counterValue);
            param1.writeShort(5539);
            param1.writeBoolean(this.blocked);
            param1.writeUTF(this.var_1474);
            param1.writeBoolean(this.available);
            param1.writeBoolean(this.activatable);
            param1.writeShort(this.counterStyle);
            param1.write(toolTipItemBar.write());
            param1.writeShort(12379);
            param1.write(toolTipSlotBar.write());
            param1.writeDouble(this.maxCounterValue);
            param1.writeBoolean(this.selected);
            param1.writeUTF(this.iconLootId);
            param1.writeBoolean(this.visible);
            return param1.Message.ToArray();
        }

        public byte[] writeCommand()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.buyable);
            param1.writeDouble(this.counterValue);
            param1.writeShort(5539);
            param1.writeBoolean(this.blocked);
            param1.writeUTF(this.var_1474);
            param1.writeBoolean(this.available);
            param1.writeBoolean(this.activatable);
            param1.writeShort(this.counterStyle);
            param1.write(toolTipItemBar.write());
            param1.writeShort(12379);
            param1.write(toolTipSlotBar.write());
            param1.writeDouble(this.maxCounterValue);
            param1.writeBoolean(this.selected);
            param1.writeUTF(this.iconLootId);
            param1.writeBoolean(this.visible);
            return param1.ToByteArray();
        }
    }
}
