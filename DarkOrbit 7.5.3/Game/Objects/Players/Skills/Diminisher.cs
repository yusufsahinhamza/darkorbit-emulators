using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Diminisher
    {
        public Player Player { get; set; }

        public bool Active = false;

        public Diminisher(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.DIMINISHER_DURATION) < DateTime.Now)
                    Disable();
                if (Player.Selected == null || Player.Selected != Player.Storage.UnderDiminisherPlayer)
                    Disable();
            }
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (Player.Ship.Id == 64 && (cooldown.AddMilliseconds(TimeManager.DIMINISHER_DURATION + TimeManager.DIMINISHER_COOLDOWN) < DateTime.Now || Player.Storage.GodMode))
            {
                var enemy = Player.Selected;
                if (enemy == null || !(enemy is Player)) return;
                if (!Player.AttackManager.TargetDefinition(enemy as Player, false)) return;

                Player.Storage.Diminisher = true;
                Player.Storage.UnderDiminisherPlayer = enemy as Player;

                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.WEAKEN_SHIELDS, 0, true));
                (enemy as Player).AddVisualModifier(new VisualModifierCommand(enemy.Id, VisualModifierCommand.WEAKEN_SHIELDS, 0, true));
                (enemy as Player).SendCommand(AbilityStopCommand.write(2, enemy.Id, new List<int>()));

                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            var enemy = Player.Storage.UnderDiminisherPlayer;
            if (enemy == null) return;

            Player.Storage.Diminisher = false;
            Player.Storage.UnderDiminisherPlayer = null;

            Player.RemoveVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS);
            (enemy as Player).RemoveVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS);

            Player.SendCooldown(ServerCommands.SKILL_DIMINISHER, TimeManager.DIMINISHER_COOLDOWN);
            Active = false;
        }
    }
}
