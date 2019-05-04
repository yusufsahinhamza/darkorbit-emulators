package utils;

/**
 Created by bpdev on 31/01/2015.
 */
public class MathUtils {

    public static double sqr(final double pValue) {
        return Math.pow(pValue, 2);
    }

    public static double hypotenuse(final double pX, final double pY) {
        return Math.sqrt(MathUtils.sqr(pX) + MathUtils.sqr(pY));
    }

}
