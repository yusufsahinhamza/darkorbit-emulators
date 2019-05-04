package simulator.map_entities;

import simulator.logic.LaserAttack;
import simulator.map_entities.movable.MovableMapEntity;

/**
 TODO possibly rename(or also create interface that defines object that may lock(aim) lockable objects)
 <p/>
 Implement in classes that may be locked (targeted)
 */
public interface Lockable {

    public int getMapEntityId();

    public LaserAttack getLaserAttack();
    
    public Lockable getLockedTarget();
    
    public boolean isDestroyed();
    
    public boolean canBeTargeted();
    
    public boolean canBeShoot();
    
    public boolean isInSecureZone();

    public int getCurrentPositionX();

    public int getCurrentPositionY();

    public int getCurrentHitPoints();

    public void addHitPointsDamage(final MovableMapEntity pMovableMapEntity, final int damage);

    public int damageShieldPoints(final MovableMapEntity pMovableMapEntity, int pDamage);

    public int changeCurrentHitPoints(final int pDifferenceHitpoints);

    public int getCurrentNanoHull();

    public int getMaximumNanoHull();

    public void changeCurrentNanoHull(final int pDifferenceNanoHull);

    public int getCurrentShieldPoints();

    public int getMaximumShieldPoints();

    public int changeCurrentShieldPoints(final int pDifferenceShieldPoints);

    public int getCurrentShieldAbsorb();

    public void destroy(final MapEntity pKillerMapEntity);
}
