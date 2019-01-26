using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class VisualModifierCommand
    {
        public static short ID = 6297;

        public const short TRAVEL_MODE = 0;
        public const short HEALING_POD = 1;
        public const short FORTIFY = 2;
        public const short PROTECT_OWNER = 3;
        public const short PROTECT_TARGET = 4;
        public const short DRAW_FIRE_OWNER = 5;
        public const short DRAW_FIRE_TARGET = 6;
        public const short ULTIMATE_EMP_TARGET = 7;
        public const short INACTIVE = 8;
        public const short FORTRESS = 9;
        public const short PRISMATIC_SHIELD = 10;
        public const short WEAKEN_SHIELDS = 11;
        public const short WEAKEN_SHIELDS_TARGET = 12;
        public const short SINGULARITY = 13;
        public const short SINGULARITY_TARGET = 14;
        public const short SHIP_WARP = 15;
        public const short NPC_INFILTRATOR = 16;
        public const short LEONOV_EFFECT = 17;
        public const short WIZARD_ATTACK = 18;
        public const short GHOST_EFFECT = 19;
        public const short MIRRORED_CONTROLS = 20;
        public const short STICKY_BOMB = 21;
        public const short GREEN_GLOW = 22;
        public const short RED_GLOW = 23;
        public const short GENERIC_GLOW = 24;
        public const short EMERGENCY_REPAIR = 25;
        public const short INVINCIBILITY = 26;
        public const short BATTLESTATION_DEFLECTOR = 27;
        public const short BATTLESTATION_DOWNTIME_TIMER = 28;
        public const short BATTLESTATION_INSTALLING = 29;
        public const short BATTLESTATION_CONSTRUCTING = 30;
        public const short OWNS_BATTLESTATION = 31;
        public const short BUFFZONE = 32;
        public const short BLOCKED_ZONE_EXPLOSION = 33;
        public const short NPC_DECLOAK_ZONE = 34;
        public const short LEGENDARY_NPC_NAME = 35;

        public int userId = 0;
        public short modifier = 0;
        public int attribute = 0;
        public Boolean activated = false;

        public VisualModifierCommand(int userId, short modifier, int attribute, bool activated)
        {
            this.userId = userId;
            this.modifier = modifier;
            this.attribute = attribute;
            this.activated = activated;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.userId);
            param1.writeShort(this.modifier);
            param1.writeInt(this.attribute);
            param1.writeBoolean(this.activated);
            return param1.Message.ToArray();
        }

        public byte[] writeCommand()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.userId);
            param1.writeShort(this.modifier);
            param1.writeInt(this.attribute);
            param1.writeBoolean(this.activated);
            return param1.ToByteArray();
        }
    }
}
