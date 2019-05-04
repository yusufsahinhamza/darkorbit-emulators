package simulator.ia;

import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import utils.MathUtils;
import utils.Tools;

/**
 Created by Pedro on 27-03-2015.
 */
public class AlienAI
        implements IArtificialInteligence {

    private final static int INTERVAL_DELAY         = 1000;
    private final static int ALIEN_DISTANCE_TO_USER = 300;

    private final MovableMapEntity mMovableMapEntity;

    private long mLastMovement        = 0L;
    private long mTargetMovementCount = 0L;

    private AIOption mAIOption = AIOption.SEARCH_FOR_ENEMIES;

    public AlienAI(final MovableMapEntity pMovableMapEntity) {
        this.mMovableMapEntity = pMovableMapEntity;
    }

    @Override
    public void checkArtificialInteligence() {
        this.checkMovement();
    }

    @Override
    public void receivedAttack(final MovableMapEntity pMovableMapEntity) {

    }

    private void checkMovement() {
        final long currentTime = System.currentTimeMillis();
        if (currentTime - this.getLastMovement() >= INTERVAL_DELAY) {
            switch (this.getAIOption()) {
                case SEARCH_FOR_ENEMIES:
                    for (final MovableMapEntity movableMapEntity : this.mMovableMapEntity.getInRangeMovableMapEntities()) {
                        if (movableMapEntity instanceof Player) {
                        	final Player player = (Player) movableMapEntity;
                        	if(!player.getAccount().isCloaked()) {
	                        	if(!player.inInSecureZone) {
		                            if (movableMapEntity.canBeTargeted()) {
		                            	if(!movableMapEntity.isInSecureZone()) {
			                                	if(this.mMovableMapEntity instanceof Alien) {
			                                		final Alien alien = (Alien) this.mMovableMapEntity;
				                                	if(alien.isAggressive()) {
				    	                                if (MathUtils.hypotenuse(
				    	                                		this.mMovableMapEntity.getCurrentPositionX() - movableMapEntity.getCurrentPositionX(),
				    	                                		this.mMovableMapEntity.getCurrentPositionY() - movableMapEntity.getCurrentPositionY()) <=
				    	                                    2000) {
				    	                                	if(!player.inInSecureZone) {
				    	                                		this.mMovableMapEntity.receivedAttack(movableMapEntity); // yaratığın oyuncuya saldırma kodu
				    	                                	}
				    	                                }
				                                	}
			                                	}
			                                this.mMovableMapEntity.setLockedTarget(movableMapEntity);
			                                this.setAIOption(AIOption.FLY_TO_ENEMY);
			                            	
		                            	} else {
		                                	this.mMovableMapEntity.setLockedTarget(null);
		                                	this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
		                            	}
		                            } else {
		                            	this.mMovableMapEntity.setLockedTarget(null);
		                            	this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
		                            }
	                        	} else {
	                            	this.mMovableMapEntity.setLockedTarget(null);
	                            	this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
	                            }
                        	} else {
                            	this.mMovableMapEntity.setLockedTarget(null);
                            	this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
                            }
                        }
                    }
                    if (!this.mMovableMapEntity.getMovement()
                                               .isMovementInProgress() &&
                        this.mMovableMapEntity.getLockedTarget() == null) {
                        final int nextPosX = Tools.sRandom.nextInt(20000);
                        final int nextPosY = Tools.sRandom.nextInt(12800);

                        this.mMovableMapEntity.getMovement()
                                              .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                                                        this.mMovableMapEntity.getCurrentPositionY(), nextPosX,
                                                        nextPosY, this.mMovableMapEntity.getBaseSpeed());
                    }
                    break;
                case FLY_TO_ENEMY:
                    if (this.mMovableMapEntity.getLockedTarget() != null) {
                    	if(!this.mMovableMapEntity.getLockedTarget().isInSecureZone()) {
                    		 if (MathUtils.hypotenuse(
                             		this.mMovableMapEntity.getCurrentPositionX() - this.mMovableMapEntity.getLockedTarget().getCurrentPositionX(),
                             		this.mMovableMapEntity.getCurrentPositionY() - this.mMovableMapEntity.getLockedTarget().getCurrentPositionY()) <=
                                 2000) {
			                        final double angle = Math.toRadians(Math.random() * 360);
			                        final int newPosX = (int) (this.mMovableMapEntity.getLockedTarget()
			                                                                         .getCurrentPositionX() +
			                                                   (ALIEN_DISTANCE_TO_USER * Math.cos(angle)));
			                        final int newPosY = (int) (this.mMovableMapEntity.getLockedTarget()
			                                                                         .getCurrentPositionY() +
			                                                   (ALIEN_DISTANCE_TO_USER * Math.sin(angle)));
			                        this.mMovableMapEntity.getMovement()
			                                              .initiate(this.mMovableMapEntity.getCurrentPositionX(),
			                                                        this.mMovableMapEntity.getCurrentPositionY(), newPosX, newPosY,
			                                                        this.mMovableMapEntity.getBaseSpeed());
			                        this.setAIOption(AIOption.WAIT_PLAYER_MOVE);
                    		 } else {
                    			 this.mMovableMapEntity.setLockedTarget(null);
                         		this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
                         	}
                    	} else {
                    		this.mMovableMapEntity.setLockedTarget(null);
                    		this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
                    	}
                    }
                    break;
                case WAIT_PLAYER_MOVE:
                    if (this.mMovableMapEntity.getLockedTarget() != null) {
                    	final MovableMapEntity player = (MovableMapEntity) this.mMovableMapEntity.getLockedTarget();
                    	if(!player.isInSecureZone()) {
                        if (player.getMovement()
                                  .getMovementCount() > this.getTargetMovementCount()) {
                            this.setTargetMovementCount(player.getMovement()
                                                              .getMovementCount());
                            this.setAIOption(AIOption.FLY_TO_ENEMY);
                        }
                    	} else {
                    		this.mMovableMapEntity.setLockedTarget(null);
                    		this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
                    	}
                    } else {
                    	this.mMovableMapEntity.setLockedTarget(null);
                        this.setAIOption(AIOption.SEARCH_FOR_ENEMIES);
                    }
                    break;
                case FLEE_FROM_ENEMY:
                    break;
            }
            this.setLastMovement(currentTime);
        }
    }

    public long getLastMovement() {
        return mLastMovement;
    }

    public void setLastMovement(final long pLastMovement) {
        mLastMovement = pLastMovement;
    }

    public AIOption getAIOption() {
        return mAIOption;
    }

    public void setAIOption(final AIOption pAIOption) {
        mAIOption = pAIOption;
    }

    public long getTargetMovementCount() {
        return mTargetMovementCount;
    }

    public void setTargetMovementCount(final long pTargetMovementCount) {
        mTargetMovementCount = pTargetMovementCount;
    }
}
