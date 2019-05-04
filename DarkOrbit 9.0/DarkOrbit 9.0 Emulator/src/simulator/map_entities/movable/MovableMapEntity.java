package simulator.map_entities.movable;

import java.util.Collection;
import java.util.HashSet;

import simulator.logic.LaserAttack;
import simulator.logic.Movement;
import simulator.logic.RocketAttack;
import simulator.logic.RocketLauncherAttack;
import simulator.map_entities.IShip;
import simulator.map_entities.Lockable;
import simulator.map_entities.MapEntity;
import simulator.netty.ServerCommand;
import simulator.netty.serverCommands.LegacyModule;
import simulator.system.SpaceMap;
import storage.SpaceMapStorage;

/**
 Everything that moves on map has to extend this class
 */
public abstract class MovableMapEntity
        extends MapEntity
        implements Lockable, IShip {

    protected final Movement     mMovement;
    protected final LaserAttack  mLaserAttack;
    protected final RocketAttack mRocketAttack;
    protected final RocketLauncherAttack mRocketLauncherAttack;
    
    protected final HashSet<MovableMapEntity> mInRangeMovableMapIntities;

    protected Lockable mLockedTarget;

    protected MovableMapEntity(final SpaceMap pCurrentSpaceMapId) {
        super(pCurrentSpaceMapId);

        this.mMovement = new Movement(this);
        this.mLaserAttack = new LaserAttack(this);
        this.mRocketAttack = new RocketAttack(this);
        this.mRocketLauncherAttack = new RocketLauncherAttack(this);
        this.mInRangeMovableMapIntities = new HashSet<>();
    }

    public MovableMapEntity(final SpaceMap pPlayerDefaultMapId, final int pMapEntityIdOverride) {
        super(pPlayerDefaultMapId, pMapEntityIdOverride);

        this.mMovement = new Movement(this);
        this.mLaserAttack = new LaserAttack(this);
        this.mRocketAttack = new RocketAttack(this);
        this.mRocketLauncherAttack = new RocketLauncherAttack(this);
        this.mInRangeMovableMapIntities = new HashSet<>();
    }

    public Movement getMovement() {
        return this.mMovement;
    }

    public LaserAttack getLaserAttack() {
        return this.mLaserAttack;
    }

    public RocketAttack getRocketAttack() {
        return this.mRocketAttack;
    }

    public RocketLauncherAttack getRocketLauncherAttack() {
        return this.mRocketLauncherAttack;
    }
    
    public boolean hasMapEntityInRange(final MovableMapEntity pAttackableMapEntity) {
        return this.mInRangeMovableMapIntities.contains(pAttackableMapEntity);
    }

    public void addInRangeMapEntity(final MovableMapEntity pAttackableMapEntity) {
        this.mInRangeMovableMapIntities.add(pAttackableMapEntity);
    }

    public void removeAllInRangeMapIntities() {
        this.mInRangeMovableMapIntities.clear();
    }

    public Collection<MovableMapEntity> getInRangeMovableMapEntities() {
        return this.mInRangeMovableMapIntities;
    }

    public void sendCommandToInRange(final ServerCommand pServerCommand) {    	
    	final SpaceMap spaceMap = SpaceMapStorage.getSpaceMap(this.getCurrentSpaceMapId());
    	try{
        	for(final MovableMapEntity inRangeEntity : spaceMap.getAllPlayers()) {
        		final Player otherPlayer = (Player) inRangeEntity;
                if ((Math.abs(this.getCurrentPositionX() - otherPlayer.getCurrentPositionX()) <= SpaceMap.VISIBILITY_RANGE) &&
                        (Math.abs(this.getCurrentPositionY() - otherPlayer.getCurrentPositionY()) <= SpaceMap.VISIBILITY_RANGE)) {
                			if(otherPlayer != this) {
                				otherPlayer.sendCommandToBoundSessions(pServerCommand);
                			}
                }
        	}
    	}catch(Exception e) {
    		System.out.print("MovableMapEntity: sendCommandToInRange problemi!");
    	}
    }

    public void sendPacketToInRange(final String pPacket) {
        this.sendCommandToInRange(new LegacyModule(pPacket));
    }
}
