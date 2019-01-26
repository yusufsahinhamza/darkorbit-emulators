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
        public const short ID = 31092;

        public KillScreenOptionTypeModule repairType;
        public PriceModule price;
        public bool affordableForPlayer;
        public int cooldownTime;
        public MessageLocalizedWildcardCommand descriptionKey;
        public MessageLocalizedWildcardCommand descriptionKeyAddon;
        public MessageLocalizedWildcardCommand toolTipKey;
        public MessageLocalizedWildcardCommand buttonKey;

        public KillScreenOptionModule(KillScreenOptionTypeModule pRepairType, PriceModule pPrice,
                                  bool pAffordableForPlayer, int pCooldownTime,
                                  MessageLocalizedWildcardCommand pDescriptionKey,
                                  MessageLocalizedWildcardCommand pDescriptionKeyAddon,
                                  MessageLocalizedWildcardCommand pToolTipKey,
                                  MessageLocalizedWildcardCommand pButtonKey)
        {
            this.repairType = pRepairType;
            this.price = pPrice;
            this.affordableForPlayer = pAffordableForPlayer;
            this.cooldownTime = pCooldownTime;
            this.descriptionKey = pDescriptionKey;
            this.descriptionKeyAddon = pDescriptionKeyAddon;
            this.toolTipKey = pToolTipKey;
            this.buttonKey = pButtonKey;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.write(descriptionKeyAddon.write());
            param1.write(buttonKey.write());
            param1.write(toolTipKey.write());
            param1.write(price.write());
            param1.write(repairType.write());
            param1.writeBoolean(affordableForPlayer);
            param1.writeShort(-16569);
            param1.writeInt(cooldownTime >> 14 | cooldownTime << 18);
            param1.write(descriptionKey.write());
            return param1.Message.ToArray();
        }
    }
}
