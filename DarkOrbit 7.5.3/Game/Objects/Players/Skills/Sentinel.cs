using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Sentinel
    {
        public Player Player { get; set; }

        public bool Active = false;

        public Sentinel(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if (Active)
                if (cooldown.AddMilliseconds(TimeManager.SENTINEL_DURATION) < DateTime.Now)
                    Disable();
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (Player.Ship.Id == 66 && (cooldown.AddMilliseconds(TimeManager.SENTINEL_DURATION + TimeManager.SENTINEL_COOLDOWN) < DateTime.Now || Player.Storage.GodMode))
            {
                Player.Storage.Sentinel = true;
                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.FORTRESS, 0, true));

                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            Player.Storage.Sentinel = false;
            Player.RemoveVisualModifier(VisualModifierCommand.FORTRESS);

            Player.SendCooldown(ServerCommands.SKILL_SENTINEL, TimeManager.SENTINEL_COOLDOWN);
            Active = false;
        }
    }
}
