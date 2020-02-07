using Newtonsoft.Json;
using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects.Mines;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ow.Game.Objects.Players.Managers
{
    public class DestructionsBase
    {
        public int fpd = 0;
        public int dbrz = 0;
    }

    public class DataBase
    {
        public long uridium = 0;
        public long credits = 0;
        public long honor = 0;
        public long experience = 0;
        public long jackpot = 0;
    }

    public class SkillTreeBase
    {
        public int engineering = 0;
        public int shieldEngineering = 0;
        public int detonation1 = 0;
        public int detonation2 = 0;
        public int heatseekingMissiles = 0;
        public int rocketFusion = 0;
        public int cruelty1 = 0;
        public int cruelty2 = 0;
        public int explosives = 0;
        public int luck1 = 0;
        public int luck2 = 0;
    }

    public class ConfigsBase
    {
        public int Config1Hitpoints = 0;
        public int Config1Damage = 0;
        public int Config1Shield = 0;
        public int Config1Speed = 0;
        public int Config2Hitpoints = 0;
        public int Config2Damage = 0;
        public int Config2Shield = 0;
        public int Config2Speed = 0;

        public ConfigsBase(int config1Hitpoints, int config1Damage, int config1Shield, int config1Speed,
            int config2Hitpoints, int config2Damage, int config2Shield, int config2Speed)
        {
            Config1Hitpoints = config1Hitpoints;
            Config1Damage = config1Damage;
            Config1Shield = config1Shield;
            Config1Speed = config1Speed;
            Config2Hitpoints = config2Hitpoints;
            Config2Damage = config2Damage;
            Config2Shield = config2Shield;
            Config2Speed = config2Speed;
        }
    }

    public class ItemsBase
    {
        public int BootyKeys = 0;

        public ItemsBase(int bootyKeys)
        {
            BootyKeys = bootyKeys;
        }
    }

    public class EquipmentBase
    {
        public ConfigsBase Configs = new ConfigsBase(0, 0, 0, 0, 0, 0, 0, 0);
        public ItemsBase Items = new ItemsBase(0);

        public EquipmentBase(ConfigsBase configs, ItemsBase items)
        {
            Configs = configs;
            Items = items;
        }
    }

    public class AudioBase
    {
        public bool notSet = false;
        public bool playCombatMusic = true;
        public int music = 100;
        public int sound = 100;
        public int voice = 100;
    }

    public class QualityBase
    {
        public bool notSet = false;
        public short qualityAttack = 0;
        public short qualityBackground = 3;
        public short qualityPresetting = 3;
        public bool qualityCustomized = true;
        public short qualityPoizone = 3;
        public short qualityShip = 0;
        public short qualityEngine = 0;
        public short qualityExplosion = 0;
        public short qualityCollectable = 0;
        public short qualityEffect = 0;
    }

    public class ClassY2TBase
    {
        public bool questsAvailableFilter = false;
        public bool questsUnavailableFilter = false;
        public bool questsCompletedFilter = false;
        public bool var_1151 = false;
        public bool var_2239 = false;
        public bool questsLevelOrderDescending = false;
    }

    public class DisplayBase
    {
        public bool notSet = false;
        public bool displayPlayerNames = true;
        public bool displayResources = true;
        public bool showPremiumQuickslotBar = true;
        public bool allowAutoQuality = true;
        public bool preloadUserShips = true;
        public bool displayHitpointBubbles = true;
        public bool showNotOwnedItems = true;
        public bool displayChat = true;
        public bool displayWindowsBackground = true;
        public bool displayNotFreeCargoBoxes = true;
        public bool dragWindowsAlways = true;
        public bool displayNotifications = true;
        public bool hoverShips = true;
        public bool displayDrones = true;
        public bool displayBonusBoxes = true;
        public bool displayFreeCargoBoxes = true;
        public bool var12P = true;
        public bool varb3N = false;
        public int displaySetting3DqualityAntialias = 4;
        public int varp3M = 4;
        public int displaySetting3DqualityEffects = 4;
        public int displaySetting3DqualityLights = 3;
        public int displaySetting3DqualityTextures = 3;
        public int var03r = 4;
        public int displaySetting3DsizeTextures = 3;
        public int displaySetting3DtextureFiltering = -1;
        public bool proActionBarEnabled = true;
        public bool proActionBarKeyboardInputEnabled = true;
        public bool proActionBarAutohideEnabled = true;
        public bool proActionBarOpened = false;
    }

    public class GameplayBase
    {
        public bool notSet = false;
        public bool autoRefinement = false;
        public bool quickSlotStopAttack = true;
        public bool autoBoost = false;
        public bool autoBuyBootyKeys = false;
        public bool doubleclickAttackEnabled = true;
        public bool autochangeAmmo = true;
        public bool autoStartEnabled = true;
        public bool varE3N = true;
    }

    public class Window
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool maximixed { get; set; }

        public Window(int x, int y, int width, int height, bool maximixed)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.maximixed = maximixed;
        }
    }

    public class WindowBase
    {
        public bool hideAllWindows = false;
        public int scale = 6;
        public string barState = "24,1|23,1|100,1|25,1|35,0|34,0|39,0|";
        public string gameFeatureBarPosition = "0,0";
        public string gameFeatureBarLayoutType = "0";
        public string genericFeatureBarPosition = "98.3,0";
        public string genericFeatureBarLayoutType = "0";
        public string categoryBarPosition = "50,85";
        public string standartSlotBarPosition = "50,85|0,40";
        public string standartSlotBarLayoutType = "0";
        public string premiumSlotBarPosition = "50,85|0,80";
        public string premiumSlotBarLayoutType = "0";
        public string proActionBarPosition = "50,85|0,120";
        public string proActionBarLayoutType = "0";

        public Dictionary<string, Window> windows = new Dictionary<string, Window>()
        {
            { "user", new Window(30, 30, 212, 88, false) },
            { "ship", new Window(30, 30, 212, 88, false) },
            { "ship_warp", new Window(50, 50, 300, 210, false) },
            { "chat", new Window(10, 10, 300, 150, false) },
            { "group", new Window(50, 50, 196, 200, false) },
            { "minimap", new Window(30, 30, 238, 180, false) },
            { "spacemap", new Window(10, 10, 650, 475, false) },
            { "log", new Window(30, 30, 240, 150, false) },
            { "pet", new Window(50, 50, 260, 130, false) },
            { "spaceball", new Window(10, 10, 170, 70, false) },
            { "booster", new Window(10, 10, 110, 150, false) },
            { "traininggrounds", new Window(10, 10, 320, 320, false) },
            { "settings", new Window(50, 50, 400, 470, false) },
            { "help", new Window(10, 10, 219, 121, false) },
            { "logout", new Window(50, 50, 200, 200, false) }
        };
    }

    public class BoundKeysBase
    {
        public short actionType { get; set; }
        public short charCode { get; set; }
        public int parameter { get; set; }
        public List<int> keyCodes { get; set; }

        public BoundKeysBase(short actionType, short charCode, int parameter, List<int> keyCodes)
        {
            this.actionType = actionType;
            this.charCode = charCode;
            this.parameter = parameter;
            this.keyCodes = keyCodes;
        }
    }

    public class InGameSettingsBase
    {
        public bool petDestroyed = false;
        public bool blockedGroupInvites = false;
        public string selectedLaser = AmmunitionManager.LCB_10;
        public string selectedRocket = AmmunitionManager.R_310;
        public string selectedRocketLauncher = AmmunitionManager.HSTRM_01;
        public string selectedFormation = DroneManager.DEFAULT_FORMATION;
        public int currentConfig = 1;
        public List<string> selectedCpus = new List<string> { CpuManager.AUTO_ROCKET_CPU, CpuManager.AUTO_HELLSTROM_CPU };
    }

    class SettingsBase
    {
        public AudioBase Audio = new AudioBase();
        public QualityBase Quality = new QualityBase();
        public ClassY2TBase ClassY2T = new ClassY2TBase();
        public DisplayBase Display = new DisplayBase();
        public GameplayBase Gameplay = new GameplayBase();
        public WindowBase Window = new WindowBase();
        public InGameSettingsBase InGameSettings = new InGameSettingsBase();

        public Dictionary<string, string> Cooldowns = new Dictionary<string, string>
        {
                { AmmunitionManager.SMB_01, "" },
                { AmmunitionManager.ISH_01, "" },
                { AmmunitionManager.EMP_01, "" },
                { "ammunition_mine", "" },
                { AmmunitionManager.DCR_250, "" },
                { AmmunitionManager.PLD_8, "" },
                { AmmunitionManager.R_IC3, "" },
                { TechManager.TECH_ENERGY_LEECH, "" },
                { TechManager.TECH_CHAIN_IMPULSE, "" },
                { TechManager.TECH_PRECISION_TARGETER, "" },
                { TechManager.TECH_BACKUP_SHIELDS, "" },
                { TechManager.TECH_BATTLE_REPAIR_BOT, "" }
        };

        public List<BoundKeysBase> BoundKeys = new List<BoundKeysBase>
        {
            new BoundKeysBase(7, 0, 0, new List<int>{49}),
            new BoundKeysBase(7, 0, 1, new List<int>{50}),
            new BoundKeysBase(7, 0, 2, new List<int>{51}),
            new BoundKeysBase(7, 0, 3, new List<int>{52}),
            new BoundKeysBase(7, 0, 4, new List<int>{53}),
            new BoundKeysBase(7, 0, 5, new List<int>{54}),
            new BoundKeysBase(7, 0, 6, new List<int>{55}),
            new BoundKeysBase(7, 0, 7, new List<int>{56}),
            new BoundKeysBase(7, 0, 8, new List<int>{57}),
            new BoundKeysBase(7, 0, 9, new List<int>{48}),
            new BoundKeysBase(8, 0, 0, new List<int>{112}),
            new BoundKeysBase(8, 0, 1, new List<int>{113}),
            new BoundKeysBase(8, 0, 2, new List<int>{114}),
            new BoundKeysBase(8, 0, 3, new List<int>{115}),
            new BoundKeysBase(8, 0, 4, new List<int>{116}),
            new BoundKeysBase(8, 0, 5, new List<int>{117}),
            new BoundKeysBase(8, 0, 6, new List<int>{118}),
            new BoundKeysBase(8, 0, 7, new List<int>{119}),
            new BoundKeysBase(8, 0, 8, new List<int>{120}),
            new BoundKeysBase(8, 0, 9, new List<int>{121}),
            new BoundKeysBase(0, 0, 0, new List<int>{74}),
            new BoundKeysBase(1, 0, 0, new List<int>{67}),
            new BoundKeysBase(2, 0, 0, new List<int>{17}),
            new BoundKeysBase(3, 0, 0, new List<int>{32}),
            new BoundKeysBase(4, 0, 0, new List<int>{69}),
            new BoundKeysBase(5, 0, 0, new List<int>{82}),
            new BoundKeysBase(13, 0, 0, new List<int>{68}),
            new BoundKeysBase(6, 0, 0, new List<int>{76}),
            new BoundKeysBase(9, 0, 0, new List<int>{72}),
            new BoundKeysBase(10, 0, 0, new List<int>{70}),
            new BoundKeysBase(11, 0, 0, new List<int>{107}),
            new BoundKeysBase(12, 0, 0, new List<int>{109}),
            new BoundKeysBase(14, 0, 0, new List<int>{13}),
            new BoundKeysBase(15, 0, 0, new List<int>{9}),
            new BoundKeysBase(8, 0, 9, new List<int>{121}),
            new BoundKeysBase(16, 0, 0, new List<int>{16})
        };

        public Dictionary<short, string> SlotBarItems = new Dictionary<short, string>() {
            { 1, AmmunitionManager.UCB_100 }
        };

        public Dictionary<short, string> PremiumSlotBarItems = new Dictionary<short, string>() {
            { 1, DroneManager.DEFAULT_FORMATION }
        };

        public Dictionary<short, string> ProActionBarItems = new Dictionary<short, string>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    class SettingsManager : AbstractManager
    {
        public const String STANDARD_SLOT_BAR = "standardSlotBar";
        public const String PREMIUM_SLOT_BAR  = "premiumSlotBar";
        public const String PRO_ACTION_BAR = "proActionBar";

        public SettingsManager(Player player) : base(player) { SetCurrentItems(); }

        public void SetCurrentItems()
        {
            Player.CurrentConfig = Player.Settings.InGameSettings.currentConfig;

            if (Player.Settings.InGameSettings.selectedCpus.Contains(CpuManager.AUTO_ROCKET_CPU))
                Player.Storage.AutoRocket = true;

            if (Player.Settings.InGameSettings.selectedCpus.Contains(CpuManager.AUTO_HELLSTROM_CPU))
            {
                Player.Storage.AutoRocketLauncher = true;
                Player.AttackManager.RocketLauncher.ReloadingActive = true;
            }

            if (Player.Settings.InGameSettings.selectedCpus.Contains(CpuManager.CLK_XL))
                Player.Invisible = true;
        }

        public static string[] LaserCategory =
        {
                AmmunitionManager.LCB_10, AmmunitionManager.MCB_25, AmmunitionManager.MCB_50,
                AmmunitionManager.UCB_100, AmmunitionManager.SAB_50, AmmunitionManager.CBO_100,
                AmmunitionManager.RSB_75 //"ammunition_laser_job-100"
        };

        public static string[] RocketsCategory =
        {
                "ammunition_rocket_r-310", "ammunition_rocket_plt-2026", "ammunition_rocket_plt-2021",
                "ammunition_rocket_plt-3030", "ammunition_specialammo_dcr-250", "ammunition_specialammo_r-ic3", "ammunition_specialammo_wiz-x",
                "ammunition_specialammo_pld-8"
               // , "ammunition_rocket_bdr-1211"
        };

        public static string[] RocketLauncherCategory =
        {
                CpuManager.ROCKET_LAUNCHER, AmmunitionManager.HSTRM_01,
                //"ammunition_rocketlauncher_ubr-100",
                //"ammunition_rocketlauncher_eco-10", "ammunition_rocketlauncher_sar-01",
                AmmunitionManager.SAR_02
            };

        public static string[] SpecialItemsCategory =
        {
                "ammunition_mine_smb-01", "equipment_extra_cpu_ish-01", "ammunition_specialammo_emp-01"
        };

        public static string[] TechsCategory =
        {
                "tech_backup-shields", "tech_battle-repair-bot", "tech_chain-impulse",
                "tech_energy-leech", "tech_precision-targeter"
        };

        public static string[] MinesCategory =
        {
                "ammunition_mine_acm-01", "ammunition_mine_empm-01", "ammunition_mine_sabm-01",
                "ammunition_mine_ddm-01", "ammunition_mine_slm-01"//, "ammunition_mine_im-01"
        };

        public static string[] CpusCategory =
        {
            "equipment_extra_cpu_cl04k-xl", "equipment_extra_cpu_arol-x", "equipment_extra_cpu_rllb-x"
              /**  "equipment_extra_cpu_aim-01", "equipment_extra_cpu_aim-02", "equipment_extra_cpu_ajp-01",
                "equipment_extra_cpu_alb-x", "equipment_extra_cpu_anti-z1", "equipment_extra_cpu_anti-z1-xl",
                "equipment_extra_cpu_arol-x", "equipment_extra_cpu_cl04k-m", "equipment_extra_cpu_cl04k-xl",
                "equipment_extra_cpu_cl04k-xs", "equipment_extra_cpu_dr-01", "equipment_extra_cpu_dr-02",
                "equipment_extra_cpu_fb-x", "equipment_extra_cpu_jp-01",
                "equipment_extra_cpu_jp-02", "equipment_extra_cpu_min-t01", "equipment_extra_cpu_min-t02",
                "equipment_extra_cpu_nc-agb", "equipment_extra_cpu_nc-awb", "equipment_extra_cpu_nc-awl",
                "equipment_extra_cpu_nc-awr", "equipment_extra_cpu_nc-rrb", "equipment_extra_cpu_rb-x",
                "equipment_extra_cpu_rd-x", "equipment_extra_cpu_rllb-x", "equipment_extra_cpu_rok-t01",
                "equipment_extra_cpu_sle-01", "equipment_extra_cpu_sle-02", "equipment_extra_cpu_sle-03",
                "equipment_extra_cpu_sle-04", "equipment_extra_hmd-07", "equipment_extra_repbot_rep-1",
                "equipment_extra_repbot_rep-2", "equipment_extra_repbot_rep-3", "equipment_extra_repbot_rep-4",
                "equipment_extra_repbot_rep-s", "equipment_weapon_rocketlauncher_hst-1",
                "equipment_weapon_rocketlauncher_hst-2", "equipment_weapon_rocketlauncher_not_present", "equipment_extra_cpu_nc-rrb-x"
               */
        };

        public static string[] BuyCategory =
        {
            /*
                "ammunition_laser_lcb-10", "ammunition_laser_mcb-25", "ammunition_laser_mcb-50",
                "ammunition_laser_sab-50", "ammunition_rocket_r-310", "ammunition_rocket_plt-2026",
                "ammunition_rocket_plt-2021", "ammunition_rocket_plt-3030"
                */
        };

        public static string[] AbilitiesCategory =
        {
            SkillManager.SPECTRUM, SkillManager.VENOM, SkillManager.SENTINEL, SkillManager.SOLACE, SkillManager.DIMINISHER,
            SkillManager.LIGHTNING, SkillManager.AEGIS_HP_REPAIR, SkillManager.AEGIS_SHIELD_REPAIR, SkillManager.AEGIS_REPAIR_POD,
            SkillManager.CITADEL_DRAW_FIRE
            /*
                SkillManager.CITADEL_FORTIFY, SkillManager.CITADEL_PROTECTION, SkillManager.CITADEL_TRAVEL, SkillManager.DIMINISHER,
                SkillManager.LIGHTNING, SkillManager.SENTINEL, SkillManager.SOLACE, SkillManager.SPEARHEAD_DOUBLE_MINIMAP,
                SkillManager.SPEARHEAD_JAM_X, SkillManager.SPEARHEAD_TARGET_MARKER, SkillManager.SPEARHEAD_ULTIMATE_CLOAK,
                SkillManager.SPECTRUM, SkillManager.VENOM*/

        };

        public static string[] FormationsCategory =
        {
                DroneManager.DEFAULT_FORMATION, DroneManager.TURTLE_FORMATION,
                DroneManager.ARROW_FORMATION, DroneManager.LANCE_FORMATION,
                DroneManager.STAR_FORMATION, DroneManager.PINCER_FORMATION,
                DroneManager.DOUBLE_ARROW_FORMATION, DroneManager.DIAMOND_FORMATION,
                DroneManager.CHEVRON_FORMATION, DroneManager.MOTH_FORMATION,
                DroneManager.CRAB_FORMATION, DroneManager.HEART_FORMATION,
                DroneManager.DRILL_FORMATION,DroneManager.RING_FORMATION,
                DroneManager.WHEEL_FORMATION,
        };

        public void SendUserKeyBindingsUpdateCommand()
        {            
            var keyBindingsModuleCommands = new List<UserKeyBindingsModule>();
            List<BoundKeysBase> actions = Player.Settings.BoundKeys;

            foreach (var action in actions)
            {
                keyBindingsModuleCommands.Add(new UserKeyBindingsModule(
                    action.actionType, action.keyCodes, action.parameter, action.charCode));
            }
            Player.SendCommand(UserKeyBindingsUpdateCommand.write(keyBindingsModuleCommands, false));         
        }

        public void SendUserSettingsCommand()
        {
            var displaySettings = Player.Settings.Display;
            var qualitySettings = Player.Settings.Quality;
            var audioSettings = Player.Settings.Audio;
            var windowSettings = Player.Settings.Window;
            var gameplaySettings = Player.Settings.Gameplay;
            var y2tSettings = Player.Settings.ClassY2T;
            Player.SendCommand(UserSettingsCommand.write(
                new QualitySettingsModule(qualitySettings.notSet, qualitySettings.qualityAttack, qualitySettings.qualityBackground, qualitySettings.qualityPresetting, qualitySettings.qualityCustomized, qualitySettings.qualityPoizone, qualitySettings.qualityShip, qualitySettings.qualityEngine, qualitySettings.qualityExplosion, qualitySettings.qualityCollectable, qualitySettings.qualityEffect),
                new DisplaySettingsModule(displaySettings.notSet, displaySettings.displayPlayerNames, displaySettings.displayResources, displaySettings.displayBonusBoxes, displaySettings.displayHitpointBubbles, displaySettings.displayChat, displaySettings.displayDrones, displaySettings.displayFreeCargoBoxes, displaySettings.displayNotFreeCargoBoxes, displaySettings.showNotOwnedItems, displaySettings.displayWindowsBackground, displaySettings.var12P, displaySettings.displayNotifications, displaySettings.preloadUserShips, displaySettings.dragWindowsAlways, displaySettings.hoverShips, displaySettings.showPremiumQuickslotBar, displaySettings.allowAutoQuality, displaySettings.varb3N, displaySettings.displaySetting3DqualityAntialias, displaySettings.varp3M, displaySettings.displaySetting3DqualityEffects, displaySettings.displaySetting3DqualityLights, displaySettings.displaySetting3DqualityTextures, displaySettings.var03r, displaySettings.displaySetting3DsizeTextures, displaySettings.displaySetting3DtextureFiltering, displaySettings.proActionBarEnabled, displaySettings.proActionBarKeyboardInputEnabled, displaySettings.proActionBarAutohideEnabled),
                new AudioSettingsModule(audioSettings.notSet, audioSettings.sound, audioSettings.music, audioSettings.voice, audioSettings.playCombatMusic),
                new WindowSettingsModule(windowSettings.scale, windowSettings.barState, windowSettings.hideAllWindows),
                new GameplaySettingsModule(gameplaySettings.notSet, gameplaySettings.autoBoost, gameplaySettings.autoRefinement, gameplaySettings.quickSlotStopAttack, gameplaySettings.doubleclickAttackEnabled, gameplaySettings.autochangeAmmo, gameplaySettings.autoStartEnabled, gameplaySettings.autoBuyBootyKeys, false, gameplaySettings.varE3N),
                new class_y2t(y2tSettings.questsAvailableFilter, y2tSettings.questsUnavailableFilter, y2tSettings.questsCompletedFilter, y2tSettings.var_1151, y2tSettings.var_2239, y2tSettings.questsLevelOrderDescending)
            ));
        }

        public void SendMenuBarsCommand()
        {
            var windowSettings = Player.Settings.Window;
            var menuBarsCommand = new List<ClientUIMenuBarModule>();
            var leftItems = new Dictionary<string, string>();

            leftItems.Add("user", "title_user");
            leftItems.Add("ship", "title_ship");
            //leftItems.Add("ship_warp", "ttip_shipWarp_btn");
            leftItems.Add("chat", "title_chat");
            leftItems.Add("group", "title_group");
            leftItems.Add("minimap", "title_map");
            leftItems.Add("spacemap", "title_spacemap");
            leftItems.Add("log", "title_log");
            if(Player.Pet != null)
                leftItems.Add("pet", "title_pet");
            if (EventManager.Spaceball.Active)
                leftItems.Add("spaceball", "title_spaceball");
            if (Player.BoosterManager.Boosters.Count >= 1)
                leftItems.Add("booster", "title_booster");
            if (Player.RankId == 21)
                leftItems.Add("traininggrounds", "title_traininggrounds");

            var topLeftMenuBarItems = new List<ClientUIMenuBarItemModule>();

            foreach (var entryLeft in leftItems)
            {
                string itemID = entryLeft.Key;
                string baseKey = entryLeft.Value;

                if (windowSettings.windows.ContainsKey(itemID))
                {
                    var tooltips = new List<ClientUITooltipModule>();
                    tooltips.Add(new ClientUITooltipModule(new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), ClientUITooltipModule.STANDARD, baseKey,
                                                           new List<ClientUITextReplacementModule>()));
                    var tooltipsCommand = new ClientUITooltipsCommand(tooltips);
                    var menuBarItem = new UpdateWindowItemCommand(windowSettings.windows[itemID].maximixed, windowSettings.windows[itemID].height, true, windowSettings.windows[itemID].y, windowSettings.windows[itemID].x, new ClientUITooltipsCommand(new List<ClientUITooltipModule>()), baseKey, windowSettings.windows[itemID].width, new ClientUITooltipsCommand(new List<ClientUITooltipModule>()), itemID);

                    topLeftMenuBarItems.Add(menuBarItem);
                }
            }

            var topLeftMenuBar = new ClientUIMenuBarModule(ClientUIMenuBarModule.GAME_FEATURE_BAR, topLeftMenuBarItems, windowSettings.gameFeatureBarPosition, windowSettings.gameFeatureBarLayoutType);
            menuBarsCommand.Add(topLeftMenuBar);

            var rightItems = new Dictionary<string, string>();

            rightItems.Add("settings", "title_settings");
            rightItems.Add("help", "title_help");
            rightItems.Add("logout", "title_logout");
            rightItems.Add("fullscreen", "ttip_fullscreen_btn");

            var topRightMenuBarItems = new List<ClientUIMenuBarItemModule>();

            foreach (var entryLeft in rightItems)
            {
                string itemID = entryLeft.Key;
                string baseKey = entryLeft.Value;

                if (windowSettings.windows.ContainsKey(itemID) || itemID == "fullscreen")
                {
                    var tooltips = new List<ClientUITooltipModule>();
                    tooltips.Add(new ClientUITooltipModule(new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), ClientUITooltipModule.STANDARD, baseKey, 
                                                           new List<ClientUITextReplacementModule>()));
                    var tooltipsCommand = new ClientUITooltipsCommand(tooltips);
                    var menuBarItem = itemID == "fullscreen" ? new UpdateWindowItemCommand(false, 0, true, 0, 0, new ClientUITooltipsCommand(new List<ClientUITooltipModule>()), baseKey, 0, new ClientUITooltipsCommand(new List<ClientUITooltipModule>()), itemID) : new UpdateWindowItemCommand(windowSettings.windows[itemID].maximixed, windowSettings.windows[itemID].height, true, windowSettings.windows[itemID].y, windowSettings.windows[itemID].x, new ClientUITooltipsCommand(new List<ClientUITooltipModule>()), baseKey, windowSettings.windows[itemID].width, new ClientUITooltipsCommand(new List<ClientUITooltipModule>()), itemID);

                    topRightMenuBarItems.Add(menuBarItem);
                }
            }

            var topRightMenuBar = new ClientUIMenuBarModule(ClientUIMenuBarModule.GENERIC_FEATURE_BAR, topRightMenuBarItems, windowSettings.genericFeatureBarPosition, windowSettings.genericFeatureBarLayoutType);
            menuBarsCommand.Add(topRightMenuBar);

            Player.SendCommand(ParseFeaturesMenuData.write(menuBarsCommand));
        }

        public void SendSlotBarCommand()
        {
            Player.SetCurrentCooldowns();

            var windowSettings = Player.Settings.Window;
            var slotBars = new List<ClientUISlotBarModule>();
            slotBars.Add(GetStandardSlotBar());
            slotBars.Add(GetPremiumSlotBar());

            if(Player.Premium && Player.Settings.Display.proActionBarEnabled)
                slotBars.Add(GetProActionSlotBar());

            var categories = new List<ClientUISlotBarCategoryModule>();
            categories.Add(GetLasersCategory());
            categories.Add(GetRocketsCategory());
            categories.Add(GetRocketLauncherCategory());
            categories.Add(GetSpecialAmmoCategory());
            categories.Add(GetMineCategory());
            categories.Add(GetCpuCategory());
            categories.Add(GetBuyCategory());
            categories.Add(GetTechCategory());
            categories.Add(GetAbilityCategory());
            categories.Add(GetFormationCategory());

            Player.SendCommand(ClientUISlotBarsCommand.write(windowSettings.categoryBarPosition, slotBars, categories));
        }

        public void SendHelpWindows()
        {
            var windows = new List<class_F2I>();
            windows.Add(new class_F2I(class_F2I.JUMP_GATES));
            windows.Add(new class_F2I(class_F2I.ATTACK));
            windows.Add(new class_F2I(class_F2I.EXTRA_CPU));
            windows.Add(new class_F2I(class_F2I.TRAINING_GROUNDS));
            windows.Add(new class_F2I(class_F2I.TECH_FACTORY));
            windows.Add(new class_F2I(class_F2I.THE_SHOP));
            windows.Add(new class_F2I(class_F2I.CHANGING_SHIPS));
            windows.Add(new class_F2I(class_F2I.JUMP_DEVICE));
            windows.Add(new class_F2I(class_F2I.GALAXY_GATE));
            windows.Add(new class_F2I(class_F2I.SECOND_CONFIGURATION));
            windows.Add(new class_F2I(class_F2I.AUCTION_HOUSE));
            windows.Add(new class_F2I(class_F2I.PREPARE_BATTLE));
            windows.Add(new class_F2I(class_F2I.SKYLAB));
            windows.Add(new class_F2I(class_F2I.SHIP_REPAIR));
            windows.Add(new class_F2I(class_F2I.POLICY_CHANGES));
            windows.Add(new class_F2I(class_F2I.INSTALLING_NEW_EQUIPMENT));
            windows.Add(new class_F2I(class_F2I.FULL_CARGO));
            windows.Add(new class_F2I(class_F2I.ITEM_UPGRADE));
            windows.Add(new class_F2I(class_F2I.BOOST_YOUR_EQUIP));
            windows.Add(new class_F2I(class_F2I.ORE_TRANSFER));
            windows.Add(new class_F2I(class_F2I.CLAN_BATTLE_STATION));
            windows.Add(new class_F2I(class_F2I.HOW_TO_FLY));
            windows.Add(new class_F2I(class_F2I.SELL_RESOURCE));
            windows.Add(new class_F2I(class_F2I.LOOKING_FOR_GROUPS));
            windows.Add(new class_F2I(class_F2I.SHIP_DESIGN));
            windows.Add(new class_F2I(class_F2I.CONTACT_LIST));
            windows.Add(new class_F2I(class_F2I.UNKOWN_DANGERS));
            windows.Add(new class_F2I(class_F2I.WEALTHY_FAMOUS));
            windows.Add(new class_F2I(class_F2I.GET_MORE_AMMO));
            windows.Add(new class_F2I(class_F2I.REQUEST_MISSION));
            windows.Add(new class_F2I(class_F2I.ROCKET_LAUNCHER));
            windows.Add(new class_F2I(class_F2I.WELCOME));
            windows.Add(new class_F2I(class_F2I.PALLADIUM));
            windows.Add(new class_F2I(class_F2I.EQUIP_YOUR_ROCKETS));
            windows.Add(new class_F2I(class_F2I.PVP_WARNING));
            windows.Add(new class_F2I(class_F2I.SKILL_TREE));
            windows.Add(new class_F2I(class_F2I.varC1l));
            windows.Add(new class_F2I(class_F2I.vard34));
            var c802 = class_p2k.write(windows);
            Player.SendCommand(c802);
        }

        public ClientUISlotBarModule GetStandardSlotBar()
        {
            var windowSettings = Player.Settings.Window;
            var standartItems = new List<ClientUISlotBarItemModule>();

            foreach (var pair in Player.Settings.SlotBarItems)
            {
                if (pair.Value == "") continue;
                ClientUISlotBarItemModule item = new ClientUISlotBarItemModule(pair.Value, pair.Key);
                standartItems.Add(item);
            }

            return new ClientUISlotBarModule(windowSettings.standartSlotBarPosition, STANDARD_SLOT_BAR, windowSettings.standartSlotBarLayoutType, standartItems, true);
        }

        public ClientUISlotBarModule GetPremiumSlotBar()
        {
            var windowSettings = Player.Settings.Window;
            var premiumItems = new List<ClientUISlotBarItemModule>();

            foreach (var pair in Player.Settings.PremiumSlotBarItems)
            {
                if (pair.Value == "") continue; 
                ClientUISlotBarItemModule item = new ClientUISlotBarItemModule(pair.Value, pair.Key);
                premiumItems.Add(item);
            }

            return new ClientUISlotBarModule(windowSettings.premiumSlotBarPosition, PREMIUM_SLOT_BAR, windowSettings.premiumSlotBarLayoutType, premiumItems, true);
        }

        public ClientUISlotBarModule GetProActionSlotBar()
        {
            var windowSettings = Player.Settings.Window;
            var proActionItems = new List<ClientUISlotBarItemModule>();

            foreach (var pair in Player.Settings.ProActionBarItems)
            {
                if (pair.Value == "") continue;
                ClientUISlotBarItemModule item = new ClientUISlotBarItemModule(pair.Value, pair.Key);
                proActionItems.Add(item);
            }

            return new ClientUISlotBarModule(windowSettings.proActionBarPosition, PRO_ACTION_BAR, windowSettings.proActionBarLayoutType, proActionItems, Player.Settings.Display.proActionBarOpened);
        }

        public ClientUISlotBarCategoryModule GetLasersCategory()
        {
            var lasersItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in LaserCategory)
            {
                var visible = true;

                if (Player.RankId != 21)
                {
                    switch (itemLootId)
                    {
                        case AmmunitionManager.CBO_100:
                            visible = false;
                            break;
                    }
                }

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), (Player.Settings.InGameSettings.selectedLaser == AmmunitionManager.RSB_75 ? 3000 : 1000), itemLootId,
                                                                   false);

                lasersItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, "ttip_laser", true, true, false, visible),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("lasers", lasersItems);
        }

        public ClientUISlotBarCategoryModule GetRocketsCategory()
        {
            var rocketItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in RocketsCategory)
            {
                var maxTime = 0;

                switch (itemLootId)
                {
                    case AmmunitionManager.R_310:
                    case AmmunitionManager.PLT_2026:
                    case AmmunitionManager.PLT_2021:
                    case AmmunitionManager.PLT_3030:
                        maxTime = 1000;
                        break;
                    case AmmunitionManager.DCR_250:
                        maxTime = TimeManager.DCR_250_COOLDOWN;
                        break;
                    case AmmunitionManager.R_IC3:
                        maxTime = TimeManager.R_IC3_COOLDOWN;
                        break;
                    case AmmunitionManager.WIZ_X:
                        maxTime = TimeManager.WIZARD_COOLDOWN;
                        break;
                    case AmmunitionManager.PLD_8:
                        maxTime = TimeManager.PLD8_COOLDOWN;
                        break;
                }

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), maxTime, itemLootId,
                                                                   false);

                rocketItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, "ttip_rocket", true, true),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("rockets", rocketItems);
        }

        public ClientUISlotBarCategoryModule GetRocketLauncherCategory()
        {
            var rocketLauncherItems = new List<ClientUISlotBarCategoryItemModule>();

            foreach (string itemLootId in RocketLauncherCategory)
            {

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), 90000000, itemLootId,
                                                                   false);

                rocketLauncherItems.Add(new ClientUISlotBarCategoryItemModule(1, itemLootId == CpuManager.ROCKET_LAUNCHER ? GetRocketLauncherItemStatus(CpuManager.ROCKET_LAUNCHER, "ttip_rocketlauncher", Player.AttackManager.RocketLauncher.CurrentLoad, false) : GetItemStatus(itemLootId, "ttip_rocket"),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      itemLootId == CpuManager.ROCKET_LAUNCHER ? ClientUISlotBarCategoryItemModule.DOT : ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("rocket_launchers", rocketLauncherItems);
        }

        public ClientUISlotBarCategoryModule GetSpecialAmmoCategory()
        {
            var specialAmmoItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in SpecialItemsCategory)
            {
                var maxTime = 0;

                switch (itemLootId)
                {
                    case AmmunitionManager.SMB_01:
                    case AmmunitionManager.ISH_01:
                        maxTime = TimeManager.ISH_COOLDOWN;
                        break;
                    case AmmunitionManager.EMP_01:
                        maxTime = TimeManager.EMP_COOLDOWN;
                        break;
                }

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), maxTime, itemLootId,
                                                                   false);

                specialAmmoItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, "ttip_explosive"),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("special_items", specialAmmoItems);
        }

        public ClientUISlotBarCategoryModule GetMineCategory()
        {
            var mineItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in MinesCategory)
            {

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), TimeManager.MINE_COOLDOWN, itemLootId,
                                                                   false);

                mineItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, "ttip_explosive"),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("mines", mineItems);
        }

        public ClientUISlotBarCategoryModule GetCpuCategory()
        {
            var cpuItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in CpusCategory)
            {
                var maxTime = 0;

                switch (itemLootId)
                {
                    case CpuManager.CLK_XL:
                        maxTime = Player.CpuManager.CloakCooldownTime;
                        break;
                }

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), maxTime, itemLootId,
                                                                   false);

                cpuItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, GetCpuTtip(itemLootId), false),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("cpus", cpuItems);
        }

        public string GetCpuTtip(string itemId)
        {
            switch (itemId)
            {
                case CpuManager.CLK_XL:
                    return "ttip_cloak_cpu";
                case CpuManager.AUTO_ROCKET_CPU:
                    return "ttip_arol_cpu";
                case CpuManager.AUTO_HELLSTROM_CPU:
                    return "ttip_rllb_cpu";
                default:
                    return "";
            }
        }

        public ClientUISlotBarCategoryModule GetTechCategory()
        {
            var techItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in TechsCategory)
            {
                var timerState = ClientUISlotBarCategoryItemTimerStateModule.short_2168;
                var maxTime = 0;
                var blocked = false;

                switch (itemLootId)
                {
                    case TechManager.TECH_BATTLE_REPAIR_BOT:
                        timerState = Player.TechManager.BattleRepairBot.Active ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168;
                        maxTime = TimeManager.BATTLE_REPAIR_BOT_COOLDOWN;
                        break;
                    case TechManager.TECH_ENERGY_LEECH:
                        timerState = Player.TechManager.EnergyLeech.Active ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168;
                        maxTime = TimeManager.ENERGY_LEECH_COOLDOWN;
                        break;
                    case TechManager.TECH_PRECISION_TARGETER:
                        timerState = Player.TechManager.PrecisionTargeter.Active ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168;
                        maxTime = TimeManager.PRECISION_TARGETER_COOLDOWN;
                        break;
                    case TechManager.TECH_BACKUP_SHIELDS:
                        maxTime = TimeManager.BACKUP_SHIELD_COOLDOWN;
                        break;
                    case TechManager.TECH_CHAIN_IMPULSE:
                        blocked = true;
                        maxTime = TimeManager.CHAIN_IMPULSE_COOLDOWN;
                        break;
                }

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(timerState), maxTime, itemLootId,
                                                                   false);

                techItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, GetTechTtip(itemLootId), false, false, false, true, blocked),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("tech_items", techItems);
        }

        public string GetTechTtip(string itemId)
        {
            switch (itemId)
            {
                case "tech_backup-shields":
                    return "tech_SHIELD_BACKUP_name";
                case "tech_battle-repair-bot":
                    return "tech_BATTLE_REPAIR_BOT_name";
                case "tech_chain-impulse":
                    return "tech_ENERGY_CHAIN_IMPULSE_name";
                case "tech_energy-leech":
                    return "tech_ENERGY_LEECH_ARRAY_name";
                case "tech_precision-targeter":
                    return "tech_ROCKET_PROBABILITY_MAXIMIZER_name";
                default:
                    return "";
            }
        }

        public ClientUISlotBarCategoryModule GetBuyCategory()
        {
            var buyItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in BuyCategory)
            {

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), 90000000, itemLootId,
                                                                   false);

                buyItems.Add(new ClientUISlotBarCategoryItemModule(1, GetBuyItemStatus(itemLootId),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("buy_now", buyItems);
        }

        public ClientUISlotBarCategoryModule GetAbilityCategory()
        {
            var abilityItems = new List<ClientUISlotBarCategoryItemModule>();

            foreach (var itemLootId in AbilitiesCategory)
            {

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(Player.Storage.Skills.ContainsKey(itemLootId) && Player.Storage.Skills[itemLootId].Active ? ClientUISlotBarCategoryItemTimerStateModule.ACTIVE : ClientUISlotBarCategoryItemTimerStateModule.short_2168), (Player.Storage.Skills.ContainsKey(itemLootId) ? Player.Storage.Skills[itemLootId].Cooldown : 0), itemLootId,
                                                                   false);

                abilityItems.Add(new ClientUISlotBarCategoryItemModule(1, GetAbilityItemStatus(itemLootId, GetAbilityTtip(itemLootId), Player.Storage.Skills.ContainsKey(itemLootId), GetAbilityDescriptionEnabled(itemLootId), false, false),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("ship_abilities", abilityItems);
        }

        public string GetAbilityTtip(string itemId)
        {
            switch (itemId)
            {
                case SkillManager.AEGIS_HP_REPAIR:
                    return "ttip_AEGIS_REPAIRHP_skill";
                case SkillManager.AEGIS_REPAIR_POD:
                    return "ttip_AEGIS_HEALINGPOD_skill";
                case SkillManager.AEGIS_SHIELD_REPAIR:
                    return "ttip_AEGIS_SHIELDRECHARGE_skill";
                case SkillManager.CITADEL_DRAW_FIRE:
                    return "ttip_CITADEL_DRAWFIRE_skill";
                case SkillManager.CITADEL_FORTIFY:
                    return "ttip_CITADEL_FORTIFY_skill";
                case SkillManager.CITADEL_PROTECTION:
                    return "ttip_CITADEL_PROTECTION_skill";
                case SkillManager.CITADEL_TRAVEL:
                    return "ttip_CITADEL_TRAVELMODE_skill";
                case SkillManager.DIMINISHER:
                    return "ttip_DIMINISHER_skill";
                case SkillManager.LIGHTNING:
                    return "ttip_LIGHTNING_skill";
                case SkillManager.SENTINEL:
                    return "ttip_SENTINEL_skill";
                case SkillManager.SOLACE:
                    return "ttip_SOLACE_skill";
                case SkillManager.SPEARHEAD_DOUBLE_MINIMAP:
                    return "ttip_SPEARHEAD_DOUBLEMINIMAP_skill";
                case SkillManager.SPEARHEAD_JAM_X:
                    return "ttip_SPEARHEAD_JAMX_skill";
                case SkillManager.SPEARHEAD_TARGET_MARKER:
                    return "ttip_SPEARHEAD_MARKTARGET_skill";
                case SkillManager.SPEARHEAD_ULTIMATE_CLOAK:
                    return "ttip_SPEARHEAD_ULTIMATECLOAKING_skill";
                case SkillManager.SPECTRUM:
                    return "ttip_SPECTRUM_skill";
                case SkillManager.VENOM:
                    return "ttip_VENOM_skill";
                default:
                    return "";
            }
        }

        public bool GetAbilityDescriptionEnabled(string itemId)
        {
            switch (itemId)
            {
                case SkillManager.DIMINISHER:
                case SkillManager.LIGHTNING:
                case SkillManager.SENTINEL:
                case SkillManager.SOLACE:
                case SkillManager.SPECTRUM:
                case SkillManager.VENOM:
                    return true;
                default:
                    return false;
            }
        }

        public bool GetAbilityItemBarStatusDescriptionEnabled(string itemId)
        {
            switch (itemId)
            {
                case SkillManager.DIMINISHER:
                case SkillManager.LIGHTNING:
                case SkillManager.SENTINEL:
                case SkillManager.SOLACE:
                case SkillManager.SPECTRUM:
                case SkillManager.VENOM:
                    return false;
                default:
                    return true;
            }
        }

        public ClientUISlotBarCategoryModule GetFormationCategory()
        {
            var formationItems = new List<ClientUISlotBarCategoryItemModule>();
            foreach (string itemLootId in FormationsCategory)
            {
                /*
                var visible = true;

                if (Player.RankId != 21)
                {
                    switch (itemLootId)
                    {
                        case DroneManager.DRILL_FORMATION:
                        case DroneManager.RING_FORMATION:
                        case DroneManager.WHEEL_FORMATION:
                            visible = false;
                            break;
                    }
                }
                */

                ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                        new ClientUISlotBarCategoryItemTimerModule(GetCooldownTime(itemLootId),
                                                                   new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.short_2168), TimeManager.FORMATION_COOLDOWN, itemLootId,
                                                                   false);

                formationItems.Add(new ClientUISlotBarCategoryItemModule(1, GetItemStatus(itemLootId, GetFormationTtip(itemLootId), false, false, false, true),
                                                                      ClientUISlotBarCategoryItemModule.SELECTION,
                                                                      ClientUISlotBarCategoryItemModule.NONE,
                                                                      GetCooldownType(itemLootId),
                                                                      categoryTimerModule));
            }
            return new ClientUISlotBarCategoryModule("drone_formations", formationItems);
        }

        public string GetFormationTtip(string itemId)
        {
            switch (itemId)
            {
                case DroneManager.DEFAULT_FORMATION:
                    return "ttip_btn_droneFormation_STD";
                case DroneManager.TURTLE_FORMATION:
                    return "ttip_btn_droneFormation_TURTLE";
                case DroneManager.ARROW_FORMATION:
                    return "ttip_btn_droneFormation_ARROW";
                case DroneManager.LANCE_FORMATION:
                    return "ttip_btn_droneFormation_LANCE";
                case DroneManager.STAR_FORMATION:
                    return "ttip_btn_droneFormation_STAR";
                case DroneManager.PINCER_FORMATION:
                    return "ttip_btn_droneFormation_PINCER";
                case DroneManager.DOUBLE_ARROW_FORMATION:
                    return "ttip_btn_droneFormation_DOUBLE_ARROW";
                case DroneManager.DIAMOND_FORMATION:
                    return "ttip_btn_droneFormation_DIAMOND";
                case DroneManager.CHEVRON_FORMATION:
                    return "ttip_btn_droneFormation_CHEVRON";
                case DroneManager.MOTH_FORMATION:
                    return "ttip_btn_droneFormation_MOTH";
                case DroneManager.CRAB_FORMATION:
                    return "ttip_btn_droneFormation_CRAB";
                case DroneManager.HEART_FORMATION:
                    return "ttip_btn_droneFormation_HEART";
                case DroneManager.BARRAGE_FORMATION:
                    return "ttip_btn_droneFormation_BARRAGE";
                case DroneManager.BAT_FORMATION:
                    return "ttip_btn_droneFormation_BAT";
                case DroneManager.DRILL_FORMATION:
                    return "ttip_btn_droneFormation_DRILL";
                case DroneManager.RING_FORMATION:
                    return "ttip_btn_droneFormation_RING";
                case DroneManager.WHEEL_FORMATION:
                    return "ttip_btn_droneFormation_WHEEL";
                default:
                    return "";
            }
        }

        public CooldownTypeModule GetCooldownType(string pItemId)
        {
            switch (pItemId)
            {
                case AmmunitionManager.EMP_01:
                    return new CooldownTypeModule(CooldownTypeModule.short_1048);
                case AmmunitionManager.ISH_01:
                    return new CooldownTypeModule(CooldownTypeModule.short_1085);
                case AmmunitionManager.SMB_01:
                    return new CooldownTypeModule(CooldownTypeModule.short_1124);
                case AmmunitionManager.RSB_75:
                    return new CooldownTypeModule(CooldownTypeModule.short_1220);

                case CpuManager.CLK_XL:
                    return new CooldownTypeModule(CooldownTypeModule.short_138);
                case CpuManager.AUTO_ROCKET_CPU:
                    return new CooldownTypeModule(CooldownTypeModule.short_1428);

                case TechManager.TECH_PRECISION_TARGETER:
                    return new CooldownTypeModule(CooldownTypeModule.ROCKET_PROBABILITY_MAXIMIZER);
                case TechManager.TECH_BACKUP_SHIELDS:
                    return new CooldownTypeModule(CooldownTypeModule.SHIELD_BACKUP);
                case TechManager.TECH_BATTLE_REPAIR_BOT:
                    return new CooldownTypeModule(CooldownTypeModule.BATTLE_REPAIR_BOT);
                case TechManager.TECH_ENERGY_LEECH:
                    return new CooldownTypeModule(CooldownTypeModule.ENERGY_LEECH_ARRAY);
                case TechManager.TECH_CHAIN_IMPULSE:
                    return new CooldownTypeModule(CooldownTypeModule.ENERGY_CHAIN_IMPULSE);

                case SkillManager.SENTINEL:
                    return new CooldownTypeModule(CooldownTypeModule.short_1439);
                case SkillManager.SOLACE:
                    return new CooldownTypeModule(CooldownTypeModule.short_1554);
                case SkillManager.DIMINISHER:
                    return new CooldownTypeModule(CooldownTypeModule.short_1587);
                case SkillManager.SPECTRUM:
                    return new CooldownTypeModule(CooldownTypeModule.short_1699);
                case SkillManager.VENOM:
                    return new CooldownTypeModule(CooldownTypeModule.short_1736);
                case SkillManager.LIGHTNING:
                    return new CooldownTypeModule(CooldownTypeModule.SPEED_BUFF);

                case SkillManager.AEGIS_HP_REPAIR:
                    return new CooldownTypeModule(CooldownTypeModule.short_2204);
                case SkillManager.AEGIS_SHIELD_REPAIR:
                    return new CooldownTypeModule(CooldownTypeModule.short_2342);
                case SkillManager.AEGIS_REPAIR_POD:
                    return new CooldownTypeModule(CooldownTypeModule.short_2419);

                case SkillManager.CITADEL_DRAW_FIRE:
                    return new CooldownTypeModule(CooldownTypeModule.short_255);

                case AmmunitionManager.R_IC3:
                    return new CooldownTypeModule(CooldownTypeModule.short_1789);
                case AmmunitionManager.DCR_250:
                    return new CooldownTypeModule(CooldownTypeModule.short_1815);
                case AmmunitionManager.WIZ_X:
                    return new CooldownTypeModule(CooldownTypeModule.short_1952);
                case AmmunitionManager.PLD_8:
                    return new CooldownTypeModule(CooldownTypeModule.short_2172);

                case AmmunitionManager.SLM_01:
                case AmmunitionManager.EMPM_01:
                case AmmunitionManager.DDM_01:
                case AmmunitionManager.ACM_01:
                case AmmunitionManager.SABM_01:
                case AmmunitionManager.IM_01:
                    return new CooldownTypeModule(CooldownTypeModule.short_2047);

                case AmmunitionManager.R_310:
                case AmmunitionManager.PLT_2026:
                case AmmunitionManager.PLT_2021:
                case AmmunitionManager.PLT_3030:
                    return new CooldownTypeModule(CooldownTypeModule.ROCKET);

                case DroneManager.DEFAULT_FORMATION:
                case DroneManager.TURTLE_FORMATION:
                case DroneManager.ARROW_FORMATION:
                case DroneManager.LANCE_FORMATION:
                case DroneManager.STAR_FORMATION:
                case DroneManager.PINCER_FORMATION:
                case DroneManager.DOUBLE_ARROW_FORMATION:
                case DroneManager.DIAMOND_FORMATION:
                case DroneManager.CHEVRON_FORMATION:
                case DroneManager.MOTH_FORMATION:
                case DroneManager.CRAB_FORMATION:
                case DroneManager.HEART_FORMATION:
                case DroneManager.BARRAGE_FORMATION:
                case DroneManager.BAT_FORMATION:
                case DroneManager.DOME_FORMATION:
                case DroneManager.DRILL_FORMATION:
                case DroneManager.RING_FORMATION:
                case DroneManager.VETERAN_FORMATION:
                case DroneManager.WHEEL_FORMATION:
                case DroneManager.WAVE_FORMATION:
                case DroneManager.X_FORMATION:
                    return new CooldownTypeModule(CooldownTypeModule.short_987);
                default:
                    return new CooldownTypeModule(CooldownTypeModule.NONE);
            }
        }

        private long GetCooldownTime(string pItemId)
        {
            switch (pItemId)
            {
                case CpuManager.CLK_XL:
                    var cloakCooldown = (DateTime.Now - Player.CpuManager.cloakCooldown).TotalMilliseconds;
                    return (int)(Player.CpuManager.CloakCooldownTime - cloakCooldown);
                case AmmunitionManager.R_310:
                case AmmunitionManager.PLT_2026:
                case AmmunitionManager.PLT_2021:
                case AmmunitionManager.PLT_3030:
                    var rocketCooldown = (DateTime.Now - Player.AttackManager.lastRocketAttack).TotalMilliseconds;
                    return (int)((Player.Premium ? 1000 : 3000) - rocketCooldown);
                case AmmunitionManager.DCR_250:
                    var dcrCooldown = (DateTime.Now - Player.AttackManager.dcr_250Cooldown).TotalMilliseconds;
                    return (int)(TimeManager.DCR_250_COOLDOWN - dcrCooldown);
                case AmmunitionManager.R_IC3:
                    var r_ic3Cooldown = (DateTime.Now - Player.AttackManager.r_ic3Cooldown).TotalMilliseconds;
                    return (int)(TimeManager.R_IC3_COOLDOWN - r_ic3Cooldown);
                case AmmunitionManager.WIZ_X:
                    var wiz_xCooldown = (DateTime.Now - Player.AttackManager.wiz_xCooldown).TotalMilliseconds;
                    return (int)(TimeManager.WIZARD_COOLDOWN - wiz_xCooldown);
                case AmmunitionManager.PLD_8:
                    var pldCooldown = (DateTime.Now - Player.AttackManager.pld8Cooldown).TotalMilliseconds;
                    return (int)(TimeManager.PLD8_COOLDOWN - pldCooldown);
                case AmmunitionManager.LCB_10:
                case AmmunitionManager.MCB_25:
                case AmmunitionManager.MCB_50:
                case AmmunitionManager.UCB_100:
                case AmmunitionManager.SAB_50:
                case AmmunitionManager.CBO_100:
                case AmmunitionManager.JOB_100:
                    var laserCooldown = (DateTime.Now - Player.AttackManager.lastAttackTime).TotalMilliseconds;
                    return (int)(1000 - laserCooldown);
                case AmmunitionManager.RSB_75:
                    var rsbCooldown = (DateTime.Now - Player.AttackManager.lastRSBAttackTime).TotalMilliseconds;
                    return (int)(3000 - rsbCooldown);
                case AmmunitionManager.SMB_01:
                    var smbCooldown = (DateTime.Now - Player.AttackManager.SmbCooldown).TotalMilliseconds;
                    return (int)(TimeManager.SMB_COOLDOWN - smbCooldown);
                case AmmunitionManager.ISH_01:
                    var ishCooldown = (DateTime.Now - Player.AttackManager.IshCooldown).TotalMilliseconds;
                    return (int)(TimeManager.ISH_COOLDOWN - ishCooldown);
                case AmmunitionManager.EMP_01:
                    var empCooldown = (DateTime.Now - Player.AttackManager.EmpCooldown).TotalMilliseconds;
                    return (int)(TimeManager.EMP_COOLDOWN - empCooldown);
                case AmmunitionManager.ACM_01:
                case AmmunitionManager.DDM_01:
                case AmmunitionManager.EMPM_01:
                case AmmunitionManager.IM_01:
                case AmmunitionManager.SABM_01:
                case AmmunitionManager.SLM_01:
                    var mineCooldown = (DateTime.Now - Player.AttackManager.mineCooldown).TotalMilliseconds;
                    return (int)(TimeManager.MINE_COOLDOWN - mineCooldown);
                case TechManager.TECH_ENERGY_LEECH:
                    var energyLeechCooldown = (DateTime.Now - Player.TechManager.EnergyLeech.cooldown).TotalMilliseconds;
                    return (int)((Player.TechManager.EnergyLeech.Active ? TimeManager.ENERGY_LEECH_DURATION : (TimeManager.ENERGY_LEECH_DURATION + TimeManager.ENERGY_LEECH_COOLDOWN)) - energyLeechCooldown);
                case TechManager.TECH_CHAIN_IMPULSE:
                    var chainImpulseCooldown = (DateTime.Now - Player.TechManager.ChainImpulse.cooldown).TotalMilliseconds;
                    return (int)(TimeManager.CHAIN_IMPULSE_COOLDOWN - chainImpulseCooldown);                
                case TechManager.TECH_PRECISION_TARGETER:
                    var precisionTargeterCooldown = (DateTime.Now - Player.TechManager.PrecisionTargeter.cooldown).TotalMilliseconds;
                    return (int)((Player.TechManager.PrecisionTargeter.Active ? TimeManager.PRECISION_TARGETER_DURATION : (TimeManager.PRECISION_TARGETER_DURATION + TimeManager.PRECISION_TARGETER_COOLDOWN)) - precisionTargeterCooldown);
                case TechManager.TECH_BACKUP_SHIELDS:
                    var backupShieldsCooldown = (DateTime.Now - Player.TechManager.BackupShields.cooldown).TotalMilliseconds;
                    return (int)(TimeManager.BACKUP_SHIELD_COOLDOWN - backupShieldsCooldown);
                case TechManager.TECH_BATTLE_REPAIR_BOT:
                    var battleRepairBotCooldown = (DateTime.Now - Player.TechManager.BattleRepairBot.cooldown).TotalMilliseconds;
                    return (int)((Player.TechManager.BattleRepairBot.Active ? TimeManager.BATTLE_REPAIR_BOT_DURATION : (TimeManager.BATTLE_REPAIR_BOT_DURATION + TimeManager.BATTLE_REPAIR_BOT_COOLDOWN)) - battleRepairBotCooldown);
                case SkillManager.SENTINEL:
                case SkillManager.SPECTRUM:
                case SkillManager.VENOM:
                case SkillManager.SOLACE:
                case SkillManager.DIMINISHER:
                case SkillManager.LIGHTNING:
                case SkillManager.AEGIS_HP_REPAIR:
                case SkillManager.AEGIS_SHIELD_REPAIR:
                case SkillManager.AEGIS_REPAIR_POD:
                case SkillManager.CITADEL_DRAW_FIRE:
                    if (Player.Storage.Skills.ContainsKey(pItemId))
                    {
                        var cooldown = (DateTime.Now - Player.Storage.Skills[pItemId].cooldown).TotalMilliseconds;
                        return (int)((Player.Storage.Skills[pItemId].Active ? Player.Storage.Skills[pItemId].Duration : (Player.Storage.Skills[pItemId].Duration + Player.Storage.Skills[pItemId].Cooldown)) - cooldown);
                    }
                    else return 0;
                case DroneManager.DEFAULT_FORMATION:
                case DroneManager.TURTLE_FORMATION:
                case DroneManager.ARROW_FORMATION:
                case DroneManager.LANCE_FORMATION:
                case DroneManager.STAR_FORMATION:
                case DroneManager.PINCER_FORMATION:
                case DroneManager.DOUBLE_ARROW_FORMATION:
                case DroneManager.DIAMOND_FORMATION:
                case DroneManager.CHEVRON_FORMATION:
                case DroneManager.MOTH_FORMATION:
                case DroneManager.CRAB_FORMATION:
                case DroneManager.HEART_FORMATION:
                case DroneManager.BARRAGE_FORMATION:
                case DroneManager.BAT_FORMATION:
                case DroneManager.DOME_FORMATION:
                case DroneManager.DRILL_FORMATION:
                case DroneManager.RING_FORMATION:
                case DroneManager.VETERAN_FORMATION:
                case DroneManager.WHEEL_FORMATION:
                case DroneManager.WAVE_FORMATION:
                case DroneManager.X_FORMATION:
                    var formationCooldown = (DateTime.Now - Player.DroneManager.formationCooldown).TotalMilliseconds;
                    return (int)(TimeManager.FORMATION_COOLDOWN - formationCooldown);
                default:
                    return 0;
            }
        }

        public ClientUISlotBarCategoryItemStatusModule GetItemStatus(string pItemId, string pTooltipId, bool descriptionEnabled = true, bool doubleClickToFire = false, bool buyEnable = false, bool visible = true, bool blocked = false)
        {

            ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(GetItemBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled, doubleClickToFire));
            ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(GetSlotBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled));

            return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, visible,
                                                               ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                               0, blocked, true,
                                                               slotBarStatusTooltip, buyEnable ? true : false, LaserCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedLaser.Equals(pItemId) :
                                                                                                               RocketsCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedRocket.Equals(pItemId) :
                                                                                                               RocketLauncherCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedRocketLauncher.Equals(pItemId) :
                                                                                                               FormationsCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedFormation.Equals(pItemId) :
                                                                                                               CpusCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedCpus.Contains(pItemId) :
                                                                                                               false,
                                                               0);
        }

        public ClientUISlotBarCategoryItemStatusModule GetBuyItemStatus(string pItemId)
        {

            ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(GetBuyItemBarStatusTooltip(pItemId, "ttip_rocket", 1000, true, "uridium"));
            ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(GetBuySlotBarStatusTooltip(pItemId, "ttip_rocket", 1000, true, "uridium"));

            return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                               ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                               0, false, true,
                                                               slotBarStatusTooltip, true, false,
                                                               0);
        }

        public ClientUISlotBarCategoryItemStatusModule GetAbilityItemStatus(string pItemId, string pTooltipId, bool visible, bool descriptionEnabled = true, bool doubleClickToFire = false, bool buyEnable = false)
        {

            ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(GetAbilityItemBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled, doubleClickToFire));
            ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(GetAbilitySlotBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled));

            return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, visible,
                                                               ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                               0, false, true,
                                                               slotBarStatusTooltip, buyEnable ? true : false, false,
                                                               0);
        }

        public ClientUISlotBarCategoryItemStatusModule GetRocketLauncherItemStatus(string pItemId, string pTooltipId, int count = 0, bool descriptionEnabled = true, bool doubleClickToFire = false)
        {

            ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(GetItemBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled, doubleClickToFire));
            ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(GetSlotBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled));

            var counterColor = Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.ECO_10 ? ClientUISlotBarCategoryItemStatusModule.BLUE :
                               Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.HSTRM_01 ? ClientUISlotBarCategoryItemStatusModule.YELLOW :
                               Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.UBR_100 ? ClientUISlotBarCategoryItemStatusModule.RED :
                               Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.SAR_01 ? ClientUISlotBarCategoryItemStatusModule.short_1167 :
                               Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.SAR_01 || Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.SAR_02 ? ClientUISlotBarCategoryItemStatusModule.short_1167 :
                               Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.CBR ? ClientUISlotBarCategoryItemStatusModule.short_790 : ClientUISlotBarCategoryItemStatusModule.BLUE;

            return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                               counterColor, pItemId,
                                                               count, false, true,
                                                               slotBarStatusTooltip, false, false,
                                                               Player.AttackManager.RocketLauncher.MaxLoad);
        }

        #region Normal GetItemBarStatusTooltip()
        public List<ClientUITooltipModule> GetItemBarStatusTooltip(string pLootId, string pTooltipId, bool pCountable, long pCount, bool descriptionEnabled, bool doubleClickToFire)
        {
            var tooltipItemBars = new List<ClientUITooltipModule>();

            var vec_721_1 = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule x_521_1 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
            ClientUITextReplacementModule x_721_1 = new ClientUITextReplacementModule("%TYPE%", x_521_1, pLootId);
            vec_721_1.Add(x_721_1);

            ClientUITooltipTextFormatModule class521_localized_1 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_1 =
                    new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_1);

            tooltipItemBars.Add(slotBarItemStatusTooltip_1);

            if (pCountable)
            {
                var vec_721_2 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule class521_plain =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
                ClientUITextReplacementModule x_721_2 =
                        new ClientUITextReplacementModule("%COUNT%", class521_plain, pCount.ToString());
                vec_721_2.Add(x_721_2);

                ClientUITooltipTextFormatModule class521_localized_2 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_2 =
                        new ClientUITooltipModule(class521_localized_2, ClientUITooltipModule.STANDARD, "ttip_count",
                                                  vec_721_2);

                tooltipItemBars.Add(slotBarItemStatusTooltip_2);
            }

            if (descriptionEnabled) {
                var vec_721_3 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule x_521_3 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

                ClientUITooltipModule slotBarItemStatusTooltip_3 =
                        new ClientUITooltipModule(x_521_3, ClientUITooltipModule.STANDARD, pLootId, vec_721_3);

                tooltipItemBars.Add(slotBarItemStatusTooltip_3);
            }

            if (doubleClickToFire) {
                var vec_721_4 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule class521_localized_4 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_4 =
                        new ClientUITooltipModule(class521_localized_4, ClientUITooltipModule.STANDARD,
                                                  "ttip_double_click_to_fire", vec_721_4);

                tooltipItemBars.Add(slotBarItemStatusTooltip_4);
            }
            return tooltipItemBars;
        }
        #endregion Normal GetItemBarStatusTooltip()

        #region Normal GetSlotBarStatusTooltip()
        public List<ClientUITooltipModule> GetSlotBarStatusTooltip(string pLootId, string pTooltipId, bool pCountable, long pCount, bool descriptionEnabled)
        {
            var tooltipSlotBars = new List<ClientUITooltipModule>();

            var vec_721_5 = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule x_521_5 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
            ClientUITextReplacementModule x_721_5 = new ClientUITextReplacementModule("%TYPE%", x_521_5, pLootId);
            vec_721_5.Add(x_721_5);

            ClientUITooltipTextFormatModule class521_localized_5 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_5 =
                    new ClientUITooltipModule(class521_localized_5, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_5);

            tooltipSlotBars.Add(slotBarItemStatusTooltip_5);
        
            if (pCountable)
            {
                var vec_721_6 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule tf_plain_6 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
                ClientUITextReplacementModule x_721_6 =
                        new ClientUITextReplacementModule("%COUNT%", tf_plain_6, pCount.ToString());
                vec_721_6.Add(x_721_6);

                ClientUITooltipTextFormatModule tf_localized_6 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_6 =
                        new ClientUITooltipModule(tf_localized_6, ClientUITooltipModule.STANDARD, "ttip_count", vec_721_6);

                tooltipSlotBars.Add(slotBarItemStatusTooltip_6);
            }

            if (descriptionEnabled) {
                var vec_721_7 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule tf_234_7 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

                ClientUITooltipModule slotBarItemStatusTooltip_7 =
                        new ClientUITooltipModule(tf_234_7, ClientUITooltipModule.STANDARD, pLootId, vec_721_7);

                tooltipSlotBars.Add(slotBarItemStatusTooltip_7);
            }

            return tooltipSlotBars;
        }
        #endregion Normal GetSlotBarStatusTooltip()

        #region Buy GetBuyItemBarStatusTooltip()
        public List<ClientUITooltipModule> GetBuyItemBarStatusTooltip(string pLootId, string pTooltipId, long pAmount, bool doubleClickToBuy, string priceType)
        {
            var tooltipItemBars = new List<ClientUITooltipModule>();

            var vec_721_1 = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule x_521_1 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
            ClientUITextReplacementModule x_721_1 = new ClientUITextReplacementModule("%TYPE%", x_521_1, pLootId);
            vec_721_1.Add(x_721_1);

            ClientUITooltipTextFormatModule class521_localized_1 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_1 =
                    new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_1);

            tooltipItemBars.Add(slotBarItemStatusTooltip_1);

            var vec_721_2 = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule class521_plain =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
            ClientUITextReplacementModule x_721_2 =
                    new ClientUITextReplacementModule("%AMOUNT%", class521_plain, pAmount.ToString());
            vec_721_2.Add(x_721_2);

            ClientUITooltipTextFormatModule class521_localized_2 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_2 =
                    new ClientUITooltipModule(class521_localized_2, ClientUITooltipModule.STANDARD, "ttip_quick_buy_amount",
                                              vec_721_2);

            tooltipItemBars.Add(slotBarItemStatusTooltip_2);

            var vec_721_2_price = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule class521_plain_price =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
            ClientUITextReplacementModule x_721_2_price =
                    new ClientUITextReplacementModule("%PRICE%", class521_plain_price, pAmount.ToString());
            vec_721_2_price.Add(x_721_2_price);

            ClientUITooltipTextFormatModule class521_localized_2_price =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_2_price =
                    new ClientUITooltipModule(class521_localized_2_price, ClientUITooltipModule.STANDARD, priceType == "uridium" ? "ttip_price_uridium" : "ttip_price_credits",
                                              vec_721_2_price);

            tooltipItemBars.Add(slotBarItemStatusTooltip_2_price);

            if (doubleClickToBuy)
            {
                var vec_721_4 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule class521_localized_4 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_4 =
                        new ClientUITooltipModule(class521_localized_4, ClientUITooltipModule.STANDARD,
                                                  "ttip_quick_buy_itembar", vec_721_4);

                tooltipItemBars.Add(slotBarItemStatusTooltip_4);
            }
            return tooltipItemBars;
        }
        #endregion Buy GetBuyItemBarStatusTooltip()

        #region Buy GetBuySlotBarStatusTooltip()
        public List<ClientUITooltipModule> GetBuySlotBarStatusTooltip(string pLootId, string pTooltipId, long pAmount, bool doubleClickToBuy, string priceType)
        {
            var tooltipSlotBars = new List<ClientUITooltipModule>();

            var vec_721_1 = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule x_521_1 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
            ClientUITextReplacementModule x_721_1 = new ClientUITextReplacementModule("%TYPE%", x_521_1, pLootId);
            vec_721_1.Add(x_721_1);

            ClientUITooltipTextFormatModule class521_localized_1 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_1 =
                    new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_1);

            tooltipSlotBars.Add(slotBarItemStatusTooltip_1);

            var vec_721_2 = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule class521_plain =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
            ClientUITextReplacementModule x_721_2 =
                    new ClientUITextReplacementModule("%AMOUNT%", class521_plain, pAmount.ToString());
            vec_721_2.Add(x_721_2);

            ClientUITooltipTextFormatModule class521_localized_2 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_2 =
                    new ClientUITooltipModule(class521_localized_2, ClientUITooltipModule.STANDARD, "ttip_quick_buy_amount",
                                              vec_721_2);

            tooltipSlotBars.Add(slotBarItemStatusTooltip_2);

            var vec_721_2_price = new List<ClientUITextReplacementModule>();

            ClientUITooltipTextFormatModule class521_plain_price =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
            ClientUITextReplacementModule x_721_2_price =
                    new ClientUITextReplacementModule("%PRICE%", class521_plain_price, pAmount.ToString());
            vec_721_2_price.Add(x_721_2_price);

            ClientUITooltipTextFormatModule class521_localized_2_price =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            ClientUITooltipModule slotBarItemStatusTooltip_2_price =
                    new ClientUITooltipModule(class521_localized_2_price, ClientUITooltipModule.STANDARD, priceType == "uridium" ? "ttip_price_uridium" : "ttip_price_credits",
                                              vec_721_2_price);

            tooltipSlotBars.Add(slotBarItemStatusTooltip_2_price);

            if (doubleClickToBuy)
            {
                var vec_721_7 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule tf_234_7 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

                ClientUITooltipModule slotBarItemStatusTooltip_7 =
                        new ClientUITooltipModule(tf_234_7, ClientUITooltipModule.STANDARD, "ttip_quick_buy_itembar", vec_721_7);

                tooltipSlotBars.Add(slotBarItemStatusTooltip_7);
            }

            return tooltipSlotBars;
        }
        #endregion Buy GetBuySlotBarStatusTooltip()

        #region Ability GetAbilityItemBarStatusTooltip()
        public List<ClientUITooltipModule> GetAbilityItemBarStatusTooltip(string pLootId, string pTooltipId, bool pCountable, long pCount, bool descriptionEnabled, bool doubleClickToFire)
        {
            var tooltipItemBars = new List<ClientUITooltipModule>();

            if (GetAbilityItemBarStatusDescriptionEnabled(pLootId)) {
                var vec_721_1 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule x_521_1 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
                ClientUITextReplacementModule x_721_1 = new ClientUITextReplacementModule("%TYPE%", x_521_1, pLootId);
                vec_721_1.Add(x_721_1);

                ClientUITooltipTextFormatModule class521_localized_1 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_1 =
                        new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_1);

                tooltipItemBars.Add(slotBarItemStatusTooltip_1);
            }

            if (pCountable)
            {
                var vec_721_2 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule class521_plain =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
                ClientUITextReplacementModule x_721_2 =
                        new ClientUITextReplacementModule("%COUNT%", class521_plain, pCount.ToString());
                vec_721_2.Add(x_721_2);

                ClientUITooltipTextFormatModule class521_localized_2 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_2 =
                        new ClientUITooltipModule(class521_localized_2, ClientUITooltipModule.STANDARD, "ttip_count",
                                                  vec_721_2);

                tooltipItemBars.Add(slotBarItemStatusTooltip_2);
            }

            if (descriptionEnabled)
            {
                var vec_721_3 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule x_521_3 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

                ClientUITooltipModule slotBarItemStatusTooltip_3 =
                        new ClientUITooltipModule(x_521_3, ClientUITooltipModule.STANDARD, pLootId, vec_721_3);

                tooltipItemBars.Add(slotBarItemStatusTooltip_3);
            }

            if (doubleClickToFire)
            {
                var vec_721_4 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule class521_localized_4 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_4 =
                        new ClientUITooltipModule(class521_localized_4, ClientUITooltipModule.STANDARD,
                                                  "ttip_double_click_to_fire", vec_721_4);

                tooltipItemBars.Add(slotBarItemStatusTooltip_4);
            }
            return tooltipItemBars;
        }
        #endregion Ability GetAbilityItemBarStatusTooltip()

        #region Ability GetAbilitySlotBarStatusTooltip()
        public List<ClientUITooltipModule> GetAbilitySlotBarStatusTooltip(string pLootId, string pTooltipId, bool pCountable, long pCount, bool descriptionEnabled)
        {
            var tooltipSlotBars = new List<ClientUITooltipModule>();

            if (GetAbilityItemBarStatusDescriptionEnabled(pLootId)) {
                var vec_721_1 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule x_521_1 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
                ClientUITextReplacementModule x_721_1 = new ClientUITextReplacementModule("%TYPE%", x_521_1, pLootId);
                vec_721_1.Add(x_721_1);

                ClientUITooltipTextFormatModule class521_localized_1 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_1 =
                        new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_1);

                tooltipSlotBars.Add(slotBarItemStatusTooltip_1);
            }

            if (pCountable)
            {
                var vec_721_6 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule tf_plain_6 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
                ClientUITextReplacementModule x_721_6 =
                        new ClientUITextReplacementModule("%COUNT%", tf_plain_6, pCount.ToString());
                vec_721_6.Add(x_721_6);

                ClientUITooltipTextFormatModule tf_localized_6 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                ClientUITooltipModule slotBarItemStatusTooltip_6 =
                        new ClientUITooltipModule(tf_localized_6, ClientUITooltipModule.STANDARD, "ttip_count", vec_721_6);

                tooltipSlotBars.Add(slotBarItemStatusTooltip_6);
            }

            if (descriptionEnabled)
            {
                var vec_721_7 = new List<ClientUITextReplacementModule>();

                ClientUITooltipTextFormatModule tf_234_7 =
                        new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

                ClientUITooltipModule slotBarItemStatusTooltip_7 =
                        new ClientUITooltipModule(tf_234_7, ClientUITooltipModule.STANDARD, pLootId, vec_721_7);

                tooltipSlotBars.Add(slotBarItemStatusTooltip_7);
            }

            return tooltipSlotBars;
        }
        #endregion Ability GetAbilitySlotBarStatusTooltip()

        public void SendNewItemStatus(string itemId)
        {
            if (LaserCategory.Contains(itemId))
            {
                Player.SendCommand(GetNewItemStatus(itemId, "ttip_laser", true, true, false));
            }
            else if (RocketsCategory.Contains(itemId))
            {
                Player.SendCommand(GetNewItemStatus(itemId, "ttip_rocket", true, true, false));
            }
            else if (CpusCategory.Contains(itemId))
            {
                Player.SendCommand(GetNewItemStatus(itemId, GetCpuTtip(itemId), false, false, false));
            }
            else if (FormationsCategory.Contains(itemId))
            {
                Player.SendCommand(GetNewItemStatus(itemId, GetFormationTtip(itemId), false, false, false));
            }
            else if (RocketLauncherCategory.Contains(itemId))
            {
                Player.SendCommand(GetNewRocketLauncherItemStatus(itemId, "ttip_rocketlauncher", Player.AttackManager.RocketLauncher.CurrentLoad, false, false, false));
            }
        }

        public void UseSlotBarItem(string pItemId)
        {
            if (LaserCategory.Contains(pItemId))
            {
                SetSelectedLaserItem(pItemId);
            }
            else if (RocketsCategory.Contains(pItemId))
            {
                SetSelectedRocketItem(pItemId);
            }
            else if (RocketLauncherCategory.Contains(pItemId))
            {
                if (pItemId == CpuManager.ROCKET_LAUNCHER)
                {
                    if (Player.AttackManager.RocketLauncher.CurrentLoad >= 1)
                        Player.AttackManager.LaunchRocketLauncher();
                    else
                        Player.AttackManager.RocketLauncher.ReloadingActive = Player.Storage.AutoRocketLauncher || Player.AttackManager.RocketLauncher.CurrentLoad == 0 ? true : false;
                }
                else
                {
                    SetSelectedRocketLauncherItem(pItemId);
                }
            }
            else if(FormationsCategory.Contains(pItemId))
            {
                Player.DroneManager.ChangeDroneFormation(pItemId);
            }
            else if (TechsCategory.Contains(pItemId))
            {
                Player.TechManager.AssembleTechCategoryRequest(pItemId);
            }
            else if (AbilitiesCategory.Contains(pItemId))
            {
                if (Player.Storage.Skills.ContainsKey(pItemId))
                    Player.Storage.Skills[pItemId].Send();
            }
            else
            {
                switch (pItemId)
                {
                    case AmmunitionManager.EMP_01:
                        Player.AttackManager.EMP();
                        break;
                    case AmmunitionManager.SMB_01:
                        Player.AttackManager.SMB();
                        break;
                    case AmmunitionManager.ISH_01:
                        Player.AttackManager.ISH();
                        break;
                    case CpuManager.CLK_XL:
                        Player.CpuManager.Cloak();
                        break;
                    case CpuManager.AUTO_ROCKET_CPU:
                        Player.CpuManager.ArolX();
                        break;
                    case CpuManager.AUTO_HELLSTROM_CPU:
                        Player.CpuManager.RllbX();
                        break;
                    case AmmunitionManager.SLM_01:
                    case AmmunitionManager.EMPM_01:
                    case AmmunitionManager.DDM_01:
                    case AmmunitionManager.ACM_01:
                    case AmmunitionManager.SABM_01:
                    case AmmunitionManager.IM_01:
                        SendMine(pItemId);
                        break;
                }
            }

            Player.UpdateCurrentCooldowns();
            QueryManager.SavePlayer.Settings(Player, "cooldowns", Player.Settings.Cooldowns);
        }

        public void SendMine(string mineLootId)
        {
            if (Player.Storage.IsInDemilitarizedZone || Player.Storage.OnBlockedMinePosition || Player.CurrentInRangePortalId != -1 || (Duel.InDuel(Player) && Player.Storage.Duel.PeaceArea)) return;

            if (Player.AttackManager.mineCooldown.AddMilliseconds(TimeManager.MINE_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                switch (mineLootId)
                {
                    case AmmunitionManager.SLM_01:
                        new SLM_01(Player, Player.Spacemap, new Position(Player.Position.X, Player.Position.Y), 7);
                        Player.SendCooldown(AmmunitionManager.SLM_01, TimeManager.MINE_COOLDOWN);
                        break;
                    case AmmunitionManager.EMPM_01:
                        new EMPM_01(Player, Player.Spacemap, new Position(Player.Position.X, Player.Position.Y), 2);
                        Player.SendCooldown(AmmunitionManager.EMPM_01, TimeManager.MINE_COOLDOWN);
                        break;
                    case AmmunitionManager.DDM_01:
                        new DDM_01(Player, Player.Spacemap, new Position(Player.Position.X, Player.Position.Y), 4);
                        Player.SendCooldown(AmmunitionManager.DDM_01, TimeManager.MINE_COOLDOWN);
                        break;
                    case AmmunitionManager.ACM_01:
                        new ACM_01(Player, Player.Spacemap, new Position(Player.Position.X, Player.Position.Y), 1);
                        Player.SendCooldown(AmmunitionManager.ACM_01, TimeManager.MINE_COOLDOWN);
                        break;
                    case AmmunitionManager.SABM_01:
                        new SABM_01(Player, Player.Spacemap, new Position(Player.Position.X, Player.Position.Y), 3);
                        Player.SendCooldown(AmmunitionManager.SABM_01, TimeManager.MINE_COOLDOWN);
                        break;
                    case AmmunitionManager.IM_01:
                        new IM_01(Player, Player.Spacemap, new Position(Player.Position.X, Player.Position.Y), 17);
                        Player.SendCooldown(AmmunitionManager.SABM_01, TimeManager.MINE_COOLDOWN);
                        break;
                }
                Player.AttackManager.mineCooldown = DateTime.Now;
            }
        }

        public void SetSelectedLaserItem(string pSelectedLaserItem)
        {
            if (Player.Settings.InGameSettings.selectedLaser.Equals(pSelectedLaserItem))
            {
                if (Player.Settings.Gameplay.quickSlotStopAttack && Player.Selected != null)
                {
                    if (Player.AttackManager.Attacking)
                        Player.DisableAttack(pSelectedLaserItem);
                    else
                        Player.EnableAttack(pSelectedLaserItem);
                }
            }
            else
            {
                string oldSelectedItem = Player.Settings.InGameSettings.selectedLaser;
                Player.Settings.InGameSettings.selectedLaser = pSelectedLaserItem;
                SendNewItemStatus(oldSelectedItem);
                SendNewItemStatus(pSelectedLaserItem);

                if (Player.Settings.Gameplay.quickSlotStopAttack && Player.Selected != null)
                {
                    if (Player.AttackManager.Attacking)
                    {
                        Player.SendCommand(RemoveMenuItemHighlightCommand.write(new class_h2P(class_h2P.ITEMS_CONTROL), oldSelectedItem, new class_K18(class_K18.ACTIVE)));
                        Player.SendCommand(AddMenuItemHighlightCommand.write(new class_h2P(class_h2P.ITEMS_CONTROL), pSelectedLaserItem, new class_K18(class_K18.ACTIVE), new class_I1W(true, 0)));
                    } else Player.EnableAttack(pSelectedLaserItem);
                }
            }
        }

        public void SetSelectedRocketItem(string pSelectedRocketItem)
        {
            if (Player.Settings.InGameSettings.selectedRocket.Equals(pSelectedRocketItem))
            {
                if (Player.Settings.Gameplay.quickSlotStopAttack && Player.Selected != null)
                    Player.AttackManager.RocketAttack();
            }
            else
            {
                string oldSelectedItem = Player.Settings.InGameSettings.selectedRocket;
                Player.Settings.InGameSettings.selectedRocket = pSelectedRocketItem;
                SendNewItemStatus(oldSelectedItem);
                SendNewItemStatus(pSelectedRocketItem);
            }
        }

        public void SetSelectedRocketLauncherItem(string pSelectedRocketLauncherItem)
        {
            if (pSelectedRocketLauncherItem != CpuManager.ROCKET_LAUNCHER)
            {
                if (pSelectedRocketLauncherItem != Player.Settings.InGameSettings.selectedRocketLauncher)
                {
                    string oldSelectedItem = Player.Settings.InGameSettings.selectedRocketLauncher;
                    Player.Settings.InGameSettings.selectedRocketLauncher = pSelectedRocketLauncherItem;
                    SendNewItemStatus(oldSelectedItem);
                    SendNewItemStatus(pSelectedRocketLauncherItem);

                    Player.AttackManager.RocketLauncher.ChangeLoad(Player.Settings.InGameSettings.selectedRocketLauncher);

                    if (Player.Storage.AutoRocketLauncher)
                        Player.AttackManager.RocketLauncher.ReloadingActive = true;
                }
            }
        }

        public byte[] GetNewItemStatus(string pItemId, string pTooltipId, bool descriptionEnabled = true, bool doubleClickToFire = false, bool buyEnable = false)
        {

            ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(GetItemBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled, doubleClickToFire));
            ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(GetSlotBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled));

            return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                               ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                               0, false, true,
                                                               slotBarStatusTooltip, buyEnable ? true : false, LaserCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedLaser.Equals(pItemId) : 
                                                                                                               RocketsCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedRocket.Equals(pItemId) : 
                                                                                                               CpusCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedCpus.Contains(pItemId) :
                                                                                                               FormationsCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedFormation.Equals(pItemId) :
                                                                                                               false,
                                                               0).writeCommand();
        }

        public byte[] GetNewRocketLauncherItemStatus(string pItemId, string pTooltipId, int count = 0, bool descriptionEnabled = true, bool doubleClickToFire = false, bool buyEnable = false)
        {

            ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(GetItemBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled, doubleClickToFire));
            ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(GetSlotBarStatusTooltip(pItemId, pTooltipId, false, 0, descriptionEnabled));

            var counterColor = Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.HSTRM_01 ? ClientUISlotBarCategoryItemStatusModule.YELLOW :
                              Player.Settings.InGameSettings.selectedRocketLauncher == AmmunitionManager.UBR_100 ? ClientUISlotBarCategoryItemStatusModule.RED :
                                                  ClientUISlotBarCategoryItemStatusModule.BLUE;

            return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                               counterColor, pItemId,
                                                               count, false, true,
                                                               slotBarStatusTooltip, buyEnable ? true : false, RocketLauncherCategory.Contains(pItemId) ? Player.Settings.InGameSettings.selectedRocketLauncher.Equals(pItemId) : false,
                                                               5).writeCommand();
        }
    }
}
