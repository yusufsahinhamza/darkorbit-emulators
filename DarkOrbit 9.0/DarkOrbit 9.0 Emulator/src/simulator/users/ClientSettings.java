package simulator.users;

import java.util.ArrayList;
import java.util.List;
import java.util.Vector;

/**
 Created by Ordepsousa on 19-02-2015.
 */
public class ClientSettings {

    private Audio     mAudio;
    private Quality   mQuality;
    private class_592 mClass_592;
    private Display   mDisplay;
    private Gameplay  mGameplay;
    private Window    mWindow;
    private Keys      mKeys;

    public ClientSettings() {
        this.mAudio = new Audio();
        this.mQuality = new Quality();
        this.mClass_592 = new class_592();
        this.mDisplay = new Display();
        this.mGameplay = new Gameplay();
        this.mWindow = new Window();
        this.mKeys = new Keys();
    }

    public ClientSettings(final boolean pAudioNotSet, final boolean pPlayCombatMusic, final int pMusic,
                          final int pSound, final int pVoice, final boolean pQualityNotSet, final int pQualityAttack,
                          final int pQualityBackground, final int pQualityCollectable, final boolean pQualityCustomized,
                          final int pQualityEffect, final int pQualityEngine, final int pQualityExplosion,
                          final int pQualityPoizone, final int pQualityPresetting, final int pQualityShip,
                          final boolean pClass_592NotSet, final boolean pQuestsAvailableFilter,
                          final boolean pQuestsUnavailableFilter, final boolean pQuestsCompletedFilter,
                          final boolean pVar_1151, final boolean pVar_2239, final boolean pQuestsLevelOrderDescending,
                          final boolean pDisplayNotSet, final boolean pDisplayPlayerNames,
                          final boolean pDisplayResources, final boolean pShowPremiumQuickslotBar,
                          final boolean pAllowAutoQuality, final boolean pPreloadUserShips,
                          final boolean pDisplayHitpointBubbles, final boolean pShowNotOwnedItems,
                          final boolean pDisplayChat, final boolean pDisplayWindowsBackground,
                          final boolean pDisplayNotFreeCargoBoxes, final boolean pDragWindowsAlways,
                          final boolean pDisplayNotifications, final boolean pHoverShips, final boolean pDisplayDrones,
                          final boolean pDisplayBonusBoxes, final boolean pDisplayFreeCargoBoxes,
                          final boolean pGameplayNotSet, final boolean pAutoRefinement,
                          final boolean pQuickSlotStopAttack, final boolean pAutoBoost, final boolean pAutoBuyBootyKeys,
                          final boolean pDoubleclickAttackEnabled, final boolean pAutochangeAmmo,
                          final boolean pAutoStartEnabled, final boolean pHideAllWindows, final int pScale,
                          final String pBarState) {
        this.setAudio(new Audio(pAudioNotSet, pPlayCombatMusic, pMusic, pSound, pVoice));
        this.setQuality(
                new Quality(pQualityNotSet, pQualityAttack, pQualityBackground, pQualityCollectable, pQualityCustomized,
                            pQualityEffect, pQualityEngine, pQualityExplosion, pQualityPoizone, pQualityPresetting,
                            pQualityShip));
        this.setClass_592(new class_592(pClass_592NotSet, pQuestsAvailableFilter, pQuestsUnavailableFilter,
                                        pQuestsCompletedFilter, pVar_1151, pVar_2239, pQuestsLevelOrderDescending));
        this.setDisplay(new Display(pDisplayNotSet, pDisplayPlayerNames, pDisplayResources, pShowPremiumQuickslotBar,
                                    pAllowAutoQuality, pPreloadUserShips, pDisplayHitpointBubbles, pShowNotOwnedItems,
                                    pDisplayChat, pDisplayWindowsBackground, pDisplayNotFreeCargoBoxes,
                                    pDragWindowsAlways, pDisplayNotifications, pHoverShips, pDisplayDrones,
                                    pDisplayBonusBoxes, pDisplayFreeCargoBoxes));
        this.setGameplay(
                new Gameplay(pGameplayNotSet, pAutoRefinement, pQuickSlotStopAttack, pAutoBoost, pAutoBuyBootyKeys,
                             pDoubleclickAttackEnabled, pAutochangeAmmo, pAutoStartEnabled));
        this.setWindow(new Window(pHideAllWindows, pScale, pBarState));
        this.setKeys(new Keys());
    }

