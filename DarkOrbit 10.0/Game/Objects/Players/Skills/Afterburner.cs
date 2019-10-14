using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Afterburner : Skill
    {
        public override string LootId { get => SkillManager.LIGHTNING; }

        public override int Duration { get => TimeManager.LIGHTNING_DURATION; }
        public override int Cooldown { get => TimeManager.LIGHTNING_COOLDOWN; }

        public Afterburner(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
        }

        public override void Send()
        {
            if (Player.Ship.Id == Ship.VENGEANCE_LIGHTNING && cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SkillManager.DisableAllSkills();

                Player.Storage.Lightning = true;
                Player.SendCommand(SetSpeedCommand.write(Player.Speed, Player.Speed));

                Player.AddVisualModifier(VisualModifierCommand.TRAVEL_MODE, 0, "", 0, true);

                Player.SendCooldown(LootId, Duration, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            Player.Storage.Lightning = false;
            Player.SendCommand(SetSpeedCommand.write(Player.Speed, Player.Speed));

            Player.RemoveVisualModifier(VisualModifierCommand.TRAVEL_MODE);

            Player.SendCooldown(LootId, Cooldown);
            Active = false;
        }
    }
}
