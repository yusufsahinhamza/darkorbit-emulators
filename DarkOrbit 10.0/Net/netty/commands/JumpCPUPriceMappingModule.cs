using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class JumpCPUPriceMappingModule
    {
        public const short ID = 3574;

        public List<int> mapIdList;     
        public int price = 0;

        public JumpCPUPriceMappingModule(int price, List<int> mapIdList)
        {
            this.price = price;
            this.mapIdList = mapIdList;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.mapIdList.Count);
            foreach(var id in this.mapIdList)
            {
                param1.writeInt(id << 11 | id >> 21);
            }
            param1.writeInt(this.price >> 1 | this.price << 31);
            param1.writeShort(5013);
            return param1.Message.ToArray();
        }
    }
}