    public Audio getAudio() {
        return mAudio;
    }

    public void setAudio(final Audio pAudio) {
        mAudio = pAudio;
    }

    public Quality getQuality() {
        return mQuality;
    }

    public void setQuality(final Quality pQuality) {
        mQuality = pQuality;
    }

    public class_592 getClass_592() {
        return mClass_592;
    }

    public void setClass_592(final class_592 pClass_592) {
        mClass_592 = pClass_592;
    }

    public Display getDisplay() {
        return mDisplay;
    }

    public void setDisplay(final Display pDisplay) {
        mDisplay = pDisplay;
    }

    public Gameplay getGameplay() {
        return mGameplay;
    }

    public void setGameplay(final Gameplay pGameplay) {
        mGameplay = pGameplay;
    }

    public Window getWindow() {
        return mWindow;
    }

    public void setWindow(final Window pWindow) {
        mWindow = pWindow;
    }

    public Keys getKeys() {
        return mKeys;
    }

    public void setKeys(final Keys pKeys) {
        mKeys = pKeys;
    }

    public class Audio {

        private boolean mNotSet          = false;
        private boolean mPlayCombatMusic = false;
        private int     mMusic           = 0;
        private int     mSound           = 0;
        private int     mVoice           = 0;

        public Audio() {
        }

        public Audio(final boolean pNotSet, final boolean pPlayCombatMusic, final int pMusic, final int pSound,
                     final int pVoice) {
            this.setNotSet(pNotSet);
            this.setPlayCombatMusic(pPlayCombatMusic);
            this.setMusic(pMusic);
            this.setSound(pSound);
            this.setVoice(pVoice);
        }

        public boolean isNotSet() {
            return mNotSet;
        }

        public void setNotSet(final boolean pNotSet) {
            mNotSet = pNotSet;
        }

        public boolean isPlayCombatMusic() {
            return mPlayCombatMusic;
        }

        public void setPlayCombatMusic(final boolean pPlayCombatMusic) {
            mPlayCombatMusic = pPlayCombatMusic;
        }

        public int getMusic() {
            return mMusic;
        }

        public void setMusic(final int pMusic) {
            mMusic = pMusic;
        }

        public int getSound() {
            return mSound;
        }

        public void setSound(final int pSound) {
            mSound = pSound;
        }

        public int getVoice() {
            return mVoice;
        }

        public void setVoice(final int pVoice) {
            mVoice = pVoice;
        }
    }

    public class Quality {

        private boolean mNotSet             = false;
        private int     mQualityAttack      = 0;
        private int     mQualityBackground  = 1;
        private int     mQualityCollectable = 0;
        private boolean mQualityCustomized  = false;
        private int     mQualityEffect      = 1;
        private int     mQualityEngine      = 0;
        private int     mQualityExplosion   = 1;
        private int     mQualityPoizone     = 0;
        private int     mQualityPresetting  = 0;
        private int     mQualityShip        = 1;

        public Quality() {

        }

        public Quality(final boolean pNotSet, final int pQualityAttack, final int pQualityBackground,
                       final int pQualityCollectable, final boolean pQualityCustomized, final int pQualityEffect,
                       final int pQualityEngine, final int pQualityExplosion, final int pQualityPoizone,
                       final int pQualityPresetting, final int pQualityShip) {
            this.setNotSet(pNotSet);
            this.setQualityAttack(pQualityAttack);
            this.setQualityBackground(pQualityBackground);
            this.setQualityCollectable(pQualityCollectable);
            this.setQualityCustomized(pQualityCustomized);
            this.setQualityEffect(pQualityEffect);
            this.setQualityEngine(pQualityEngine);
            this.setQualityExplosion(pQualityExplosion);
            this.setQualityPoizone(pQualityPoizone);
            this.setQualityPresetting(pQualityPresetting);
            this.setQualityShip(pQualityShip);
        }

