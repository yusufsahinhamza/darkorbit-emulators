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

        public const int GOLIATH_BASTION = 59;
        public const int GOLIATH_ENFORCER = 56;
        public const int GOLIATH_CENTAUR = 110;
        public const int GOLIATH_DIMINISHER = 64;
        public const int GOLIATH_EXALTED = 62;
        public const int GOLIATH_GOAL = 88;
        public const int GOLIATH_KICK = 86;
        public const int GOLIATH_PEACEMAKER = 142;
        public const int GOLIATH_REFEREE = 87;
        public const int GOLIATH_SATURN = 109;
        public const int GOLIATH_SENTINEL = 66;
        public const int GOLIATH_SOLACE = 63;
        public const int GOLIATH_SOVEREIGN = 141;
        public const int GOLIATH_SPECTRUM = 65;
        public const int GOLIATH_VANQUISHER = 140;
        public const int GOLIATH_VENOM = 67;
        public const int GOLIATH_VETERAN = 61;
        public const int GOLIATH_HEZARFEN = 155;
        public const int GOLIATH_INDEPENDENCE = 57;
        public const int GOLIATH = 10;
        public const int GOLIATH_RAZER = 153;

        public const int VENGEANCE_AVENGER = 60;
        public const int VENGEANCE_REVENGE = 58;
        public const int VENGEANCE_CORSAIR = 17;
        public const int VENGEANCE_ADEPT = 16;
        public const int VENGEANCE_LIGHTNING = 18;

        public const int AEGIS = 49;

        public string Name { get; set; }
        public int Id { get; set; }
        public int BaseHitpoints { get; set; }
        public string LootId { get; set; }
        public ShipRewards Rewards { get; set; }

        public Ship(string name, int shipID, int baseHitpoints, string lootID, ShipRewards rewards)
        {
            Name = name;
            Id = shipID;
            BaseHitpoints = baseHitpoints;
            LootId = lootID;
            Rewards = rewards;
        }

        public int GetHitPointsBoost(int pHitPoints)
        {
            switch (Id)
            {
                case GOLIATH_CENTAUR:
                    return pHitPoints += Maths.GetPercentage(pHitPoints, 10);
                case GOLIATH_SATURN:
                    return pHitPoints += Maths.GetPercentage(pHitPoints, 20);
                default:
                    return pHitPoints;
            }
        }

        public int GetShieldPointsBoost(int pShieldPoints)
        {
            switch (Id)
            {
                case GOLIATH_SPECTRUM:
                case GOLIATH_SOLACE:
                case GOLIATH_SENTINEL:
                case GOLIATH_KICK:
                case GOLIATH_BASTION:
                case VENGEANCE_AVENGER:
                    return pShieldPoints += Maths.GetPercentage(pShieldPoints, 10);  
                default:
                    return pShieldPoints;
            }
        }

        public int GetLaserDamageBoost(int pDamage, int thisFactionId, int otherFactionId)
        {
            switch (Id)
            {
                case GOLIATH_PEACEMAKER:
                case GOLIATH_VANQUISHER:
                case GOLIATH_SOVEREIGN:
                    if (otherFactionId != 0 && thisFactionId != otherFactionId)
                        return pDamage += Maths.GetPercentage(pDamage, 7);
                    else return pDamage;
                case GOLIATH_DIMINISHER:
                case GOLIATH_ENFORCER:
                case GOLIATH_REFEREE:
                case GOLIATH_VENOM:
                case VENGEANCE_REVENGE:
                case VENGEANCE_LIGHTNING:
                    return pDamage += Maths.GetPercentage(pDamage, 5);
                default:
                    return pDamage;
            }
        }

        public int GetHonorBoost(int pHonor)
        {
            switch (Id)
            {
                case GOLIATH_EXALTED:
                case VENGEANCE_CORSAIR:
                    return pHonor += Maths.GetPercentage(pHonor, 10);
                default:
                    return pHonor;
            }
        }

        public int GetExperienceBoost(int pExperience)
        {
            switch (Id)
            {
                case GOLIATH_VETERAN:
                case GOLIATH_GOAL:
                case VENGEANCE_ADEPT:
                    return pExperience += Maths.GetPercentage(pExperience, 10);
                default:
                    return pExperience;
            }
        }

        public short GroupShipId
        {
            get
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
                    case GOLIATH_DIMINISHER:
                    case GOLIATH_SPECTRUM:
                    case GOLIATH_SENTINEL:
                    case GOLIATH_VENOM:
                        return GroupPlayerShipModule.ENFORCER;
                    case VENGEANCE_LIGHTNING:
                    case VENGEANCE_REVENGE:
                    case VENGEANCE_AVENGER:
                    case VENGEANCE_ADEPT:
                        return GroupPlayerShipModule.REVENGE;
                    default:
                        return GroupPlayerShipModule.DEFAULT;
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
