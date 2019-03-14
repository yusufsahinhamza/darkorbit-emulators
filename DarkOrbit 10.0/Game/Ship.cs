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
        public const int CYBORG = 445;
        public const int HAMMERCLAW = 446;
        public const int GOLIATH_SPECTRUM_FROST = 447;
        public const int GOLIATH_SENTINEL_FROST = 448;
        public const int GOLIATH_SENTINEL_LEGEND = 449;
        public const int GOLIATH_SPECTRUM_LEGEND = 450;
        public const int G_CHAMPION_LEGEND = 451;
        public const int CYBORG_LAVA = 452;
        public const int HAMMERCLAW_LAVA = 453;
        public const int SURGEON = 454;
        public const int SURGEON_CICADA = 455;
        public const int SURGEON_LOCUST = 456;
        public const int GOLIATH = 10;
        public const int GOLIATH_RAZER = 153;

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
                case HAMMERCLAW:
                    return pHitPoints += Maths.GetPercentage(pHitPoints, 20);
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
                case GOLIATH_SPECTRUM_FROST:
                case GOLIATH_SPECTRUM:
                case GOLIATH_SOLACE:
                case GOLIATH_SENTINEL_FROST:
                case GOLIATH_SENTINEL:
                case GOLIATH_KICK:
                case GOLIATH_BASTION:
                    return pShieldPoints += Maths.GetPercentage(pShieldPoints, 10);  
                case GOLIATH_SENTINEL_LEGEND:
                case GOLIATH_SPECTRUM_LEGEND:
                    return pShieldPoints += Maths.GetPercentage(pShieldPoints, 15);
                default:
                    return pShieldPoints;
            }
        }

        public int GetLaserDamageBoost(int pDamage)
        {
            switch (Id)
            {
                case GOLIATH_DIMINISHER:
                case GOLIATH_ENFORCER:
                case GOLIATH_PEACEMAKER:
                case GOLIATH_REFEREE:
                case GOLIATH_SOVEREIGN:
                case GOLIATH_VANQUISHER:
                case GOLIATH_VENOM:
                    return pDamage += Maths.GetPercentage(pDamage, 5);
                case SURGEON:
                case SURGEON_CICADA:
                case SURGEON_LOCUST:
                    return pDamage += Maths.GetPercentage(pDamage, 7);
                case GOLIATH_HEZARFEN:
                case GOLIATH_INDEPENDENCE:
                case G_CHAMPION_LEGEND:
                case CYBORG:
                    return pDamage += Maths.GetPercentage(pDamage, 10);
                default:
                    return pDamage;
            }
        }

        public int GetHonorBoost(int pHonor)
        {
            switch (Id)
            {
                case SURGEON:
                case SURGEON_CICADA:
                case SURGEON_LOCUST:
                    return pHonor += Maths.GetPercentage(pHonor, 7);
                case GOLIATH_EXALTED:
                    return pHonor += Maths.GetPercentage(pHonor, 10);
                case GOLIATH_GOAL:
                case GOLIATH_PEACEMAKER:
                case GOLIATH_SOVEREIGN:
                case GOLIATH_VANQUISHER:
                case G_CHAMPION_LEGEND:
                case GOLIATH_HEZARFEN:
                case GOLIATH_INDEPENDENCE:
                case GOLIATH_SPECTRUM_LEGEND:
                case GOLIATH_SENTINEL_LEGEND:
                    return pHonor += Maths.GetPercentage(pHonor, 15);
                default:
                    return pHonor;
            }
        }

        public int GetExperienceBoost(int pExperience)
        {
            switch (Id)
            {
                case SURGEON:
                case SURGEON_CICADA:
                case SURGEON_LOCUST:
                    return pExperience += Maths.GetPercentage(pExperience, 7);
                case GOLIATH_VETERAN:
                    return pExperience += Maths.GetPercentage(pExperience, 10);
                case GOLIATH_GOAL:
                case GOLIATH_HEZARFEN:
                case GOLIATH_INDEPENDENCE:
                case GOLIATH_SPECTRUM_LEGEND:
                case GOLIATH_SENTINEL_LEGEND:
                case GOLIATH_PEACEMAKER:
                case GOLIATH_SOVEREIGN:
                case GOLIATH_VANQUISHER:
                case G_CHAMPION_LEGEND:
                    return pExperience += Maths.GetPercentage(pExperience, 15);
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
                    default:
                        return GroupPlayerShipModule.DEFAULT;
                }
            }
        }

        private static Random random = new Random();
        public static string GetRandomShipLootId(string currentShipLootId)
        {
            var ships = new List<string>();
            foreach (var ship in GameManager.Ships.Values)
                ships.Add(ship.LootId);

            int randomed = random.Next(55, 71);

            if (ships[randomed] == currentShipLootId)
                return GetRandomShipLootId(currentShipLootId);
            else
                return ships[randomed];
        }
    }
}
