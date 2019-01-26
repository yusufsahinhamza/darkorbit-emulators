using Ow.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    interface IHandler
    {
        void execute(GameSession gameSession, byte[] bytes);
    }
}
