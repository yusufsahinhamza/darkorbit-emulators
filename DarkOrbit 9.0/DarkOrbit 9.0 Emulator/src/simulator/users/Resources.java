package simulator.users;

/**
 Created by Pedro on 02-04-2015.
 */
public class Resources {

    private long mPrometium = 0L;
    private long mEndurium  = 0L;
    private long mTerbium   = 0L;
    private long mXenomit   = 0L;
    private long mPalladium = 0L;
    private long mPrometid  = 0L;
    private long mDuranium  = 0L;
    private long mPromerium = 0L;
    private long mSeprom    = 0L;
    private short mLasersResourceType;
    private long mLasersResourceAmount = 0L;
    private short mRocketsResourceType;
    private long mRocketsResourceAmount = 0L;
    private short mGeneratorsResourceType;
    private long    mGeneratorsResourceTimeFinish = 0L;
    private boolean mIsGeneratorsBoostActivated   = false;
    private short mShieldsResourceType;
    private long    mShieldsResourceTimeFinish = 0L;
    private boolean mIsShieldBoostActivated    = false;

    public Resources() {
    }

    public Resources(final long pPrometium, final long pEndurium, final long pTerbium, final long pXenomit,
                     final long pPalladium, final long pPrometid, final long pDuranium, final long pPromerium,
                     final long pSeprom, final short pLasersResourceType, final long pLasersResourceAmount,
                     final short pRocketsResourceType, final long pRocketsResourceAmount,
                     final short pGeneratorsResourceType, final long pGeneratorsResourceTimeFinish,
                     final short pShieldsResourceType, final long pShieldsResourceTimeFinish) {
        mPrometium = pPrometium;
        mEndurium = pEndurium;
        mTerbium = pTerbium;
        mXenomit = pXenomit;
        mPalladium = pPalladium;
        mPrometid = pPrometid;
        mDuranium = pDuranium;
        mPromerium = pPromerium;
        mSeprom = pSeprom;
        mLasersResourceType = pLasersResourceType;
        mLasersResourceAmount = pLasersResourceAmount;
        mRocketsResourceType = pRocketsResourceType;
        mRocketsResourceAmount = pRocketsResourceAmount;
        mGeneratorsResourceType = pGeneratorsResourceType;
        mGeneratorsResourceTimeFinish = pGeneratorsResourceTimeFinish;
        mShieldsResourceType = pShieldsResourceType;
        mShieldsResourceTimeFinish = pShieldsResourceTimeFinish;
    }

    public long getPrometium() {
        return mPrometium;
    }

    public void setPrometium(final long pPrometium) {
        mPrometium = pPrometium;
    }

    public void changePrometium(final long pDifferencePrometium) {
        mPrometium += pDifferencePrometium;
    }

    public long getEndurium() {
        return mEndurium;
    }

    public void setEndurium(final long pEndurium) {
        mEndurium = pEndurium;
    }

    public void changeEndurium(final long pDifferenceEndurium) {
        mEndurium += pDifferenceEndurium;
    }

    public long getTerbium() {
        return mTerbium;
    }

    public void setTerbium(final long pTerbium) {
        mTerbium = pTerbium;
    }

    public void changeTerbium(final long pDifferenceTerbium) {
        mTerbium += pDifferenceTerbium;
    }

    public long getXenomit() {
        return mXenomit;
    }

    public void setXenomit(final long pXenomit) {
        mXenomit = pXenomit;
    }

    public void changeXenomit(final long pDifferenceXenomit) {
        mXenomit += pDifferenceXenomit;
    }

    public long getPalladium() {
        return mPalladium;
    }

    public void setPalladium(final long pPalladium) {
        mPalladium = pPalladium;
    }

    public void changePalladium(final long pDifferencePalladium) {
        mPalladium += pDifferencePalladium;
    }

    public long getPrometid() {
        return mPrometid;
    }

    public void setPrometid(final long pPrometid) {
        mPrometid = pPrometid;
    }

    public void changePrometid(final long pDifferencePrometid) {
        mPrometid += pDifferencePrometid;
    }

