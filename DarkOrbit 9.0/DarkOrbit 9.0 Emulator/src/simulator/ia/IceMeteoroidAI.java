package simulator.ia;

import java.util.HashMap;

import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.system.ships.ShipsConstants;
import utils.Tools;

/**
Created by LEJYONER on 09/09/2017.
*/
public class IceMeteoroidAI
        implements IArtificialInteligence {

    private static final int MAX_ICY_NUMBER                       = 15;
    private static final int ICE_METEOROID_MAX_DISTANCE_FROM_INITIAL_POSITION = 500;

    private HashMap<Integer, Alien> mIcys = new HashMap<>();

    private final MovableMapEntity mMovableMapEntity;

    private final int mInitialPositionX;
    private final int mInitialPositionY;

    private MovableMapEntity mLockedMapEntity;

    public IceMeteoroidAI(final MovableMapEntity pMovableMapEntity) {
        this.mMovableMapEntity = pMovableMapEntity;

        this.mInitialPositionX = pMovableMapEntity.getCurrentPositionX();
        this.mInitialPositionY = pMovableMapEntity.getCurrentPositionY();
    }

    @Override
    public void checkArtificialInteligence() {
        this.checkMovement();
    }

    @Override
    public void receivedAttack(final MovableMapEntity pMovableMapEntity) {
        mLockedMapEntity = pMovableMapEntity;
        this.createIcys();
    }

    private void checkMovement() {
        if (!this.mMovableMapEntity.getMovement()
                                   .isMovementInProgress()) {
            final int nextPosX = Tools.getRandom(this.mInitialPositionX - ICE_METEOROID_MAX_DISTANCE_FROM_INITIAL_POSITION,
                                                 this.mInitialPositionX + ICE_METEOROID_MAX_DISTANCE_FROM_INITIAL_POSITION);
            final int nextPosY = Tools.getRandom(this.mInitialPositionY - ICE_METEOROID_MAX_DISTANCE_FROM_INITIAL_POSITION,
                                                 this.mInitialPositionY + ICE_METEOROID_MAX_DISTANCE_FROM_INITIAL_POSITION);

            this.mMovableMapEntity.getMovement()
                                  .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                                            this.mMovableMapEntity.getCurrentPositionY(), nextPosX, nextPosY,
                                            this.mMovableMapEntity.getBaseSpeed());
        }
    }

    private void createIcys() {
        while (this.mIcys.size() < MAX_ICY_NUMBER) {
            final Alien alien = new Alien(ShipsConstants.ICY_ID, this.mMovableMapEntity.getCurrentSpaceMap(),
                                          this.mMovableMapEntity);
            this.mMovableMapEntity.getCurrentSpaceMap()
                                  .addAlien(alien);
            this.addIcy(alien);
            alien.receivedAttack(this.mLockedMapEntity);
        }
    }

    private void addIcy(final Alien pAlien) {
    	mIcys.put(pAlien.getAlienId(), pAlien);
    }
}
