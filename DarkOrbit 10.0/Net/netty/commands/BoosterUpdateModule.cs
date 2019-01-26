using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BoosterUpdateModule
    {
        public const short ID = 6195;

        public BoostedAttributeTypeModule attributeType;
        public float value;
        public List<BoosterTypeModule> boosterTypes;

        public BoosterUpdateModule(BoostedAttributeTypeModule attributeType, float value, List<BoosterTypeModule> boosterTypes)
        {
            this.attributeType = attributeType;
            this.value = value;
            this.boosterTypes = boosterTypes;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.write(attributeType.write());
            param1.writeInt(this.boosterTypes.Count);
            foreach(var type in this.boosterTypes)
            {
                param1.write(type.write());
            }
            param1.writeFloat(this.value);
            return param1.Message.ToArray();
        }
    }
}
