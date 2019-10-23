using Ow.Game.Objects.Players.Skills;
using Ow.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class SkillManager : AbstractManager
    {
        public const String SENTINEL = "ability_sentinel";
        public const String DIMINISHER = "ability_diminisher";
        public const String VENOM = "ability_venom";
        public const String SPECTRUM = "ability_spectrum";
        public const String SOLACE = "ability_solace";
        public const String AEGIS_HP_REPAIR = "ability_aegis_hp-repair";
        public const String AEGIS_REPAIR_POD = "ability_aegis_repair-pod";
        public const String AEGIS_SHIELD_REPAIR = "ability_aegis_shield-repair";
        public const String CITADEL_DRAW_FIRE = "ability_citadel_draw-fire";
        public const String CITADEL_FORTIFY = "ability_citadel_fortify";
        public const String CITADEL_PROTECTION = "ability_citadel_protection";
        public const String CITADEL_TRAVEL = "ability_citadel_travel";
        public const String SPEARHEAD_DOUBLE_MINIMAP = "ability_spearhead_double-minimap";
        public const String SPEARHEAD_JAM_X = "ability_spearhead_jam-x";
        public const String SPEARHEAD_TARGET_MARKER = "ability_spearhead_target-marker";
        public const String SPEARHEAD_ULTIMATE_CLOAK = "ability_spearhead_ultimate-cloak";
        public const String LIGHTNING = "ability_lightning";

        public SkillManager(Player player) : base(player) { InitiateSkills(); }

        public void InitiateSkills(bool updateSlotbar = false)
        {
            Player.Storage.Skills.Clear();

            if (Ship.SENTINELS.Contains(Player.Ship.Id))
                Player.Storage.Skills.Add(SkillManager.SENTINEL, new Sentinel(Player));
            else if (Ship.SPECTRUMS.Contains(Player.Ship.Id))
                Player.Storage.Skills.Add(SkillManager.SPECTRUM, new Spectrum(Player));
            else if (Ship.DIMINISHERS.Contains(Player.Ship.Id))
                Player.Storage.Skills.Add(SkillManager.DIMINISHER, new Diminisher(Player));
            else
            {
                switch (Player.Ship.Id)
                {
                    case Ship.GOLIATH_SOLACE:
                        Player.Storage.Skills.Add(SkillManager.SOLACE, new Solace(Player));
                        break;
                    case Ship.GOLIATH_VENOM:
                        Player.Storage.Skills.Add(SkillManager.VENOM, new Venom(Player));
                        break;
                    case Ship.VENGEANCE_LIGHTNING:
                        Player.Storage.Skills.Add(SkillManager.LIGHTNING, new Afterburner(Player));
                        break;
                    case Ship.AEGIS:
                    case Ship.AEGIS_VETERAN:
                    case Ship.AEGIS_ELITE:
                        Player.Storage.Skills.Add(SkillManager.AEGIS_HP_REPAIR, new AegisHpRepair(Player));
                        Player.Storage.Skills.Add(SkillManager.AEGIS_SHIELD_REPAIR, new AegisShieldRepair(Player));
                        Player.Storage.Skills.Add(SkillManager.AEGIS_REPAIR_POD, new AegisRepairPod(Player));
                        break;
                    case Ship.CITADEL:
                        Player.Storage.Skills.Add(SkillManager.CITADEL_DRAW_FIRE, new DrawFire(Player));
                        break;
                }
            }

            if (updateSlotbar)
                Player.SettingsManager.SendSlotBarCommand();
        }

        public void Tick()
        {
            foreach (var skill in Player.Storage.Skills.Values)
                skill.Tick();
        }

        public void DisableAllSkills()
        {
            foreach (var skill in Player.Storage.Skills.Values)
                skill.Disable();
        }
    }
}
