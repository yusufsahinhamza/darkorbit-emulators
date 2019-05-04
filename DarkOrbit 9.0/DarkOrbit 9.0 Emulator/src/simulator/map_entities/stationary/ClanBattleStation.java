package simulator.map_entities.stationary;

import simulator.logic.LaserAttack;
import simulator.map_entities.Lockable;
import simulator.map_entities.MapEntity;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.netty.serverCommands.AssetCreateCommand;
import simulator.system.SpaceMap;

/**
 Created by bpdev on 01/02/2015.
 */
public class ClanBattleStation
        extends StationaryMapEntity
        implements Lockable {

    private final short mClanBattleStationId;

    private short mStatus;

    private int mClanId;

    public ClanBattleStation(final short pClanBattleStationId, final SpaceMap pCurrentSpaceMapId, final short pStatus,
                             final int pClanId) {
        super(pCurrentSpaceMapId);

        this.mClanBattleStationId = pClanBattleStationId;

        this.mStatus = pStatus;

        this.mClanId = pClanId;

    }

    /**
     <b>Note: to get map entity ID use {@link simulator.map_entities.MapEntity#getMapEntityId()}</b>

     @return CBS ID from DB
     */
    public short getClanBattleStationId() {
        return this.mClanBattleStationId;
    }

    public short getStatus() {
        return this.mStatus;
    }

    public void setStatus(final short pStatus) {
        this.mStatus = pStatus;
    }

    public int getClanId() {
        return this.mClanId;
    }

    public void setClanId(final int pClanId) {
        this.mClanId = pClanId;
    }

    @Override
    public boolean canBeTargeted() {
        return true;
    }

    @Override
    public boolean isInSecureZone() {
        return false;
    }

    @Override
    public int getCurrentHitPoints() {
        return 0;
    }

    @Override
    public void addHitPointsDamage(final MovableMapEntity pMovableMapEntity, final int damage) {

    }

    @Override
    public int damageShieldPoints(final MovableMapEntity pMovableMapEntity, final int pDamage) {
        return 0;
    }

    @Override
    public int changeCurrentHitPoints(final int pDifferenceHitpoints) {
        return 0;
    }

    @Override
    public int getCurrentNanoHull() {
        return 0;
    }

    @Override
    public int getMaximumNanoHull() {
        return 0;
    }

    @Override
    public void changeCurrentNanoHull(final int pDifferenceNanoHull) {

    }

    @Override
    public int getCurrentShieldPoints() {
        return 0;
    }

    @Override
    public int getMaximumShieldPoints() {
        return 0;
    }

    @Override
    public int changeCurrentShieldPoints(final int pDifferenceShieldPoints) {
        return 0;
    }

    @Override
    public int getCurrentShieldAbsorb() {
        return 0;
    }

    @Override
    public void destroy(final MapEntity pKillerMapEntity) {

    }

    @Override
    public String getAssetName() {
        return null;
    }

    @Override
    public short getAssetType() {
        return 0;
    }

    @Override
    public AssetCreateCommand getAssetCreateCommand() {
        return null;
    }

	@Override
	public LaserAttack getLaserAttack() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Lockable getLockedTarget() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public boolean canBeShoot() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean isDestroyed() {
		// TODO Auto-generated method stub
		return false;
	}
}
