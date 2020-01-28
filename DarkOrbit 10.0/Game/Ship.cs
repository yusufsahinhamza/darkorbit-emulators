using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game
{
    class ShipRewards
    {
        public int Experience { get; set; }
        public int Honor { get; set; }
        public int Credits { get; set; }
        public int Uridium { get; set; }
    }

    class Ship
    {
        public static int SPACEBALL_SUMMER = 442;
        public static int SPACEBALL_WINTER = 443;
        public static int SPACEBALL_SOCCER = 444;

        public const int GOLIATH = 10;
        public const int GOLIATH_ENFORCER = 56;
        public const int GOLIATH_BASTION = 59;
        public const int GOLIATH_VETERAN = 61;
        public const int GOLIATH_EXALTED = 62;
        public const int GOLIATH_SOLACE = 63;
        public const int GOLIATH_VENOM = 67;
        public const int GOLIATH_KICK = 86;
        public const int GOLIATH_REFEREE = 87;
        public const int GOLIATH_GOAL = 88;
        public const int GOLIATH_SATURN = 109;
        public const int GOLIATH_CENTAUR = 110;
        public const int GOLIATH_RAZER = 153;

        public const int VENGEANCE_ADEPT = 16;
        public const int VENGEANCE_CORSAIR = 17;
        public const int VENGEANCE_LIGHTNING = 18;
        public const int VENGEANCE_REVENGE = 58;
        public const int VENGEANCE_AVENGER = 60;

        public const int AEGIS = 49;
        public const int AEGIS_VETERAN = 157;
        public const int AEGIS_ELITE = 158;

        public const int SPEARHEAD = 70;
        public const int SPEARHEAD_VETERAN = 161;
        public const int SPEARHEAD_ELITE = 162;

        public const int CITADEL = 69;
        public const int CITADEL_VETERAN = 159;
        public const int CITADEL_ELITE = 160;

        public List<int> G_CHAMPIONS = new List<int>()
        {
            57, 155, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472
        };

        public static List<int> SURGEONS = new List<int>()
        {
            156, 473, 474
        };

        public static List<int> SENTINELS = new List<int>()
        {
            66, 483, 484, 485
        };

        public static List<int> DIMINISHERS = new List<int>()
        {
            64, 493, 494, 495
        };

        public static List<int> SPECTRUMS = new List<int>()
        {
            65, 486, 487, 488, 489, 490, 491, 492
        };

        public static List<int> COMPANY_GOLIATHS = new List<int>()
        {
            140, 141, 142
        };

        public string Name { get; set; }
        public int Id { get; set; }
        public int Damage { get; set; }
        public int BaseHitpoints { get; set; }
        public int BaseShieldPoints { get; set; }
        public int BaseSpeed { get; set; }
        public string LootId { get; set; }
        public bool Aggressive { get; set; }
        public bool Respawnable { get; set; }
        public ShipRewards Rewards { get; set; }

        public Ship(string name, int id, int baseHitpoints, int baseShieldPoints, int speed, string lootID, int damage, bool aggressive, bool respawnable, ShipRewards rewards)
        {
            Name = name;
            Id = id;
            Damage = damage;
            BaseShieldPoints = baseShieldPoints;
            BaseHitpoints = baseHitpoints;
            BaseSpeed = speed;
            LootId = lootID;
            Aggressive = aggressive;
            Respawnable = respawnable;
            Rewards = rewards;
        }

        public int GetHitPointsBoost(int hitpoints)
        {
            switch (Id)
            {
                case GOLIATH_CENTAUR:
                    return hitpoints += Maths.GetPercentage(hitpoints, 10);
                case GOLIATH_SATURN:
                    return hitpoints += Maths.GetPercentage(hitpoints, 20);
                default:
                    return hitpoints;
            }
        }

        public int GetShieldPointsBoost(int shield)
        {
            if (SENTINELS.Contains(Id) || SPECTRUMS.Contains(Id))
                return shield += Maths.GetPercentage(shield, 10);
            else
            {
                switch (Id)
                {
                    case GOLIATH_SOLACE:
                    case GOLIATH_KICK:
                    case GOLIATH_BASTION:
                    case VENGEANCE_AVENGER:
                        return shield += Maths.GetPercentage(shield, 10);
                    default:
                        return shield;
                }
            }
        }

        public int GetLaserDamageBoost(int damage, int thisFactionId, int otherFactionId)
        {
            if (G_CHAMPIONS.Contains(Id) || DIMINISHERS.Contains(Id))
                return damage += Maths.GetPercentage(damage, 5);
            else if (SURGEONS.Contains(Id))
                return damage += Maths.GetPercentage(damage, 6);
            else if (COMPANY_GOLIATHS.Contains(Id))
            {
                if (otherFactionId != 0 && thisFactionId != otherFactionId)
                    return damage += Maths.GetPercentage(damage, 7);
                else return damage;
            }
            else
            {
                switch (Id)
                {
                    case GOLIATH_ENFORCER:
                    case GOLIATH_REFEREE:
                    case GOLIATH_VENOM:
                    case VENGEANCE_REVENGE:
                    case VENGEANCE_LIGHTNING:
                    case AEGIS_ELITE:
                    case SPEARHEAD_ELITE:
                    case CITADEL_ELITE:
                        return damage += Maths.GetPercentage(damage, 5);
                    default:
                        return damage;
                }
            }
        }

        public int GetHonorBoost(int honor)
        {
            if (G_CHAMPIONS.Contains(Id))
                return honor += Maths.GetPercentage(honor, 10);
            else if (SURGEONS.Contains(Id))
                return honor += Maths.GetPercentage(honor, 6);
            else
            {
                switch (Id)
                {
                    case GOLIATH_EXALTED:
                    case VENGEANCE_CORSAIR:
                        return honor += Maths.GetPercentage(honor, 10);
                    case AEGIS_VETERAN:
                    case SPEARHEAD_VETERAN:
                    case CITADEL_VETERAN:
                        return honor += Maths.GetPercentage(honor, 5);
                    default:
                        return honor;
                }
            }
        }

        public int GetExperienceBoost(int experience)
        {
            if (SURGEONS.Contains(Id))
                return experience += Maths.GetPercentage(experience, 6);
            else
            {
                switch (Id)
                {
                    case GOLIATH_VETERAN:
                    case GOLIATH_GOAL:
                    case VENGEANCE_ADEPT:
                        return experience += Maths.GetPercentage(experience, 10);
                    case AEGIS_VETERAN:
                    case SPEARHEAD_VETERAN:
                    case CITADEL_VETERAN:
                        return experience += Maths.GetPercentage(experience, 5);
                    default:
                        return experience;
                }
            }
        }

        public short GroupShipId
        {
            get
            {
                if (SENTINELS.Contains(Id) || SPECTRUMS.Contains(Id) || DIMINISHERS.Contains(Id))
                    return GroupPlayerShipModule.ENFORCER;
                else
                {
                    switch (Id)
                    {
                        case 22:
                            return GroupPlayerShipModule.PET;
                        case GOLIATH:
                        case GOLIATH_ENFORCER:
                        case GOLIATH_BASTION:
                        case GOLIATH_VETERAN:
                        case GOLIATH_EXALTED:
                        case GOLIATH_SOLACE:
                        case GOLIATH_VENOM:
                            return GroupPlayerShipModule.ENFORCER;
                        case VENGEANCE_LIGHTNING:
                        case VENGEANCE_REVENGE:
                        case VENGEANCE_AVENGER:
                        case VENGEANCE_ADEPT:
                        case VENGEANCE_CORSAIR:
                            return GroupPlayerShipModule.REVENGE;
                        default:
                            return GroupPlayerShipModule.DEFAULT;
                    }
                }
            }
        }

        private static Random random = new Random();
        public static int GetRandomShipId(int currentShipId)
        {
            int randomed = random.Next(63, 67);

            if (randomed == currentShipId)
                return GetRandomShipId(currentShipId);
            else
                return randomed;
        }
    }
}
