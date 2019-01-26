using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_592
    {
        public const short ID = 4323;

        public bool var_1151;
        public bool var_2239;
        public bool mQuestsLevelOrderDescending;
        public bool mQuestsAvailableFilter;
        public bool mQuestsUnavailableFilter;
        public bool mQuestsCompletedFilter;

        public class_592(bool pQuestsAvailableFilter, bool pQuestsUnavailableFilter, bool pQuestsCompletedFilter,
                     bool var_1151, bool var_2239, bool pQuestsLevelOrderDescending)
        {
            this.mQuestsAvailableFilter = pQuestsAvailableFilter;
            this.mQuestsUnavailableFilter = pQuestsUnavailableFilter;
            this.mQuestsCompletedFilter = pQuestsCompletedFilter;
            this.var_1151 = var_1151;
            this.var_2239 = var_2239;
            this.mQuestsLevelOrderDescending = pQuestsLevelOrderDescending;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.mQuestsAvailableFilter);
            param1.writeBoolean(this.mQuestsUnavailableFilter);
            param1.writeBoolean(this.var_2239);
            param1.writeBoolean(this.mQuestsCompletedFilter);
            param1.writeShort(22624);
            param1.writeBoolean(this.mQuestsLevelOrderDescending);
            param1.writeBoolean(this.var_1151);
            return param1.Message.ToArray();
        }
    }
}
