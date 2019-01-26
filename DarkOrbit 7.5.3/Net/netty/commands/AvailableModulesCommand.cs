using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AvailableModulesCommand
    {
        public static short ID = 28039;

        public List<StationModuleModule> modules;

        public AvailableModulesCommand(List<StationModuleModule> modules)
        {
            this.modules = modules;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.modules.Count);
            foreach(var module in this.modules)
            {
                param1.write(module.write());
            }
            return param1.Message.ToArray();
        }
    }
}
