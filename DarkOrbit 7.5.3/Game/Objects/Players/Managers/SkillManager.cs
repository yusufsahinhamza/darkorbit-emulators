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

        public const int SENTINEL = 4;
        public const int DIMINISHER = 2;
        public const int VENOM = 5;
        public const int SPECTRUM = 3;
        public const int SOLACE = 1;

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

        public void AssembleSkillCategoryRequest(int skillId)
        {
            switch (skillId)
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
