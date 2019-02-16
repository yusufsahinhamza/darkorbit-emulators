using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ow.Game.Objects;
using Ow.Game.Objects.Mines;
using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;
using Ow.Net.netty;

namespace Ow.Game.Objects.Players.Managers
{
    public class DataBase
    {
        public int uridium { get; set; }
        public int credits { get; set; }
        public int honor { get; set; }
        public int experience { get; set; }
        public int jackpot { get; set; }
    }

    public class EquipmentBase
    {
        public int Config1Hitpoints { get; set; }
        public int Config1Damage { get; set; }
        public int Config1Shield { get; set; }
        public int Config1Speed { get; set; }
        public int Config2Hitpoints { get; set; }
        public int Config2Damage { get; set; }
        public int Config2Shield { get; set; }
        public int Config2Speed { get; set; }
    }

    class PlayerSettings
    {
        public AudioBase Audio = new AudioBase();
        public QualityBase Quality = new QualityBase();
        public DisplayBase Display = new DisplayBase();
        public GameplayBase Gameplay = new GameplayBase();
        public WindowBase Window = new WindowBase();
        public InGameSettingsBase InGameSettings = new InGameSettingsBase();
        public ShipSettingsBase ShipSettings = new ShipSettingsBase();
        public CurrentCooldownsBase CurrentCooldowns = new CurrentCooldownsBase();
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
            new BoundKeysBase(12, 0, 0, new List<int>{109})
        };

        public class AudioBase
        {
            public bool notSet = false;
            public bool sound = true;
            public bool music = true;
        }

        public class ShipSettingsBase
        {
            public string quickbarSlots = "6,-1,-1,-1,-1,-1,-1,-1,-1,-1";
            public string quickbarSlotsPremium = "77,-1,-1,-1,-1,-1,-1,-1,-1,-1";
            public int selectedLaser = 1;
            public int selectedRocket = 1;
            public int selectedRocketLauncher = -1;
        }

        public class QualityBase
        {
            public bool notSet = false;
            public short qualityAttack = 0;
            public short qualityBackground = 3;
            public short qualityPresetting = 3;
            public bool qualityCustomized = false;
            public short qualityPoizone = 3;
            public short qualityShip = 3;
            public short qualityEngine = 3;
            public short qualityExplosion = 3;
            public short qualityCollectable = 3;
            public short qualityEffect = 3;
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
            public bool displayChat = true;
            public bool displayWindowsBackground = true;
            public bool displayNotFreeCargoBoxes = true;
            public bool dragWindowsAlways = true;
            public bool displayNotifications = true;
            public bool hoverShips = true;
            public bool displayDrones = true;
            public bool displayBonusBoxes = true;
            public bool displayFreeCargoBoxes = true;
        }

        public class GameplayBase
        {
            public bool notSet = false;
            public bool autoRefinement = false;
            public bool quickSlotStopAttack = true;
            public bool autoBoost = false;
            public bool autoBuyBootyKeys = false;
            public bool doubleclickAttackEnabled = true;
            public bool autoChangeAmmo = false;
            public bool autoStartEnabled = true;
        }

        public class WindowBase
        {
            public bool notSet = false;
            public int clientResolutionId = 0;
            public string windowSettings = "0,276,8,1,1,503,7,1,3,286,130,1,5,9,7,1,10,17,357,1,13,50,10,0,16,212,242,1,20,3,320,1,23,60,183,1,24,732,211,0,36,530,130,1";
            public string resizableWindows = "5,240,150,20,316,178,36,260,130,";
            public int minmapScale = 11;
            public string mainmenuPosition = "363,393";
            public string barStatus = "100,1,23,1,24,1,25,1,26,1,27,1";
            public string slotmenuPosition = "347,334";
            public string slotMenuOrder = "0";
            public string slotMenuPremiumPosition = "363,364";
            public string slotMenuPremiumOrder = "0";
        }

        public class BoundKeysBase
        {
            public short actionType { get; set; }
            public short charCode { get; set; }
            public int parameter { get; set; }
            public List<int> keyCodes { get; set; }

            public BoundKeysBase(short ActionType, short CharCode, int Parameter, List<int> KeyCodes)
            {
                actionType = ActionType;
                charCode = CharCode;
                parameter = Parameter;
                keyCodes = KeyCodes;
            }
        }

        public class InGameSettingsBase
        {
            public bool inEquipZone = true;
            public bool blockedGroupInvites = false;
            public int selectedFormation = 0;
            public int currentConfig = 1;
            public List<string> selectedCpus = new List<string>();
        }

