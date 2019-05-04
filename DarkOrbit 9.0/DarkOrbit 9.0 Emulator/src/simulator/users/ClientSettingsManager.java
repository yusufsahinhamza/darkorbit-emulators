package simulator.users;

import mysql.QueryManager;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;
import java.util.Vector;

import simulator.logic.LaserAttack;
import simulator.netty.serverCommands.AudioSettingsModule;
import simulator.netty.serverCommands.ClientUIMenuBarItemModule;
import simulator.netty.serverCommands.ClientUIMenuBarModule;
import simulator.netty.serverCommands.ClientUIMenuBarsCommand;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemStatusModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemTimerModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemTimerStateModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryModule;
import simulator.netty.serverCommands.ClientUISlotBarItemModule;
import simulator.netty.serverCommands.ClientUISlotBarModule;
import simulator.netty.serverCommands.ClientUISlotBarsCommand;
import simulator.netty.serverCommands.ClientUITextReplacementModule;
import simulator.netty.serverCommands.ClientUITooltipModule;
import simulator.netty.serverCommands.ClientUITooltipTextFormatModule;
import simulator.netty.serverCommands.ClientUITooltipsCommand;
import simulator.netty.serverCommands.CooldownTypeModule;
import simulator.netty.serverCommands.DisplaySettingsModule;
import simulator.netty.serverCommands.GameplaySettingsModule;
import simulator.netty.serverCommands.QualitySettingsModule;
import simulator.netty.serverCommands.UserKeyBindingsModuleCommand;
import simulator.netty.serverCommands.UserKeyBindingsUpdateCommand;
import simulator.netty.serverCommands.UserSettingsCommand;
import simulator.netty.serverCommands.WindowSettingsModule;
import simulator.netty.serverCommands.class_592;
import utils.Settings;

/**
 This class will manage client graphics settings, client UI (bars) etc
 Edited & Fixed by LEJYONER.
 */

public class ClientSettingsManager {

    public static final String STANDARD_SLOT_BAR = "standardSlotBar";
    public static final String PREMIUM_SLOT_BAR  = "premiumSlotBar";

    private final Account mAccount;

    private final ClientSettings mClientSettings = new ClientSettings();
    // client graphics, audio etc settings
    //    private UserSettingsCommand mUserSettingsCommand;

    private ClientUIMenuBarsCommand mClientUIMenuBarsCommand;

    private Map<Short, String> mSlotBarItems        = new TreeMap<>();
    private Map<Short, String> mPremiumSlotBarItems = new TreeMap<>();

    private String mSelectedLaserItem  = AmmunitionManager.LCB_10;
    private String mSelectedRocketItem = AmmunitionManager.R_310;
    private String mSelectedRocketLauncherItem = AmmunitionManager.ECO_10;
    
    // 1. slot bar items [10]
    // 2. premium slot bar items [10]
    // 3. settings
    // 4. top left bar items (booleans to show)
    // 5.

    public ClientSettingsManager(final Account pAccount) {
        this.mAccount = pAccount;
    }

