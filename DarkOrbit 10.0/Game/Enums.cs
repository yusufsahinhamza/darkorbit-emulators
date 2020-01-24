using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game
{
    public enum POIDesigns
    {
        NONE = 0,
        ASTEROIDS = 1,
        ASTEROIDS_BLUE = 2,
        ASTEROIDS_MIXED_WITH_SCRAP = 3,
        SCRAP = 4,
        NEBULA = 5,
        SIMPLE = 6,
        SECTOR_CONTROL_HOME_ZONE = 7,
        SECTOR_CONTROL_SECTOR_ZONE = 8,
    }

    public enum NpcAIOption
    {
        DO_NOTHING,
        SEARCH_FOR_ENEMIES,
        FLY_TO_ENEMY,
        WAIT_PLAYER_MOVE,
        ATTACK_ENEMY,
        FLEE_FROM_ENEMY,
        EMP,
        RANDOM_POSITION_MOVE
    }

    public enum DataType
    {
        URIDIUM,
        CREDITS,
        HONOR,
        EXPERIENCE,
        JACKPOT
    }

    public enum ChangeType
    {
        INCREASE,
        DECREASE
    }

    public enum HealType
    {
        HEALTH,
        SHIELD
    }

    public enum POIShapes
    {
        CIRCLE = 0,
        POLYGON = 1,
        RECTANGLE = 2
    }

    public enum POITypes
    {
        GENERIC = 0,
        FACTORIZED = 1,
        TRIGGER = 2,
        DAMAGE = 3,
        HEALING = 4,
        NO_ACCESS = 5,
        FACTION_NO_ACCESS = 6,
        ENTER_LEAVE = 7,
        RADIATION = 8,
        CAGE = 9,
        MINE_FIELD = 10,
        BUFF_ZONE = 11,
        SECTOR_CONTROL_HOME_ZONE = 12,
        SECTOR_CONTROL_SECTOR_ZONE = 13
    }

    public enum BoosterType
    {
        DMG_B01,
        DMG_B02,
        EP_B01,
        EP_B02,
        EP50,
        HON_B01,
        HON_B02,
        HON50,
        HP_B01,
        HP_B02,
        REP_B01,
        REP_B02,
        REP_S01,
        RES_B01,
        RES_B02,
        SHD_B01,
        SHD_B02,
        SREG_B01,
        SREG_B02,
        BB_01,
        QR_01,
        CD_B01,
        CD_B02,
        KAPPA_B01,
        HONM_1,
        XPM_1,
        DMGM_1
    }

    public enum BoostedAttributeType
    {
        EP,
        HONOUR,
        DAMAGE,
        SHIELD,
        REPAIR,
        SHIELDRECHARGE,
        RESOURCE,
        MAXHP,
        ABILITY_COOLDOWN,
        BONUSBOXES,
        QUESTREWARD
    }

    public enum DestructionType
    {
        PLAYER = 0,
        NPC = 1,
        RADIATION = 2,
        MINE = 3,
        MISC = 4,
        BATTLESTATION = 5,
        PET = 6
    }

    public enum Diplomacy
    {
        NONE = 0,
        ALLIED = 1,
        NON_AGGRESSION_PACT = 2,
        AT_WAR = 3
    }

    public enum DamageType
    {
        ROCKET = 0,
        LASER = 1,
        MINE = 2,
        RADIATION = 3,
        PLASMA = 4,
        ECI = 5,
        SL = 6,
        CID = 7,
        SINGULARITY = 8,
        KAMIKAZE = 9,
        REPAIR = 10,
        DECELERATION = 11,
        SHIELD_ABSORBER_ROCKET_CREDITS = 12,
        SHIELD_ABSORBER_ROCKET_URIDIUM = 13,
        STICKY_BOMB = 14
    }

}
