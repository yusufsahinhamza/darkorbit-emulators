package simulator.ia;

import simulator.map_entities.movable.MovableMapEntity;
import utils.Tools;

/**
 Created by Pedro on 27-03-2015.
 */
public class ProtegitAI
        implements IArtificialInteligence {

    private static final int PROTEGITS_MAX_DISTANCE_FROM_CUBIKON = 700;

    private final MovableMapEntity mMovableMapEntity;
    private final MovableMapEntity mCubikonEntity;

    public ProtegitAI(final MovableMapEntity pMovableMapEntity, final MovableMapEntity pCubikonEntity) {
        this.mMovableMapEntity = pMovableMapEntity;
        this.mCubikonEntity = pCubikonEntity;
    }

    @Override
    public void checkArtificialInteligence() {
        if (!this.mMovableMapEntity.getMovement()
                                   .isMovementInProgress()) {
            this.generateNextMovement();
        }
    }

    @Override
    public void receivedAttack(final MovableMapEntity pMovableMapEntity) {

    }

    private void generateNextMovement() {
        final int nextPosX =
                Tools.getRandom(this.mCubikonEntity.getCurrentPositionX() - PROTEGITS_MAX_DISTANCE_FROM_CUBIKON,
                                this.mCubikonEntity.getCurrentPositionX() + PROTEGITS_MAX_DISTANCE_FROM_CUBIKON);
        final int nextPosY =
                Tools.getRandom(this.mCubikonEntity.getCurrentPositionY() - PROTEGITS_MAX_DISTANCE_FROM_CUBIKON,
                                this.mCubikonEntity.getCurrentPositionY() + PROTEGITS_MAX_DISTANCE_FROM_CUBIKON);

        this.mMovableMapEntity.getMovement()
                              .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                                        this.mMovableMapEntity.getCurrentPositionY(), nextPosX, nextPosY,
                                        this.mMovableMapEntity.getBaseSpeed());
    }
}
