using Ow.Game.Objects.Players.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class SkillManager
    {
        public Player Player { get; set; }

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

        public Sentinel Sentinel { get; set; }
        public Solace Solace { get; set; }
        public Diminisher Diminisher { get; set; }
        public Spectrum Spectrum { get; set; }
        public Venom Venom { get; set; }

        public SkillManager(Player player) { Player = player; InitiateSkills(); }

        public void InitiateSkills()
        {
            Sentinel = new Sentinel(Player);
            Solace = new Solace(Player);
            Diminisher = new Diminisher(Player);
            Spectrum = new Spectrum(Player);
            Venom = new Venom(Player);
        }

        public void Tick()
        {
            Sentinel.Tick();
            Diminisher.Tick();
            Spectrum.Tick();
            Venom.Tick();
        }

        public void DisableAllSkills()
        {
            Sentinel.Disable();
            Diminisher.Disable();
            Spectrum.Disable();
            Venom.Disable();
        }

        public void AssembleSkillCategoryRequest(string pTechItem)
        {
            switch (pTechItem)
            {
                case SENTINEL:
                    Sentinel.Send();
                    break;
                case SOLACE:
                    Solace.Send();
                    break;
                case DIMINISHER:
                    Diminisher.Send();
                    break;
                case SPECTRUM:
                    Spectrum.Send();
                    break;
                case VENOM:
                    Venom.Send();
                    break;
            }
        }
    }
}
