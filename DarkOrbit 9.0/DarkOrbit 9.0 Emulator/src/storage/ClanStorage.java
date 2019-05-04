package storage;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import simulator.system.clans.Clan;

/**
 Stores all clan data loaded from DB. Provides static access to all stored data
 */
public class ClanStorage {

    private static Map<Integer, Clan> sClans = new HashMap<>();

    public static void addClan(final Clan pClan) {
        sClans.put(pClan.getClanId(), pClan);
    }

    public static Clan getClan(final int pClanId) {
        return sClans.get(pClanId);
    }

    public static void removeClan(final int pClanId) {
        sClans.remove(pClanId);
    }

    public static Collection<Clan> getClanCollection() {
        return sClans.values();
    }

    public static int getClanCount() {
        return sClans.size();
    }

}
