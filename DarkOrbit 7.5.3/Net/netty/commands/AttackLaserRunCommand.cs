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
        public const short ID = 30791;

        public static byte[] write(int attackerId, int targetId, int laserColor, bool isDiminishedBySkillShield, bool skilledLaser)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(attackerId);
            param1.writeInt(targetId);
            param1.writeInt(laserColor);
            param1.writeBoolean(isDiminishedBySkillShield);
            param1.writeBoolean(skilledLaser);
            return param1.ToByteArray();
        }
    }
}
