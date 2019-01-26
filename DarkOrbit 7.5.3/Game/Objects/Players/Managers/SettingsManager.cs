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
    class PlayerSettings
    {
        public AudioBase Audio { get; set; }
        public QualityBase Quality { get; set; }
        public DisplayBase Display { get; set; }
        public GameplayBase Gameplay { get; set; }
        public WindowBase Window { get; set; }
        public InGameSettingsBase InGameSettings { get; set; }
        public ShipSettingsBase ShipSettings { get; set; }
        public CurrentCooldownsBase CurrentCooldowns { get; set; }
        public List<BoundKeysBase> BoundKeys { get; set; }

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

        public class AudioBase
        {
            public bool notSet { get; set; }
            public bool sound { get; set; }
            public bool music { get; set; }
        }

        public class ShipSettingsBase
        {
            public string quickbarSlots { get; set; }
            public string quickbarSlotsPremium { get; set; }
            public int selectedLaser { get; set; }
            public int selectedRocket { get; set; }
            public int selectedRocketLauncher { get; set; }
        }

        public class QualityBase
        {
            public bool notSet { get; set; }
            public short qualityAttack { get; set; }
            public short qualityBackground { get; set; }
            public short qualityPresetting { get; set; }
            public bool qualityCustomized { get; set; }
            public short qualityPoizone { get; set; }
            public short qualityShip { get; set; }
            public short qualityEngine { get; set; }
            public short qualityExplosion { get; set; }
            public short qualityCollectable { get; set; }
            public short qualityEffect { get; set; }
        }

        public class DisplayBase
        {
            public bool notSet { get; set; }
            public bool displayPlayerNames { get; set; }
            public bool displayResources { get; set; }
            public bool showPremiumQuickslotBar { get; set; }
            public bool allowAutoQuality { get; set; }
            public bool preloadUserShips { get; set; }
            public bool displayHitpointBubbles { get; set; }
            public bool displayChat { get; set; }
            public bool displayWindowsBackground { get; set; }
            public bool displayNotFreeCargoBoxes { get; set; }
            public bool dragWindowsAlways { get; set; }
            public bool displayNotifications { get; set; }
            public bool hoverShips { get; set; }
            public bool displayDrones { get; set; }
            public bool displayBonusBoxes { get; set; }
            public bool displayFreeCargoBoxes { get; set; }
        }

        public class GameplayBase
        {
            public bool notSet { get; set; }
            public bool autoRefinement { get; set; }
            public bool quickSlotStopAttack { get; set; }
            public bool autoBoost { get; set; }
            public bool autoBuyBootyKeys { get; set; }
            public bool doubleclickAttackEnabled { get; set; }
            public bool autoChangeAmmo { get; set; }
            public bool autoStartEnabled { get; set; }
        }

        public class WindowBase
        {
            public bool notSet { get; set; }
            public int clientResolutionId { get; set; }
            public string windowSettings { get; set; }
            public string resizableWindows { get; set; }
            public int minmapScale { get; set; }
            public string mainmenuPosition { get; set; }
            public string barStatus { get; set; }
            public string slotmenuPosition { get; set; }
            public string slotMenuOrder { get; set; }
            public string slotMenuPremiumPosition { get; set; }
            public string slotMenuPremiumOrder { get; set; }
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
            public bool inEquipZone { get; set; }
            public bool blockedGroupInvites { get; set; }
            public int selectedFormation { get; set; }
            public int currentConfig { get; set; }
            public List<string> selectedCpus { get; set; }
        }

        public class CurrentCooldownsBase
        {
            public int smbCooldown { get; set; }
            public int ishCooldown { get; set; }
            public int empCooldown { get; set; }
            public int mineCooldown { get; set; }
            public int dcrCooldown { get; set; }
            public int pldCooldown { get; set; }
            public int energyLeechCooldown { get; set; }
            public int chainImpulseCooldown { get; set; }
            public int precisionTargeterCooldown { get; set; }
            public int backupShieldsCooldown { get; set; }
            public int battleRepairBotCooldown { get; set; }
            public int sentinelCooldown { get; set; }
            public int diminisherCooldown { get; set; }
            public int venomCooldown { get; set; }
            public int spectrumCooldown { get; set; }
            public int solaceCooldown { get; set; }
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