        public boolean isNotSet() {
            return mNotSet;
        }

        public void setNotSet(final boolean pNotSet) {
            mNotSet = pNotSet;
        }

        public int getQualityAttack() {
            return mQualityAttack;
        }

        public void setQualityAttack(final int pQualityAttack) {
            mQualityAttack = pQualityAttack;
        }

        public int getQualityBackground() {
            return mQualityBackground;
        }

        public void setQualityBackground(final int pQualityBackground) {
            mQualityBackground = pQualityBackground;
        }

        public int getQualityCollectable() {
            return mQualityCollectable;
        }

        public void setQualityCollectable(final int pQualityCollectable) {
            mQualityCollectable = pQualityCollectable;
        }

        public boolean getQualityCustomized() {
            return mQualityCustomized;
        }

        public void setQualityCustomized(final boolean pQualityCustomized) {
            mQualityCustomized = pQualityCustomized;
        }

        public int getQualityEffect() {
            return mQualityEffect;
        }

        public void setQualityEffect(final int pQualityEffect) {
            mQualityEffect = pQualityEffect;
        }

        public int getQualityEngine() {
            return mQualityEngine;
        }

        public void setQualityEngine(final int pQualityEngine) {
            mQualityEngine = pQualityEngine;
        }

        public int getQualityExplosion() {
            return mQualityExplosion;
        }

        public void setQualityExplosion(final int pQualityExplosion) {
            mQualityExplosion = pQualityExplosion;
        }

        public int getQualityPoizone() {
            return mQualityPoizone;
        }

        public void setQualityPoizone(final int pQualityPoizone) {
            mQualityPoizone = pQualityPoizone;
        }

        public int getQualityPresetting() {
            return mQualityPresetting;
        }

        public void setQualityPresetting(final int pQualityPresetting) {
            mQualityPresetting = pQualityPresetting;
        }

        public int getQualityShip() {
            return mQualityShip;
        }

        public void setQualityShip(final int pQualityShip) {
            mQualityShip = pQualityShip;
        }
    }

    public class class_592 {

        private boolean mNotSet                     = false;
        private boolean mQuestsAvailableFilter      = false;
        private boolean mQuestsUnavailableFilter    = false;
        private boolean mQuestsCompletedFilter      = false;
        private boolean var_1151                    = false;
        private boolean var_2239                    = false;
        private boolean mQuestsLevelOrderDescending = false;

        public class_592() {
        }

        public class_592(final boolean pNotSet, final boolean pQuestsAvailableFilter,
                         final boolean pQuestsUnavailableFilter, final boolean pQuestsCompletedFilter,
                         final boolean pVar_1151, final boolean pVar_2239, final boolean pQuestsLevelOrderDescending) {
            this.setNotSet(pNotSet);
            this.setQuestsAvailableFilter(pQuestsAvailableFilter);
            this.setQuestsUnavailableFilter(pQuestsUnavailableFilter);
            this.setQuestsCompletedFilter(pQuestsCompletedFilter);
            this.setVar_1151(pVar_1151);
            this.setVar_2239(pVar_2239);
            this.setQuestsLevelOrderDescending(pQuestsLevelOrderDescending);
        }

        public boolean isNotSet() {
            return mNotSet;
        }

        public void setNotSet(final boolean pNotSet) {
            mNotSet = pNotSet;
        }

        public boolean isQuestsAvailableFilter() {
            return mQuestsAvailableFilter;
        }

        public void setQuestsAvailableFilter(final boolean pQuestsAvailableFilter) {
            mQuestsAvailableFilter = pQuestsAvailableFilter;
        }

        public boolean isQuestsUnavailableFilter() {
            return mQuestsUnavailableFilter;
        }

        public void setQuestsUnavailableFilter(final boolean pQuestsUnavailableFilter) {
            mQuestsUnavailableFilter = pQuestsUnavailableFilter;
        }

        public boolean isQuestsCompletedFilter() {
            return mQuestsCompletedFilter;
        }

        public void setQuestsCompletedFilter(final boolean pQuestsCompletedFilter) {
            mQuestsCompletedFilter = pQuestsCompletedFilter;
        }

