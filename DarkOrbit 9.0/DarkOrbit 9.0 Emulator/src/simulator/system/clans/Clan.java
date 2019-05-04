package simulator.system.clans;

import java.util.ArrayList;
import java.util.List;

/**
 Clan representation
 */
public class Clan {

    private int   mClanId;
    private int   mRankPoints;
    private short mFactionId;

    private String mClanName;
    private String mClanTag;

    private final List<Diplomacy>     mDiplomacies;
    private final List<simulator.map_entities.stationary.stations.BattleStation> mBattleStations;

    public Clan(final int pClanId, final int pRankPoints, final String pClanName, final String pClanTag, final short pFactionId,
                final String pClanMembersJson, final List<Diplomacy> pDiplomacies,
                final ArrayList<simulator.map_entities.stationary.stations.BattleStation> arrayList) {
        this.mClanId = pClanId;
        this.mRankPoints = pRankPoints;
        this.mClanName = pClanName;
        this.mClanTag = pClanTag;
        this.mFactionId = pFactionId;
        this.mDiplomacies = pDiplomacies;
        this.mBattleStations = arrayList;
    }

    public Clan(final int pClanId, final String pClanName, final String pClanTag, final short pFactionId,
                final int pFounderId) {
        this.mClanId = pClanId;
        this.mClanName = pClanName;
        this.mClanTag = pClanTag;
        this.mFactionId = pFactionId;
        this.mDiplomacies = new ArrayList<>();
        this.mBattleStations = new ArrayList<>();
    }
    
    public List<Diplomacy> getDiplomacies(){
        return this.mDiplomacies;
    }

    public int getRankPoints() {
        return this.mRankPoints;
    }
    
    public int getClanId() {
        return this.mClanId;
    }

    public void setClanId(final int pClanId) {
        this.mClanId = pClanId;
    }

    public String getClanName() {
        return this.mClanName;
    }

    public void setClanName(final String pClanName) {
        this.mClanName = pClanName;
    }

    public String getClanTag() {
        return this.mClanTag;
    }

    public void setClanTag(final String pClanTag) {
        this.mClanTag = pClanTag;
    }

    public short getFactionId() {
        return this.mFactionId;
    }

    public void setFactionId(final short pClanTag) {
        this.mFactionId = pClanTag;
    }

}
