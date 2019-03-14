using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerInformationsModule : command_i3O
    {
        public const short ID = 25891;

        public int maxShield = 0;
        public int hp = 0;
        public int nanoHull = 0;
        public int maxNanoHull = 0;
        public int shield = 0;
        public int maxHp = 0;

        public GroupPlayerInformationsModule(int hp, int maxHp, int shield, int maxShield, int nanoHull, int maxNanoHull)
        {
            this.hp = hp;
            this.maxHp = maxHp;
            this.shield = shield;
            this.maxShield = maxShield;
            this.nanoHull = nanoHull;
            this.maxNanoHull = maxNanoHull;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeInt(maxShield << 15 | maxShield >> 17);
            param1.writeInt(hp >> 6 | hp << 26);
            param1.writeInt(nanoHull << 8 | nanoHull >> 24);
            param1.writeInt(maxNanoHull << 13 | maxNanoHull >> 19);
            param1.writeInt(shield << 16 | shield >> 16);
            param1.writeInt(maxHp << 8 | maxHp >> 24);
            return param1.Message.ToArray();
        }
    }
}
