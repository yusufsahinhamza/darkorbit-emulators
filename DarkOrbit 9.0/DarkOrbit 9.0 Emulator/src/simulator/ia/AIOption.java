package simulator.ia;

/**
 Created by Pedro on 27-03-2015.
 */
public enum AIOption {
    // stay still and ignore everything
    DO_NOTHING,
    // move randomly earching for enemies
    SEARCH_FOR_ENEMIES,
    //move to enemy
    FLY_TO_ENEMY,
    //wait for player movement
    WAIT_PLAYER_MOVE,
    // attack locked enemy
    ATTACK_ENEMY,
    // flee from locked enemy
    FLEE_FROM_ENEMY,
    EMP,
    RANDOM_POSITION_MOVE
}
