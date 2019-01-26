using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_g1a
    {
        public const short ID = 3946;

        public Boolean varXt = false;      
        public Boolean showRequests = false;     
        public Boolean varL4m = false;      
        public Boolean varQ3T = false;

        public class_g1a(bool varL4m, bool varXt, bool showRequests, bool varQ3T)
        {
            this.varL4m = varL4m;
            this.varXt = varXt;
            this.showRequests = showRequests;
            this.varQ3T = varQ3T;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.varXt);
            param1.writeBoolean(this.showRequests);
            param1.writeBoolean(this.varL4m);
            param1.writeBoolean(this.varQ3T);
            return param1.Message.ToArray();
        }
    }
}