        public boolean isVar_1151() {
            return var_1151;
        }

        public void setVar_1151(final boolean pVar_1151) {
            var_1151 = pVar_1151;
        }

        public boolean isVar_2239() {
            return var_2239;
        }

        public void setVar_2239(final boolean pVar_2239) {
            var_2239 = pVar_2239;
        }

        public boolean isQuestsLevelOrderDescending() {
            return mQuestsLevelOrderDescending;
        }

        public void setQuestsLevelOrderDescending(final boolean pQuestsLevelOrderDescending) {
            mQuestsLevelOrderDescending = pQuestsLevelOrderDescending;
        }
    }

    public class Display {

        private boolean mNotSet                   = false;
        private boolean mDisplayPlayerNames       = true;
        private boolean mDisplayResources         = false;
        private boolean mShowPremiumQuickslotBar  = false;
        private boolean mAllowAutoQuality         = false;
        private boolean mPreloadUserShips         = false;
        private boolean mDisplayHitpointBubbles   = true;
        private boolean mShowNotOwnedItems        = false;
        private boolean mDisplayChat              = true;
        private boolean mDisplayWindowsBackground = true;
        private boolean mDisplayNotFreeCargoBoxes = false;
        private boolean mDragWindowsAlways        = true;
        private boolean mDisplayNotifications     = false;
        private boolean mHoverShips               = false;
        private boolean mDisplayDrones            = false;
        private boolean mDisplayBonusBoxes        = false;
        private boolean mDisplayFreeCargoBoxes    = false;

        public Display() {
        }

        public Display(final boolean pNotSet, final boolean pDisplayPlayerNames, final boolean pDisplayResources,
                       final boolean pShowPremiumQuickslotBar, final boolean pAllowAutoQuality,
                       final boolean pPreloadUserShips, final boolean pDisplayHitpointBubbles,
                       final boolean pShowNotOwnedItems, final boolean pDisplayChat,
                       final boolean pDisplayWindowsBackground, final boolean pDisplayNotFreeCargoBoxes,
                       final boolean pDragWindowsAlways, final boolean pDisplayNotifications, final boolean pHoverShips,
                       final boolean pDisplayDrones, final boolean pDisplayBonusBoxes,
                       final boolean pDisplayFreeCargoBoxes) {
            this.setNotSet(pNotSet);
            this.setDisplayPlayerNames(pDisplayPlayerNames);
            this.setDisplayResources(pDisplayResources);
            this.setShowPremiumQuickslotBar(pShowPremiumQuickslotBar);
            this.setAllowAutoQuality(pAllowAutoQuality);
            this.setPreloadUserShips(pPreloadUserShips);
            this.setDisplayHitpointBubbles(pDisplayHitpointBubbles);
            this.setShowNotOwnedItems(pShowNotOwnedItems);
            this.setDisplayChat(pDisplayChat);
            this.setDisplayWindowsBackground(pDisplayWindowsBackground);
            this.setDisplayNotFreeCargoBoxes(pDisplayNotFreeCargoBoxes);
            this.setDragWindowsAlways(pDragWindowsAlways);
            this.setDisplayNotifications(pDisplayNotifications);
            this.setHoverShips(pHoverShips);
            this.setDisplayDrones(pDisplayDrones);
            this.setDisplayBonusBoxes(pDisplayBonusBoxes);
            this.setDisplayFreeCargoBoxes(pDisplayFreeCargoBoxes);
        }

        public boolean isNotSet() {
            return mNotSet;
        }

        public void setNotSet(final boolean pNotSet) {
            mNotSet = pNotSet;
        }

        public boolean isDisplayPlayerNames() {
            return mDisplayPlayerNames;
        }

        public void setDisplayPlayerNames(final boolean pDisplayPlayerNames) {
            mDisplayPlayerNames = pDisplayPlayerNames;
        }

        public boolean isDisplayResources() {
            return mDisplayResources;
        }

        public void setDisplayResources(final boolean pDisplayResources) {
            mDisplayResources = pDisplayResources;
        }

        public boolean isShowPremiumQuickslotBar() {
            return mShowPremiumQuickslotBar;
        }

