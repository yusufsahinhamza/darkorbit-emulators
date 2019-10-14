using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class DrawFire : Skill
    {
        public override string LootId { get => SkillManager.CITADEL_DRAW_FIRE; }

        public override int Duration { get => TimeManager.CITADEL_DRAWFIRE_DURATION; }
        public override int Cooldown { get => TimeManager.CITADEL_DRAWFIRE_COOLDOWN; }

        public DrawFire(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
            }
        }

        public override void Send()
        {
            var citadelIds = new List<int> { 69 };

            if (citadelIds.Contains(Player.Ship.Id) && (cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode))
            {
                Active = true;

                Player.AddVisualModifier(VisualModifierCommand.DRAW_FIRE_OWNER, 0, "", 0, true);

                foreach (var character in Player.InRangeCharacters.Values)
                {
                    if (Player.Group != null)
                    {
                        if (character is Pet && Player.Group.Members.ContainsKey((character as Pet).Owner.Id)) continue;
                        if (character is Player && Player.Group.Members.ContainsKey(character.Id)) continue;
                    }

                    if (!Player.TargetDefinition(character, false)) continue;

                    if (character.Position.DistanceTo(Player.Position) < 500)
                    {
                        character.AddVisualModifier(VisualModifierCommand.DRAW_FIRE_TARGET, 0, "", 0, true);
                        character.Deselection();

                        if (character is Player player)
                        {
                            player.Storage.underDrawFire = true;
                            player.Storage.underDrawFireTime = DateTime.Now;
                            player.SelectEntity(Player.Id);
                            player.EnableAttack(player.Settings.InGameSettings.selectedLaser);
                        }
                        else
                            character.Selected = Player;
                    }
                }

                Player.SendCooldown(LootId, Duration, true);
                Player.CpuManager.DisableCloak();

                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            Active = false;
            Player.SendCooldown(LootId, Cooldown);
            Player.RemoveVisualModifier(VisualModifierCommand.DRAW_FIRE_OWNER);
        }
    }
}
