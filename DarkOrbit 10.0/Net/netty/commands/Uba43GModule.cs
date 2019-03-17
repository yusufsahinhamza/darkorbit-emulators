using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class Uba43GModule : command_NQ
    {
        public const short ID = 8290;

        public float playerScore = 0;      
        public List<Ubaf3kModule> varCp;    
        public float varhe = 0;    
        public string var12h = "";     
        public short varX4F = 0;     
        public int varI1G = 0; 
        public int varv3x = 0;

        public Uba43GModule(List<Ubaf3kModule> varCp, short varX4F, int varI1G, int varv3x, string var12h, float playerScore, float varhe)
        {
            this.varCp = varCp;
            this.varX4F = varX4F;
            this.varI1G = varI1G;
            this.varv3x = varv3x;
            this.var12h = var12h;
            this.playerScore = playerScore;
            this.varhe = varhe;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeFloat(this.playerScore);
            param1.writeInt(this.varCp.Count);
            foreach(var _loc2_ in this.varCp)
            {
                param1.write(_loc2_.write());
            }
            param1.writeFloat(this.varhe);
            param1.writeUTF(this.var12h);
            param1.writeShort(this.varX4F);
            param1.writeInt(this.varI1G << 7 | this.varI1G >> 25);
            param1.writeInt(this.varv3x << 10 | this.varv3x >> 22);
            return param1.Message.ToArray();
        }
    }
}