        public void setShowPremiumQuickslotBar(final boolean pShowPremiumQuickslotBar) {
            mShowPremiumQuickslotBar = pShowPremiumQuickslotBar;
        }

        public boolean isAllowAutoQuality() {
            return mAllowAutoQuality;
        }

        public void setAllowAutoQuality(final boolean pAllowAutoQuality) {
            mAllowAutoQuality = pAllowAutoQuality;
        }

        public boolean isPreloadUserShips() {
            return mPreloadUserShips;
        }

        public void setPreloadUserShips(final boolean pPreloadUserShips) {
            mPreloadUserShips = pPreloadUserShips;
        }

        public boolean isDisplayHitpointBubbles() {
            return mDisplayHitpointBubbles;
        }

        public void setDisplayHitpointBubbles(final boolean pDisplayHitpointBubbles) {
            mDisplayHitpointBubbles = pDisplayHitpointBubbles;
        }

        public boolean isShowNotOwnedItems() {
            return mShowNotOwnedItems;
        }

        public void setShowNotOwnedItems(final boolean pShowNotOwnedItems) {
            mShowNotOwnedItems = pShowNotOwnedItems;
        }

        public boolean isDisplayChat() {
            return mDisplayChat;
        }

        public void setDisplayChat(final boolean pDisplayChat) {
            mDisplayChat = pDisplayChat;
        }

        public boolean isDisplayWindowsBackground() {
            return mDisplayWindowsBackground;
        }

        public void setDisplayWindowsBackground(final boolean pDisplayWindowsBackground) {
            mDisplayWindowsBackground = pDisplayWindowsBackground;
        }

        public boolean isDisplayNotFreeCargoBoxes() {
            return mDisplayNotFreeCargoBoxes;
        }

        public void setDisplayNotFreeCargoBoxes(final boolean pDisplayNotFreeCargoBoxes) {
            mDisplayNotFreeCargoBoxes = pDisplayNotFreeCargoBoxes;
        }

        public boolean isDragWindowsAlways() {
            return mDragWindowsAlways;
        }

        public void setDragWindowsAlways(final boolean pDragWindowsAlways) {
            mDragWindowsAlways = pDragWindowsAlways;
        }

        public boolean isDisplayNotifications() {
            return mDisplayNotifications;
        }

        public void setDisplayNotifications(final boolean pDisplayNotifications) {
            mDisplayNotifications = pDisplayNotifications;
        }

        public boolean isHoverShips() {
            return mHoverShips;
        }

        public void setHoverShips(final boolean pHoverShips) {
            mHoverShips = pHoverShips;
        }

        public boolean isDisplayDrones() {
            return mDisplayDrones;
        }

        public void setDisplayDrones(final boolean pDisplayDrones) {
            mDisplayDrones = pDisplayDrones;
        }

        public boolean isDisplayBonusBoxes() {
            return mDisplayBonusBoxes;
        }

        public void setDisplayBonusBoxes(final boolean pDisplayBonusBoxes) {
            mDisplayBonusBoxes = pDisplayBonusBoxes;
        }

        public boolean isDisplayFreeCargoBoxes() {
            return mDisplayFreeCargoBoxes;
        }

        public void setDisplayFreeCargoBoxes(final boolean pDisplayFreeCargoBoxes) {
            mDisplayFreeCargoBoxes = pDisplayFreeCargoBoxes;
        }
    }

    public class Gameplay {

        private boolean mNotSet                   = false;
        private boolean mAutoRefinement           = false;
        private boolean mQuickSlotStopAttack      = false;
        private boolean mAutoBoost                = false;
        private boolean mAutoBuyBootyKeys         = false;
        private boolean mDoubleclickAttackEnabled = false;
        private boolean mAutochangeAmmo           = false;
        private boolean mAutoStartEnabled         = false;

        public Gameplay() {
        }

