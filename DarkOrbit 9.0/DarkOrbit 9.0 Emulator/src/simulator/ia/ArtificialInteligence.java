package simulator.ia;

import simulator.map_entities.movable.MovableMapEntity;

/**
 Created by Pedro on 20-03-2015.
 */
public abstract class ArtificialInteligence {

    private static final int ALIEN_DISTANCE_TO_USER = 150;
    private static final int MAX_ALIENS_PER_USER    = 5;
    //    private static final int CHANGE_ENEMY_TIME      = 10000;
    private final static int INTERVAL_DELAY         = 2000;

    public enum AIOption {
        // stay still and ignore everything
        DO_NOTHING,
        // move randomly earching for enemies
        SEARCH_FOR_ENEMIES,
        //move to enemy
        FLY_TO_ENEMY,
        // attack locked enemy
        ATTACK_ENEMY,
        // flee from locked enemy
        FLEE_FROM_ENEMY,
    }

    final MovableMapEntity mAttackableMapEntity;

    private long mLastRun = 0L;

    private long mMovementsToTargetCount = 0;
    private long mTargetMovementsCount   = 0;

    private AIOption mAIOption = AIOption.DO_NOTHING;

    public ArtificialInteligence(final MovableMapEntity pAttackableMapEntity, final AIOption pDefaultAiOption) {
        this.mAttackableMapEntity = pAttackableMapEntity;
        this.setAIOption(pDefaultAiOption);
    }

    public long getLastRun() {
        return mLastRun;
    }

    public void setLastRun(final long pLastRun) {
        mLastRun = pLastRun;
    }

    /**
     @return current AIOption of NPC
     */
    public AIOption getAIOption() {
        return this.mAIOption;
    }

    /**
     @param pAIOption
     sets new AIOption for NPC
     */
    public void setAIOption(final AIOption pAIOption) {
        this.mAIOption = pAIOption;
    }

    public void run() {
        //        final long currentTime = System.currentTimeMillis();
        //        if ((currentTime - this.getLastRun()) > INTERVAL_DELAY) {
        //            if (this.getAIOption() == AIOption.DO_NOTHING) {
        //
        //            } else if (this.getAIOption() == AIOption.SEARCH_FOR_ENEMIES) {
        //                for (final MovableMapEntity movableMapEntity : this.mAttackableMapEntity.getInRangeMovableMapEntities()) {
        //                    if (movableMapEntity instanceof Player) {
        //                        //                        if (this.mMovableMapEntity.getLockedTarget() != movableMapEntity) {
        //                        final Player player = (Player) this.mAttackableMapEntity.getLockedTarget();
        //                        final boolean canTargetOtherPlayer =
        //                                player == null || player.mAliensCount > MAX_ALIENS_PER_USER;
        //                        if (movableMapEntity.canBeTargeted() && canTargetOtherPlayer) {
        //                            //                            this.mAttackableMapEntity.setLockedTarget(movableMapEntity);
        //                            ((Player) movableMapEntity).mAliensCount += 1;
        //                            this.mTargetMovementsCount = movableMapEntity.getMovement()
        //                                                                         .getMovementCount();
        //                            this.mMovementsToTargetCount = 0;
        //                            this.setAIOption(AIOption.FLY_TO_ENEMY);
        //                            break;
        //                        }
        //                        //                        }
        //                    }
        //                }
        //            } else if (this.getAIOption() == AIOption.FLY_TO_ENEMY) {
        //                if (this.mAttackableMapEntity.getLockedTarget() != null) {
        //                    final long targetMovementsCount = this.mAttackableMapEntity.getLockedTarget()
        //                                                                               .getMovement()
        //                                                                               .getMovementCount();
        //                    if (this.mMovementsToTargetCount == 0 || this.mTargetMovementsCount != targetMovementsCount ||
        //                        this.mAttackableMapEntity.getLockedTarget()
        //                                                 .getMovement()
        //                                                 .isMovementInProgress()) {
        //                        this.mTargetMovementsCount = targetMovementsCount;
        //                        final double angle = Math.toRadians(Math.random() * 360);
        //                        final int newPosX = (int) (this.mAttackableMapEntity.getLockedTarget()
        //                                                                            .getCurrentPositionX() +
        //                                                   (ALIEN_DISTANCE_TO_USER * Math.cos(angle)));
        //                        final int newPosY = (int) (this.mAttackableMapEntity.getLockedTarget()
        //                                                                            .getCurrentPositionY() +
        //                                                   (ALIEN_DISTANCE_TO_USER * Math.sin(angle)));
        //                        this.mAttackableMapEntity.getMovement()
        //                                                 .initiate(this.mAttackableMapEntity.getCurrentPositionX(),
        //                                                           this.mAttackableMapEntity.getCurrentPositionY(), newPosX,
        //                                                           newPosY, this.mAttackableMapEntity.getBaseSpeed());
        //                        this.mMovementsToTargetCount += 1;
        //                    } else if (((Player) this.mAttackableMapEntity.getLockedTarget()).mAliensCount >
        //                               MAX_ALIENS_PER_USER) {
        //                        ((Player) this.mAttackableMapEntity.getLockedTarget()).mAliensCount -= 1;
        //                        this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
        //                    }
        //                } else {
        //                    this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
        //                }
        //            } else if (this.getAIOption() == (AIOption.ATTACK_ENEMY)) {
        //
        //            } else if (this.getAIOption() == (AIOption.FLEE_FROM_ENEMY)) {
        //
        //            }
        //            this.setLastRun(currentTime);
        //        }
    }
}
