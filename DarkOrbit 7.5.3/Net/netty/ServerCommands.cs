using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty
{
    class ServerCommands
    {
        public const string PET = "PET";

        public const string PET_INIT = "I";

        public const string PET_ACQUIRED = "Q";

        public const string ACTIVATE_HERO_PET = "L";

        public const string ACTIVATE_EXTERNAL_PET = "C";

        public const string TOGGLE_PET_ACTIVATION_SHORTCUT = "T";

        public const string ACTIVATE_PET_GUARD_MODE_SHORTCUT = "GM";

        public const string DEACTIVATE_PET = "D";

        public const string PET_DISTROYED = "Z";

        public const string PET_STOP_LASER_ATTACK = "SL";

        public const string PET_REPAIR_BUTTON = "RB";

        public const string TRACKING = "TR";

        public const string MAP_READY_HANDSHAKE = "HS";

        public const string REPAIR_PET = "R";

        public const string PET_STATUS = "S";

        public const string PET_GEAR_INFO = "G";

        public const string PET_PROTOCOL_INFO = "P";

        public const string PET_ATTRIBUTE_INFO = "A";

        public const string PET_BUFF_INFO = "E";

        public const string FUEL_INFO = "FL";

        public const string TRADE_WINDOW_ACTIVATION = "TW";

        public const string OUT_OF_FUEL = "NOFL";

        public const string PET_IDLE = "IDLE";

        public const string PET_FUEL_AVAILABLE = "FL";

        public const string GEAR_TARGET_LIST = "TL";

        public const string LOCATOR_GEAR_INFO = "LS";

        public const string BLOCK_PET_WINDOW = "W";

        public const string EVASION_PROTOCOL_INFO = "EV";

        public const string ADD_TO_PET = "A";

        public const string REMOVE_FROM_PET = "R";

        public const string SELECT = "SEL";

        public const string PET_RESET = "RESET";

        public const string PET_LEVEL_UP = "LUP";

        public const string SHIP_LOADED = "SHP";

        public const string UI_WINDOW_LOADED = "WND";

        public const string UI_QUICKSLOT_LOADED = "QSL";

        public const string CHAT_LOADED = "CHA";

        public const string LOG_LOADED = "LOG";

        public const string LOG_MESSAGE = "LM";

        public const string LOOT_DISCOUNT = "LD";

        public const string REWARD_DISCOUNT = "RD";

        public const string DISCOUNT_MESSAGE = "DIS";

        public const string BOX_COLLECT_RESPONSE = "y";

        public const string NEWBIE_BOOSTER = "NB";

        public const string BOOSTER_QUEST_REWARD = "QB";

        public const string BOOSTER_BONUS_BOX = "BB";

        public const string BOX_CONTENT_ORE = "CAR";

        public const string BOX_CONTENT_JACKPOT = "JPE";

        public const string BOX_CONTENT_CREDITS = "CRE";

        public const string BOX_CONTENT_URIDIUM = "URI";

        public const string BOX_CONTENT_EXPERIENCE_POINTS = "EP";

        public const string BOX_CONTENT_HONOR_POINTS = "HON";

        public const string BOX_CONTENT_HITPOINTS = "HTP";

        public const string BOX_CONTENT_ROCKETS = "ROK";

        public const string BOX_CONTENT_LASER_BATTERIES = "BAT";

        public const string BOX_CONTENT_BANKING_MULTIPLIKATOR = "BMP";

        public const string BOX_CONTENT_DEDUCTION_HITPOINTS = "DHP";

        public const string BOX_CONTENT_EXTRA_ENERGY = "XEN";

        public const string SET_ITEM_LOOTING_ACTIVE = "SLA";

        public const string SET_ITEM_LOOTING_CANCELLED = "SLC";

        public const string ASSEMBLE_COLLECTION_BEAM_ACTIVE = "SLA";

        public const string ASSEMBLE_COLLECTION_BEAM_CANCELLED = "SLC";

        public const string BOX_CONTENT_MINE = "AMI";

        public const string ITEM_LOOT = "LOT";

        public const string BOX_CONTENT_PET_FUEL = "PFL";

        public const string BOX_CONTENT_JUMP_VOUCHERS = "JV";

        public const string BOX_CONTENT_LEVEL_UP = "NL";

        public const string BOX_CONTENT_FIREWORK = "FW";

        public const string BOX_TOO_BIG = "BTB";

        public const string BOX_ALREADY_COLLECTED = "BAH";

        public const string MINE_EXPLODE = "MIN";

        public const string MINE_ACM = "ACM";

        public const string MINE_EMP = "EMP";

        public const string MINE_SAB = "SAB";

        public const string MINE_DDM = "DDM";

        public const string MINE_SLM = "SLM";

        public const string AVAILABLE_SHIPS_ON_MAP = "SLE";

        public const string YOU_WIN = "JPW";

        public const string LOGFILE = "LOG";

        public const string SET_ATTRIBUTE = "A";

        public const string SERVER_MSG = "STD";

        public const string TEXTBOX_MSG = "KMS";

        public const string LOCALIZED_SERVER_MSG = "STM";

        public const string EXTRAS_INFO = "ITM";

        public const string SET_FLASH_SETTINGS = "SET";

        public const string SHIELD_INFO = "SHD";

        public const string HITPOINTS_INFO = "HPT";

        public const string ROCKET_COOLDOWN_COMPLETED = "RCD";

        public const string EXPERIENCE_POINTS_UPDATE = "EP";

        public const string CREDITS_UPDATE = "C";

        public const string LEVEL_UPDATE = "LUP";

        public const string ACHIEVEMENT_GAIN = "ACG";

        public const string VELOCITY_UPDATE = "v";

        public const string CARGO_CHANGE = "c";

        public const string AMMUNITION_CAPACITY_CHANGE = "a";

        public const string UPDATE_CONFIGURATION_COUNT = "CC";

        public const string INIT_UPDATE_BOOSTERS = "BS";

        public const string INIT_UPDATE_HIGHSCOREGATE_STATS = "HSGU";

        public const string INIT_UPDATE_SCORE_EVENT_STATS = "SCE";

        public const string KILL_SCORE_EVENT_WINDOW = "KSCE";

        public const string UPDATE_SCORE_EVENT_LIVES_DISPLAY = "SCEL";

        public const string KILL_SCORE_EVENT_LIVES_DISPLAY = "SCEKL";

        public const string UPDATE_HIGHSCOREGATE_SCORE = "HSGS";

        public const string DISPLAY_KILLSTREAK_MESSAGE_AND_SOUND = "KSMSG";

        public const string INIT_UPDATE_PIRATE_HUNT_STATS = "PH";

        public const string INIT_UPDATE_PIRATE_HUNT_CLAN_STATS = "PHC";

        public const string RANKED_HUNT_EVENT_UPDATE = "RHE";

        public const string RANKED_HUNT_EVENT_INFO = "NFO";

        public const string RANKED_HUNT_EVENT_END = "END";

        public const string RANKED_HUNT_EVENT_STATS_CLASS_PLAYER = "P";

        public const string RANKED_HUNT_EVENT_STATS_CLASS_CLAN = "C";

        public const string RANKED_HUNT_EVENT_TARGET_MATCH_CLASS_PLAYER = "P";

        public const string RANKED_HUNT_EVENT_TARGET_MATCH_CLASS_NPC = "N";

        public const string UPDATE_COMMAND_LINE_INTERFACE = "CLI";

        public const string FIREWORKS = "FWX";

        public const string FIREWORK_INSTALLATIONS_LEFT = "INL";

        public const string FIREWORKS_LEFT = "FWL";

        public const string FIREWORKS_IGNITE = "FWI";

        public const string FIREWORKS_IGNITE_GROUP = "FWG";

        public const string WIZ_ROCKET = "WIZ";

        public const string DCR_ROCKET = "DCR";

        public const string REPAIR_SKILL_UPDATE = "RS";

        public const string SHIELD_SKILL_UPDATE = "SHS";

        public const string SET_REPAIR_DATA = "REP";

        public const string SET_GS_IO_LOGGING = "IOLOG";

        public const string SET_DISPLAY_CROSSHAIR = "DCH";

        public const string HEAL = "HL";

        public const string STATS_TYPE_SHIELD = "SHD";

        public const string STATS_TYPE_HITPOINTS = "HPT";

        public const string SERVER_VERSION = "VERSION";

        public const string SET_COOLDOWN = "CLD";

        public const string COOLDOWN_COMPLETED = "CLR";

        public const string MINE_COOLDOWN = "MIN";

        public const string SMARTBOMB_COOLDOWN = "SMB";

        public const string INSTASHIELD_COOLDOWN = "ISH";

        public const string ROCKET_COOLDOWN = "ROK";

        public const string RSB_COOLDOWN = "RSB";

        public const string PLASMA_DISCONNECT_COOLDOWN = "PLA";

        public const string EMP_COOLDOWN = "EMP";

        public const string DRONE_FORMATION_COOLDOWN = "DRF";

        public const string ADVANCED_JUMP_CPU_COOLDOWN = "SJ";

        public const string CPU_INFO = "CPU";

        public const string JUMP_CPU = "J";

        public const string ADVANCED_JUMP_CPU = "JCPU";

        public const string TRADE_DRONE_INFO = "T";

        public const string DRONEREPAIR_CPU_INFO = "D";

        public const string DIPLO_CPU_INFO = "E";

        public const string AIM_CPU_INFO = "A";

        public const string CLOAK_CPU_INFO = "C";

        public const string AUTO_ROCKET_CPU_INFO = "R";

        public const string ROCKETLAUNCHER_AUTO_CPU_INFO = "Y";

        public const string QUESTFM_INFO = "9";

        public const string QUESTFM_UPDATE = "upd";

        public const string QUESTFM_INIT = "ini";

        public const string QUESTFM_PRIVILEGE_QUEST = "p";

        public const string QUESTFM_ACCOMPLISH_QUEST = "a";

        public const string QUESTFM_ABORT_QUEST = "a";

        public const string QUESTFM_CANCEL_QUEST = "c";

        public const string QUESTFM_FAIL_QUEST = "f";

        public const string QUESTFM_SUBSEQUENT_QUEST = "SUBSEQ";

        public const string QUESTFM_HIGHLIGHT_QUEST = "HLT";

        public const string STARTUP_QUESTS = "ach";

        public const string STARTUP_QUEST_SET = "set";

        public const string STARTUP_QUEST_REMOVE = "rm";

        public const string STARTUP_QUEST_END = "end";

        public const string STARTUP_QUEST_BUY = "buy";

        public const string GROUPSYSTEM = "ps";

        public const string GROUPSYSTEM_INIT_UI = "nüscht";

        public const string GROUPSYSTEM_INIT = "init";

        public const string GROUPSYSTEM_INIT_SUB_GROUP = "grp";

        public const string GROUPSYSTEM_INIT_SUB_PLAYER = "plr";

        public const string GROUPSYSTEM_ERROR = "err";

        public const string GROUPSYSTEM_ERROR_CONNECTION = "conn";

        public const string GROUPSYSTEM_INFO_CANDIDATES = "all";

        public const string GROUPSYSTEM_INFO_ME = "me";

        public const string GROUPSYSTEM_INFO_GRP = "grp";

        public const string GROUPSYSTEM_BLOCK_INVITATIONS = "blk";

        public const string GROUPSYSTEM_GROUP_EVENT_MEMBER_LOGOUT = "mlo";

        public const string GROUPSYSTEM_GROUP_EVENT_MEMBER_RETURN = "back";

        public const string GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES = "lp";

        public const string GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_LEAVE = "lv";

        public const string GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_KICK = "kick";

        public const string GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_NONE = "none";

        public const string GROUPSYSTEM_GROUP_EVENT_END = "end";

        public const string GROUPSYSTEM_GROUP_EVENT_NEW_LEADER = "nl";

        public const string GROUPSYSTEM_GROUP_EVENT_INVITATION_BEHAVIOUR_CHANGE = "chib";

        public const string GROUPSYSTEM_GROUP_EVENT_STATS_CHANGE = "sc";

        public const string GROUPSYSTEM_GROUP_EVENT_UPDATE = "upd";

        public const string GROUPSYSTEM_GROUP_EVENT_JUMP = "jump";

        public const string GROUPSYSTEM_GROUP_EVENT_PING = "png";

        public const string GROUPSYSTEM_GROUP_EVENT_KILL = "kill";

        public const string GROUPSYSTEM_GROUP_EVENT_ERROR = "err";

        public const string GROUPSYSTEM_GROUP_EVENT_ERROR_SUB_ATTACK = "a";

        public const string GROUPSYSTEM_GROUP_EVENT_ERROR_SUB_FOLLOW = "f";

        public const string GROUPSYSTEM_GROUP_EVENT_ERROR_SUB_PING = "png";

        public const string GROUPSYSTEM_GROUP_INVITE = "inv";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_BY_ID = "new";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_BY_NAME = "name";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_REJECT = "rjc";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_REVOKE = "rji";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_DELETE = "del";

        public const string GROUPSYSTEM_GROUP_INVITATION_DELETE_REVOKE = "rv";

        public const string GROUPSYSTEM_GROUP_INVITATION_DELETE_REJECT = "rj";

        public const string GROUPSYSTEM_GROUP_INVITATION_DELETE_NONE = "none";

        public const string GROUPSYSTEM_GROUP_INVITATION_DELETE_TIMEOUT = "to";

        public const string GROUPSYSTEM_GROUP_INVITATION_DELETE_ACCEPT = "ack";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ACKNOWLEDGE = "ack";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_BOSS_YES = "byes";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_BOSS_NO = "bno";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR = "err";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_BLOCKED = "blk";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_SPAM = "spam";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NON_EXISTANT = "cnx";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NOT_AVAILABLE = "cna";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_BOSS_ONLY = "boss";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_DUPLICATE = "dpl";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_IN_GROUP = "cig";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_GROUP_FULL = "full";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_INVITER_NONEXISTENT = "inx";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_NO_INVITATION = "noi";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_MAX_INVITATIONS_INVITER = "mxi";

        public const string GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_MAX_INVITATIONS_CANDIDATE = "mxc";

        public const string QUEST_INFO = "Q";

        public const string QUEST_DONE = "DONE";

        public const string QUEST_CANCEL = "CANCEL";

        public const string QUEST_STATUS = "STA";

        public const string LASER_ATTACK = "a";

        public const string ROCKET_ATTACK = "v";

        public const string OUT_OF_RANGE = "O";

        public const string ESCAPE = "V";

        public const string IN_NO_ATTACK_ZONE = "P";

        public const string NO_AMMUNITION = "W";

        public const string AUTO_AMMUNITION_CHANGE = "4";

        public const string TARGET_IN_RANGE = "X";

        public const string ATTACKED_SHIP_INFO = "H";

        public const string ATTACK_INFO = "Y";

        public const string SHOOT_MISSED_A = "M";

        public const string SHOOT_MISSED_T = "Z";

        public const string ATTACK_STOPPED_A = "F";

        public const string ATTACK_STOPPED_T = "J";

        public const string NEW_MAP = "m";

        public const string CREATE_STATION = "s";

        public const string CREATE_PORTAL = "p";

        public const string CREATE_ORE = "r";

        public const string ORE_COLLECTED_BY_HERO = "e";

        public const string CARGO_FULL = "f";

        public const string BOX_DISABLED = "h";

        public const string REMOVE_ORE = "q";

        public const string CREATE_BOX = "c";

        public const string CREATE_MINE = "L";

        public const string SET_MAP_PVP_STATUS = "SMP";

        public const string CHANGE_HEALTH_STATION_STATUS = "CSS";

        public const string NEW_ASSET = "CRE";

        public const string ASSET_INFO = "NFO";

        public const string ASSET_HIT = "HIT";

        public const string REMOVE_ASSET = "REM";

        public const string POI = "POI";

        public const string POI_CREATE = "CRE";

        public const string POI_READY = "RDY";

        public const string POI_REMOVE = "REM";

        public const string POI_ENTER = "ENT";

        public const string POI_LEAVE = "LEA";

        public const string POI_DAMAGE = "DMG";

        public const string POI_RDY = "POIRDY";

        public const string MAP_EVENT = "n";

        public const string DISPLAY_MESSAGE = "MSG";

        public const string ORE_COLLECTED = "RCO";

        public const string BOX_COLLECTED = "BCO";

        public const string TARGET_FADE_TO_GRAY = "LSH";

        public const string TARGET_FADE_TO_GRAY_ABORT = "USH";

        public const string TARGET_INVISIBLE = "INV";

        public const string SET_DRONES = "d";

        public const string SET_PORTAL = "p";

        public const string SET_PORTAL_REMOVE = "REM";

        public const string SET_PORTAL_REMOVE_ALL = "ALL";

        public const string SET_DRONE_DISPLAY = "e";

        public const string ENEMY_WARNING = "w";

        public const string SPAWN_ENEMIES = "s";

        public const string SET_TITLE = "t";

        public const string REMOVE_TITLE = "trm";

        public const string SET_PERMANENT_TITLE = "pt";

        public const string MULTIPLIER_FOUND = "MDL";

        public const string SMARTBOMB = "SMB";

        public const string INSTASHIELD = "ISH";

        public const string EMP = "EMP";

        public const string BOOSTER_FOUND = "fbo";

        public const string MALUS = "MAL";

        public const string SET_PLAYER_ATTACKABLE = "pvp";

        public const string PLAY_SPECIAL_EXPLOSION = "BOOOM";

        public const string SAB_SHOT = "SAB_SHOT";

        public const string SPAWN = "Spawn";

        public const string DESPAWN = "Despawn";

        public const string HEAL_RAY = "HEAL_RAY";

        public const string INDEPENDENCE_DAY_MODE = "ID4";

        public const string MALUS_SET = "SET";

        public const string MALUS_REMOVE = "REM";

        public const string INIT_SCOREBOARD = "ssi";

        public const string SET_SCORE = "ssc";

        public const string SET_SPEED = "sss";

        public const string INIT_INVASION_SCOREBOARD = "isi";

        public const string SET_INVASION_SCORE = "isc";

        public const string SET_INVASION_WAVE = "isw";

        public const string CTB = "ctb";

        public const string CTB_INIT_SCOREBOARD = "m";

        public const string CTB_UPDATE_BEACON_POSITION = "p";

        public const string CTB_UPDATE_SCOREBOARD = "s";

        public const string CTB_SET_HOMEZONES = "z";

        public const string CTB_ATTACH_BEACON_TO_USER = "c";

        public const string CTB_REMOVE_BEACON_FROM_USER = "r";

        public const string INIT_SCORE = "sgi";

        public const string SET_SPECIFIC_SCORE = "sgs";

        public const string SET_MULTI_SCORE = "sgm";

        public const string SET_DISCIPLINE = "sgd";

        public const string TEAM_DEATHMATCH = "tdm";

        public const string DRAFT = "drf";

        public const string GAMES_COUNT = "gms";

        public const string MESSAGE = "msg";

        public const string GATE_MAPS_MESSAGE = "quu";

        public const string TDM_EVENT = "evt";

        public const string TDM_INTRO_PHASE = "dmz";

        public const string TDM_KICK_OFF = "go!";

        public const string TDM_STATS_INIT = "nfo";

        public const string TDM_MATCH_RESULT = "fnl";

        public const string TECHS_UPDATE = "TX";

        public const string TECHS_ACTIVATE = "A";

        public const string TECHS_DEACTIVATE = "D";

        public const string SKILL_DESIGNS = "SD";

        public const string REMOVE_SKILL_FX = "R";

        public const string SKILLS_ACTIVATE = "A";

        public const string SKILLS_DEACTIVATE = "D";

        public const string SKILL_SOLACE = "IH";

        public const string SKILL_DIMINISHER = "WS";

        public const string SKILL_SPECTRUM = "PS";

        public const string SKILL_SENTINEL = "FOR";

        public const string SKILL_VENOM = "SIN";

        public const string SPEED_BUFF = "SPEED_BUFF";

        public const string SPEED_BUFF_COOL_DOWN = "SB";

        public const string HEALING_BEAM = "HPA";

        public const string SHIELD_RECHARGE = "SHR";

        public const string HEALING_POD = "HPD";

        public const string DRAW_FIRE = "DFA";

        public const string TRAVEL_MODE = "TM";

        public const string FORTIFY = "FRT";

        public const string PROTECTION = "PRT";

        public const string ULTIMATE_CLOAKING = "UCLK";

        public const string ULTIMATE_EMP = "UEMP";

        public const string MARK_TARGET = "MTG";

        public const string DOUBLE_MINIMAP = "DMM";

        public const string GRAPHIC_FX = "fx";

        public const string GRAPHIC_FX_START = "start";

        public const string GRAPHIC_FX_END = "end";

        public const string GRAPHIC_FX_RAGE = "RAGE";

        public const string GRAPHIC_FX_SABOTEUR_DEBUFF = "SABOTEUR_DEBUFF";

        public const string GRAPHIC_FX_SKULL = "SKULL";

        public const string GRAPHIC_FX_INVINCIBILITY = "INVINCIBILITY";

        public const string GRAPHIC_FX_KAMIKAZE = "KAM";

        public const string RESPAWN_PROTECTION = "RESPAWN_PROTECTION";

        public const string HERO_INIT = "I";

        public const string SHIP_SELECTED = "N";

        public const string CREATE_SHIP = "C";

        public const string REMOVE_SHIP = "R";

        public const string SHIP_MOVEMENT = "1";

        public const string HERO_MOVEMENT = "T";

        public const string BEACON = "D";

        public const string PRIMARY_WEAPON_INFO = "B";

        public const string SECONDARY_WEAPON_INFO = "3";

        public const string DESTROY_SHIP = "K";

        public const string PLAY_PORTAL_ANIMATION = "U";

        public const string PORTAL_JUMP = "i";

        public const string ERROR = "ERR";

        public const short LOGIN_FAILED = 1;
      
        public const short NOT_LOGGED_IN = 2;
      
        public const short DOUBLE_LOGGED_IN = 3;
      
        public const short INVALID_SESSION = 4;
      
        public const short TWICE_LOGGED = 41;
      
        public const short LOGIN_CHECK_FAILED = 42;
      
        public const string LOGOUT = "l";

        public const string LOGOUT_CANCEL_FROM_SERVER = "t";

        public const string GET_ORE_PRICES = "b";

        public const string SET_PRICES = "g";

        public const string SET_AMMO_PRICES = "a";

        public const string SET_ORE_PRICES = "r";

        public const string SET_ORE_COUNT = "E";

        public const string SELL_ORE = "T";

        public const string EXCHANGE_PALLADIUM = "XCP";

        public const string REMOVE_BOX = "2";

        public const string CHANGE_MAP = "z";

        public const string GRACEFUL_KILL = "GKL";

        public const string KICKED = "KIK";

        public const string PING = "PNG";

        public const string REQUEST_SHIP = "i";

        public const string CLIENT_SETTING = "7";

        public const string CLIENT_RESOLUTION = "CLIENT_RESOLUTION";

        public const string SET_RESOLUTION = "SET_RESOLUTION";

        public const string SET_QUICKBAR_SLOT = "QUICKBAR_SLOT";

        public const string SET_SLOTMENU_ORDER = "SLOTMENU_ORDER";

        public const string SET_SLOTMENU_POSITION = "SLOTMENU_POSITION";

        public const string SET_MAINMENU_POSITION = "MAINMENU_POSITION";

        public const string WINDOW_SETTINGS = "WINDOW_SETTINGS";

        public const string SET_MINIMAP_SCALE = "MINIMAP_SCALE";

        public const string SET_RESIZABLE_WINDOWS = "RESIZABLE_WINDOWS";

        public const string SET_BAR_STATUS = "BAR_STATUS";

        public const string SET_AUTO_REFINEMENT = "AUTO_REFINEMENT";

        public const string SET_QUICKSLOT_STOP_ATTACK = "QUICKSLOT_STOP_ATTACK";

        public const string SET_SHOW_DRONES = "SHOW_DRONES";

        public const string SET_AUTO_START = "AUTO_START";

        public const string SET_DOUBLECLICK_ATTACK = "DOUBLECLICK_ATTACK";

        public const string SET_AUTO_BUY_BOOTY_KEYS = "AUTO_BUY_BOOTY_KEY";

        public const string SET_SHOW_INSTANT_LOG = "SHOW_INSTANT_LOG";

        public const string SET_AUTO_BOOST = "AUTO_BOOST";

        public const string SET_DISPLAY_HITPOINT_BUBBLES = "DISPLAY_HITPOINT_BUBBLES";

        public const string SET_DISPLAY_PLAYER_NAMES = "DISPLAY_PLAYER_NAMES";

        public const string SET_DISPLAY_ORE = "DISPLAY_ORE";

        public const string SET_DISPLAY_BONUS_BOXES = "DISPLAY_BONUS_BOXES";

        public const string SET_PLAY_SFX = "PLAY_SFX";

        public const string SET_PLAY_MUSIC = "PLAY_MUSIC";

        public const string SET_SELECTED_BATTERY = "SELECTED_BATTERY";

        public const string SET_SELECTED_ROCKET = "SELECTED_ROCKET";

        public const string SET_DISPLAY_NOTIFICATIONS = "DISPLAY_NOTIFICATIONS";

        public const string SET_DISPLAY_CHAT = "DISPLAY_CHAT";

        public const string SET_DISPLAY_FREE_CARGO_BOXES = "DISPLAY_FREE_CARGO_BOXES";

        public const string SET_DISPLAY_NOT_FREE_CARGO_BOXES = "DISPLAY_NOT_FREE_CARGO_BOXES";

        public const string SET_AUTO_AMMO_CHANGE = "AUTO_AMMO_CHANGE";

        public const string SET_DISPLAY_WINDOW_BACKGROUND = "DISPLAY_WINDOW_BACKGROUND";

        public const string SET_ALWAYS_DRAGGABLE_WINDOWS = "ALWAYS_DRAGGABLE_WINDOWS";

        public const string SET_PRELOAD_USER_SHIPS = "PRELOAD_USER_SHIPS";

        public const string SETTING_KEY_SEPERATOR = ",";

        public const string SETTING_PROPERTY_SEPERATOR = ",";

        public const string REMOVE_KEY = "REM";

        public const string SET_QUALITY_PRESETTING = "QUALITY_PRESETTING";

        public const string SET_QUALITY_CUSTOMIZED = "QUALITY_CUSTOMIZED";

        public const string SET_QUALITY_BACKGROUND = "QUALITY_BACKGROUND";

        public const string SET_QUALITY_POIZONE = "QUALITY_POIZONE";

        public const string SET_QUALITY_SHIP = "QUALITY_SHIP";

        public const string SET_QUALITY_ENGINE = "QUALITY_ENGINE";

        public const string SET_QUALITY_COLLECTABLE = "QUALITY_COLLECTABLE";

        public const string SET_QUALITY_ATTACK = "QUALITY_ATTACK";

        public const string SET_QUALITY_EFFECT = "QUALITY_EFFECT";

        public const string SET_QUALITY_EXPLOSION = "QUALITY_EXPLOSION";

        public const string SET_ALLOW_AUTO_QUALITY = "ALLOW_AUTO_QUALITY";

        public const string SET_HOVER_SHIPS = "HOVER_SHIPS";

        public const string SET_PREMIUM_QUICKSLOT_VISIBILITY = "PREMIUM_QUICKSLOT_VISIBILITY";

        public const string BUY = "5";

        public const string BUY_LASER = "b";

        public const string BUY_ROCKET = "r";

        public const string BUY_SUCCESS = "o";

        public const string BUY_FAILED = "f";

        public const string BUY_FAILED_NO_MONEY = "F";

        public const string BUY_FAILED_NO_CARGO = "S";

        public const string JUMP_FAILED = "k";

        public const string LAB = "LAB";

        public const string UPDATE = "UPD";

        public const string GET = "GET";

        public const string INFO = "INFO";

        public const string SET = "SET";

        public const string REFINEMENT = "REF";

        public const string PRODUCE = "PROD";

        public const string USER_INTERFACE = "UI";

        public const string MINIMAP = "MM";

        public const string NOISE = "NOISE";

        public const string SHOW_MARKER = "SM";

        public const string HIDE_MARKER = "HM";

        public const string EMP_MALUS_BOLT = "EMAL";

        public const string ADVERTISING_BANNER = "AD";

        public const string CREATE_WINDOW = "CW";

        public const string DESTROY_WINDOW = "DW";

        public const string VIDEO_WINDOW = "VID";

        public const string CREATE_VIDEO_WINDOW = "CW";

        public const string DESTROY_VIDEO_WINDOW = "DW";

        public const string NEXT_PAGE = "NP";

        public const string WINDOW_DESTROYED = "WD";

        public const string ARROW = "AR";

        public const string SHOW_ARROW = "SA";

        public const string HIDE_ARROW = "HA";

        public const string WINDOW = "W";

        public const string BUTTON = "B";

        public const string SHOW_WINDOW = "SW";

        public const string HIDE_WINDOW = "HW";

        public const string SHOW_BUTTON = "SB";

        public const string HIDE_BUTTON = "HB";

        public const string SHOW_FLASH = "SF";

        public const string HIDE_FLASH = "HF";

        public const string MINIMIZE_WINDOW = "MIW";

        public const string MAXIMIZE_WINDOW = "MAW";

        public const string SET_MENU_VISIBILITY = "MV";

        public const string SHOW_MENU = "SM";

        public const string HIDE_MENU = "HM";

        public const string SET_MENUBUTTON_ACCESS = "MBA";

        public const string MENUBUTTON_ENABLED = "EB";

        public const string MENUBUTTON_DISABLED = "DB";

        public const string TECHS = "TX";

        public const string CHAIN_BOLT = "ECI";

        public const string TECH_BATTLE_REP_BOT = "BRB";

        public const string SET_STATUS = "S";

        public const string TECH_ENERGY_LEECH = "ELA";

        public const string TECH_SHIELD_BACK_UP = "SBU";

        public const string TECH_ELECTRIC_CHAIN_IMPULSE = "ECI";

        public const string TECH_ROCKET_PROBABILITY_MAXIMIZER = "RPM";

        public const string ROCKETLAUNCHER = "RL";

        public const string ROCKETLAUNCHER_ATTACK = "A";

        public const string ROCKETLAUNCHER_ATTACK_LOWER = "a";

        public const string ROCKETLAUNCHER_STATUS = "S";

        public const string ROCKETLAUNCHER_STATUS_LOWER = "s";

        public const string SET_ROCKETLAUNCHER_ROCKETS = "R";

        public const string SPECIAL_ENEMY = "SE";

        public const string ALIENMOTHERSHIP = "AMS";

        public const string CREATE = "C";

        public const string ROTATE = "R";

        public const string PREPARE_ATTACK = "PA";

        public const string PREPARE_BIG_ATTACK = "PBA";

        public const string CLOAK = "CL";

        public const string IDLE = "I";

        public const string MOVE = "M";

        public const string KILL = "K";

        public const string SET_SPECIAL_OFFERS_NEEDED = "SON";

        public const string ADVANCED_JUMP_CPU_INIT = "I";

        public const string ADVANCED_JUMP_CPU_SELECTED_MAP_FEEDBACK = "C";

        public const string JUMP_VOUCHERS_UPDATE = "JV";

        public const string SET_MARKER = "MARK";

        public const string REMOVE_MARKERS = "NOMARK";

        public const string BOOTY_KEYS_UPDATE = "BK";

        public const string BOOTY_KEYS_BLUE_UPDATE = "BKB";

        public const string BOOTY_KEYS_RED_UPDATE = "BKR";

        public const string CAPTCHA = "CPTCH";

        public const string CAPTCHA_INIT = "I";

        public const string CAPTCHA_SUCCESS = "S";

        public const string CAPTCHA_FAIL = "F";

        public const string CAPTCHA_REFRESH = "R";
    }
}
