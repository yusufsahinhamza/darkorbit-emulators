package utils;

import java.util.Random;

/**
 Description: Here will be useful methods

 @author Manulaiko
 @date 19/02/2014
 @file Tools.java
 @package utils
 @project SpaceBattles */

public class Tools {

    public static Random sRandom = new Random();

    public static int getRandom(int min, int max) {
        int n = sRandom.nextInt(max - min) + min;
        return n;
    }

    public static int getRandomDamage(int damage) {
        if (damage <= 0) {
            return 0;
        }
        int max = damage + 1000;
        int min = damage - 1000;
        if (min < 0) {
            min = 0;
        }
        int n = sRandom.nextInt(max - min) + min;
        return n;
    }

    public static double getBoost(double total, double boost) {
        return ((total * boost) / 100) + total;
    }

}
