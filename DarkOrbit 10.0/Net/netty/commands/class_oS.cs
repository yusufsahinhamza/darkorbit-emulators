using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_oS
    {
        public const short ID = 25425;

        public int varQN = 0;
        public int varVl = 0;
        public int varF1R = 0;
        public int vare4Q = 0;
        public int vara4i = 0;
        public int vart3J = 0;
        public int varP4q = 0;
        public int varzM = 0;
        public int varZ2e = 0;
        public int varxl = 0;

        public class_oS(int vara4i, int varxl, int varZ2e, int varVl, int varzM, int varF1R, int varQN, int vare4Q, int vart3J, int varP4q)
        {
            this.vara4i = vara4i;
            this.varxl = varxl;
            this.varZ2e = varZ2e;
            this.varVl = varVl;
            this.varzM = varzM;
            this.varF1R = varF1R;
            this.varQN = varQN;
            this.vare4Q = vare4Q;
            this.vart3J = vart3J;
            this.varP4q = varP4q;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(varQN >> 2 | varQN << 30);
            param1.writeInt(varVl << 12 | varVl >> 20);
            param1.writeShort(-23056);
            param1.writeShort(22778);
            param1.writeInt(varF1R >> 1 | varF1R << 31);
            param1.writeInt(vare4Q << 6 | vare4Q >> 26);
            param1.writeInt(vara4i >> 7 | vara4i << 25);
            param1.writeInt(vart3J >> 14 | vart3J << 18);
            param1.writeInt(varP4q >> 6 | varP4q << 26);
            param1.writeInt(varzM >> 11 | varzM << 21);
            param1.writeInt(varZ2e >> 9 | varZ2e << 23);
            param1.writeInt(varxl << 15 | varxl >> 17);
            return param1.Message.ToArray();
        }
    }
}
