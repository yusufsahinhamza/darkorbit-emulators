package simulator.users;

import java.util.Arrays;
import java.util.List;

import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;

/**
 Created by bpdev on 31/01/2015.
 Edited & Fixed by LEJYONER.
 */

public class TechsManager
        extends AbstractAccountInternalManager {

    private static final String TECHS_DEFAULT_JSON =
            "[{\"productID\":0,\"amount\":0,\"isProducing\":false,\"pruducingDate\":0,\"isBlocked\":false},{\"productID\":0,\"amount\":0,\"isProducing\":false,\"pruducingDate\":0,\"isBlocked\":true},{\"productID\":0,\"amount\":0,\"isProducing\":false,\"pruducingDate\":0,\"isBlocked\":true}]";

    static final String TECH_ENERGY_LEECH       = "tech_energy-leech";
    static final String TECH_CHAIN_IMPULSE      = "tech_chain-impulse";
    static final String TECH_PRECISION_TARGETER = "tech_precision-targeter";
    static final String TECH_BACKUP_SHIELDS     = "tech_backup-shields";
    static final String TECH_BATTLE_REPAIR_BOT  = "tech_battle-repair-bot";

    public static final List<String> techsCategory =
            Arrays.asList(TECH_ENERGY_LEECH, /**TECH_CHAIN_IMPULSE, TECH_PRECISION_TARGETER, */TECH_BACKUP_SHIELDS,
                          TECH_BATTLE_REPAIR_BOT);

    private static final int    TECH_ENERGY_LEECH_DURATION          = 300000;
    private static final double TECH_ENERGY_LEECH_REPAIR_PERCENTAGE = 0.10;
    private static final int    TECH_ENERGY_LEECH_COOLDOWN          = 150000;
    private static final int    TECH_PRECISION_TARGETER_DURATION    = 300000;
    private static final int    TECH_PRECISION_TARGETER_COOLDOWN    = 600000;
    private static final int    TECH_BACKUP_SHIELD_REPAIR           = 75000;
    private static final int    TECH_BACKUP_SHIELDS_COOLDOWN        = 120000;
    private static final int    TECH_BATTLE_REPAIR_BOT_DURATION     = 10000;
    private static final int    TECH_BATTLE_REPAIR_BOT_DELAY        = 1000;
    private static final int    TECH_BATTLE_REPAIR_BOT_REPAIR_RATE  = 10000;
    private static final int    TECH_BATTLE_REPAIR_BOT_COOLDOWN     = 120000;

    private String mTechsJSON;

    private long mEnergyLeechEffectFinishTime         = 0L;
    private long mEnergyLeechCooldownFinishTime       = 0L;
    private long mChainImpulseCooldownFinishTime      = 0L;
    private long mPrecisionTargeterEffectFinishTime   = 0L;
    private long mPrecisionTargeterCooldownFinishTime = 0L;
    private long mBackupShieldsCooldownFinishTime     = 0L;
    private long mBattleRepairBotEffectFinishTime     = 0L;
    private long mBattleRepairBotLastRepairTime       = 0L;
    private long mBattleRepairBotCooldownFinishTime   = 0L;

    private boolean mEnergyLeechEffectActivated         = false;
    private boolean mBattleRepairBotEffectActivated     = false;
    private boolean mPrecisionTargeterEffectActivated   = false;

    public TechsManager(final Account pAccount) {
        super(pAccount);
    }

    @Override
    public void setFromJSON(final String pTechsJSON) {
        this.mTechsJSON = pTechsJSON;
    }

    @Override
    public void setNewAccount() {
        this.mTechsJSON = TECHS_DEFAULT_JSON;
    }

    @Override
    public String packToJSON() {
        return this.mTechsJSON;
    }

    public void onTickCheckMethods() {
        this.checkBattleRepairBot();
        this.checkEnergyLeech();
        this.checkPrecisionTargeter();
    }

    public void checkPrecisionTargeter() {
    	final Player player = this.getAccount().getPlayer();
    	if (this.isPrecisionTargeterActivated()) {

        } else if (this.isPrecisionTargeterEffectActivated()) {
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_PRECISION_TARGETER),
                    player.getAccount().getAmmunitionManager().getItemTimerState(TECH_PRECISION_TARGETER), TECH_PRECISION_TARGETER_COOLDOWN,
                    TECH_PRECISION_TARGETER_COOLDOWN));
            this.setPrecisionTargeterEffectActivated(false);
        }
    }
    
    public void assembleTechCategoryRequest(final String pTechItem) {
        switch (pTechItem) {
            case TECH_ENERGY_LEECH:
                this.activateEnergyLeech();
                break;
            case TECH_CHAIN_IMPULSE:
                break;
            case TECH_PRECISION_TARGETER:
                this.activatePrecisionTargeter();
                break;
            case TECH_BACKUP_SHIELDS:
                this.activateBackupShields();
                break;
            case TECH_BATTLE_REPAIR_BOT:
                this.activateBattleRepairBot();
                break;
        }
    }

    private void activateEnergyLeech() {
        final long currentTime = System.currentTimeMillis();
        if ((currentTime - this.getEnergyLeechCooldownFinishTime()) >= 0) {
            final Player player = this.getAccount()
                                      .getPlayer();
            final String packet = "0|TX|A|S|ELA|" + player.getMapEntityId();
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);

            this.setEnergyLeechEffectActivated(true);
            this.setEnergyLeechEffectFinishTime(currentTime + TECH_ENERGY_LEECH_DURATION);
            this.setEnergyLeechCooldownFinishTime(
                    currentTime + TECH_ENERGY_LEECH_DURATION + TECH_ENERGY_LEECH_COOLDOWN);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_ENERGY_LEECH),
                    player.getAccount().getAmmunitionManager().getItemTimerState(""), TECH_ENERGY_LEECH_DURATION,
                    TECH_ENERGY_LEECH_DURATION));
        }
    }

    private void checkEnergyLeech() {
        if (!this.isEnergyLeechActivated() && this.isEnergyLeechEffectActivated()) {
            final Player player = this.getAccount()
                                      .getPlayer();
            final String packet = "0|TX|D|S|ELA|" + player.getMapEntityId();
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_ENERGY_LEECH),
                    player.getAccount().getAmmunitionManager().getItemTimerState(TECH_ENERGY_LEECH), TECH_ENERGY_LEECH_COOLDOWN,
                    TECH_ENERGY_LEECH_COOLDOWN));
            this.setEnergyLeechEffectActivated(false);
        }
    }

    public void onLaserAttack(final int pLaserDamage) {
        if (this.isEnergyLeechActivated()) {
            final Player player = this.getAccount()
                                      .getPlayer();
            int repair = (int) (pLaserDamage * TECH_ENERGY_LEECH_REPAIR_PERCENTAGE);

            player.healEntity(repair, player.HEAL_HITPOINTS);
        }
    }

    public boolean isEnergyLeechActivated() {
        return (System.currentTimeMillis() - this.getEnergyLeechEffectFinishTime()) < 0;
    }

    private void activatePrecisionTargeter() {
        final long currentTime = System.currentTimeMillis();
        if ((currentTime - this.getPrecisionTargeterCooldownFinishTime()) >= 0) {
            final Player player = this.getAccount()
                    .getPlayer();
            this.setPrecisionTargeterEffectActivated(true);
            this.setPrecisionTargeterEffectFinishTime(currentTime + TECH_PRECISION_TARGETER_DURATION);
            this.setPrecisionTargeterCooldownFinishTime(
                    currentTime + TECH_PRECISION_TARGETER_DURATION + TECH_PRECISION_TARGETER_COOLDOWN);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_PRECISION_TARGETER),
                    player.getAccount().getAmmunitionManager().getItemTimerState(""), TECH_PRECISION_TARGETER_DURATION,
                    TECH_PRECISION_TARGETER_DURATION));
        }
    }

    private void activateBackupShields() {
        final long currentTime = System.currentTimeMillis();
        if ((currentTime - this.getBackupShieldsCooldownFinishTime()) >= 0) {

            final Player player = this.getAccount()
                                      .getPlayer();
            final String packet = "0|TX|A|S|SBU|" + player.getMapEntityId();
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);

            player.healEntity(TECH_BACKUP_SHIELD_REPAIR, player.HEAL_SHIELD);
            this.setBackupShieldsCooldownFinishTime(currentTime + TECH_BACKUP_SHIELDS_COOLDOWN);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_BACKUP_SHIELDS),
                    player.getAccount().getAmmunitionManager().getItemTimerState(TECH_BACKUP_SHIELDS), TECH_BACKUP_SHIELDS_COOLDOWN,
                    TECH_BACKUP_SHIELDS_COOLDOWN));
        }
    }

    private void activateBattleRepairBot() {
        final long currentTime = System.currentTimeMillis();
        if ((currentTime - this.getBattleRepairBotCooldownFinishTime()) >= 0) {
            final Player player = this.getAccount()
                                      .getPlayer();
            final String packet = "0|TX|A|S|BRB|" + player.getMapEntityId();
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);

            this.setBattleRepairBotEffectActivated(true);
            this.setBattleRepairBotEffectFinishTime(currentTime + TECH_BATTLE_REPAIR_BOT_DURATION);
            this.setBattleRepairBotCooldownFinishTime(
                    currentTime + TECH_BATTLE_REPAIR_BOT_DURATION + TECH_BATTLE_REPAIR_BOT_COOLDOWN);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_BATTLE_REPAIR_BOT),
                    player.getAccount().getAmmunitionManager().getItemTimerState(""), TECH_BATTLE_REPAIR_BOT_DURATION,
                    TECH_BATTLE_REPAIR_BOT_DURATION));
        }
    }

    private boolean isBattleRepairBotActivated() {
        return (System.currentTimeMillis() - this.getBattleRepairBotEffectFinishTime()) < 0;
    }

    private boolean isPrecisionTargeterActivated() {
        return (System.currentTimeMillis() - this.getPrecisionTargeterEffectFinishTime()) < 0;
    }
    
    private void checkBattleRepairBot() {
        final long currentTime = System.currentTimeMillis();
        final Player player = this.getAccount()
                                  .getPlayer();

        if (this.isBattleRepairBotActivated()) {
            if ((currentTime - this.getBattleRepairBotLastRepairTime()) >= TECH_BATTLE_REPAIR_BOT_DELAY) {

                int heal = TECH_BATTLE_REPAIR_BOT_REPAIR_RATE;

                player.healEntity(heal, player.HEAL_HITPOINTS);

                this.setBattleRepairBotLastRepairTime(currentTime);
            }
        } else if (this.isBattleRepairBotEffectActivated()) {

            final String packet = "0|TX|D|S|BRB|" + player.getMapEntityId();
            player.sendPacketToInRange(packet);
            player.sendPacketToBoundSessions(packet);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    player.getAccount().getAmmunitionManager().getCooldownType(TECH_BATTLE_REPAIR_BOT),
                    player.getAccount().getAmmunitionManager().getItemTimerState(TECH_BATTLE_REPAIR_BOT), TECH_BATTLE_REPAIR_BOT_COOLDOWN,
                    TECH_BATTLE_REPAIR_BOT_COOLDOWN));
            this.setBattleRepairBotEffectActivated(false);
        }
    }

    public long getEnergyLeechEffectFinishTime() {
        return mEnergyLeechEffectFinishTime;
    }

    public void setEnergyLeechEffectFinishTime(final long pEnergyLeechEffectFinishTime) {
        mEnergyLeechEffectFinishTime = pEnergyLeechEffectFinishTime;
    }

    public long getEnergyLeechCooldownFinishTime() {
        return mEnergyLeechCooldownFinishTime;
    }

    public void setEnergyLeechCooldownFinishTime(final long pEnergyLeechCooldownFinishTime) {
        mEnergyLeechCooldownFinishTime = pEnergyLeechCooldownFinishTime;
    }

    public long getChainImpulseCooldownFinishTime() {
        return mChainImpulseCooldownFinishTime;
    }

    public void setChainImpulseCooldownFinishTime(final long pChainImpulseCooldownFinishTime) {
        mChainImpulseCooldownFinishTime = pChainImpulseCooldownFinishTime;
    }

    public long getPrecisionTargeterEffectFinishTime() {
        return mPrecisionTargeterEffectFinishTime;
    }

    public void setPrecisionTargeterEffectFinishTime(final long pPrecisionTargeterEffectFinishTime) {
        mPrecisionTargeterEffectFinishTime = pPrecisionTargeterEffectFinishTime;
    }
    
    public boolean isPrecisionTargeterEffectActivated() {
        return mPrecisionTargeterEffectActivated;
    }

    public void setPrecisionTargeterEffectActivated(final boolean pPrecisionTargeterEffectActivated) {
        mPrecisionTargeterEffectActivated = pPrecisionTargeterEffectActivated;
    }
    
    public long getPrecisionTargeterCooldownFinishTime() {
        return mPrecisionTargeterCooldownFinishTime;
    }

    public void setPrecisionTargeterCooldownFinishTime(final long pPrecisionTargeterCooldownFinishTime) {
        mPrecisionTargeterCooldownFinishTime = pPrecisionTargeterCooldownFinishTime;
    }

    public long getBackupShieldsCooldownFinishTime() {
        return mBackupShieldsCooldownFinishTime;
    }

    public void setBackupShieldsCooldownFinishTime(final long pBackupShieldsCooldownFinishTime) {
        mBackupShieldsCooldownFinishTime = pBackupShieldsCooldownFinishTime;
    }

    public long getBattleRepairBotEffectFinishTime() {
        return mBattleRepairBotEffectFinishTime;
    }

    public void setBattleRepairBotEffectFinishTime(final long pBattleRepairBotEffectFinishTime) {
        mBattleRepairBotEffectFinishTime = pBattleRepairBotEffectFinishTime;
    }

    public long getBattleRepairBotCooldownFinishTime() {
        return mBattleRepairBotCooldownFinishTime;
    }

    public void setBattleRepairBotCooldownFinishTime(final long pBattleRepairBotCooldownFinishTime) {
        mBattleRepairBotCooldownFinishTime = pBattleRepairBotCooldownFinishTime;
    }

    public long getBattleRepairBotLastRepairTime() {
        return mBattleRepairBotLastRepairTime;
    }

    public void setBattleRepairBotLastRepairTime(final long pBattleRepairBotLastRepairTime) {
        mBattleRepairBotLastRepairTime = pBattleRepairBotLastRepairTime;
    }

    public boolean isEnergyLeechEffectActivated() {
        return mEnergyLeechEffectActivated;
    }

    public void setEnergyLeechEffectActivated(final boolean pEnergyLeechEffectActivated) {
        mEnergyLeechEffectActivated = pEnergyLeechEffectActivated;
    }

    public boolean isBattleRepairBotEffectActivated() {
        return mBattleRepairBotEffectActivated;
    }

    public void setBattleRepairBotEffectActivated(final boolean pBattleRepairBotEffectActivated) {
        mBattleRepairBotEffectActivated = pBattleRepairBotEffectActivated;
    }
}
