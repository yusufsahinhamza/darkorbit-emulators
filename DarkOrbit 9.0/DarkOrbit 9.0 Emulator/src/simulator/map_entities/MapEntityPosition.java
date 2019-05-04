package simulator.map_entities;

import simulator.utils.DefaultAssignings;

/**
 Created by Pedro on 08-04-2015.
 */
public class MapEntityPosition
        implements DefaultAssignings {

    // position on map
    protected int mCurrentPositionX = DEFAULT_INC_VALUE;
    protected int mCurrentPositionY = DEFAULT_INC_VALUE;

    public int getCurrentPositionX() {
        return this.mCurrentPositionX;
    }

    protected void setCurrentPositionX(final int pPositionX) {
        this.mCurrentPositionX = pPositionX;
    }

    public int getCurrentPositionY() {
        return this.mCurrentPositionY;
    }

    protected void setCurrentPositionY(final int pPositionY) {
        this.mCurrentPositionY = pPositionY;
    }

    public void setPositionXY(final int pPositionX, final int pPositionY) {
        this.setCurrentPositionX(pPositionX);
        this.setCurrentPositionY(pPositionY);
    }

}
