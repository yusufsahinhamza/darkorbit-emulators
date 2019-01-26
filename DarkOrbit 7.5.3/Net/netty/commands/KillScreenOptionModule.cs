using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class KillScreenOptionModule
    {
        public const short ID = 31597;

        public KillScreenOptionTypeModule repairType;
        public PriceModule price;
        public bool affordableForPlayer;
        public int cooldownTime;
        public MessageLocalizedWildcardCommand descriptionKey;
        public MessageLocalizedWildcardCommand descriptionKeyAddon;
        public MessageLocalizedWildcardCommand toolTipKey;
        public MessageLocalizedWildcardCommand buttonKey;

        public KillScreenOptionModule(KillScreenOptionTypeModule repairType, PriceModule price, bool affordableForPlayer, int cooldownTime,
                                  MessageLocalizedWildcardCommand descriptionKey, MessageLocalizedWildcardCommand descriptionKeyAddon,
                                  MessageLocalizedWildcardCommand toolTipKey, MessageLocalizedWildcardCommand buttonKey)
        {
            this.repairType = repairType;
            this.price = price;
            this.affordableForPlayer = affordableForPlayer;
            this.cooldownTime = cooldownTime;
            this.descriptionKey = descriptionKey;
            this.descriptionKeyAddon = descriptionKeyAddon;
            this.toolTipKey = toolTipKey;
            this.buttonKey = buttonKey;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.write(repairType.write());
            param1.write(price.write());
            param1.writeBoolean(this.affordableForPlayer);
            param1.writeInt(this.cooldownTime);
            param1.write(descriptionKey.write());
            param1.write(descriptionKeyAddon.write());
            param1.write(toolTipKey.write());
            param1.write(buttonKey.write());
            return param1.Message.ToArray();
        }
    }
}
