using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty
{
    class ServerCommands
    {
        public const String PET = "PET";

        public const String PET_INIT = "I";

        public const String PET_ACQUIRED = "Q";

        public const String ACTIVATE_HERO_PET = "L";

        public const String ACTIVATE_EXTERNAL_PET = "C";

        public const String TOGGLE_PET_ACTIVATION_SHORTCUT = "T";

        public const String ACTIVATE_PET_GUARD_MODE_SHORTCUT = "GM";

        public const String DEACTIVATE_PET = "D";

        public const String PET_DISTROYED = "Z";

        public const String PET_STOP_LASER_ATTACK = "SL";

        public const String PET_REPAIR_BUTTON = "RB";

        public const String TRACKING = "TR";

        public const String MAP_READY_HANDSHAKE = "HS";

        public const String REPAIR_PET = "R";

        public const String PET_STATUS = "S";

        public const String PET_GEAR_INFO = "G";

        public const String PET_PROTOCOL_INFO = "P";

        public const String PET_ATTRIBUTE_INFO = "A";

        public const String PET_BUFF_INFO = "E";

        public const String FUEL_INFO = "FL";

        public const String TRADE_WINDOW_ACTIVATION = "TW";

        public const String OUT_OF_FUEL = "NOFL";

        public const String PET_IDLE = "IDLE";

        public const String PET_FUEL_AVAILABLE = "FL";

        public const String GEAR_TARGET_LIST = "TL";

        public const String LOCATOR_GEAR_INFO = "LS";

        public const String BLOCK_PET_WINDOW = "W";

        public const String EVASION_PROTOCOL_INFO = "EV";

        public const String ADD_TO_PET = "A";

        public const String REMOVE_FROM_PET = "R";

        public const String SELECT = "SEL";

        public const String PET_RESET = "RESET";

        public const String PET_LEVEL_UP = "LUP";

        public const String SHIP_LOADED = "SHP";

        public const String UI_WINDOW_LOADED = "WND";

        public const String UI_QUICKSLOT_LOADED = "QSL";

        public const String CHAT_LOADED = "CHA";

        public const String LOG_LOADED = "LOG";

        public const String LOG_MESSAGE = "LM";

        public const String LOOT_DISCOUNT = "LD";

        public const String REWARD_DISCOUNT = "RD";

        public const String DISCOUNT_MESSAGE = "DIS";

        public const String BOX_COLLECT_RESPONSE = "y";

        public const String NEWBIE_BOOSTER = "NB";

        public const String BOOSTER_QUEST_REWARD = "QB";

        public const String BOOSTER_BONUS_BOX = "BB";

        public const String BOX_CONTENT_ORE = "CAR";

        public const String BOX_CONTENT_JACKPOT = "JPE";

        public const String BOX_CONTENT_CREDITS = "CRE";

        public const String BOX_CONTENT_URIDIUM = "URI";

        public const String BOX_CONTENT_EXPERIENCE_POINTS = "EP";

        public const String BOX_CONTENT_HONOR_POINTS = "HON";

        public const String BOX_CONTENT_HITPOINTS = "HTP";

        public const String BOX_CONTENT_ROCKETS = "ROK";

        public const String BOX_CONTENT_LASER_BATTERIES = "BAT";

        public const String BOX_CONTENT_BANKING_MULTIPLIKATOR = "BMP";

        public const String BOX_CONTENT_DEDUCTION_HITPOINTS = "DHP";

        public const String BOX_CONTENT_EXTRA_ENERGY = "XEN";

        public const String SET_ITEM_LOOTING_ACTIVE = "SLA";

        public const String SET_ITEM_LOOTING_CANCELLED = "SLC";

        public const String ASSEMBLE_COLLECTION_BEAM_ACTIVE = "SLA";

        public const String ASSEMBLE_COLLECTION_BEAM_CANCELLED = "SLC";

        public const String BOX_CONTENT_MINE = "AMI";

        public const String ITEM_LOOT = "LOT";

        public const String BOX_CONTENT_PET_FUEL = "PFL";

        public const String BOX_CONTENT_JUMP_VOUCHERS = "JV";

        public const String BOX_CONTENT_LEVEL_UP = "NL";

        public const String BOX_CONTENT_FIREWORK = "FW";

        public const String BOX_TOO_BIG = "BTB";

        public const String BOX_ALREADY_COLLECTED = "BAH";

        public const String MINE_EXPLODE = "MIN";

        public const String MINE_ACM = "ACM";

        public const String MINE_EMP = "EMP";

        public const String MINE_SAB = "SAB";

        public const String MINE_DDM = "DDM";

        public const String MINE_SLM = "SLM";

        public const String AVAILABLE_SHIPS_ON_MAP = "SLE";

        public const String YOU_WIN = "JPW";

        public const String LOGFILE = "LOG";

        public const String SET_ATTRIBUTE = "A";

        public const String SERVER_MSG = "STD";

        public const String TEXTBOX_MSG = "KMS";

        public const String LOCALIZED_SERVER_MSG = "STM";

        public const String EXTRAS_INFO = "ITM";

        public const String SET_FLASH_SETTINGS = "SET";

        public const String SHIELD_INFO = "SHD";

        public const String HITPOINTS_INFO = "HPT";

        public const String ROCKET_COOLDOWN_COMPLETED = "RCD";

        public const String EXPERIENCE_POINTS_UPDATE = "EP";

        public const String CREDITS_UPDATE = "C";

        public const String LEVEL_UPDATE = "LUP";

        public const String ACHIEVEMENT_GAIN = "ACG";

        public const String VELOCITY_UPDATE = "v";

        public const String CARGO_CHANGE = "c";

        public const String AMMUNITION_CAPACITY_CHANGE = "a";

        public const String UPDATE_CONFIGURATION_COUNT = "CC";

        public const String INIT_UPDATE_BOOSTERS = "BS";

        public const String INIT_UPDATE_HIGHSCOREGATE_STATS = "HSGU";

        public const String INIT_UPDATE_SCORE_EVENT_STATS = "SCE";

        public const String KILL_SCORE_EVENT_WINDOW = "KSCE";

        public const String UPDATE_SCORE_EVENT_LIVES_DISPLAY = "SCEL";

        public const String KILL_SCORE_EVENT_LIVES_DISPLAY = "SCEKL";

        public const String UPDATE_HIGHSCOREGATE_SCORE = "HSGS";

        public const String DISPLAY_KILLSTREAK_MESSAGE_AND_SOUND = "KSMSG";

        public const String INIT_UPDATE_PIRATE_HUNT_STATS = "PH";

        public const String INIT_UPDATE_PIRATE_HUNT_CLAN_STATS = "PHC";

        public const String RANKED_HUNT_EVENT_UPDATE = "RHE";

        public const String RANKED_HUNT_EVENT_INFO = "NFO";

        public const String RANKED_HUNT_EVENT_END = "END";

        public const String RANKED_HUNT_EVENT_STATS_CLASS_PLAYER = "P";

        public const String RANKED_HUNT_EVENT_STATS_CLASS_CLAN = "C";

        public const String RANKED_HUNT_EVENT_TARGET_MATCH_CLASS_PLAYER = "P";

        public const String RANKED_HUNT_EVENT_TARGET_MATCH_CLASS_NPC = "N";

        public const String UPDATE_COMMAND_LINE_INTERFACE = "CLI";

        public const String FIREWORKS = "FWX";

        public const String FIREWORK_INSTALLATIONS_LEFT = "INL";

        public const String FIREWORKS_LEFT = "FWL";

        public const String FIREWORKS_IGNITE = "FWI";

        public const String FIREWORKS_IGNITE_GROUP = "FWG";

        public const String WIZ_ROCKET = "WIZ";

        public const String DCR_ROCKET = "DCR";

        public const String REPAIR_SKILL_UPDATE = "RS";

        public const String SHIELD_SKILL_UPDATE = "SHS";

        public const String SET_REPAIR_DATA = "REP";

        public const String SET_GS_IO_LOGGING = "IOLOG";

        public const String SET_DISPLAY_CROSSHAIR = "DCH";

        public const String HEAL = "HL";

        public const String STATS_TYPE_SHIELD = "SHD";

        public const String STATS_TYPE_HITPOINTS = "HPT";

        public const String SERVER_VERSION = "VERSION";

        public const String SET_COOLDOWN = "CLD";

        public const String COOLDOWN_COMPLETED = "CLR";

        public const String MINE_COOLDOWN = "MIN";

        public const String SMARTBOMB_COOLDOWN = "SMB";

        public const String INSTASHIELD_COOLDOWN = "ISH";

        public const String ROCKET_COOLDOWN = "ROK";

        public const String RSB_COOLDOWN = "RSB";

        public const String PLASMA_DISCONNECT_COOLDOWN = "PLA";

        public const String EMP_COOLDOWN = "EMP";

        public const String DRONE_FORMATION_COOLDOWN = "DRF";

        public const String ADVANCED_JUMP_CPU_COOLDOWN = "SJ";

        public const String CPU_INFO = "CPU";

        public const String JUMP_CPU = "J";

        public const String ADVANCED_JUMP_CPU = "JCPU";

        public const String TRADE_DRONE_INFO = "T";

        public const String DRONEREPAIR_CPU_INFO = "D";

        public const String DIPLO_CPU_INFO = "E";

        public const String AIM_CPU_INFO = "A";

        public const String CLOAK_CPU_INFO = "C";

        public const String AUTO_ROCKET_CPU_INFO = "R";

        public const String ROCKETLAUNCHER_AUTO_CPU_INFO = "Y";

        public const String QUESTFM_INFO = "9";

        public const String QUESTFM_UPDATE = "upd";

        public const String QUESTFM_INIT = "ini";

        public const String QUESTFM_PRIVILEGE_QUEST = "p";

        public const String QUESTFM_ACCOMPLISH_QUEST = "a";

        public const String QUESTFM_ABORT_QUEST = "a";

        public const String QUESTFM_CANCEL_QUEST = "c";

        public const String QUESTFM_FAIL_QUEST = "f";

        public const String QUESTFM_SUBSEQUENT_QUEST = "SUBSEQ";

        public const String QUESTFM_HIGHLIGHT_QUEST = "HLT";

        public const String STARTUP_QUESTS = "ach";

        public const String STARTUP_QUEST_SET = "set";

        public const String STARTUP_QUEST_REMOVE = "rm";

        public const String STARTUP_QUEST_END = "end";

        public const String STARTUP_QUEST_BUY = "buy";

        public const String GROUPSYSTEM = "ps";

        public const String GROUPSYSTEM_INIT_UI = "nuscht";

        public const String GROUPSYSTEM_INIT = "init";

        public const String GROUPSYSTEM_INIT_SUB_GROUP = "grp";

        public const String GROUPSYSTEM_INIT_SUB_PLAYER = "plr";

        public const String GROUPSYSTEM_ERROR = "err";

        public const String GROUPSYSTEM_ERROR_CONNECTION = "conn";

        public const String GROUPSYSTEM_INFO_CANDIDATES = "all";

        public const String GROUPSYSTEM_INFO_ME = "me";

        public const String GROUPSYSTEM_INFO_GRP = "grp";

        public const String GROUPSYSTEM_BLOCK_INVITATIONS = "blk";

        public const String GROUPSYSTEM_GROUP_EVENT_MEMBER_LOGOUT = "mlo";

        public const String GROUPSYSTEM_GROUP_EVENT_MEMBER_RETURN = "back";

        public const String GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES = "lp";

        public const String GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_LEAVE = "lv";

        public const String GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_KICK = "kick";

        public const String GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_NONE = "none";

        public const String GROUPSYSTEM_GROUP_EVENT_END = "end";

        public const String GROUPSYSTEM_GROUP_EVENT_NEW_LEADER = "nl";

        public const String GROUPSYSTEM_GROUP_EVENT_INVITATION_BEHAVIOUR_CHANGE = "chib";

        public const String GROUPSYSTEM_GROUP_EVENT_STATS_CHANGE = "sc";

        public const String GROUPSYSTEM_GROUP_EVENT_UPDATE = "upd";

        public const String GROUPSYSTEM_GROUP_EVENT_JUMP = "jump";

        public const String GROUPSYSTEM_GROUP_EVENT_PING = "png";

        public const String GROUPSYSTEM_GROUP_EVENT_KILL = "kill";

        public const String GROUPSYSTEM_GROUP_EVENT_ERROR = "err";

        public const String GROUPSYSTEM_GROUP_EVENT_ERROR_SUB_ATTACK = "a";

        public const String GROUPSYSTEM_GROUP_EVENT_ERROR_SUB_FOLLOW = "f";

        public const String GROUPSYSTEM_GROUP_EVENT_ERROR_SUB_PING = "png";

        public const String GROUPSYSTEM_GROUP_INVITE = "inv";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_BY_ID = "new";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_BY_NAME = "name";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_REJECT = "rjc";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_REVOKE = "rji";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_DELETE = "del";

        public const String GROUPSYSTEM_GROUP_INVITATION_DELETE_REVOKE = "rv";

        public const String GROUPSYSTEM_GROUP_INVITATION_DELETE_REJECT = "rj";

        public const String GROUPSYSTEM_GROUP_INVITATION_DELETE_NONE = "none";

        public const String GROUPSYSTEM_GROUP_INVITATION_DELETE_TIMEOUT = "to";

        public const String GROUPSYSTEM_GROUP_INVITATION_DELETE_ACCEPT = "ack";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ACKNOWLEDGE = "ack";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_BOSS_YES = "byes";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_BOSS_NO = "bno";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR = "err";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_BLOCKED = "blk";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_SPAM = "spam";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NON_EXISTANT = "cnx";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NOT_AVAILABLE = "cna";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_BOSS_ONLY = "boss";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_DUPLICATE = "dpl";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_IN_GROUP = "cig";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_GROUP_FULL = "full";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_INVITER_NONEXISTENT = "inx";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_NO_INVITATION = "noi";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_MAX_INVITATIONS_INVITER = "mxi";

        public const String GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_MAX_INVITATIONS_CANDIDATE = "mxc";

        public const String QUEST_INFO = "Q";

        public const String QUEST_DONE = "DONE";

        public const String QUEST_CANCEL = "CANCEL";

        public const String QUEST_STATUS = "STA";

        public const String LASER_ATTACK = "a";

        public const String ROCKET_ATTACK = "v";

        public const String OUT_OF_RANGE = "O";

        public const String ESCAPE = "V";

        public const String IN_NO_ATTACK_ZONE = "P";

        public const String NO_AMMUNITION = "W";

        public const String AUTO_AMMUNITION_CHANGE = "4";

        public const String TARGET_IN_RANGE = "X";

        public const String ATTACKED_SHIP_INFO = "H";

        public const String ATTACK_INFO = "Y";

        public const String SHOOT_MISSED_A = "M";

        public const String SHOOT_MISSED_T = "Z";

        public const String ATTACK_STOPPED_A = "F";

        public const String ATTACK_STOPPED_T = "J";

        public const String NEW_MAP = "m";

        public const String CREATE_STATION = "s";

        public const String CREATE_PORTAL = "p";

        public const String CREATE_ORE = "r";

        public const String ORE_COLLECTED_BY_HERO = "e";

        public const String CARGO_FULL = "f";

        public const String BOX_DISABLED = "h";

        public const String REMOVE_ORE = "q";

        public const String CREATE_BOX = "c";

        public const String CREATE_MINE = "L";

        public const String SET_MAP_PVP_STATUS = "SMP";

        public const String CHANGE_HEALTH_STATION_STATUS = "CSS";

        public const String NEW_ASSET = "CRE";

        public const String ASSET_INFO = "NFO";

        public const String ASSET_HIT = "HIT";

        public const String REMOVE_ASSET = "REM";

        public const String POI = "POI";

        public const String POI_CREATE = "CRE";

        public const String POI_READY = "RDY";

        public const String POI_REMOVE = "REM";

        public const String POI_ENTER = "ENT";

        public const String POI_LEAVE = "LEA";

        public const String POI_DAMAGE = "DMG";

        public const String POI_RDY = "POIRDY";

        public const String MAP_EVENT = "n";

        public const String DISPLAY_MESSAGE = "MSG";

        public const String ORE_COLLECTED = "RCO";

        public const String BOX_COLLECTED = "BCO";

        public const String TARGET_FADE_TO_GRAY = "LSH";

        public const String TARGET_FADE_TO_GRAY_ABORT = "USH";

        public const String TARGET_INVISIBLE = "INV";

        public const String SET_DRONES = "d";

        public const String SET_PORTAL = "p";

        public const String SET_PORTAL_REMOVE = "REM";

        public const String SET_PORTAL_REMOVE_ALL = "ALL";

        public const String SET_DRONE_DISPLAY = "e";

        public const String ENEMY_WARNING = "w";

        public const String SPAWN_ENEMIES = "s";

        public const String SET_TITLE = "t";

        public const String REMOVE_TITLE = "trm";

        public const String SET_PERMANENT_TITLE = "pt";

        public const String MULTIPLIER_FOUND = "MDL";

        public const String SMARTBOMB = "SMB";

        public const String INSTASHIELD = "ISH";

        public const String EMP = "EMP";

        public const String BOOSTER_FOUND = "fbo";

        public const String MALUS = "MAL";

        public const String SET_PLAYER_ATTACKABLE = "pvp";

        public const String PLAY_SPECIAL_EXPLOSION = "BOOOM";

        public const String SAB_SHOT = "SAB_SHOT";

        public const String SPAWN = "Spawn";

        public const String DESPAWN = "Despawn";

        public const String HEAL_RAY = "HEAL_RAY";

        public const String INDEPENDENCE_DAY_MODE = "ID4";

        public const String MALUS_SET = "SET";

        public const String MALUS_REMOVE = "REM";

        public const String INIT_SCOREBOARD = "ssi";

        public const String SET_SCORE = "ssc";

        public const String SET_SPEED = "sss";

        public const String INIT_INVASION_SCOREBOARD = "isi";

        public const String SET_INVASION_SCORE = "isc";

        public const String SET_INVASION_WAVE = "isw";

        public const String CTB = "ctb";

        public const String CTB_INIT_SCOREBOARD = "m";

        public const String CTB_UPDATE_BEACON_POSITION = "p";

        public const String CTB_UPDATE_SCOREBOARD = "s";

        public const String CTB_SET_HOMEZONES = "z";

        public const String CTB_ATTACH_BEACON_TO_USER = "c";

        public const String CTB_REMOVE_BEACON_FROM_USER = "r";

        public const String INIT_SCORE = "sgi";

        public const String SET_SPECIFIC_SCORE = "sgs";

        public const String SET_MULTI_SCORE = "sgm";

        public const String SET_DISCIPLINE = "sgd";

        public const String TEAM_DEATHMATCH = "tdm";

        public const String DRAFT = "drf";

        public const String GAMES_COUNT = "gms";

        public const String MESSAGE = "msg";

        public const String GATE_MAPS_MESSAGE = "quu";

        public const String TDM_EVENT = "evt";

        public const String TDM_INTRO_PHASE = "dmz";

        public const String TDM_KICK_OFF = "go!";

        public const String TDM_STATS_INIT = "nfo";

        public const String TDM_MATCH_RESULT = "fnl";

        public const String TECHS_UPDATE = "TX";

        public const String TECHS_ACTIVATE = "A";

        public const String TECHS_DEACTIVATE = "D";

        public const String SKILL_DESIGNS = "SD";

        public const String REMOVE_SKILL_FX = "R";

        public const String SKILLS_ACTIVATE = "A";

        public const String SKILLS_DEACTIVATE = "D";

        public const String SKILL_SOLACE = "IH";

        public const String SKILL_DIMINISHER = "WS";

        public const String SKILL_SPECTRUM = "PS";

        public const String SKILL_SENTINEL = "FOR";

        public const String SKILL_VENOM = "SIN";

        public const String SPEED_BUFF = "SPEED_BUFF";

        public const String SPEED_BUFF_COOL_DOWN = "SB";

        public const String HEALING_BEAM = "HPA";

        public const String SHIELD_RECHARGE = "SHR";

        public const String HEALING_POD = "HPD";

        public const String DRAW_FIRE = "DFA";

        public const String TRAVEL_MODE = "TM";

        public const String FORTIFY = "FRT";

        public const String PROTECTION = "PRT";

        public const String ULTIMATE_CLOAKING = "UCLK";

        public const String ULTIMATE_EMP = "UEMP";

        public const String MARK_TARGET = "MTG";

        public const String DOUBLE_MINIMAP = "DMM";

        public const String GRAPHIC_FX = "fx";

        public const String GRAPHIC_FX_START = "start";

        public const String GRAPHIC_FX_END = "end";

        public const String GRAPHIC_FX_RAGE = "RAGE";

        public const String GRAPHIC_FX_SABOTEUR_DEBUFF = "SABOTEUR_DEBUFF";

        public const String GRAPHIC_FX_SKULL = "SKULL";

        public const String GRAPHIC_FX_INVINCIBILITY = "INVINCIBILITY";

        public const String GRAPHIC_FX_KAMIKAZE = "KAM";

        public const String RESPAWN_PROTECTION = "RESPAWN_PROTECTION";

        public const String HERO_INIT = "I";

        public const String SHIP_SELECTED = "N";

        public const String CREATE_SHIP = "C";

        public const String REMOVE_SHIP = "R";

        public const String SHIP_MOVEMENT = "1";

        public const String HERO_MOVEMENT = "T";

        public const String BEACON = "D";

        public const String PRIMARY_WEAPON_INFO = "B";

        public const String SECONDARY_WEAPON_INFO = "3";

        public const String DESTROY_SHIP = "K";

        public const String PLAY_PORTAL_ANIMATION = "U";

        public const String PORTAL_JUMP = "i";

        public const String ERROR = "ERR";

        public const short LOGIN_FAILED = 1;

        public const short NOT_LOGGED_IN = 2;

        public const short DOUBLE_LOGGED_IN = 3;

        public const short INVALID_SESSION = 4;

        public const short TWICE_LOGGED = 41;

        public const short LOGIN_CHECK_FAILED = 42;

        public const String LOGOUT = "l";

        public const String LOGOUT_CANCEL_FROM_SERVER = "t";

        public const String GET_ORE_PRICES = "b";

        public const String SET_PRICES = "g";

        public const String SET_AMMO_PRICES = "a";

        public const String SET_ORE_PRICES = "r";

        public const String SET_ORE_COUNT = "E";

        public const String SELL_ORE = "T";

        public const String EXCHANGE_PALLADIUM = "XCP";

        public const String REMOVE_BOX = "2";

        public const String CHANGE_MAP = "z";

        public const String GRACEFUL_KILL = "GKL";

        public const String KICKED = "KIK";

        public const String PING = "PNG";

        public const String REQUEST_SHIP = "i";

        public const String CLIENT_SETTING = "7";

        public const String CLIENT_RESOLUTION = "CLIENT_RESOLUTION";

        public const String SET_RESOLUTION = "SET_RESOLUTION";

        public const String SET_QUICKBAR_SLOT = "QUICKBAR_SLOT";

        public const String SET_SLOTMENU_ORDER = "SLOTMENU_ORDER";

        public const String SET_SLOTMENU_POSITION = "SLOTMENU_POSITION";

        public const String SET_MAINMENU_POSITION = "MAINMENU_POSITION";

        public const String WINDOW_SETTINGS = "WINDOW_SETTINGS";

        public const String SET_MINIMAP_SCALE = "MINIMAP_SCALE";

        public const String SET_RESIZABLE_WINDOWS = "RESIZABLE_WINDOWS";

        public const String SET_BAR_STATUS = "BAR_STATUS";

        public const String SET_AUTO_REFINEMENT = "AUTO_REFINEMENT";

        public const String SET_QUICKSLOT_STOP_ATTACK = "QUICKSLOT_STOP_ATTACK";

        public const String SET_SHOW_DRONES = "SHOW_DRONES";

        public const String SET_AUTO_START = "AUTO_START";

        public const String SET_DOUBLECLICK_ATTACK = "DOUBLECLICK_ATTACK";

        public const String SET_AUTO_BUY_BOOTY_KEYS = "AUTO_BUY_BOOTY_KEY";

        public const String SET_SHOW_INSTANT_LOG = "SHOW_INSTANT_LOG";

        public const String SET_AUTO_BOOST = "AUTO_BOOST";

        public const String SET_DISPLAY_HITPOINT_BUBBLES = "DISPLAY_HITPOINT_BUBBLES";

        public const String SET_DISPLAY_PLAYER_NAMES = "DISPLAY_PLAYER_NAMES";

        public const String SET_DISPLAY_ORE = "DISPLAY_ORE";

        public const String SET_DISPLAY_BONUS_BOXES = "DISPLAY_BONUS_BOXES";

        public const String SET_PLAY_SFX = "PLAY_SFX";

        public const String SET_PLAY_MUSIC = "PLAY_MUSIC";

        public const String SET_SELECTED_BATTERY = "SELECTED_BATTERY";

        public const String SET_SELECTED_ROCKET = "SELECTED_ROCKET";

        public const String SET_DISPLAY_NOTIFICATIONS = "DISPLAY_NOTIFICATIONS";

        public const String SET_DISPLAY_CHAT = "DISPLAY_CHAT";

        public const String SET_DISPLAY_FREE_CARGO_BOXES = "DISPLAY_FREE_CARGO_BOXES";

        public const String SET_DISPLAY_NOT_FREE_CARGO_BOXES = "DISPLAY_NOT_FREE_CARGO_BOXES";

        public const String SET_AUTO_AMMO_CHANGE = "AUTO_AMMO_CHANGE";

        public const String SET_DISPLAY_WINDOW_BACKGROUND = "DISPLAY_WINDOW_BACKGROUND";

        public const String SET_ALWAYS_DRAGGABLE_WINDOWS = "ALWAYS_DRAGGABLE_WINDOWS";

        public const String SET_PRELOAD_USER_SHIPS = "PRELOAD_USER_SHIPS";

        public const String SETTING_KEY_SEPERATOR = ",";

        public const String SETTING_PROPERTY_SEPERATOR = ",";

        public const String REMOVE_KEY = "REM";

        public const String SET_QUALITY_PRESETTING = "QUALITY_PRESETTING";

        public const String SET_QUALITY_CUSTOMIZED = "QUALITY_CUSTOMIZED";

        public const String SET_QUALITY_BACKGROUND = "QUALITY_BACKGROUND";

        public const String SET_QUALITY_POIZONE = "QUALITY_POIZONE";

        public const String SET_QUALITY_SHIP = "QUALITY_SHIP";

        public const String SET_QUALITY_ENGINE = "QUALITY_ENGINE";

        public const String SET_QUALITY_COLLECTABLE = "QUALITY_COLLECTABLE";

        public const String SET_QUALITY_ATTACK = "QUALITY_ATTACK";

        public const String SET_QUALITY_EFFECT = "QUALITY_EFFECT";

        public const String SET_QUALITY_EXPLOSION = "QUALITY_EXPLOSION";

        public const String SET_ALLOW_AUTO_QUALITY = "ALLOW_AUTO_QUALITY";

        public const String SET_HOVER_SHIPS = "HOVER_SHIPS";

        public const String SET_PREMIUM_QUICKSLOT_VISIBILITY = "PREMIUM_QUICKSLOT_VISIBILITY";

        public const String BUY = "5";

        public const String BUY_LASER = "b";

        public const String BUY_ROCKET = "r";

        public const String BUY_SUCCESS = "o";

        public const String BUY_FAILED = "f";

        public const String BUY_FAILED_NO_MONEY = "F";

        public const String BUY_FAILED_NO_CARGO = "S";

        public const String JUMP_FAILED = "k";

        public const String LAB = "LAB";

        public const String UPDATE = "UPD";

        public const String GET = "GET";

        public const String INFO = "INFO";

        public const String SET = "SET";

        public const String REFINEMENT = "REF";

        public const String PRODUCE = "PROD";

        public const String USER_INTERFACE = "UI";

        public const String MINIMAP = "MM";

        public const String NOISE = "NOISE";

        public const String SHOW_MARKER = "SM";

        public const String HIDE_MARKER = "HM";

        public const String EMP_MALUS_BOLT = "EMAL";

        public const String ADVERTISING_BANNER = "AD";

        public const String CREATE_WINDOW = "CW";

        public const String DESTROY_WINDOW = "DW";

        public const String VIDEO_WINDOW = "VID";

        public const String CREATE_VIDEO_WINDOW = "CW";

        public const String DESTROY_VIDEO_WINDOW = "DW";

        public const String NEXT_PAGE = "NP";

        public const String WINDOW_DESTROYED = "WD";

        public const String ARROW = "AR";

        public const String SHOW_ARROW = "SA";

        public const String HIDE_ARROW = "HA";

        public const String WINDOW = "W";

        public const String BUTTON = "B";

        public const String SHOW_WINDOW = "SW";

        public const String HIDE_WINDOW = "HW";

        public const String SHOW_BUTTON = "SB";

        public const String HIDE_BUTTON = "HB";

        public const String SHOW_FLASH = "SF";

        public const String HIDE_FLASH = "HF";

        public const String MINIMIZE_WINDOW = "MIW";

        public const String MAXIMIZE_WINDOW = "MAW";

        public const String SET_MENU_VISIBILITY = "MV";

        public const String SHOW_MENU = "SM";

        public const String HIDE_MENU = "HM";

        public const String SET_MENUBUTTON_ACCESS = "MBA";

        public const String MENUBUTTON_ENABLED = "EB";

        public const String MENUBUTTON_DISABLED = "DB";

        public const String TECHS = "TX";

        public const String CHAIN_BOLT = "ECI";

        public const String TECH_BATTLE_REP_BOT = "BRB";

        public const String SET_STATUS = "S";

        public const String TECH_ENERGY_LEECH = "ELA";

        public const String TECH_SHIELD_BACK_UP = "SBU";

        public const String TECH_ELECTRIC_CHAIN_IMPULSE = "ECI";

        public const String TECH_ROCKET_PROBABILITY_MAXIMIZER = "RPM";

        public const String ROCKETLAUNCHER = "RL";

        public const String ROCKETLAUNCHER_ATTACK = "A";

        public const String ROCKETLAUNCHER_ATTACK_LOWER = "a";

        public const String ROCKETLAUNCHER_STATUS = "S";

        public const String ROCKETLAUNCHER_STATUS_LOWER = "s";

        public const String SET_ROCKETLAUNCHER_ROCKETS = "R";

        public const String SPECIAL_ENEMY = "SE";

        public const String ALIENMOTHERSHIP = "AMS";

        public const String CREATE = "C";

        public const String ROTATE = "R";

        public const String PREPARE_ATTACK = "PA";

        public const String PREPARE_BIG_ATTACK = "PBA";

        public const String CLOAK = "CL";

        public const String IDLE = "I";

        public const String MOVE = "M";

        public const String KILL = "K";

        public const String SET_SPECIAL_OFFERS_NEEDED = "SON";

        public const String ADVANCED_JUMP_CPU_INIT = "I";

        public const String ADVANCED_JUMP_CPU_SELECTED_MAP_FEEDBACK = "C";

        public const String JUMP_VOUCHERS_UPDATE = "JV";

        public const String SET_MARKER = "MARK";

        public const String REMOVE_MARKERS = "NOMARK";

        public const String BOOTY_KEYS_UPDATE = "BK";

        public const String BOOTY_KEYS_BLUE_UPDATE = "BKB";

        public const String BOOTY_KEYS_RED_UPDATE = "BKR";

        public const String CAPTCHA = "CPTCH";

        public const String CAPTCHA_INIT = "I";

        public const String CAPTCHA_SUCCESS = "S";

        public const String CAPTCHA_FAIL = "F";

        public const String CAPTCHA_REFRESH = "R";
    }
}