        public Gameplay(final boolean pNotSet, final boolean pAutoRefinement, final boolean pQuickSlotStopAttack,
                        final boolean pAutoBoost, final boolean pAutoBuyBootyKeys,
                        final boolean pDoubleclickAttackEnabled, final boolean pAutochangeAmmo,
                        final boolean pAutoStartEnabled) {
            this.setNotSet(pNotSet);
            this.setAutoRefinement(pAutoRefinement);
            this.setQuickSlotStopAttack(pQuickSlotStopAttack);
            this.setAutoBoost(pAutoBoost);
            this.setAutoBuyBootyKeys(pAutoBuyBootyKeys);
            this.setDoubleclickAttackEnabled(pDoubleclickAttackEnabled);
            this.setAutochangeAmmo(pAutochangeAmmo);
            this.setAutoStartEnabled(pAutoStartEnabled);
        }

        public boolean isNotSet() {
            return mNotSet;
        }

        public void setNotSet(final boolean pNotSet) {
            mNotSet = pNotSet;
        }

        public boolean isAutoRefinement() {
            return mAutoRefinement;
        }

        public void setAutoRefinement(final boolean pAutoRefinement) {
            mAutoRefinement = pAutoRefinement;
        }

        public boolean isQuickSlotStopAttack() {
            return mQuickSlotStopAttack;
        }

        public void setQuickSlotStopAttack(final boolean pQuickSlotStopAttack) {
            mQuickSlotStopAttack = pQuickSlotStopAttack;
        }

        public boolean isAutoBoost() {
            return mAutoBoost;
        }

        public void setAutoBoost(final boolean pAutoBoost) {
            mAutoBoost = pAutoBoost;
        }

        public boolean isAutoBuyBootyKeys() {
            return mAutoBuyBootyKeys;
        }

        public void setAutoBuyBootyKeys(final boolean pAutoBuyBootyKeys) {
            mAutoBuyBootyKeys = pAutoBuyBootyKeys;
        }

        public boolean isDoubleclickAttackEnabled() {
            return mDoubleclickAttackEnabled;
        }

        public void setDoubleclickAttackEnabled(final boolean pDoubleclickAttackEnabled) {
            mDoubleclickAttackEnabled = pDoubleclickAttackEnabled;
        }

        public boolean isAutochangeAmmo() {
            return mAutochangeAmmo;
        }

        public void setAutochangeAmmo(final boolean pAutochangeAmmo) {
            mAutochangeAmmo = pAutochangeAmmo;
        }

        public boolean isAutoStartEnabled() {
            return mAutoStartEnabled;
        }

        public void setAutoStartEnabled(final boolean pAutoStartEnabled) {
            mAutoStartEnabled = pAutoStartEnabled;
        }
    }

    public class Window {

        private boolean mHideAllWindows = false;
        private int     mScale          = 1;
        private String  mBarState       = "";

        public Window() {
        }

        public Window(final boolean pHideAllWindows, final int pScale, final String pBarState) {
            this.setHideAllWindows(pHideAllWindows);
            this.setScale(pScale);
            this.setBarState(pBarState);
        }

        public boolean isHideAllWindows() {
            return mHideAllWindows;
        }

        public void setHideAllWindows(final boolean pHideAllWindows) {
            mHideAllWindows = pHideAllWindows;
        }

        public int getScale() {
            return mScale;
        }

        public void setScale(final int pScale) {
            mScale = pScale;
        }

        public String getBarState() {
            return mBarState;
        }

        public void setBarState(final String pBarState) {
            mBarState = pBarState;
        }
    }

    public class Keys {

        private List<Actions> mActions = new ArrayList<>();

        public List<Actions> getActions() {
            return this.mActions;
        }

        public void clearActions() {
            this.mActions.clear();
        }

        public void addAction(final short pActionType, final short pCharCode, final int pParameter,
                              final Vector<Integer> pKeyCodes) {
            this.mActions.add(new Actions(pActionType, pCharCode, pParameter, pKeyCodes));
        }

        class Actions {

            short           mActionType;
            short           mCharCode;
            int             mParameter;
            Vector<Integer> mKeyCodes;

            public Actions(final short pActionType, final short pCharCode, final int pParameter,
                           final Vector<Integer> pKeyCodes) {
                this.mActionType = pActionType;
                this.mCharCode = pCharCode;
                this.mParameter = pParameter;
                this.mKeyCodes = pKeyCodes;
            }

        }

    }
}