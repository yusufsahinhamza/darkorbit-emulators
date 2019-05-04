package simulator.users;

import org.json.JSONObject;

import java.util.concurrent.TimeUnit;

import simulator.netty.clientCommands.OreTypeModule;
import utils.Tools;

/**
 Created by bpdev on 31/01/2015.
 */
public class ResourcesManager
        extends AbstractAccountInternalManager {

    public static final  short PROMETID_REFINEMENT_PROMETIUM = 20;
    public static final  short PROMETID_REFINEMENT_ENDURIUM  = 10;
    public static final  short DURANIUM_REFINEMENT_ENDURIUM  = 10;
    public static final  short DURANIUM_REFINEMENT_TERBIUM   = 20;
    public static final  short PROMERIUM_REFINEMENT_PROMETID = 10;
    public static final  short PROMERIUM_REFINEMENT_DURANIUM = 10;
    public static final  short PROMERIUM_REFINEMENT_XENOMIT  = 1;
    private static final short LASER_DAMAGE_PROMETID_BOOST   = 15;
    private static final short LASER_DAMAGE_PROMERIUM_BOOST  = 30;
    private static final short LASER_DAMAGE_SEPROM_BOOST     = 60;
    private static final short ROCKET_DAMAGE_PROMETID_BOOST  = 15;
    private static final short ROCKET_DAMAGE_PROMERIUM_BOOST = 30;
    private static final short ROCKET_DAMAGE_SEPROM_BOOST    = 60;
    private static final short SPEED_DURANIUM_BOOST          = 10;
    private static final short SPEED_PROMERIUM_BOOST         = 20;
    private static final short SHIELD_DURANIUM_BOOST         = 10;
    private static final short SHIELD_PROMERIUM_BOOST        = 20;
    private static final short SHIELD_SEPROM_BOOST           = 40;

    private Resources mResources = new Resources();

    public ResourcesManager(final Account pAccount) {
        super(pAccount);
    }

    @Override
    public void setFromJSON(final String pResourcesJSON) {
        //{"resources":{"prometium":0,"endurium":0,"terbium":0,"xenomit":0,"palladium":0,"prometid":0,"duranium":0,"promerium":0,"seprom":0},"upgrades":{"lasers":{"resourceType":0,"resourceAmount":0},"rockets":{"resourceType":0,"resourceAmount":0},"generators":{"resourceType":0,"resourceAmount":0},"shields":{"resourceType":0,"resourceAmount":0}}}
        final JSONObject resourcesJson = new JSONObject(pResourcesJSON);
        final JSONObject resources = resourcesJson.getJSONObject("resources");
        final JSONObject upgrades = resourcesJson.getJSONObject("upgrades");
        final long prometium = resources.getLong("prometium");
        final long endurium = resources.getLong("endurium");
        final long terbium = resources.getLong("terbium");
        final long xenomit = resources.getLong("xenomit");
        final long palladium = resources.getLong("palladium");
        final long prometid = resources.getLong("prometid");
        final long duranium = resources.getLong("duranium");
        final long promerium = resources.getLong("promerium");
        final long seprom = resources.getLong("seprom");
        final JSONObject lasers = upgrades.getJSONObject("lasers");
        final JSONObject rockets = upgrades.getJSONObject("rockets");
        final JSONObject generators = upgrades.getJSONObject("generators");
        final JSONObject shields = upgrades.getJSONObject("shields");
        final short upgradeLasersResourceType = (short) lasers.getInt("resourceType");
        final long upgradeLasersResourceAmount = lasers.getLong("resourceAmount");
        final short upgradeRocketsResourceType = (short) rockets.getInt("resourceType");
        final long upgradeRocketsResourceAmount = rockets.getLong("resourceAmount");
        final short upgradeGeneratorsResourceType = (short) generators.getInt("resourceType");
        final long upgradeGeneratorsResourceAmount = generators.getLong("resourceAmount");
        final short upgradeShieldsResourceType = (short) shields.getInt("resourceType");
        final long upgradeShieldsResourceAmount = shields.getLong("resourceAmount");

        this.mResources =
                new Resources(prometium, endurium, terbium, xenomit, palladium, prometid, duranium, promerium, seprom,
                              upgradeLasersResourceType, upgradeLasersResourceAmount, upgradeRocketsResourceType,
                              upgradeRocketsResourceAmount, upgradeGeneratorsResourceType,
                              upgradeGeneratorsResourceAmount, upgradeShieldsResourceType,
                              upgradeShieldsResourceAmount);
        if (this.getGeneratorsTimeLeft() > 0) {
            this.getResources()
                .setGeneratorsBoostActivated(true);
        }
        if (this.getShieldsTimeLeft() > 0) {
            this.getResources()
                .setShieldBoostActivated(true);
        }
    }

    @Override
    public void setNewAccount() {
        this.mResources = new Resources();
    }

    @Override
    public String packToJSON() {
        final JSONObject resourcesJSON = new JSONObject();
        final JSONObject resources = new JSONObject();
        resources.put("prometium", this.getResources()
                                       .getPrometium());
        resources.put("endurium", this.getResources()
                                      .getEndurium());
        resources.put("terbium", this.getResources()
                                     .getTerbium());
        resources.put("xenomit", this.getResources()
                                     .getXenomit());
        resources.put("palladium", this.getResources()
                                       .getPalladium());
        resources.put("prometid", this.getResources()
                                      .getPrometid());
        resources.put("duranium", this.getResources()
                                      .getDuranium());
        resources.put("promerium", this.getResources()
                                       .getPromerium());
        resources.put("seprom", this.getResources()
                                    .getSeprom());
        final JSONObject upgrades = new JSONObject();
        final JSONObject lasers = new JSONObject();
        lasers.put("resourceType", this.getResources()
                                       .getLasersResourceType());
        lasers.put("resourceAmount", this.getResources()
                                         .getLasersResourceAmount());
        final JSONObject rockets = new JSONObject();
        rockets.put("resourceType", this.getResources()
                                        .getRocketsResourceType());
        rockets.put("resourceAmount", this.getResources()
                                          .getRocketsResourceAmount());
        final JSONObject generators = new JSONObject();
        generators.put("resourceType", this.getResources()
                                           .getGeneratorsResourceType());
        generators.put("resourceAmount", this.getResources()
                                             .getGeneratorsResourceTimeFinish());
        final JSONObject shields = new JSONObject();
        shields.put("resourceType", this.getResources()
                                        .getShieldsResourceType());
        shields.put("resourceAmount", this.getResources()
                                          .getShieldsResourceTimeFinish());
        upgrades.put("lasers", lasers);
        upgrades.put("rockets", rockets);
        upgrades.put("generators", generators);
        upgrades.put("shields", shields);
        resourcesJSON.put("resources", resources);
        resourcesJSON.put("upgrades", upgrades);

        return resourcesJSON.toString();
    }

    public void doTick() {
        if (this.getResources()
                .isGeneratorsBoostActivated() && this.getGeneratorsTimeLeft() < 0) {
            this.getResources()
                .setGeneratorsBoostActivated(false);
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getPlayer()
                                                .getSetSpeedCommand());
        }
        if (this.getResources()
                .isShieldBoostActivated() && this.getShieldsTimeLeft() < 0) {
            this.getResources()
                .setShieldBoostActivated(false);
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getPlayer()
                                                .getShieldUpdateCommand());
        }
    }

    public int getResourcesCountInCargo() {
        return (int) (this.getResources()
                          .getPrometium() + this.getResources()
                                                .getEndurium() + this.getResources()
                                                                     .getTerbium() + this.getResources()
                                                                                         .getXenomit() +
                      this.getResources()
                          .getPalladium() +
                      this.getResources()
                          .getPrometid() + this.getResources()
                                               .getDuranium() + this.getResources()
                                                                    .getPromerium() + this.getResources()
                                                                                          .getSeprom());
    }

    public void refinePrometid(final long pAmount) {
        final long prometiumRemoveAmount = pAmount * PROMETID_REFINEMENT_PROMETIUM;
        final long enduriumRemoveAmount = pAmount * PROMETID_REFINEMENT_ENDURIUM;
        final boolean canRemovePrometium = this.getResources()
                                               .getPrometium() - prometiumRemoveAmount >= 0;
        final boolean canRemoveEndurium = this.getResources()
                                              .getEndurium() - enduriumRemoveAmount >= 0;
        if (canRemovePrometium && canRemoveEndurium) {
            this.getResources()
                .changePrometium(-prometiumRemoveAmount);
            this.getResources()
                .changeEndurium(-enduriumRemoveAmount);
            this.getResources()
                .changePrometid(+pAmount);
        }
    }

    public void refineDuranium(final long pAmount) {
        final long enduriumRemoveAmount = pAmount * DURANIUM_REFINEMENT_ENDURIUM;
        final long terbiumRemoveAmount = pAmount * DURANIUM_REFINEMENT_TERBIUM;
        final boolean canRemoveEndurium = this.getResources()
                                              .getEndurium() - enduriumRemoveAmount >= 0;
        final boolean canRemoveTerbium = this.getResources()
                                             .getTerbium() - terbiumRemoveAmount >= 0;
        if (canRemoveEndurium && canRemoveTerbium) {
            this.getResources()
                .changeEndurium(-enduriumRemoveAmount);
            this.getResources()
                .changeTerbium(-terbiumRemoveAmount);
            this.getResources()
                .changeDuranium(+pAmount);
        }
    }

    public void refinePromerium(final long pAmount) {
        final long prometidRemoveAmount = pAmount * PROMERIUM_REFINEMENT_PROMETID;
        final long duraniumRemoveAmount = pAmount * PROMERIUM_REFINEMENT_DURANIUM;
        final long xenomitRemoveAmount = pAmount * PROMERIUM_REFINEMENT_XENOMIT;
        final boolean canRemovePrometid = this.getResources()
                                              .getPrometid() - prometidRemoveAmount >= 0;
        final boolean canRemoveDuranium = this.getResources()
                                              .getDuranium() - duraniumRemoveAmount >= 0;
        final boolean canRemoveXenomit = this.getResources()
                                             .getXenomit() - xenomitRemoveAmount >= 0;
        if (canRemovePrometid && canRemoveDuranium && canRemoveXenomit) {
            this.getResources()
                .changePrometid(-prometidRemoveAmount);
            this.getResources()
                .changeDuranium(-duraniumRemoveAmount);
            this.getResources()
                .changeXenomit(-xenomitRemoveAmount);
            this.getResources()
                .changePromerium(+pAmount);
        }
    }

    private static final int UPDATE_ITEM_UNIT = 10;

    public void upgradeLasers(final OreTypeModule pOreTypeModule, final long pAmount) {
        boolean bought = false;
        switch (pOreTypeModule.typeValue) {
            case OreTypeModule.PROMETID:
                if (this.getResources()
                        .getPrometid() - pAmount >= 0) {
                    this.getResources()
                        .changePrometid(-pAmount);

                    bought = true;
                }
                break;
            case OreTypeModule.PROMERIUM:
                if (this.getResources()
                        .getPromerium() - pAmount >= 0) {
                    this.getResources()
                        .changePromerium(-pAmount);
                    bought = true;
                }
                break;
            case OreTypeModule.SEPROM:
                if (this.getResources()
                        .getSeprom() - pAmount >= 0) {
                    this.getResources()
                        .changeSeprom(-pAmount);
                    bought = true;
                }
                break;
        }
        if (bought) {
            if (this.getResources()
                    .getLasersResourceType() != pOreTypeModule.typeValue) {
                this.getResources()
                    .setLasersResourceType(pOreTypeModule.typeValue);
                this.getResources()
                    .setLasersResourceAmount(pAmount * UPDATE_ITEM_UNIT);
            } else {
                this.getResources()
                    .changeLasersResourceAmount(+(pAmount * UPDATE_ITEM_UNIT));
            }
        }
    }

    public void upgradeRockets(final OreTypeModule pOreTypeModule, final long pAmount) {
        boolean bought = false;
        switch (pOreTypeModule.typeValue) {
            case OreTypeModule.PROMETID:
                if (this.getResources()
                        .getPrometid() - pAmount >= 0) {
                    this.getResources()
                        .changePrometid(-pAmount);

                    bought = true;
                }
                break;
            case OreTypeModule.PROMERIUM:
                if (this.getResources()
                        .getPromerium() - pAmount >= 0) {
                    this.getResources()
                        .changePromerium(-pAmount);
                    bought = true;
                }
                break;
            case OreTypeModule.SEPROM:
                if (this.getResources()
                        .getSeprom() - pAmount >= 0) {
                    this.getResources()
                        .changeSeprom(-pAmount);
                    bought = true;
                }
                break;
        }
        if (bought) {
            if (this.getResources()
                    .getRocketsResourceType() != pOreTypeModule.typeValue) {
                this.getResources()
                    .setRocketsResourceType(pOreTypeModule.typeValue);
                this.getResources()
                    .setRocketsResourceAmount(pAmount * UPDATE_ITEM_UNIT);
            } else {
                this.getResources()
                    .changeRocketsResourceAmount(+(pAmount * UPDATE_ITEM_UNIT));
            }
        }
    }

    public void upgradeGenerators(final OreTypeModule pOreTypeModule, final long pAmount) {
        boolean bought = false;
        switch (pOreTypeModule.typeValue) {
            case OreTypeModule.DURANIUM:
                if (this.getResources()
                        .getDuranium() - pAmount >= 0) {
                    this.getResources()
                        .changeDuranium(-pAmount);

                    bought = true;
                }
                break;
            case OreTypeModule.PROMERIUM:
                if (this.getResources()
                        .getPromerium() - pAmount >= 0) {
                    this.getResources()
                        .changePromerium(-pAmount);
                    bought = true;
                }
                break;
        }
        if (bought) {
            final long currentTimeMinutes = TimeUnit.MILLISECONDS.toMinutes(System.currentTimeMillis());
            final long timeFinish = currentTimeMinutes + (pAmount * UPDATE_ITEM_UNIT);
            if (this.getResources()
                    .getGeneratorsResourceType() != pOreTypeModule.typeValue) {
                this.getResources()
                    .setGeneratorsResourceType(pOreTypeModule.typeValue);
                this.getResources()
                    .setGeneratorsResourceTimeFinish(timeFinish);
            } else {
                this.getResources()
                    .changeGeneratorsResourceTimeFinish(+timeFinish);
            }
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getPlayer()
                                                .getSetSpeedCommand());
        }
    }

    public void upgradeShields(final OreTypeModule pOreTypeModule, final long pAmount) {
        boolean bought = false;
        switch (pOreTypeModule.typeValue) {
            case OreTypeModule.DURANIUM:
                if (this.getResources()
                        .getDuranium() - pAmount >= 0) {
                    this.getResources()
                        .changeDuranium(-pAmount);

                    bought = true;
                }
                break;
            case OreTypeModule.PROMERIUM:
                if (this.getResources()
                        .getPromerium() - pAmount >= 0) {
                    this.getResources()
                        .changePromerium(-pAmount);
                    bought = true;
                }
                break;
            case OreTypeModule.SEPROM:
                if (this.getResources()
                        .getSeprom() - pAmount >= 0) {
                    this.getResources()
                        .changeSeprom(-pAmount);
                    bought = true;
                }
                break;
        }
        if (bought) {
            final long currentTimeMinutes = TimeUnit.MILLISECONDS.toMinutes(System.currentTimeMillis());
            final long timeFinish = currentTimeMinutes + (pAmount * UPDATE_ITEM_UNIT);
            if (this.getResources()
                    .getShieldsResourceType() != pOreTypeModule.typeValue) {
                this.getResources()
                    .setShieldsResourceType(pOreTypeModule.typeValue);
                this.getResources()
                    .setShieldsResourceTimeFinish(timeFinish);
            } else {
                this.getResources()
                    .changeShieldsResourceTimeFinish(+(timeFinish));
            }
            this.getAccount()
                .getPlayer()
                .sendCommandToBoundSessions(this.getAccount()
                                                .getPlayer()
                                                .getShieldUpdateCommand());
        }
    }

    public int getLaserDamageBoost(final int pDamage, final int pLasersRounds) {
        if (this.getResources()
                .getLasersResourceAmount() > 0) {
            this.getResources()
                .changeLasersResourceAmount(-pLasersRounds);
            switch (this.getResources()
                        .getLasersResourceType()) {
                case OreTypeModule.PROMETID:
                    return (int) Tools.getBoost(pDamage, LASER_DAMAGE_PROMETID_BOOST);
                case OreTypeModule.PROMERIUM:
                    return (int) Tools.getBoost(pDamage, LASER_DAMAGE_PROMERIUM_BOOST);
                case OreTypeModule.SEPROM:
                    return (int) Tools.getBoost(pDamage, LASER_DAMAGE_SEPROM_BOOST);
                default:
                    return pDamage;
            }
        }
        return pDamage;
    }

    public int getRocketDamageBoost(final int pDamage, final int pRocketsRounds) {
        if (this.getResources()
                .getRocketsResourceAmount() > 0) {
            this.getResources()
                .changeRocketsResourceAmount(-pRocketsRounds);
            switch (this.getResources()
                        .getRocketsResourceType()) {
                case OreTypeModule.PROMETID:
                    return (int) Tools.getBoost(pDamage, ROCKET_DAMAGE_PROMETID_BOOST);
                case OreTypeModule.PROMERIUM:
                    return (int) Tools.getBoost(pDamage, ROCKET_DAMAGE_PROMERIUM_BOOST);
                case OreTypeModule.SEPROM:
                    return (int) Tools.getBoost(pDamage, ROCKET_DAMAGE_SEPROM_BOOST);
                default:
                    return pDamage;
            }
        }
        return pDamage;
    }

    public int getSpeedBoost(final int pSpeed) {
        if (this.getGeneratorsTimeLeft() > 0) {
            switch (this.getResources()
                        .getGeneratorsResourceType()) {
                case OreTypeModule.DURANIUM:
                    return (int) Tools.getBoost(pSpeed, SPEED_DURANIUM_BOOST);
                case OreTypeModule.PROMERIUM:
                    return (int) Tools.getBoost(pSpeed, SPEED_PROMERIUM_BOOST);
                default:
                    return pSpeed;
            }
        }
        return pSpeed;
    }

    public int getShieldPointsBoost(final int pShieldPoints) {
        if (this.getShieldsTimeLeft() > 0) {
            switch (this.getResources()
                        .getShieldsResourceType()) {
                case OreTypeModule.DURANIUM:
                    return (int) Tools.getBoost(pShieldPoints, SHIELD_DURANIUM_BOOST);
                case OreTypeModule.PROMERIUM:
                    return (int) Tools.getBoost(pShieldPoints, SHIELD_PROMERIUM_BOOST);
                case OreTypeModule.SEPROM:
                    return (int) Tools.getBoost(pShieldPoints, SHIELD_SEPROM_BOOST);
                default:
                    return pShieldPoints;
            }
        }
        return pShieldPoints;
    }

    public long getGeneratorsTimeLeft() {
        final long currentTimeMinutes = TimeUnit.MILLISECONDS.toMinutes(System.currentTimeMillis());
        final long leftTime = currentTimeMinutes - this.getResources()
                                                       .getGeneratorsResourceTimeFinish();
        if (leftTime > 0) {
            return 0;
        }
        return -leftTime;
    }

    public long getShieldsTimeLeft() {
        final long currentTimeMinutes = TimeUnit.MILLISECONDS.toMinutes(System.currentTimeMillis());
        final long leftTime = currentTimeMinutes - this.getResources()
                                                       .getShieldsResourceTimeFinish();
        if (leftTime > 0) {
            return 0;
        }
        return -leftTime;
    }

    public Resources getResources() {
        return mResources;
    }
}