        public class CurrentCooldownsBase
        {
            public int smbCooldown = 0;
            public int ishCooldown = 0;
            public int empCooldown = 0;
            public int mineCooldown = 0;
            public int dcrCooldown = 0;
            public int pldCooldown = 0;
            public int energyLeechCooldown = 0;
            public int chainImpulseCooldown = 0;
            public int precisionTargeterCooldown = 0;
            public int backupShieldsCooldown = 0;
            public int battleRepairBotCooldown = 0;
            public int sentinelCooldown = 0;
            public int diminisherCooldown = 0;
            public int venomCooldown = 0;
            public int spectrumCooldown = 0;
            public int solaceCooldown = 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    class SettingsManager
    {
        public int SelectedLaser = AmmunitionTypeModule.X1;
        public int SelectedRocket = AmmunitionTypeModule.R310;
        public int SelectedRocketLauncher = AmmunitionTypeModule.HELLSTORM;
        public int SelectedFormation = DroneManager.DEFAULT_FORMATION;
        public List<string> SelectedCpus = new List<string>();

        public Player Player { get; set; }

        public SettingsManager(Player player) { Player = player; SetCurrentItems(); }

        public void SetCurrentItems()
        {
            SelectedLaser = Player.Settings.ShipSettings.selectedLaser;
            SelectedRocket = Player.Settings.ShipSettings.selectedRocket;
            SelectedRocketLauncher = Player.Settings.ShipSettings.selectedRocketLauncher;
            SelectedFormation = Player.Settings.InGameSettings.selectedFormation;
            Player.CurrentConfig = Player.Settings.InGameSettings.currentConfig;
        }

        public void SendUserKeyBindingsUpdateCommand()
        {
            var keyBindingsModuleCommands = new List<UserKeyBindingsModule>();
            List<PlayerSettings.BoundKeysBase> actions = Player.Settings.BoundKeys;

            foreach (var action in actions)
            {
                keyBindingsModuleCommands.Add(new UserKeyBindingsModule(
                    action.actionType, action.keyCodes, action.parameter, action.charCode));
            }
            Player.SendCommand(UserKeyBindingsUpdateCommand.write(keyBindingsModuleCommands, false));
        }

        public void SendSlotBarItems()
        {
            var formations = new List<int>();
            for (var i = 0; i <= 13; i++) formations.Add(i);
            Player.SendCommand(DroneFormationAvailableFormationsCommand.write(formations));

            var shipId = Player.Ship.Id;
            var skill = shipId == 63 ? 1 : shipId == 64 ? 2 : shipId == 65 ? 3 : shipId == 66 ? 4 : shipId == 67 ? 5 : 0;

            if (shipId != 0)
                Player.SendPacket("0|SD|S|" + skill + "|1");

            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|74"); // SELECTION_ROCKET_BDR_1211
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|50"); // SELECTION_LAUNCHER_ROCKET_ECO10
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|75"); // SELECTION_LAUNCHER_ROCKET_SAR_01
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|109"); // SELECTION_LAUNCHER_ROCKET_BDR_1212
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|108"); // SELECTION_LASER_JOB100
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|36"); // SELECTION_FIREWORK_1
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|37"); // SELECTION_FIREWORK_2
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|38"); // SELECTION_FIREWORK_3
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|40"); // FIREWORK_IGNITE
            Player.SendPacket("0|UI|" + ServerCommands.BUTTON + "|" + ServerCommands.HIDE_BUTTON + "|107"); // ACTIVATION_FORMATION_MOSQUITO

            Player.SendPacket("0|A|" + ServerCommands.EXTRAS_INFO + "|0|0|0|0|1||0|1|1|0|0|1|1|1|0|0|0");
            Player.SendPacket("0|A|" + ServerCommands.CPU_INFO + "|" + ServerCommands.CLOAK_CPU_INFO + "|50");
            Player.SendPacket("0|" + ServerCommands.ROCKETLAUNCHER + "|" + ServerCommands.ROCKETLAUNCHER_STATUS + "|2|" + Player.AttackManager.GetSelectedLauncherId() + "|0");

            Player.TechManager.SendTechStatus();

            if (Player.Settings.InGameSettings.selectedCpus.Contains(CpuManager.AUTO_ROCKET_CPU))
                Player.CpuManager.EnableArolX();

            if (Player.Settings.InGameSettings.selectedCpus.Contains(CpuManager.AUTO_HELLSTROM_CPU))
                Player.CpuManager.EnableRllbX();

            if (Player.Settings.InGameSettings.selectedCpus.Contains(CpuManager.CLK_XL))
                Player.CpuManager.EnableCloak();

            var items = new List<AmmunitionCountModule>();
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.X1), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.X2), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.X3), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.X4), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.RSB), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.SAB), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.CBO), 1000));

            Player.SendCommand(AmmunitionCountUpdateCommand.write(items));
            items.Clear();

            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.R310), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.PLT2026), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.PLT2021), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.PLT3030), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.DECELERATION), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.WIZARD), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.PLASMA), 1000));

            Player.SendCommand(AmmunitionCountUpdateCommand.write(items));
            items.Clear();

            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.MINE), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.MINE_DD), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.MINE_EMP), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.MINE_SAB), 1000));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.MINE_SL), 1000));

            Player.SendCommand(AmmunitionCountUpdateCommand.write(items));
            items.Clear();

            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.EMP), 100));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.INSTANT_SHIELD), 100));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.SMARTBOMB), 100));

            Player.SendCommand(AmmunitionCountUpdateCommand.write(items));
            items.Clear();

            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.HELLSTORM), 100));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.UBER_ROCKET), 100));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.SAR02), 100));
            items.Add(new AmmunitionCountModule(new AmmunitionTypeModule(AmmunitionTypeModule.CBR), 100));

            Player.SendCommand(AmmunitionCountUpdateCommand.write(items));
        }

        public void SendRemoveWindows()
        {
            Player.SendPacket("0|" + ServerCommands.USER_INTERFACE + "|" + ServerCommands.WINDOW + "|" + ServerCommands.SHOW_WINDOW + "|36");
            Player.SendPacket("0|" + ServerCommands.USER_INTERFACE + "|" + ServerCommands.WINDOW + "|" + ServerCommands.HIDE_WINDOW + "|10");
            Player.SendPacket("0|" + ServerCommands.USER_INTERFACE + "|" + ServerCommands.WINDOW + "|" + (EventManager.Spaceball.Active ? ServerCommands.SHOW_WINDOW : ServerCommands.HIDE_WINDOW) + "|16");
        }

        public void SendUserSettingsCommand()
        {
            var displaySettings = Player.Settings.Display;
            var qualitySettings = Player.Settings.Quality;
            var audioSettings = Player.Settings.Audio;
            var windowSettings = Player.Settings.Window;
            var gameplaySettings = Player.Settings.Gameplay;
            var shipSettings = Player.Settings.ShipSettings;

            Player.SendCommand(UserSettingsCommand.write(
                new QualitySettingsModule(qualitySettings.notSet, qualitySettings.qualityAttack, qualitySettings.qualityBackground, qualitySettings.qualityPresetting, qualitySettings.qualityCustomized, qualitySettings.qualityPoizone, qualitySettings.qualityShip, qualitySettings.qualityEngine, qualitySettings.qualityExplosion, qualitySettings.qualityCollectable, qualitySettings.qualityEffect),
                new DisplaySettingsModule(displaySettings.notSet, displaySettings.displayPlayerNames, displaySettings.displayResources, displaySettings.displayBonusBoxes, displaySettings.displayHitpointBubbles, displaySettings.displayChat, displaySettings.displayDrones, displaySettings.displayFreeCargoBoxes, displaySettings.displayNotFreeCargoBoxes, displaySettings.displayWindowsBackground, displaySettings.displayNotifications, displaySettings.preloadUserShips, displaySettings.dragWindowsAlways, displaySettings.hoverShips, true, displaySettings.allowAutoQuality),
                new AudioSettingsModule(audioSettings.notSet, audioSettings.sound, audioSettings.music),
                new WindowSettingsModule(windowSettings.notSet, windowSettings.clientResolutionId, windowSettings.windowSettings, windowSettings.resizableWindows, windowSettings.minmapScale, windowSettings.mainmenuPosition, windowSettings.barStatus, windowSettings.slotmenuPosition, windowSettings.slotMenuOrder, windowSettings.slotMenuPremiumPosition, windowSettings.slotMenuPremiumOrder),
                new GameplaySettingsModule(gameplaySettings.notSet, gameplaySettings.autoBoost, gameplaySettings.autoRefinement, gameplaySettings.quickSlotStopAttack, gameplaySettings.doubleclickAttackEnabled, gameplaySettings.autoChangeAmmo, gameplaySettings.autoStartEnabled, gameplaySettings.autoBuyBootyKeys)
            ));

            Player.SendCommand(ShipSettingsCommand.write(shipSettings.quickbarSlots, shipSettings.quickbarSlotsPremium, Player.AttackManager.GetSelectedLaser() + 1, Player.AttackManager.GetSelectedRocket(), Player.AttackManager.GetSelectedLauncherId()));
        }
    }
}
