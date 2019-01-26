using Ow.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Managers
{
    class EventManager
    {
        public static JackpotBattle JackpotBattle { get; set; }
        public static Spaceball Spaceball { get; set; }
        public static UltimateBattleArena UltimateBattleArena { get; set; }

        public static void InitiateEvents()
        {
            JackpotBattle = new JackpotBattle();
            Spaceball = new Spaceball();
            UltimateBattleArena = new UltimateBattleArena();
        }
    }
}
