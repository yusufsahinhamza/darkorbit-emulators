package simulator.map_entities.collectables;


import java.util.Random;

import simulator.map_entities.MapEntityPosition;
import utils.Tools;

public abstract class Collectable
        extends MapEntityPosition {

    private final String mHash;
    private final int    mTopLeftX;
    private final int    mTopLeftY;
    private final int    mBottomRightX;
    private final int    mBottomRightY;

    protected Collectable(final int pTopLeftX, final int pTopLeftY, final int pBottomRightX, final int pBottomRightY) {
        this.mHash = generateHash();

        this.mTopLeftX = pTopLeftX;
        this.mTopLeftY = pTopLeftY;
        this.mBottomRightX = pBottomRightX;
        this.mBottomRightY = pBottomRightY;

        this.setNewPosition();
    }

    public void setNewPosition() {
        this.setPositionXY(Tools.getRandom(mTopLeftX, mBottomRightX), Tools.getRandom(mTopLeftY, mBottomRightY));
    }

    private String generateHash() {
        String alphabet = new String("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"); //9
        int n = alphabet.length(); //10

        String result = new String();
        Random r = new Random(); //11

        for (int i = 0; i < 15; i++) {
            result = result + alphabet.charAt(r.nextInt(n)); //13
        }

        return result;
    }

    public String getHash() {
        return mHash;
    }
}