    public long getDuranium() {
        return mDuranium;
    }

    public void setDuranium(final long pDuranium) {
        mDuranium = pDuranium;
    }

    public void changeDuranium(final long pDifferenceDuranium) {
        mDuranium += pDifferenceDuranium;
    }

    public long getPromerium() {
        return mPromerium;
    }

    public void setPromerium(final long pPromerium) {
        mPromerium = pPromerium;
    }

    public void changePromerium(final long pDifferencePromerium) {
        mPromerium += pDifferencePromerium;
    }

    public long getSeprom() {
        return mSeprom;
    }

    public void setSeprom(final long pSeprom) {
        mSeprom = pSeprom;
    }

    public void changeSeprom(final long pDifferenceSeprom) {
        mSeprom += pDifferenceSeprom;
    }

    public short getLasersResourceType() {
        return mLasersResourceType;
    }

    public void setLasersResourceType(final short pLasersResourceType) {
        mLasersResourceType = pLasersResourceType;
    }

    public long getLasersResourceAmount() {
        return mLasersResourceAmount;
    }

    public void setLasersResourceAmount(final long pLasersResourceAmount) {
        mLasersResourceAmount = pLasersResourceAmount;
    }

    public void changeLasersResourceAmount(final long pDifferenceLasersResourceAmount) {
        mLasersResourceAmount += pDifferenceLasersResourceAmount;
    }

    public short getRocketsResourceType() {
        return mRocketsResourceType;
    }

    public void setRocketsResourceType(final short pRocketsResourceType) {
        mRocketsResourceType = pRocketsResourceType;
    }

    public long getRocketsResourceAmount() {
        return mRocketsResourceAmount;
    }

    public void setRocketsResourceAmount(final long pRocketsResourceAmount) {
        mRocketsResourceAmount = pRocketsResourceAmount;
    }

    public void changeRocketsResourceAmount(final long pDifferenceRocketsResourceAmount) {
        mRocketsResourceAmount += pDifferenceRocketsResourceAmount;
    }

    public short getGeneratorsResourceType() {
        return mGeneratorsResourceType;
    }

    public void setGeneratorsResourceType(final short pGeneratorsResourceType) {
        mGeneratorsResourceType = pGeneratorsResourceType;
    }

    public long getGeneratorsResourceTimeFinish() {
        return mGeneratorsResourceTimeFinish;
    }

    public void setGeneratorsResourceTimeFinish(final long pGeneratorsResourceTimeFinish) {
        mGeneratorsResourceTimeFinish = pGeneratorsResourceTimeFinish;
    }

    public void changeGeneratorsResourceTimeFinish(final long pDifferenceGeneratorsResourceTimeFinish) {
        mGeneratorsResourceTimeFinish += pDifferenceGeneratorsResourceTimeFinish;
    }

    public short getShieldsResourceType() {
        return mShieldsResourceType;
    }

    public void setShieldsResourceType(final short pShieldsResourceType) {
        mShieldsResourceType = pShieldsResourceType;
    }

    public long getShieldsResourceTimeFinish() {
        return mShieldsResourceTimeFinish;
    }

    public void setShieldsResourceTimeFinish(final long pShieldsResourceTimeFinish) {
        mShieldsResourceTimeFinish = pShieldsResourceTimeFinish;
    }

    public void changeShieldsResourceTimeFinish(final long pDifferenceShieldsResourceTimeFinish) {
        mShieldsResourceTimeFinish += pDifferenceShieldsResourceTimeFinish;
    }

    public boolean isGeneratorsBoostActivated() {
        return mIsGeneratorsBoostActivated;
    }

    public void setGeneratorsBoostActivated(final boolean pIsGeneratorsBoostActivated) {
        mIsGeneratorsBoostActivated = pIsGeneratorsBoostActivated;
    }

    public boolean isShieldBoostActivated() {
        return mIsShieldBoostActivated;
    }

    public void setShieldBoostActivated(final boolean pIsShieldBoostActivated) {
        mIsShieldBoostActivated = pIsShieldBoostActivated;
    }
}
