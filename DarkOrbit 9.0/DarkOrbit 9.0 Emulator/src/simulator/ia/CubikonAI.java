package simulator.ia;

import java.util.HashMap;

import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.system.ships.ShipsConstants;
import utils.Tools;

/**
 Created by Pedro on 27-03-2015.
 */
public class CubikonAI
        implements IArtificialInteligence {

    private static final int MAX_PROTEGITS_NUMBER                       = 30;
    private static final int CUBIKON_MAX_DISTANCE_FROM_INITIAL_POSITION = 100;

    public HashMap<Integer, Alien> mProtegits = new HashMap<>();

    private final MovableMapEntity mMovableMapEntity;

    private final int mInitialPositionX;
    private final int mInitialPositionY;

    private MovableMapEntity mLockedMapEntity;

    public CubikonAI(final MovableMapEntity pMovableMapEntity) {
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
        this.createProtegits();
    }

    private void checkMovement() {
        if (!this.mMovableMapEntity.getMovement()
                                   .isMovementInProgress()) {
            final int nextPosX = Tools.getRandom(this.mInitialPositionX - CUBIKON_MAX_DISTANCE_FROM_INITIAL_POSITION,
                                                 this.mInitialPositionX + CUBIKON_MAX_DISTANCE_FROM_INITIAL_POSITION);
            final int nextPosY = Tools.getRandom(this.mInitialPositionY - CUBIKON_MAX_DISTANCE_FROM_INITIAL_POSITION,
                                                 this.mInitialPositionY + CUBIKON_MAX_DISTANCE_FROM_INITIAL_POSITION);

            this.mMovableMapEntity.getMovement()
                                  .initiate(this.mMovableMapEntity.getCurrentPositionX(),
                                            this.mMovableMapEntity.getCurrentPositionY(), nextPosX, nextPosY,
                                            this.mMovableMapEntity.getBaseSpeed());
        }
    }
    
    private Thread mProtegitThread;
    
    private void createProtegits() {
    	
    	if (this.mProtegitThread == null || !this.mProtegitThread.isAlive()) {
    		this.mProtegitThread = new Thread() {
                public void run() {
                    try {
                        int i = 0;
                        while (true) {
                            if (i >= 5) {
                                while (mProtegits.size() < MAX_PROTEGITS_NUMBER) {
                                    final Alien alien = new Alien(ShipsConstants.PROTEGIT_ID, mMovableMapEntity.getCurrentSpaceMap(),
                                                                  mMovableMapEntity);
                                    mMovableMapEntity.getCurrentSpaceMap()
                                                          .addAlien(alien);
                                    addProtegit(alien);
                                    alien.receivedAttack(mLockedMapEntity);
                                }
                                break;
                            }
                            Thread.sleep(1000);
                            i++;
                        }
                    } catch (InterruptedException e) {
                        
                    }
                }
            };
            this.mProtegitThread.start();
        }
    	    	
    }

    private void addProtegit(final Alien pAlien) {
        mProtegits.put(pAlien.getAlienId(), pAlien);
    }
}
