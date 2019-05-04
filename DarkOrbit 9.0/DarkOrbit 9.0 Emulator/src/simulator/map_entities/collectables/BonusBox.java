package simulator.map_entities.collectables;

/**
 Created by LEJYONER on 15/09/2017.
 */

public class BonusBox
        extends Collectable {

    private final short mCollectableId;

    public BonusBox(final short pCollectableId, final int pTopLeftX,
                    final int pTopLeftY, final int pBottomRightX, final int pBottomRightY) {
        super(pTopLeftX, pTopLeftY, pBottomRightX, pBottomRightY);

        this.mCollectableId = pCollectableId;
    }

    public short getCollectableId() {
        return mCollectableId;
    }
}
