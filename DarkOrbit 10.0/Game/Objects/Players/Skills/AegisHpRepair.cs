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
    class AegisHpRepair : Skill
    {
        public override string LootId { get => SkillManager.AEGIS_HP_REPAIR; }

        public override int Duration { get => TimeManager.AEGIS_HP_REPAIR_DURATION; }

        public override int Cooldown
        {
            get
            {
                var value = TimeManager.AEGIS_HP_REPAIR_COOLDOWN;

                if (Player.Ship.Id == Ship.AEGIS_ELITE)
                    value -= Maths.GetPercentage(value, 10);

                return value;
            }
        }

        public AegisHpRepair(Player player) : base(player) { }

        public List<int> targetIds = new List<int>();

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
                else
                    ExecuteHeal();
            }
        }

        public override void Send()
        {
            var aegisIds = new List<int> { Ship.AEGIS, Ship.AEGIS_VETERAN, Ship.AEGIS_ELITE };

            if (aegisIds.Contains(Player.Ship.Id) && (cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode))
            {
                Active = true;

                var target = Player.Selected;

                if (target != null)
                {
                    short relationType = Player.Clan.Id != 0 && target.Clan.Id != 0 ? Player.Clan.GetRelation(target.Clan) : (short)0;

                    if ((Player.Group != null && Player.Group.Members.ContainsKey(target.Id)) || relationType != ClanRelationModule.AT_WAR)
                        targetIds.Add(target.Id);
                }

                Player.SendCooldown(LootId, Duration, true);
                Player.CpuManager.DisableCloak();

                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            Active = false;
            targetIds.Clear();
            Player.SendCooldown(LootId, Cooldown);

            var abilityStopCommand = AbilityStopCommand.write(101, Player.Id, targetIds);
            var abilityEffectDeActivationCommand = AbilityEffectDeActivationCommand.write(101, Player.Id, targetIds);

            foreach (var id in targetIds)
            {
                var player = GameManager.GetPlayerById(id);

                player.SendCommand(abilityStopCommand);
                player.SendCommand(abilityEffectDeActivationCommand);
            }

            Player.SendCommand(abilityStopCommand);
            Player.SendCommand(abilityEffectDeActivationCommand);
        }

        public DateTime HealTime = new DateTime();
        public void ExecuteHeal()
        {
            if (HealTime.AddSeconds(1) < DateTime.Now)
            {
                Player.Heal(20000);

                foreach (var id in targetIds)
                {
                    var player = GameManager.GetPlayerById(id);

                    if (player != null && player.Position.DistanceTo(Player.Position) < 500)
                    {
                        var abilityEffectActivationCommand = AbilityEffectActivationCommand.write(101, Player.Id, targetIds);

                        Player.SendCommand(abilityEffectActivationCommand);
                        Player.SendCommandToInRangePlayers(abilityEffectActivationCommand);

                        player.Heal(40000, Player.Id);
                    }
                }

                HealTime = DateTime.Now;
            }
        }
    }
}