    public void setFromJSON(final String pClientSettingsJSON) {
        try {
            final JSONObject settings = new JSONObject(pClientSettingsJSON);
            final JSONObject audioSettings = settings.getJSONObject("audio");
            final JSONObject qualitySettings = settings.getJSONObject("quality");
            final JSONObject class592Settings = settings.getJSONObject("class592"); //gereksiz
            final JSONObject displaySettings = settings.getJSONObject("display");
            final JSONObject gameplaySettings = settings.getJSONObject("gameplay");
            final JSONObject windowSettings = settings.getJSONObject("window");
            final JSONArray boundKeysJson = settings.getJSONArray("boundKeys");
            final JSONObject slotbarItems = settings.getJSONObject("slotbarItems");
            final JSONObject premiumSotbarItems = settings.getJSONObject("premiumSlotbarItems");
            
            //Ses ayarları başlangıç
            this.getClientSettings()
            .getAudio()
            .setNotSet(audioSettings.getBoolean("notSet"));
        this.getClientSettings()
            .getAudio()
            .setPlayCombatMusic(audioSettings.getBoolean("playCombatMusic"));
        this.getClientSettings()
            .getAudio()
            .setMusic(audioSettings.getInt("music"));
        this.getClientSettings()
            .getAudio()
            .setSound(audioSettings.getInt("sound"));
        this.getClientSettings()
            .getAudio()
            .setVoice(audioSettings.getInt("voice"));
            //Ses ayarları son

            //Kalite ayarları başlangıç
        this.getClientSettings()
        .getQuality()
        .setNotSet(qualitySettings.getBoolean("notSet"));
        this.getClientSettings()
        .getQuality()
        .setQualityAttack(qualitySettings.getInt("qualityAttack"));
        this.getClientSettings()
        .getQuality()
        .setQualityBackground(qualitySettings.getInt("qualityBackground"));
        this.getClientSettings()
        .getQuality()
        .setQualityPresetting(qualitySettings.getInt("qualityPresetting"));
        this.getClientSettings()
        .getQuality()
        .setQualityCustomized(qualitySettings.getBoolean("qualityCustomized"));
        this.getClientSettings()
        .getQuality()
        .setQualityPoizone(qualitySettings.getInt("qualityPoizone"));
        this.getClientSettings()
        .getQuality()
        .setQualityShip(qualitySettings.getInt("qualityShip"));
        this.getClientSettings()
        .getQuality()
        .setQualityEngine(qualitySettings.getInt("qualityEngine"));
        this.getClientSettings()
        .getQuality()
        .setQualityExplosion(qualitySettings.getInt("qualityExplosion"));
        this.getClientSettings()
        .getQuality()
        .setQualityCollectable(qualitySettings.getInt("qualityCollectable"));
        this.getClientSettings()
        .getQuality()
        .setQualityEffect(qualitySettings.getInt("qualityEffect"));
            //Kalite ayarları son
        
            //Görüntüleme ayarları başlangıç
        this.getClientSettings()
        .getDisplay()
        .setNotSet(displaySettings.getBoolean("notSet"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayPlayerNames(displaySettings.getBoolean("displayPlayerNames"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayResources(displaySettings.getBoolean("displayResources"));
        this.getClientSettings()
        .getDisplay()
        .setShowPremiumQuickslotBar(displaySettings.getBoolean("showPremiumQuickslotBar"));
        this.getClientSettings()
        .getDisplay()
        .setAllowAutoQuality(displaySettings.getBoolean("allowAutoQuality"));
        this.getClientSettings()
        .getDisplay()
        .setPreloadUserShips(displaySettings.getBoolean("preloadUserShips"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayHitpointBubbles(displaySettings.getBoolean("displayHitpointBubbles"));
        this.getClientSettings()
        .getDisplay()
        .setShowNotOwnedItems(displaySettings.getBoolean("showNotOwnedItems"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayChat(displaySettings.getBoolean("displayChat"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayWindowsBackground(displaySettings.getBoolean("displayWindowsBackground"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayNotFreeCargoBoxes(displaySettings.getBoolean("displayNotFreeCargoBoxes"));
        this.getClientSettings()
        .getDisplay()
        .setDragWindowsAlways(displaySettings.getBoolean("dragWindowsAlways"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayNotifications(displaySettings.getBoolean("displayNotifications"));
        this.getClientSettings()
        .getDisplay()
        .setHoverShips(displaySettings.getBoolean("hoverShips"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayDrones(displaySettings.getBoolean("displayDrones"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayBonusBoxes(displaySettings.getBoolean("displayBonusBoxes"));
        this.getClientSettings()
        .getDisplay()
        .setDisplayFreeCargoBoxes(displaySettings.getBoolean("displayFreeCargoBoxes"));
            //Görüntüleme ayarları son
        
            //Oynayış ayarları başlangıç
        this.getClientSettings()
        .getGameplay()
        .setNotSet(gameplaySettings.getBoolean("notSet"));
        this.getClientSettings()
        .getGameplay()
        .setAutoRefinement(gameplaySettings.getBoolean("autoRefinement"));
        this.getClientSettings()
        .getGameplay()
        .setQuickSlotStopAttack(gameplaySettings.getBoolean("quickSlotStopAttack"));
        this.getClientSettings()
        .getGameplay()
        .setAutoBoost(gameplaySettings.getBoolean("autoBoost"));
        this.getClientSettings()
        .getGameplay()
        .setAutoBuyBootyKeys(gameplaySettings.getBoolean("autoBuyBootyKeys"));
        this.getClientSettings()
        .getGameplay()
        .setDoubleclickAttackEnabled(gameplaySettings.getBoolean("doubleclickAttackEnabled"));
        this.getClientSettings()
        .getGameplay()
        .setAutochangeAmmo(gameplaySettings.getBoolean("autochangeAmmo"));
        this.getClientSettings()
        .getGameplay()
        .setAutoStartEnabled(gameplaySettings.getBoolean("autoStartEnabled"));
            //Oynayış ayarları son
        
            //Pencere ayarları başlangıç
        this.getClientSettings()
        .getWindow()
        .setHideAllWindows(windowSettings.getBoolean("hideAllWindows"));
        this.getClientSettings()
        .getWindow()
        .setScale(windowSettings.getInt("mScale"));
        this.getClientSettings()
        .getWindow()
        .setBarState(windowSettings.getString("mBarState"));
            //Pencere ayarları son
        
            for (int i = 0; i < boundKeysJson.length(); i++) {
                final JSONObject boundKey = boundKeysJson.getJSONObject(i);
                final JSONArray keyCodesJson = boundKey.getJSONArray("keyCodes");
                final Vector<Integer> keyCodes = new Vector<>();
                for (int i2 = 0; i2 < keyCodesJson.length(); i2++) {
                    keyCodes.add(keyCodesJson.getInt(i2));
                }
                this.mClientSettings.getKeys()
                                    .addAction((short) boundKey.getInt("actionType"),
                                               (short) boundKey.getInt("charCode"), boundKey.getInt("parameter"),
                                               keyCodes);
            }
            final Iterator<String> slotbarItemsIterator = slotbarItems.keys();
            while (slotbarItemsIterator.hasNext()) {
                final String key = slotbarItemsIterator.next();
                final String value = slotbarItems.getString(key);
                this.mSlotBarItems.put(Short.valueOf(key), value);
            }
            final Iterator<String> premiumSotbarItemsIterator = premiumSotbarItems.keys();
            while (premiumSotbarItemsIterator.hasNext()) {
                final String key = premiumSotbarItemsIterator.next();
                final String value = premiumSotbarItems.getString(key);
                this.mPremiumSlotBarItems.put(Short.valueOf(key), value);
            }
        } catch (JSONException e) {
        }
    }

    public String packToJSON() {
        final JSONObject settings = new JSONObject();
        final JSONObject audio = new JSONObject();
        audio.put("notSet", this.mClientSettings.getAudio()
                                                .isNotSet());
        audio.put("playCombatMusic", this.mClientSettings.getAudio()
                                                         .isPlayCombatMusic());
        audio.put("music", this.mClientSettings.getAudio()
                                               .getMusic());
        audio.put("sound", this.mClientSettings.getAudio()
                                               .getSound());
        audio.put("voice", this.mClientSettings.getAudio()
                                               .getVoice());
        settings.put("audio", audio);
        final JSONObject quality = new JSONObject();
        quality.put("notSet", this.mClientSettings.getQuality()
                                                  .isNotSet());
        quality.put("qualityAttack", this.mClientSettings.getQuality()
                                                         .getQualityAttack());
        quality.put("qualityBackground", this.mClientSettings.getQuality()
                                                             .getQualityBackground());
        quality.put("qualityPresetting", this.mClientSettings.getQuality()
                                                             .getQualityPresetting());
        quality.put("qualityCustomized", this.mClientSettings.getQuality()
                                                             .getQualityCustomized());
        quality.put("qualityPoizone", this.mClientSettings.getQuality()
                                                          .getQualityPoizone());
        quality.put("qualityShip", this.mClientSettings.getQuality()
                                                       .getQualityShip());
        quality.put("qualityEngine", this.mClientSettings.getQuality()
                                                         .getQualityEngine());
        quality.put("qualityExplosion", this.mClientSettings.getQuality()
                                                            .getQualityExplosion());
        quality.put("qualityCollectable", this.mClientSettings.getQuality()
                                                              .getQualityCollectable());
        quality.put("qualityEffect", this.mClientSettings.getQuality()
                                                         .getQualityEffect());
        settings.put("quality", quality);
        final JSONObject class592 = new JSONObject();
        class592.put("notSet", this.mClientSettings.getQuality()
                                                   .isNotSet());
        class592.put("questsAvailableFilter", this.mClientSettings.getClass_592()
                                                                  .isQuestsAvailableFilter());
        class592.put("questsUnavailableFilter", this.mClientSettings.getClass_592()
                                                                    .isQuestsUnavailableFilter());
        class592.put("questsCompletedFilter", this.mClientSettings.getClass_592()
                                                                  .isQuestsCompletedFilter());
        class592.put("var_1151", this.mClientSettings.getClass_592()
                                                     .isVar_1151());
        class592.put("var_2239", this.mClientSettings.getClass_592()
                                                     .isVar_2239());
        class592.put("questsLevelOrderDescending", this.mClientSettings.getClass_592()
                                                                       .isQuestsLevelOrderDescending());
        settings.put("class592", class592);
        final JSONObject display = new JSONObject();
        display.put("notSet", this.mClientSettings.getDisplay()
                                                  .isNotSet());
        display.put("displayPlayerNames", this.mClientSettings.getDisplay()
                                                              .isDisplayPlayerNames());
        display.put("displayResources", this.mClientSettings.getDisplay()
                                                            .isDisplayResources());
        display.put("showPremiumQuickslotBar", this.mClientSettings.getDisplay()
                                                                   .isShowPremiumQuickslotBar());
        display.put("allowAutoQuality", this.mClientSettings.getDisplay()
                                                            .isAllowAutoQuality());
        display.put("preloadUserShips", this.mClientSettings.getDisplay()
                                                            .isPreloadUserShips());
        display.put("displayHitpointBubbles", this.mClientSettings.getDisplay()
                                                                  .isDisplayHitpointBubbles());
        display.put("showNotOwnedItems", this.mClientSettings.getDisplay()
                                                             .isShowNotOwnedItems());
        display.put("displayChat", this.mClientSettings.getDisplay()
                                                       .isDisplayChat());
        display.put("displayWindowsBackground", this.mClientSettings.getDisplay()
                                                                    .isDisplayWindowsBackground());
        display.put("displayNotFreeCargoBoxes", this.mClientSettings.getDisplay()
                                                                    .isDisplayNotFreeCargoBoxes());
        display.put("dragWindowsAlways", this.mClientSettings.getDisplay()
                                                             .isDragWindowsAlways());
        display.put("displayNotifications", this.mClientSettings.getDisplay()
                                                                .isDisplayNotifications());
        display.put("hoverShips", this.mClientSettings.getDisplay()
                                                      .isHoverShips());
        display.put("displayDrones", this.mClientSettings.getDisplay()
                                                         .isDisplayDrones());
        display.put("displayBonusBoxes", this.mClientSettings.getDisplay()
                                                             .isDisplayBonusBoxes());
        display.put("displayFreeCargoBoxes", this.mClientSettings.getDisplay()
                                                                 .isDisplayFreeCargoBoxes());
        settings.put("display", display);

        final JSONObject gameplay = new JSONObject();
        gameplay.put("notSet", this.mClientSettings.getGameplay()
                                                   .isNotSet());
        gameplay.put("autoRefinement", this.mClientSettings.getGameplay()
                                                           .isAutoRefinement());
        gameplay.put("quickSlotStopAttack", this.mClientSettings.getGameplay()
                                                                .isQuickSlotStopAttack());
        gameplay.put("autoBoost", this.mClientSettings.getGameplay()
                                                      .isAutoBoost());
        gameplay.put("autoBuyBootyKeys", this.mClientSettings.getGameplay()
                                                             .isAutoBuyBootyKeys());
        gameplay.put("doubleclickAttackEnabled", this.mClientSettings.getGameplay()
                                                                     .isDoubleclickAttackEnabled());
        gameplay.put("autochangeAmmo", this.mClientSettings.getGameplay()
                                                           .isAutochangeAmmo());
        gameplay.put("autoStartEnabled", this.mClientSettings.getGameplay()
                                                             .isAutoStartEnabled());
        settings.put("gameplay", gameplay);

        final JSONObject window = new JSONObject();
        window.put("hideAllWindows", this.mClientSettings.getWindow()
                                                         .isHideAllWindows());
        window.put("mScale", this.mClientSettings.getWindow()
                                                 .getScale());
        window.put("mBarState", this.mClientSettings.getWindow()
                                                    .getBarState());
        settings.put("window", window);

        final JSONArray boundKeys = new JSONArray();
        for (final ClientSettings.Keys.Actions action : this.mClientSettings.getKeys()
                                                                            .getActions()) {
            final JSONObject boundKey = new JSONObject();
            boundKey.put("actionType", action.mActionType);
            boundKey.put("charCode", action.mCharCode);
            boundKey.put("parameter", action.mParameter);
            final JSONArray keyCodes = new JSONArray();
            for (final Integer keyCode : action.mKeyCodes) {
                keyCodes.put(keyCode);
            }
            boundKey.put("keyCodes", keyCodes);
            boundKeys.put(boundKey);
        }
        settings.put("boundKeys", boundKeys);

        final JSONObject slotbarItems = new JSONObject();
        final Iterator<Map.Entry<Short, String>> slotbarItemsIterator = this.mSlotBarItems.entrySet()
                                                                                          .iterator();
        while (slotbarItemsIterator.hasNext()) {
            final Map.Entry<Short, String> slotbarItemsEntry = slotbarItemsIterator.next();
            final short index = slotbarItemsEntry.getKey();
            final String item = slotbarItemsEntry.getValue();
            slotbarItems.put(String.valueOf(index), item);
        }
        settings.put("slotbarItems", slotbarItems);
        final JSONObject premiumSlotbarItems = new JSONObject();
        final Iterator<Map.Entry<Short, String>> premiumSlotbarItemsIterator = this.mPremiumSlotBarItems.entrySet()
                                                                                                        .iterator();
        while (premiumSlotbarItemsIterator.hasNext()) {
            final Map.Entry<Short, String> premiumslotbarItemsEntry = premiumSlotbarItemsIterator.next();
            final short index = premiumslotbarItemsEntry.getKey();
            final String item = premiumslotbarItemsEntry.getValue();
            premiumSlotbarItems.put(String.valueOf(index), item);
        }
        settings.put("premiumSlotbarItems", premiumSlotbarItems);
        return settings.toString();
    }

    public UserSettingsCommand getUserSettingsCommand() {
        // TODO init from JSON (not "defaults")

        // SETTINGS TAB # (Name inside game)
        // TAB 1 (Display)
        final QualitySettingsModule qualitySettings = new QualitySettingsModule(this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .isNotSet(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityAttack(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityBackground(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityPresetting(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityCustomized(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityPoizone(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityShip(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityEngine(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityExplosion(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityCollectable(),
                                                                                this.getClientSettings()
                                                                                    .getQuality()
                                                                                    .getQualityEffect());
        // TAB 3 (Interface)
        final DisplaySettingsModule displaySettings = new DisplaySettingsModule(this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isNotSet(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayPlayerNames(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayResources(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayBonusBoxes(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayHitpointBubbles(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayChat(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayDrones(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayFreeCargoBoxes(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayNotFreeCargoBoxes(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isShowNotOwnedItems(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayWindowsBackground(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDisplayNotifications(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isPreloadUserShips(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isDragWindowsAlways(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isHoverShips(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isShowPremiumQuickslotBar(),
                                                                                this.getClientSettings()
                                                                                    .getDisplay()
                                                                                    .isAllowAutoQuality());
        // TAB 4 (Sound)
        final AudioSettingsModule audioSettings = new AudioSettingsModule(this.getClientSettings()
                                                                              .getAudio()
                                                                              .isNotSet(), this.getClientSettings()
                                                                                               .getAudio()
                                                                                               .getSound(),
                                                                          this.getClientSettings()
                                                                              .getAudio()
                                                                              .getMusic(), this.getClientSettings()
                                                                                               .getAudio()
                                                                                               .getVoice(),
                                                                          this.getClientSettings()
                                                                              .getAudio()
                                                                              .isPlayCombatMusic());
        // TAB
        // WINDOWS 6, "25,1|23,1|24,1|100,1|", false
        final WindowSettingsModule windowSettings = new WindowSettingsModule(this.getClientSettings()
                                                                                 .getWindow()
                                                                                 .getScale(), this.getClientSettings()
                                                                                                  .getWindow()
                                                                                                  .getBarState(),
                                                                             this.getClientSettings()
                                                                                 .getWindow()
                                                                                 .isHideAllWindows());
        // TAB 2 (Gameplay)
        final GameplaySettingsModule gameplaySettings = new GameplaySettingsModule(this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isNotSet(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isAutoBoost(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isAutoRefinement(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isQuickSlotStopAttack(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isDoubleclickAttackEnabled(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isAutochangeAmmo(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isAutoStartEnabled(),
                                                                                   this.getClientSettings()
                                                                                       .getGameplay()
                                                                                       .isAutoBuyBootyKeys(), false);
        // TAB
        final class_592 c592 = new class_592(this.getClientSettings()
                                                 .getClass_592()
                                                 .isQuestsAvailableFilter(), this.getClientSettings()
                                                                                 .getClass_592()
                                                                                 .isQuestsUnavailableFilter(),
                                             this.getClientSettings()
                                                 .getClass_592()
                                                 .isQuestsCompletedFilter(), this.getClientSettings()
                                                                                 .getClass_592()
                                                                                 .isVar_1151(), this.getClientSettings()
                                                                                                    .getClass_592()
                                                                                                    .isVar_2239(),
                                             this.getClientSettings()
                                                 .getClass_592()
                                                 .isQuestsLevelOrderDescending());

        // all tabs -> one command
        return new UserSettingsCommand(audioSettings, qualitySettings, c592, displaySettings, gameplaySettings,
                                       windowSettings);
    }

    private void createClientUIMenuBarsCommand() {
        // TODO init from JSON (not "defaults")

        final Vector<ClientUIMenuBarModule> menuBarsCommand = new Vector<>();

        /** TOP LEFT BAR **/
        //create Map<itemID, baseKey> of left UI items
        Map<String, String> leftItems = new LinkedHashMap<>();
        leftItems.put("user", "title_user");
        leftItems.put("ship", "title_ship");
        leftItems.put("ship_warp", "ttip_shipWarp_btn");
        if (this.getAccount().isAdmin()) {
        	leftItems.put("group", "title_group");
        	leftItems.put("refinement", "title_refinement");
        	leftItems.put("jackpot_status_ui", "title_jackpot_status_ui");
        	//leftItems.put("quests", "title_quests");
        }
        leftItems.put("chat", "title_chat");
        leftItems.put("minimap", "title_map");
        leftItems.put("log", "title_log");
        leftItems.put("word_puzzle", "title_wordpuzzle");
        if (Settings.SPACEBALL_ENABLED) {
        	leftItems.put("spaceball", "title_spaceball");
        }
        leftItems.put("pet", "title_pet");
        
        //leftItems.put("invasion", "title_invasion");
        
        //spaceball, title_spaceball
        //invasion, title_invasion
        //ctb, title_ctb
        //tdm, title_tdm
        //ranked_hunt, title_ranked_hunt
        //highscoregate, title_highscoregate
        //scoreevent, title_scoreevent
        //infiltration, title_ifg
        //word_puzzle, title_wordpuzzle
        //sectorcontrol, title_sectorcontrol_ingame_status
        //jackpot_status_ui, title_jackpot_status_ui
        //curcubitor, httip_countdownHalloweenNPCs
        //influence, httip_influence
        //domination, title_domination
        //traininggrounds, title_traininggrounds
        //company_hq, ttip_company_hq

        Vector<ClientUIMenuBarItemModule> topLeftMenuBarItems = new Vector<>();
        for (final Map.Entry<String, String> entryLeft : leftItems.entrySet()) {
            String itemID = entryLeft.getKey();
            String baseKey = entryLeft.getValue();

            ClientUITooltipTextFormatModule tf_localized =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);
            Vector<ClientUITooltipModule> tooltips = new Vector<>();
            Vector<ClientUITextReplacementModule> textReplacementModules = new Vector<>();
            tooltips.add(new ClientUITooltipModule(tf_localized, ClientUITooltipModule.STANDARD, baseKey,
                                                   textReplacementModules));
            ClientUITooltipsCommand tooltipsCommand = new ClientUITooltipsCommand(tooltips);
            ClientUIMenuBarItemModule menuBarItem = new ClientUIMenuBarItemModule(true, tooltipsCommand, itemID);

            topLeftMenuBarItems.add(menuBarItem);
        }
        ClientUIMenuBarModule topLeftMenuBar =
                new ClientUIMenuBarModule("0,0", ClientUIMenuBarModule.GAME_FEATURE_BAR, topLeftMenuBarItems, "0");
        menuBarsCommand.add(topLeftMenuBar);

        /** TOP RIGHT BAR **/
        //create Map<itemID, baseKey> of right UI items
        Map<String, String> rightItems = new LinkedHashMap<>();
        rightItems.put("fullscreen", "ttip_fullscreen_btn");
        rightItems.put("settings", "title_settings");
        rightItems.put("help", "title_help");
        rightItems.put("logout", "title_logout");

        Vector<ClientUIMenuBarItemModule> topRightMenuBarItems = new Vector<>();
        for (final Map.Entry<String, String> entryRight : rightItems.entrySet()) {
            String itemID = entryRight.getKey();
            String baseKey = entryRight.getValue();

            ClientUITooltipTextFormatModule tf_localized =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);
            Vector<ClientUITooltipModule> tooltips = new Vector<>();
            Vector<ClientUITextReplacementModule> textReplacementModules = new Vector<>();
            tooltips.add(new ClientUITooltipModule(tf_localized, (short) 0, baseKey, textReplacementModules));
            ClientUITooltipsCommand tooltipsCommand = new ClientUITooltipsCommand(tooltips);
            ClientUIMenuBarItemModule menuBarItem = new ClientUIMenuBarItemModule(true, tooltipsCommand, itemID);

            topRightMenuBarItems.add(menuBarItem);
        }
        ClientUIMenuBarModule topRightMenuBar =
                new ClientUIMenuBarModule("98.3,0", ClientUIMenuBarModule.GENERIC_FEATURE_BAR, topRightMenuBarItems,
                                          "0");
        menuBarsCommand.add(topRightMenuBar);

        /** 3rd BAR **/
        // XXX unknown purpose of this bar
        ClientUIMenuBarModule bar3 = new ClientUIMenuBarModule("", ClientUIMenuBarModule.NOT_ASSIGNED,
                                                               new Vector<ClientUIMenuBarItemModule>(), "");
        menuBarsCommand.add(bar3);

        this.mClientUIMenuBarsCommand = new ClientUIMenuBarsCommand(menuBarsCommand);

    }

    /**
     Description: Parses json slot menu order to itemsSlotBar list

     @param slotMenuOrder:
     slot menu order json array string
     */
    private void parseItemsSlotBar(final String slotMenuOrder) {
        final JSONArray itemsID = new JSONArray(slotMenuOrder);
        final int len = itemsID.length();
        for (int i = 0; i < len; i++) {
            final String itemID = itemsID.getString(i);
            this.mSlotBarItems.put((short) i, itemID);
        }
    }

    /**
     Description: Parses json premium slot menu order to itemsPremiumSlotBar list

     @param slotMenuPremiumOrder:
     slot menu order json array string
     */
    private void parseItemsPremiumSlotBar(final String slotMenuPremiumOrder) {
        final JSONArray itemsID = new JSONArray(slotMenuPremiumOrder);
        final int len = itemsID.length();
        for (int i = 0; i < len; i++) {
            final String itemID = itemsID.getString(i);
            this.mPremiumSlotBarItems.put((short) i, itemID);
        }
    }

    public ClientUISlotBarsCommand getClientUISlotBarsCommand() {
        /** CREATING SLOTBARS **/
        final Vector<ClientUISlotBarModule> slotBars = new Vector<>();
        slotBars.add(this.getStandardSlotBar());
   //     if (this.getAccount()
   //             .isPremiumAccount()) {
            slotBars.add(this.getPremiumSlotBar());
   //     }
        /** END CREATING SLOTBARS **/

        /** CREATING CATEGORIES **/
        Vector<ClientUISlotBarCategoryModule> categories = new Vector<>();
        categories.add(this.getAccount()
                 .getAmmunitionManager()
                 .getLasersCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getRocketsCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getRocketLauncherCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getSpecialAmmoCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getMinesCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getCpusCategory());
        categories.add(this.getAccount()
        		.getAmmunitionManager()
        		.getBuyCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getTechCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getAbilityCategory());
        categories.add(this.getAccount()
                .getAmmunitionManager()
                .getDroneFormationsCategory());

        return new ClientUISlotBarsCommand("50,85", slotBars, categories);
    }

    public ClientUISlotBarModule getStandardSlotBar() {

        Vector<ClientUISlotBarItemModule> standardItems = new Vector<>();
        Iterator<Map.Entry<Short, String>> iteratorItems = this.mSlotBarItems.entrySet()
                                                                             .iterator();
        while (iteratorItems.hasNext()) {
            Map.Entry<Short, String> pair = iteratorItems.next();
            ClientUISlotBarItemModule item = new ClientUISlotBarItemModule(pair.getValue(), pair.getKey());
            standardItems.add(item);
        }
        //adding standard slot bar
        return new ClientUISlotBarModule("50,85|0,40", STANDARD_SLOT_BAR, "0", standardItems);
    }

    public ClientUISlotBarModule getPremiumSlotBar() {
        Vector<ClientUISlotBarItemModule> premiumItems = new Vector<>();
        Iterator<Map.Entry<Short, String>> iteratorPremiumItems = this.mPremiumSlotBarItems.entrySet()
                                                                                           .iterator();
        while (iteratorPremiumItems.hasNext()) {
            Map.Entry<Short, String> pair = iteratorPremiumItems.next();
            ClientUISlotBarItemModule item = new ClientUISlotBarItemModule(pair.getValue(), pair.getKey());
            premiumItems.add(item);
        }

        //adding premium slot bar
        return new ClientUISlotBarModule("50,85|0,80", PREMIUM_SLOT_BAR, "0", premiumItems);
    }

    private ClientUISlotBarCategoryItemModule createCategoryItem(final String pLootId, final String pTooltipId,
                                                                 final boolean pCountable, final long pCount,
                                                                 final long pCooldownTime, final boolean pAvailable,
                                                                 final boolean pSelected) {

        // item bar tooltip
        Vector<ClientUITooltipModule> tooltipItemBars = new Vector<>();

        // --------------------------------------------------------
        // last argument vector (vec<class_721>)
        Vector<ClientUITextReplacementModule> vec_721_1 = new Vector<>();

        // fill last argument vector (class_521 -> vec<721>)
        ClientUITooltipTextFormatModule x_521_1 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
        ClientUITextReplacementModule x_721_1 = new ClientUITextReplacementModule("%TYPE%", x_521_1, pLootId);
        vec_721_1.add(x_721_1);

        // text format
        ClientUITooltipTextFormatModule class521_localized_1 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

        // create tooltip
        ClientUITooltipModule slotBarItemStatusTooltip_1 =
                new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_1);

        // add tooltip to tooltip bar
        tooltipItemBars.add(slotBarItemStatusTooltip_1);

        // --------------------------------------------------------

        if (pCountable) {
            // last argument vector (vec<class_721>)
            Vector<ClientUITextReplacementModule> vec_721_2 = new Vector<>();

            // fill last argument vector (class_521 -> vec<721>)
            ClientUITooltipTextFormatModule class521_plain =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
            ClientUITextReplacementModule x_721_2 =
                    new ClientUITextReplacementModule("%COUNT%", class521_plain, String.valueOf(pCount));
            vec_721_2.add(x_721_2);

            // TODO shall we use new one or reuse old one???
            // text format
            ClientUITooltipTextFormatModule class521_localized_2 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            // create tooltip
            ClientUITooltipModule slotBarItemStatusTooltip_2 =
                    new ClientUITooltipModule(class521_localized_2, ClientUITooltipModule.STANDARD, "ttip_count",
                                              vec_721_2);

            // add tooltip to tooltip bar
            tooltipItemBars.add(slotBarItemStatusTooltip_2);
        }

        // --------------------------------------------------------


        // last argument vector (vec<class_721>)
        Vector<ClientUITextReplacementModule> vec_721_3 = new Vector<>();

        // text format
        ClientUITooltipTextFormatModule x_521_3 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

        ClientUITooltipModule slotBarItemStatusTooltip_3 =
                new ClientUITooltipModule(x_521_3, ClientUITooltipModule.STANDARD, pLootId, vec_721_3);

        // add tooltip to tooltip bar
        tooltipItemBars.add(slotBarItemStatusTooltip_3);

        // --------------------------------------------------------


        // last argument vector (vec<class_721>)
        Vector<ClientUITextReplacementModule> vec_721_4 = new Vector<>();

        // text format
        ClientUITooltipTextFormatModule class521_localized_4 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

        // create tooltip
        ClientUITooltipModule slotBarItemStatusTooltip_4 =
                new ClientUITooltipModule(class521_localized_4, ClientUITooltipModule.STANDARD,
                                          "ttip_double_click_to_fire", vec_721_4);

        // add tooltip to tooltip bar
        tooltipItemBars.add(slotBarItemStatusTooltip_4);

        // ========================================================
        // ========================================================

        //slot bar tooltip
        Vector<ClientUITooltipModule> tooltipSlotBars = new Vector<>();

        // last argument vector (vec<class_721>)
        Vector<ClientUITextReplacementModule> vec_721_5 = new Vector<>();

        // fill last argument vector (class_521 -> vec<721>)
        ClientUITooltipTextFormatModule x_521_5 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_2514);
        ClientUITextReplacementModule x_721_5 = new ClientUITextReplacementModule("%TYPE%", x_521_5, pLootId);
        vec_721_5.add(x_721_5);

        ClientUITooltipTextFormatModule class521_localized_5 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

        // create tooltip
        ClientUITooltipModule slotBarItemStatusTooltip_5 =
                new ClientUITooltipModule(class521_localized_5, ClientUITooltipModule.STANDARD, pTooltipId, vec_721_5);

        // add tooltip to tooltip slot bar
        tooltipSlotBars.add(slotBarItemStatusTooltip_5);

        // --------------------------------------------------------

        if (pCountable) {
            // last argument vector (vec<class_721>)
            Vector<ClientUITextReplacementModule> vec_721_6 = new Vector<>();

            // fill last argument vector (class_521 -> vec<721>)
            ClientUITooltipTextFormatModule tf_plain_6 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.PLAIN);
            ClientUITextReplacementModule x_721_6 =
                    new ClientUITextReplacementModule("%COUNT%", tf_plain_6, String.valueOf(pCount));
            vec_721_6.add(x_721_6);

            //text format
            ClientUITooltipTextFormatModule tf_localized_6 =
                    new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

            // create tooltip
            ClientUITooltipModule slotBarItemStatusTooltip_6 =
                    new ClientUITooltipModule(tf_localized_6, ClientUITooltipModule.STANDARD, "ttip_count", vec_721_6);

            // add tooltip to tooltip slot bar
            tooltipSlotBars.add(slotBarItemStatusTooltip_6);
        }

        // --------------------------------------------------------

        // last argument vector (vec<class_721>)
        Vector<ClientUITextReplacementModule> vec_721_7 = new Vector<>();

        // text format
        ClientUITooltipTextFormatModule tf_234_7 =
                new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.const_234);

        // create tooltip
        ClientUITooltipModule slotBarItemStatusTooltip_7 =
                new ClientUITooltipModule(tf_234_7, ClientUITooltipModule.STANDARD, pLootId, vec_721_7);

        // add tooltip to tooltip slot bar
        tooltipSlotBars.add(slotBarItemStatusTooltip_7);

        // ========================================================

        // create item bar & slot bar tooltip commands
        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(tooltipItemBars);
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(tooltipSlotBars);

        ClientUISlotBarCategoryItemStatusModule rocketsCategoryItemStatus =
                new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, pAvailable, pLootId, true,
                                                            ClientUISlotBarCategoryItemStatusModule.BLUE, pLootId,
                                                            pCount, false, pAvailable, slotBarStatusTooltip, false,
                                                            pSelected, 0);


        // create category timer
        ClientUISlotBarCategoryItemTimerStateModule categoryItemTimerState =
                new ClientUISlotBarCategoryItemTimerStateModule(ClientUISlotBarCategoryItemTimerStateModule.ACTIVE);
        ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                new ClientUISlotBarCategoryItemTimerModule(pCooldownTime, categoryItemTimerState, 90000000, pLootId,
                                                           false);

        // create 5th parameter
        CooldownTypeModule cooldownType = new CooldownTypeModule(CooldownTypeModule.NONE);

        final short counterType =
                pCountable ? ClientUISlotBarCategoryItemModule.SELECTION : ClientUISlotBarCategoryItemModule.NONE;
        // create rockets category item
        return new ClientUISlotBarCategoryItemModule(1, rocketsCategoryItemStatus,
                                                     ClientUISlotBarCategoryItemModule.SELECTION, counterType,
                                                     cooldownType, categoryTimerModule);
    }

    //    public UserSettingsCommand getUserSettingsCommand() {
    //
    //        if (this.mUserSettingsCommand == null) {
    //            this.createUserSettingsCommand();
    //        }
    //
    //        return this.mUserSettingsCommand;
    //    }

    public ClientUIMenuBarsCommand getClientUIMenuBarsCommand() {

   //     if (this.mClientUIMenuBarsCommand == null) {
   //         this.createClientUIMenuBarsCommand();
   //     }
    	
    	this.createClientUIMenuBarsCommand();
        return this.mClientUIMenuBarsCommand;
    }

    public UserKeyBindingsUpdateCommand getUserKeyBindingsUpdateCommand() {
        Vector<UserKeyBindingsModuleCommand> keyBindingsModuleCommands = new Vector<>();
        List<ClientSettings.Keys.Actions> actions = this.getClientSettings()
                                                        .getKeys()
                                                        .getActions();
        for (ClientSettings.Keys.Actions action : actions) {
            keyBindingsModuleCommands.add(
                    new UserKeyBindingsModuleCommand(action.mActionType, action.mKeyCodes, action.mParameter,
                                                     action.mCharCode));
        }
        return new UserKeyBindingsUpdateCommand(keyBindingsModuleCommands, false);
    }

    public void selectMenuBarItem(final String pItemId) {
        if (AmmunitionManager.laserCategory.contains(pItemId)) {
            this.setSelectedLaserItem(pItemId);
        } else if (AmmunitionManager.rocketCategory.contains(pItemId)) {
            this.setSelectedRocketItem(pItemId);
        } else if (AmmunitionManager.rocketLauncherCategory.contains(pItemId)) {
            this.setSelectedRocketLauncherItem(pItemId);
        } else if (TechsManager.techsCategory.contains(pItemId)) {
            this.getAccount()
                .getTechsManager()
                .assembleTechCategoryRequest(pItemId);
        } else if (DroneManager.droneCategory.contains(pItemId)) {
            this.getAccount()
                .getDroneManager()
                .setSelectedFormation(pItemId);
        } else {
            switch (pItemId) {
            case SkillsManager.AEGIS_HP_REPAIR:
                this.getAccount()
                    .getSkillsManager()
                    .sendAegisHpRepairAbility();
                break;
            case SkillsManager.AEGIS_SHIELD_REPAIR:
                this.getAccount()
                    .getSkillsManager()
                    .sendAegisShieldRepairAbility();
                break;
            case SkillsManager.SPECTRUM_ABILITY:
                this.getAccount()
                    .getSkillsManager()
                    .sendSpectrumAbility();
                break;
            case SkillsManager.VENOM_ABILITY:
                this.getAccount()
                    .getSkillsManager()
                    .sendVenomAbility();
                break;
            case SkillsManager.SOLACE_ABILITY:
                this.getAccount()
                    .getSkillsManager()
                    .sendSolaceAbility();
                break;
           /**  case SkillsManager.DIMINISHER_ABILITY:
                this.getAccount()
                    .getSkillsManager()
                    .sendDiminisherAbility();
                break;
                
            case SkillsManager.SENTINEL_ABILITY:
                this.getAccount()
                    .getSkillsManager()
                    .sendSentinelAbility();
                break; 
                */
            case AmmunitionManager.SLM_01:
                this.getAccount()
                    .getAmmunitionManager()
                    .sendSLMine();
                break;
            case CpusManager.CLK_XL:
                this.getAccount()
                    .getCpusManager()
                    .sendCloak();
                break;
            case CpusManager.AUTO_ROCKET_CPU:
                this.getAccount()
                    .getCpusManager()
                    .sendAutoRocket();
                break;
            case CpusManager.AUTO_HELLSTROM_CPU:
                this.getAccount()
                    .getCpusManager()
                    .sendAutoRocketLauncher();
                break;
            case CpusManager.GALAXY_JUMP_CPU:
                this.getAccount()
                     .getCpusManager()
                     .sendJumpCpu();
                    break;
            case AmmunitionManager.EMP_01:
                this.getAccount()
                     .getAmmunitionManager()
                     .sendEMP();
                    break;
            case AmmunitionManager.SMB_01:
                this.getAccount()
                     .getAmmunitionManager()
                     .sendSMB();
                    break;
            case AmmunitionManager.ISH_01:
                this.getAccount()
                     .getAmmunitionManager()
                     .sendISH();
                    break;
            case BuyManager.ECO_10_BUY:
                this.getAccount()
                     .getBuyManager()
                     .buyECO_10();
                    break;
            case BuyManager.SAR_02_BUY:
                this.getAccount()
                     .getBuyManager()
                     .buySAR_02();
                    break;
            }
        }
    }

    public void addSlotBarItem(final short pIndex, final String pItemId) {
        this.mSlotBarItems.put(pIndex, pItemId);
    }

    public void removeSlotBarItem(final short pIndex) {
        this.mSlotBarItems.remove(pIndex);
    }

    public void addPremiumSlotBarItem(final short pIndex, final String pItemId) {
        this.mPremiumSlotBarItems.put(pIndex, pItemId);
    }

    public void removePremiumSlotBarItem(final short pIndex) {
        this.mPremiumSlotBarItems.remove(pIndex);
    }

    public ClientSettings getClientSettings() {
        return mClientSettings;
    }

    public String getSelectedLaserItem() {
        return mSelectedLaserItem;
    }

    public void setSelectedLaser(final String pSelectedLaserItem) {
    	this.mSelectedLaserItem = pSelectedLaserItem;
    }
    
    public void setSelectedLaserItem(final String pSelectedLaserItem) {
        if (mSelectedLaserItem.equals(pSelectedLaserItem)) {
            if (this.mClientSettings.getGameplay()
                                    .isQuickSlotStopAttack()) {
                final LaserAttack laserAttack = this.getAccount()
                                                    .getPlayer()
                                                    .getLaserAttack();
                if (laserAttack.isAttackInProgress()) {
                    laserAttack.setAttackInProgress(false);
                } else {
                    this.getAccount()
                        .getPlayer()
                        .initiateAttack();
                }
            }
        } else {
            final String oldSelectedItem = mSelectedLaserItem;
            mSelectedLaserItem = pSelectedLaserItem;
            QueryManager.saveAccount(this.getAccount());
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getAmmunitionManager()
                                                .getLaserItemStatus(oldSelectedItem));
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getAmmunitionManager()
                                                .getLaserItemStatus(pSelectedLaserItem));
        }
    }

    public String getSelectedRocketItem() {
        return mSelectedRocketItem;
    }

    public void setSelectedRocket(final String pSelectedRocketItem) {
    	this.mSelectedRocketItem = pSelectedRocketItem;
    }
    
    public void setSelectedRocketItem(final String pSelectedRocketItem) {
        if (mSelectedRocketItem.equalsIgnoreCase(pSelectedRocketItem)) {
            if (this.mClientSettings.getGameplay()
                                    .isQuickSlotStopAttack()) {
                this.getAccount()
                    .getPlayer()
                    .getRocketAttack()
                    .attack();
            }
        }
        else {
            final String oldSelectedItem = mSelectedRocketItem;
            mSelectedRocketItem = pSelectedRocketItem;
            QueryManager.saveAccount(this.getAccount());
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getAmmunitionManager()
                                                .getRocketItemStatus(oldSelectedItem));
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getAmmunitionManager()
                                                .getRocketItemStatus(pSelectedRocketItem));
        }
    }

    public void setSelectedRocketLauncherItem(final String pSelectedRocketLauncherItem) {
        if (mSelectedRocketLauncherItem.equalsIgnoreCase(pSelectedRocketLauncherItem)) {
            if (this.mClientSettings.getGameplay()
                                    .isQuickSlotStopAttack()) {
                this.getAccount()
                    .getPlayer()
                    .getRocketLauncherAttack()
                    .attack();
            }
        }
        else {
            final String oldSelectedItem = mSelectedRocketLauncherItem;
            mSelectedRocketLauncherItem = pSelectedRocketLauncherItem;
            QueryManager.saveAccount(this.getAccount());
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getAmmunitionManager()
                                                .getRocketLauncherItemStatus(oldSelectedItem));
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getAmmunitionManager()
                                                .getRocketLauncherItemStatus(pSelectedRocketLauncherItem));
        }
    }
    
    public String getSelectedRocketLauncherItem() {
        return mSelectedRocketLauncherItem;
    }

    public void setSelectedRocketLauncher(final String pSelectedRocketLauncherItem) {
    	this.mSelectedRocketLauncherItem = pSelectedRocketLauncherItem;
    }
    
    public Account getAccount() {
        return mAccount;
    }
}
