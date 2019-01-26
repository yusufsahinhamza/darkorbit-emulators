using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_F2I
    {
        public const short ID = 29864;

        public const short JUMP_GATES = 6;
        public const short JUMP_DEVICE = 20;
        public const short ITEM_UPGRADE = 23;
        public const short SHIP_REPAIR = 0;
        public const short WELCOME = 12;
        public const short SKYLAB = 1;
        public const short TECH_FACTORY = 26;
        public const short ROCKET_LAUNCHER = 30;
        public const short POLICY_CHANGES = 16;
        public const short ORE_TRANSFER = 35;
        public const short FULL_CARGO = 21;
        public const short INSTALLING_NEW_EQUIPMENT = 5;
        public const short SKILL_TREE = 25;
        public const short vard34 = 15;      
        public const short SHIP_DESIGN = 32;
        public const short AUCTION_HOUSE = 29;
        public const short BOOST_YOUR_EQUIP = 9;
        public const short THE_SHOP = 3;
        public const short WEALTHY_FAMOUS = 11;
        public const short TRAINING_GROUNDS = 36;
        public const short PALLADIUM = 28;
        public const short CLAN_BATTLE_STATION = 27;
        public const short SELL_RESOURCE = 10;
        public const short PREPARE_BATTLE = 7;
        public const short ATTACK = 19;
        public const short EQUIP_YOUR_ROCKETS = 17;
        public const short CHANGING_SHIPS = 4;
        public const short GALAXY_GATE = 24;
        public const short UNKOWN_DANGERS = 18;
        public const short varC1l = 37;     
        public const short CONTACT_LIST = 34;
        public const short HOW_TO_FLY = 13;
        public const short PVP_WARNING = 2;
        public const short GET_MORE_AMMO = 8;
        public const short SECOND_CONFIGURATION = 22;
        public const short LOOKING_FOR_GROUPS = 33;
        public const short REQUEST_MISSION = 14;
        public const short EXTRA_CPU = 31;

        public short content = 0;

        public class_F2I(short param1)
        {
            content = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(31864);
            param1.writeShort(this.content);
            return param1.Message.ToArray();
        }
    }
}
