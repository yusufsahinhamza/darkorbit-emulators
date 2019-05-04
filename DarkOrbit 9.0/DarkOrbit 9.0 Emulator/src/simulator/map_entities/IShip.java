package simulator.map_entities;

import simulator.map_entities.movable.MovableMapEntity;
import simulator.netty.serverCommands.AttributeHitpointUpdateCommand;
import simulator.netty.serverCommands.AttributeShieldUpdateCommand;
import simulator.netty.serverCommands.ShipCreateCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;

/**
 Created by Pedro on 18-03-2015.
 */
public interface IShip {

    public void setLockedTarget(final Lockable pLockedTarget);

    public Lockable getLockedTarget();

    public void receivedAttack(final MovableMapEntity pMovableMapEntity);

    public int getBaseHitPoints();

    public int getBaseSpeed();

    public int getCurrentDamage();

    public int getMaximumHitPoints();

    public int getMaximumShieldPoints();

    public ShipCreateCommand getShipCreateCommand(short relationType, boolean sameClan);

    public AttributeHitpointUpdateCommand getHitpointsUpdateCommand();

    public AttributeShieldUpdateCommand getShieldUpdateCommand();

    public void addInRangeMapEntity(final MovableMapEntity pMovableMapEntity);

    public void doTick();

    public ShipRemoveCommand removeInRangeMapEntity(final MovableMapEntity pMovableMapEntity);

}
