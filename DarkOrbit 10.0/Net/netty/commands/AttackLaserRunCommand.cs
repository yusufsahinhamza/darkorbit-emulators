using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttackLaserRunCommand
    {
        public const short ID = 28127;

        public static byte[] write(int attackerId, int targetId, int laserColor, bool isDiminishedBySkillShield, bool skilledLaser)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeBoolean(isDiminishedBySkillShield);
            param1.writeInt(targetId >> 14 | targetId << 18);
            param1.writeInt(laserColor << 10 | laserColor >> 22);
            param1.writeInt(attackerId >> 10 | attackerId << 22);
            param1.writeBoolean(skilledLaser);
            return param1.ToByteArray();
        }
    }
}
