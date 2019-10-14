using Ow.Game.Movements;
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
    class AegisRepairPod : Skill
    {
        public override string LootId { get => SkillManager.AEGIS_REPAIR_POD; }

        public override int Duration { get => TimeManager.AEGIS_REPAIR_POD_DURATION; }

        public override int Cooldown
        {
            get
            {
                var value = TimeManager.AEGIS_REPAIR_POD_COOLDOWN;

                if (Player.Ship.Id == Ship.AEGIS_ELITE)
                    value += Maths.GetPercentage(value, 50);

                return value;
            }
        }

        public AegisRepairPod(Player player) : base(player) { }

        public Asset Pod { get; set; }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
                else
                    ExecuteHeal();
            }
            else if (!Player.VisualModifiers.ContainsKey(VisualModifierCommand.HEALING_POD) && cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now)
                Player.AddVisualModifier(VisualModifierCommand.HEALING_POD, 0, "", 0, true);
        }

        public override void Send()
        {
            var aegisIds = new List<int> { Ship.AEGIS, Ship.AEGIS_VETERAN, Ship.AEGIS_ELITE };

            if (aegisIds.Contains(Player.Ship.Id) && (cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode))
            {
                Active = true;

                Pod = new Asset(Player.Spacemap, Player.Position, AssetTypeModule.HEALING_POD);

                Player.SendCooldown(LootId, Duration, true);
                Player.CpuManager.DisableCloak();
                Player.RemoveVisualModifier(VisualModifierCommand.HEALING_POD);

                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            Active = false;

            if (Pod != null)
            {
                Pod.Remove();
                Pod = null;
            }

            Player.SendCooldown(LootId, Cooldown);
        }

        public DateTime HealTime = new DateTime();
        public void ExecuteHeal()
        {
            if (HealTime.AddSeconds(1) < DateTime.Now)
            {
                if (Pod != null)
                {
                    foreach (var character in Pod.Spacemap.Characters.Values)
                    {
                        if (character is Player player)
                        {
                            if (player == Player || (Player.Group != null && Player.Group.Members.ContainsKey(player.Id)))
                            {
                                if (player.Position.DistanceTo(Pod.Position) < 350)
                                    player.Heal(20000, Player.Id);
                            }
                        }
                    }
                }

                HealTime = DateTime.Now;
            }
        }
    }
}
