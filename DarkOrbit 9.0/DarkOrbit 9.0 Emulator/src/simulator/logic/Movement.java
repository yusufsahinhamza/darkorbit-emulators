package simulator.logic;

import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.MoveCommand;
import utils.MathUtils;

/**
 <b>Not thread-safe</b>
 */
public class Movement {
    //TODO thread safety??

//    private static final int MOVEMENT_END_DISTANCE = 5;

    private final MovableMapEntity mMovableMapEntity;

    private boolean mMovementInProgress;

    // movement initial position (FROM where)
    private int mInitialPositionX;
    private int mInitialPositionY;

    // movement current position (NOW where)
    private int mCurrentPositionX;
    private int mCurrentPositionY;

    // movement target position (TO where)
    private int mTargetPositionX;
    private int mTargetPositionY;

    private int mMovementSpeed;

    private int mTotalDistanceX;
    private int mTotalDistanceY;

    //--------------------------------------
    private long mFullDurationMillis;
    private long mEndTravelTimeMillis;

    private long mMovementCount;
    //-------------------------------------

    public Movement(final MovableMapEntity pMovableMapEntity) {

        this.mMovableMapEntity = pMovableMapEntity;

        this.setCurrentPosition(this.mMovableMapEntity.getCurrentPositionX(),
                                this.mMovableMapEntity.getCurrentPositionY());

    }

    /**
     <p>Initiates movement from defined initial position to defined target position</p>

     @param pInitialPositionX
     initial position X
     @param pInitialPositionY
     initial position Y
     @param pTargetPositionX
     target position X
     @param pTargetPositionY
     target position Y
     @param pMovementSpeed
     movement speed
     */
    public synchronized void initiate(final int pInitialPositionX, final int pInitialPositionY, //
                                      final int pTargetPositionX, final int pTargetPositionY, //
                                      final int pMovementSpeed//
    ) {

        this.setMovementInProgress(true);

        this.setInitialPosition(pInitialPositionX, pInitialPositionY);
        this.setCurrentPosition(pInitialPositionX, pInitialPositionY);
        this.setTargetPosition(pTargetPositionX, pTargetPositionY);
        this.setMovementSpeed(pMovementSpeed);


        this.mTotalDistanceX = this.getTargetPositionX() - this.getCurrentPositionX();
        this.mTotalDistanceY = this.getTargetPositionY() - this.getCurrentPositionY();

        // duration until the end of the travel
        this.mFullDurationMillis = this.getDuration();// -> ms
        //time that ship will stop move (end of route)
        this.mEndTravelTimeMillis = System.currentTimeMillis() + this.mFullDurationMillis;//ms

        this.mMovableMapEntity.sendCommandToInRange(this.getMoveCommand());
        
        this.mMovementCount++;

    }

    public MoveCommand getMoveCommand() {
        return new MoveCommand(this.getMovableMapEntity()
                                   .getMapEntityId(), this.getTargetPositionX(), this.getTargetPositionY(),
                               (int) this.getDuration());
    }

    private long getDuration() {
        final long distanceX = this.getTargetPositionX() - this.getCurrentPositionX();
        final long distanceY = this.getTargetPositionY() - this.getCurrentPositionY();

        //distance to travel
        final double distance = MathUtils.hypotenuse(distanceX, distanceY);

        return (long) (1000.0D * distance / this.getMovementSpeed());
    }

    public void move() {
        if (this.isMovementInProgress()) {

            final long leftTimeMillis = this.mEndTravelTimeMillis - System.currentTimeMillis();//ms

            if (leftTimeMillis > 0L) {

                final int distanceLeftX =
                        (int) (((double) this.mTotalDistanceX / this.mFullDurationMillis) * leftTimeMillis);
                final int distanceLeftY =
                        (int) (((double) this.mTotalDistanceY / this.mFullDurationMillis) * leftTimeMillis);

                this.updateCurrentPosition(distanceLeftX, distanceLeftY);
                this.updateMovableMapEntityPosition();

            } else {
                this.setCurrentPosition(this.getTargetPositionX(), this.getTargetPositionY());
                this.setMovementInProgress(false);
            }

            if (this.mMovableMapEntity instanceof Player) {
                this.mMovableMapEntity.getCurrentSpaceMap()
                                      .onPlayerMovement((Player) this.mMovableMapEntity);
            }
        }
    }

    private void updateMovableMapEntityPosition() {
        this.mMovableMapEntity.setPositionXY(this.getCurrentPositionX(), this.getCurrentPositionY());
    }

    public MovableMapEntity getMovableMapEntity() {
        return this.mMovableMapEntity;
    }

    private void updateCurrentPosition(final int pDistanceLeftX, final int pDistanceLeftY) {
        this.setCurrentPosition(this.getTargetPositionX() - pDistanceLeftX, this.getTargetPositionY() - pDistanceLeftY);
    }

    public void setCurrentPosition(final int pCurrentPositionX, final int pCurrentPositionY) {
        this.mCurrentPositionX = pCurrentPositionX;
        this.mCurrentPositionY = pCurrentPositionY;
    }

    public int getCurrentPositionX() {
        return this.mCurrentPositionX;
    }

    public int getCurrentPositionY() {
        return this.mCurrentPositionY;
    }

    public void setInitialPosition(final int pInitialPositionX, final int pInitialPositionY) {
        this.mInitialPositionX = pInitialPositionX;
        this.mInitialPositionY = pInitialPositionY;
    }

    public int getInitialPositionX() {
        return this.mInitialPositionX;
    }

    public int getInitialPositionY() {
        return this.mInitialPositionY;
    }

    private void setTargetPosition(final int pTargetPositionX, final int pTargetPositionY) {
        this.mTargetPositionY = pTargetPositionY;
        this.mTargetPositionX = pTargetPositionX;
    }

    public int getTargetPositionX() {
        return this.mTargetPositionX;
    }

    public int getTargetPositionY() {
        return this.mTargetPositionY;
    }

    public boolean isMovementInProgress() {
        return this.mMovementInProgress;
    }

    public void setMovementInProgress(final boolean pMovementInProgress) {
        this.mMovementInProgress = pMovementInProgress;
    }

    public int getMovementSpeed() {
        return this.mMovementSpeed;
    }

    public void setMovementSpeed(final int pMovementSpeed) {
        this.mMovementSpeed = pMovementSpeed;
    }

    public void changeMovementSpeed(final int pMovementSpeed) {
        // re-initiate movement with different speed
        this.initiate(this.getCurrentPositionX(), this.getCurrentPositionY(), this.getTargetPositionX(),
                      this.getTargetPositionY(), pMovementSpeed);
    }

    public long getMovementCount() {
        return mMovementCount;
    }
}
