package simulator.map_entities.collectables;

/**
 Created by bpdev on 08/02/2015.
 */
public class Resource
        extends Collectable {

    private final short mCollectableId;

    public Resource(final short pCollectableId, final int pTopLeftX,
                    final int pTopLeftY, final int pBottomRightX, final int pBottomRightY) {
        super(pTopLeftX, pTopLeftY, pBottomRightX, pBottomRightY);

        this.mCollectableId = pCollectableId;
    }

    public short getCollectableId() {
        return mCollectableId;
    }
}
