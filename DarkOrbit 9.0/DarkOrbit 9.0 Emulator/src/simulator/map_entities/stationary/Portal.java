package simulator.map_entities.stationary;

import net.game_server.GameSession;

import java.util.Timer;
import java.util.TimerTask;
import java.util.Vector;

import simulator.GameManager;
import simulator.map_entities.movable.Alien;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.ServerCommand;
import simulator.netty.serverCommands.ActivatePortalCommand;
import simulator.netty.serverCommands.CreatePortalCommand;
import simulator.netty.serverCommands.ShipRemoveCommand;
import simulator.system.SpaceMap;

/**
 Portal data holder
 */
public class Portal
        extends ActivatableStationaryMapEntity {

	public static int JUMP_DELAY_NOW    = 1000;
	public static int JUMP_DELAY        = 0; //normal = 3250
    public static int ACTIVATED_RANGE   = 500;
    public static int SECURE_ZONE_RANGE = 1500;

    private final short mTargetSpaceMapId;
    private final int   mTargetPositionX;
    private final int   mTargetPositionY;

    private final short mLevelRequirement;

    private final short mGraphicsId;
    private final short mFactionIconId;// XXX needed?

    private boolean mVisible;
    private boolean mWorking;// XXX needed?
    private short   mActivated;

    public Portal(final SpaceMap pSpaceMap, final int pPositionX, final int pPositionY,//
                  final short pTargetSpaceMapId, final int pTargetPositionX, final int pTargetPositionY,//
                  final short pLevelRequirement,//
                  final short pGraphicsId,//
                  final short pFactionIconId,//
                  final boolean pVisible,//
                  final boolean pWorking//
    ) {
        super(pSpaceMap);

        this.setCurrentPositionX(pPositionX);
        this.setCurrentPositionY(pPositionY);

        this.mTargetSpaceMapId = pTargetSpaceMapId;
        this.mTargetPositionX = pTargetPositionX;
        this.mTargetPositionY = pTargetPositionY;

        this.mLevelRequirement = pLevelRequirement;

        this.mGraphicsId = pGraphicsId;

        this.mFactionIconId = pFactionIconId;

        this.mVisible = pVisible;
        this.mWorking = pWorking;


    }

    public short getFactionIconId() {
        return this.mFactionIconId;
    }

    public short getGraphicsId() {
        return this.mGraphicsId;
    }

    public short getLevelRequirement() {
        return this.mLevelRequirement;
    }

    public int getTargetPositionX() {
        return this.mTargetPositionX;
    }

    public int getTargetPositionY() {
        return this.mTargetPositionY;
    }

    public short getTargetSpaceMapId() {
        return this.mTargetSpaceMapId;
    }

    public boolean isVisible() {
        return this.mVisible;
    }

    public boolean isWorking() {
        return this.mWorking;
    }

    public short isActivated() {
        return this.mActivated;
    }

    @Override
    public int getActivatedRange() {
        return ACTIVATED_RANGE;
    }

    @Override
    public void handleClick(final GameSession pGameSession) {
        final Player player = pGameSession.getPlayer();
        if (!player.isJumping()) {
            player.setJumping(true);
            ActivatePortalCommand apc = new ActivatePortalCommand(this.getTargetSpaceMapId(),//next map id
                                                                  this.getMapEntityId() //
            );
            pGameSession.getGameServerClientConnection()
                        .sendToSendCommand(apc);
            new Timer().schedule(new TimerTask() {

                @Override
                public void run() {      
                	
                    boolean petWillOpen = false;                    
                    if(player.getAccount().getPetManager().isActivated()) {
                    	player.getAccount().getPetManager().Deactivate(); 
                    	petWillOpen = true;
                    }
                    
                    for(Alien al: player.getCurrentSpaceMap().getAllAliens()){
                    	if(al != null) {
	                        for (final MovableMapEntity movableMapEntity : al.getInRangeMovableMapEntities()) {
	                            if (movableMapEntity instanceof Player) {
	                                Player pl = ((Player) movableMapEntity);
	                                if(pl.getAccount().getUserId() == player.getAccount().getUserId()){
	                                    al.setLockedTarget(null);
	                                }
	                            }
	                        }
                    	}
                    }
                    
                    final ShipRemoveCommand shipRemoveCommand = new ShipRemoveCommand(player.getAccount().getUserId());
                    for (MovableMapEntity inRangeEntity : player.getCurrentSpaceMap().getAllPlayers()) {
                    	if(inRangeEntity != null) {
                    		if(inRangeEntity instanceof Player) {
                    			if(inRangeEntity != player) {
                    				final Player inRangePlayers = (Player) inRangeEntity;
                    				inRangePlayers.sendCommandToBoundSessions(shipRemoveCommand);
                    			}
                    		}
                    	}
                    }
                    
                    player.getCurrentSpaceMap()
                	      .removePlayer(player.getMapEntityId());
                    player.getMovement()
                	      .setMovementInProgress(false);
                    player.setPositionXY(getTargetPositionX(), getTargetPositionY());
                    player.setCurrentSpaceMap(getTargetSpaceMapId());
                    player.setCurrentInRangePortalId(INVALID_ID);
                    player.setJumping(false);
                    try {
						Thread.sleep(JUMP_DELAY_NOW);
					} catch (InterruptedException e) {
					}
                    GameManager.tryJump(player.getMapEntityId(), getTargetSpaceMapId()); 
                    
                    if(petWillOpen) {
                    	player.getAccount().getPetManager().Activate(); petWillOpen = false;
                    }
                }
            }, JUMP_DELAY);
        }
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
    public ServerCommand getAssetCreateCommand() {
        return new CreatePortalCommand(this.getMapEntityId(), this.getFactionIconId(), this.getGraphicsId(),
                                       this.getCurrentPositionX(), this.getCurrentPositionY(), this.isWorking(),
                                       this.isVisible(), new Vector<Integer>());
    }
}
