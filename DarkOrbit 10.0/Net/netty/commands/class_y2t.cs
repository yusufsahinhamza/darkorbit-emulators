using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_y2t
    {
        public const short ID = 21683;

        public bool varf1t;
        public bool varOn;
        public bool QuestsLevelOrderDescending;
        public bool QuestsAvailableFilter;
        public bool QuestsUnavailableFilter;
        public bool QuestsCompletedFilter;

        public class_y2t(bool param1, bool param2, bool param3, bool param4, bool param5, bool param6)
        {
            this.QuestsAvailableFilter = param1;
            this.QuestsUnavailableFilter = param2;
            this.QuestsCompletedFilter = param3;
            this.varf1t = param4;
            this.varOn = param5;
            this.QuestsLevelOrderDescending = param6;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(QuestsLevelOrderDescending);
            param1.writeBoolean(QuestsAvailableFilter);
            param1.writeBoolean(QuestsUnavailableFilter);
            param1.writeBoolean(QuestsCompletedFilter);
            param1.writeShort(-19304);
            param1.writeBoolean(varf1t);
            param1.writeBoolean(varOn);
            return param1.Message.ToArray();
        }
    }
}
