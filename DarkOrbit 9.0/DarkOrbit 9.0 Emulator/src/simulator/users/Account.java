package simulator.users;

import java.util.HashMap;
import java.util.Map;

import org.json.JSONObject;

import simulator.logic.PetMovement;
import simulator.map_entities.movable.Player;
import simulator.system.clans.Clan;
import simulator.system.ships.ShipFactory;
import simulator.utils.DefaultAssignings;
import storage.ClanStorage;
import storage.SpaceMapStorage;

/**
 This class is used to store account data and only it. Check fields to see what data is stored

 Created by bpdev.
 Edited & Fixed by LEJYONER. 
 */

public class Account
        implements DefaultAssignings {

    private static final long   INITIAL_CREDITS_VALUE = DEFAULT_INC_VALUE;
    private static final long   INITIAL_URIDIUM_VALUE = DEFAULT_INC_VALUE;
    private static final short  INITIAL_LEVEL_VALUE = DEFAULT_INC_VALUE;
    private static final short  INITIAL_RANK_ID       = 1;
    private static final String INITIAL_AVATAR_URL    =
    "http://localhost/do_img/pilotSheet/profilePage/avatar.jpg";
    private static final short  INITIAL_FACTION_ID    = DEFAULT_INC_VALUE;
    private static final long   INITIAL_EXPERIENCE_VALUE = DEFAULT_INC_VALUE;
    private static final long   INITIAL_HONOR_VALUE = DEFAULT_INC_VALUE;
    private static final int    INITIAL_GREENBOOTYKEYS_VALUE = DEFAULT_INC_VALUE;
    private static float        INITIAL_JACKPOT_VALUE = DEFAULT_INC_VALUE;
    private static int          INITIAL_JUMPCREDITS_VALUE = DEFAULT_INC_VALUE;
    private final int mGlobalID; // Global UID of the account(the one and unique)
    private final int mUserId; // UID of the account on this specific server
    private final int mShipID; // Hesabın gemi id'si
    private final int mServerID; // Hesabın server id'si
    private String mSessionId;
    private short mFactionId = INITIAL_FACTION_ID; // User faction ID
    private int mRankPoints; // rank points
    private long mExperience = INITIAL_EXPERIENCE_VALUE; // exp
    private long mHonor = INITIAL_HONOR_VALUE; // hon
    private int mClanHonor; // Klan şeref puanı
    private short mLevel = INITIAL_LEVEL_VALUE; // lvl
    private long mCredits = INITIAL_CREDITS_VALUE; // Main currency
    private long mUridium = INITIAL_URIDIUM_VALUE; // Rare/donate currency
    private long mKilledGoliath;
    private long mKilledVengeance;
    private long mOwnKilled;
    private float mJackpot = INITIAL_JACKPOT_VALUE; // jackpot
    private int mJumpTickets = INITIAL_JUMPCREDITS_VALUE; // jump credits
    private int mRepairVouchers; // repair credits
    private int mBlueBootyKeys; // blue booty keys
    private int mRedBootyKeys; // red booty keys
    private int mGreenBootyKeys = INITIAL_GREENBOOTYKEYS_VALUE; // green booty keys
    private int mPositionX; // harita üzerindeki x değeri
    private int mPositionY; // harita üzerindeki y değeri
    private int mRepairUp; //tamir argesi
    private int mRocketDmgUp; //roket argesi
    private int mPremiumYesOrNo; //premium evet veya hayır
    private int mIOil;
    private int mECO_10;
    private int mSAR_02;
    private boolean mOnline;
    private short mMapID;
    public short mRankId = INITIAL_RANK_ID; // rank ID (based on top pilots)
    public boolean mPremiumAccount = false; // is premium or not
    public boolean mCloaked; // kamuflaj var veya yok
    public boolean mIsAdmin; // admin veya değil
    public boolean mIsCm; // cm veya değil
    public boolean mHavePet; // peti var veya yok  
    private String mTitle; // başlık
    private String mUsername; // username (login)
    private String mShipUsername;
    private String mPetName;
    private String mPasswordMD5; // pass
    private String mEmail; // email
    private String mAvatarUrl = INITIAL_AVATAR_URL; // web URL to user avatar
    public Clan mClan = null; // clan
    
    public Map<String, Boolean> puzzleLetters = new HashMap<>();
    // cephane
    private final AmmunitionManager     mAmmunitionManager     = new AmmunitionManager(this);
    // teknolojiler
    private final TechsManager          mTechsManager          = new TechsManager(this);
    // kaynaklar
    private final ResourcesManager      mResourcesManager      = new ResourcesManager(this);
    // ekipman
    private final EquipmentManager      mEquipmentManager      = new EquipmentManager(this);
    // diroitler
    private       DroneManager          mDroneManager          = new DroneManager(this);
    // yetenekler
    private       SkillsManager         mSkillsManager         = new SkillsManager(this);
    // cpular
    private       CpusManager           mCpusManager           = new CpusManager(this);
    // pet
    private final PetManager            mPetManager            = new PetManager(this);
    // client settings
    private final ClientSettingsManager mClientSettingsManager = new ClientSettingsManager(this);
    //satın alım
    private final BuyManager			mBuyManager 		   = new BuyManager(this);

    // ==============================================================

    private final Player mPlayer;
    
    private final PetMovement mPetMovement;
    /**
     Creates Account object along with given values setups
    */
    public Account(
    		       final int pUserID,
    		       final int pGlobalID,
    		       final int pServerID,
    		       final short pFactionID,
    		       final int pShipID,
    		       final int pClanID,
    		       final short pRankID,
    		       final boolean pIsAdmin,
    		       final boolean pIsCm,
    		       final boolean pHavePet,
    		       final String pSessionID,
    		       final String pUsername,
    		       final String pShipUsername,
    		       final String pTitle,
    		       final short pMapID,
    		       final int pPositionX,
    		       final int pPositionY,
    		       final long pExperience,
    		       final long pHonor,
    		       final long pCredits,
    		       final long pUridium,
    		       final long pKilledGoliath,
    		       final long pKilledVengeance,
    		       final long pOwnKilled,
    		       final double pJackpot,
    		       final short pLevel,
    		       final boolean pCloaked,
    		       final boolean pPremium,
    		       final int pRepairUp,
    		       final int pRocketDmgUp,
    		       final String pAvatar,
    		       final String pClientSettingsJSON,
    		       final String pResourcesJSON,
    		       final String pLastSelectedLaser,
    		       final String pLastSelectedRocket,
    		       final boolean pOnline,
    		       final String pPetName,
    		       final int pRankPoints,
    		       final int pIOil,
    		       final int pECO_10,
    		       final int pSAR_02,
    		       final String pPuzzleLettersJSON
    ) {    	
        this.setLettersFromJSON(pPuzzleLettersJSON);
    	this.mIOil = pIOil;
    	this.mECO_10 = pECO_10;
    	this.mSAR_02 = pSAR_02;
    	this.mOnline = pOnline;
    	this.mMapID = pMapID;
    	this.mIsAdmin = pIsAdmin;
    	this.mIsCm = pIsCm;
    	this.mHavePet = pHavePet;
        this.mGlobalID = pGlobalID;
        this.mUserId = pUserID;
        this.mServerID = pServerID;
        this.mShipID = pShipID;
        this.mRankPoints = pRankPoints;
        this.setSessionId(pSessionID);
        this.setRepairUp(pRepairUp);
        this.setRocketDmgUp(pRocketDmgUp);
        this.setFactionId(pFactionID);
        this.setLastSelectedLaser(pLastSelectedLaser);
        this.setLastSelectedRocket(pLastSelectedRocket);
        if(pFactionID == 1) {
        	mPositionX = 2000; //4-1 = 1600
        	mPositionY = 6000; //4-1 = 1600
        	mMapID = 20;  //4-1 = 13
        } else if(pFactionID == 2) {
        	mPositionX = 10000; //4-2 = 19500
        	mPositionY = 2000; //4-2 = 1500
        	mMapID = 24; //4-2 = 14
        } else if(pFactionID == 3) {
        	mPositionX = 18500; //4-2 = 19500
        	mPositionY = 6000; //4-2 = 11600
        	mMapID = 28; //4-2 = 15
        } 
        /** X-4 DOĞUM
        if(pFactionID == 1) {
        	mPositionX = 1600; //4-1 = 1600
        	mPositionY = 1600; //4-1 = 1600
        	mMapID = 13;  //4-1 = 13
        } else if(pFactionID == 2) {
        	mPositionX = 19500; //4-2 = 19500
        	mPositionY = 1500; //4-2 = 1500
        	mMapID = 14; //4-2 = 14
        } else if(pFactionID == 3) {
        	mPositionX = 19500; //4-2 = 19500
        	mPositionY = 11600; //4-2 = 11600
        	mMapID = 15; //4-2 = 15
        }    
        */   
        this.setRankId(pRankID);
        this.setExperience(pExperience);
        this.setHonor(pHonor);
        this.setCloaked(pCloaked);
        this.setCredits(pCredits);
        this.setUridium(pUridium);
        this.setKilledGoliath(pKilledGoliath);
        this.setKilledVengeance(pKilledVengeance);
        this.setOwnKilled(pOwnKilled);        
        this.setPremiumAccount(pPremium);
        this.setTitle(pTitle);
/**
Buradaki kodlar eğer oyuncunun rütbesi 21 yani admin ise oyundaki isminin herhangi bir yerine yazı yazdırıyor.
Not: Bu kodlar aktif olacak ise; bu kodların altındaki 1 kod satırı silinmelidir!
*/        
   //     if (mRankId == 21)
   //     {
   //     	this.setUsername("{ADMIN} " + pShipUsername);
   //     }
   //     else
   //     {
   //     this.setUsername(pShipUsername);
   //     }
        
/**
Buradaki kodlar server id'sine göre ismin sonuna TR veya EN ekliyor.
Not: Bu kodlar aktif olacak ise; bu kodların altındaki 1 kod satırı silinmelidir!
*/        
 //       if (mServerID == 1)
 //       {
 //       	this.setUsername(pShipUsername + " [OW1]");
 //       }
 //       else if(mServerID == 2)
 //       {
 //       	this.setUsername(pShipUsername + " [OW2]");
 //       }
 //      else
 //       {
 //       	this.setUsername(pShipUsername);
 //       }
                this.setUsername(pUsername);
                this.setShipUsername(pShipUsername);
                this.setPetName(pPetName);
                this.setAvatarUrl(pAvatar);
                this.setClan(pClanID);
                this.setAdmin(isAdmin());
 
        this.mPlayer = new Player(this, SpaceMapStorage.getSpaceMap(mMapID),ShipFactory.getPlayerShip(this.getShipID()));
        this.mClientSettingsManager.setFromJSON(pClientSettingsJSON);
        this.mResourcesManager.setFromJSON(pResourcesJSON);
        this.mDroneManager = new DroneManager(this);
        this.mPetMovement = new PetMovement(this);

    }

    public boolean getPuzzleLetter(final int pLetterIndex) {  
    	return this.puzzleLetters.get("puzzleLetter"+ pLetterIndex +""); 
    }   
    
    public void setLettersFromJSON(final String pLettersJSON) {
        final JSONObject letters = new JSONObject(pLettersJSON);  
    	for (int i = 10; i > 0; i--) {	        	
        	puzzleLetters.put("puzzleLetter"+ i +"", letters.getBoolean("puzzleLetter"+ i +""));	
        }        
    }
    
    public String lettersToJSON() {
        final JSONObject resourcesJSON = new JSONObject();        
        for(Map.Entry<String, Boolean> entry : puzzleLetters.entrySet()){    
            String key = entry.getKey();  
            boolean value = entry.getValue();  
            resourcesJSON.put(key, value);   
        }
        return resourcesJSON.toString();
    }

   public boolean getOnline() {
	   return this.mOnline;
   }
   
   public void setOnline(final boolean pOnline) {
	   this.mOnline = pOnline;
   }
   
   public void setLastSelectedLaser(final String pLastSelectedLaser) {
	   this.getClientSettingsManager().setSelectedLaser(pLastSelectedLaser);
   }
   
   public void setLastSelectedRocket(final String pLastSelectedRocket) {
	   this.getClientSettingsManager().setSelectedRocket(pLastSelectedRocket);
   }
   
   public int getShipID() {
    	return this.mShipID;
   }
   
   public int getRocketDmgUp() {
        return this.mRocketDmgUp;
   }

   public void setRocketDmgUp(final int pRocketDmgUp) {
        this.mRocketDmgUp = pRocketDmgUp;
   }
    
   public int getRepairUp() {
        return this.mRepairUp;
   }

   public void setRepairUp(final int pRepairUp) {
        this.mRepairUp = pRepairUp;
   }
   
   /**
     @return account global ID
     */
    public int getGlobalId() {
        return this.mGlobalID;
    }

    public int getServerId() {
        return this.mServerID;
    }
    
    /**
     @return account ID on this server
     */
    public int getUserId() {
        return this.mUserId;
    }

    /**
     @return current valid SID for this Account
     */
    public String getSessionId() {
        return this.mSessionId;
    }
     
    /**
     @param pSessionID
     new valid SID for this Account
     */
    public void setSessionId(final String pSessionID) {
        this.mSessionId = pSessionID;
    }

    // ==============================================================

    /**
     @return user's faction ID
     */
    public short getFactionId() {
        return this.mFactionId;
    }

    /**
     @param pFactionId
     faction ID to set
     */
    public void setFactionId(final short pFactionId) {
        this.mFactionId = pFactionId;
    }

    // ==============================================================

    /**
     @return user's rank points count
     */
    public int getRankPoints() {
        return this.mRankPoints;
    }
	
    /**
     @param pRankPoints
     rankId to set
     */
    public void setRankPoints(final int pRankPoints) {
        this.mRankPoints = pRankPoints;
    }

    /**
     @return EXP count
     */
    public long getExperience() {
        return this.mExperience;
    }

    /**
     Note: by setting experience you also affect level

     @param pExperience
     EXP to set
     */
    public void setExperience(final long pExperience) {

        this.mExperience = pExperience;

        this.setLevel(pExperience);

    }

    public void changeExperience(final long pDifferenceExperience) {
        this.mExperience += pDifferenceExperience;
        this.setLevel(this.mExperience);
    }

    /**
     @return HON count
     */
    public long getHonor() {
        return this.mHonor;
    }

    /**
     @param pHonor
     HON to set
     */
    public void setHonor(final long pHonor) {
        this.mHonor = pHonor;
    }

    public void changeHonor(final long pDifferenceHonor) {
        this.mHonor += pDifferenceHonor;
    }

    public int getClanHonor() {
        return this.mClanHonor;
    }
    
    public void setClanHonor(final int pClanHonor) {
        this.mClanHonor = pClanHonor;
    }
    
    public void changeClanHonor(final int pDifferenceClanHonor) {
        this.mClanHonor += pDifferenceClanHonor;
    }
    
    /**
     @return account level
     */
    public short getLevel() {
        return this.mLevel;
    }

    /**
     Note: only allows setting level based on experience, unavailable outside of class

     @param pExperience
     EXP based on which level is set
     */
    private void setLevel(final long pExperience) {
        this.mLevel = this.getUserLevelByExp(pExperience);
    }

    public short getUserLevelByExp(final long pUserExperience) {

        short lvl = 1;
        long expNext = 10000;

        while (pUserExperience >= expNext) {
            expNext *= 2;
            lvl++;
        }

        return lvl;
    }
    
    // ==============================================================

    /**
     @return credits count
     */
    public long getCredits() {
        return this.mCredits;
    }

    /**
     @param pCredits
     credits to set
     */
    public void setCredits(final long pCredits) {
        this.mCredits = pCredits;
    }

    public void changeCredits(final long pDifferenceCredits) {
        this.mCredits += pDifferenceCredits;
    }

    /**
     @return URI count
     */
    public long getUridium() {
        return this.mUridium;
    }

    /**
     @param pUridium
     URI to set
     */
    public void setUridium(final long pUridium) {
        this.mUridium = pUridium;
    }

    public void changeUridium(final long pDifferenceUridium) {
        this.mUridium += pDifferenceUridium;
    }

    // ==============================================================

    /**
     @return JP count
     */
    public float getJackpot() {
        return this.mJackpot;
    }

    /**
     @param pJackpot
     JP to set
     */
    public void setJackpot(final float pJackpot) {
        this.mJackpot = pJackpot;
    }

    /**
     @return jump credits quantity
     */
    public int getJumpTickets() {
        return this.mJumpTickets;
    }

    /**
     @param pJumpTickets
     jump credits to set
     */
    public void setJumpTickets(final int pJumpTickets) {
        this.mJumpTickets = pJumpTickets;
    }

    /**
     @return repair credits quantity
     */
    public int getRepairVouchers() {
        return this.mRepairVouchers;
    }

    /**
     @param pRepairVouchers
     repair credits to set
     */
    public void setRepairVouchers(final int pRepairVouchers) {
        this.mRepairVouchers = pRepairVouchers;
    }

    /**
     @return blue keys quantity
     */
    public int getBlueBootyKeys() {
        return this.mBlueBootyKeys;
    }

    public int getPositionX() {
        return this.mPositionX;
    }
    
    public int getPositionY() {
        return this.mPositionY;
    }
    
    public void setPositionX(final int pPositionX) {
        this.mPositionX = pPositionX;
    }
    
    public void setPositionY(final int pPositionY) {
        this.mPositionY = pPositionY;
    }
    
    public boolean isCloaked() {
        return this.mCloaked;
    }
    
    public void setCloaked(final boolean pCloaked) {
        this.mCloaked = pCloaked;
    }
    
    /**
     @param pBlueBootyKeys
     blue keys to set
     */
    public void setBlueBootyKeys(final int pBlueBootyKeys) {
        this.mBlueBootyKeys = pBlueBootyKeys;
    }

    /**
     @return red keys quantity
     */
    public int getRedBootyKeys() {
        return this.mRedBootyKeys;
    }

    /**
     @param pRedBootyKeys
     red keys to set
     */
    public void setRedBootyKeys(final int pRedBootyKeys) {
        this.mRedBootyKeys = pRedBootyKeys;
    }

    /**
     @return green keys quantity
     */
    public int getGreenBootyKeys() {
        return this.mGreenBootyKeys;
    }

    /**
     @param pGreenBootyKeys
     green keys to set
     */
    public void setGreenBootyKeys(final int pGreenBootyKeys) {
        this.mGreenBootyKeys = pGreenBootyKeys;
    }

    /**
    @return Öldürülen Goliath sayısı
    */
   public long getKilledGoliath() {
       return this.mKilledGoliath;
   }
   
   /**
    @param pKilledGoliath
    Ayarlamak için öldürülen Goliath sayısı
    */
   public void setKilledGoliath(final long pKilledGoliath) {
       this.mKilledGoliath = pKilledGoliath;
   }
   
   /**
   @return Öldürülen Goliath sayısı
   */
   public void changeKilledGoliath(final long pDifferenceKilledGoliath) {
       this.mKilledGoliath += pDifferenceKilledGoliath;
   }
   
   /**
   @return Öldürülen Vengeance sayısı
   */
  public long getKilledVengeance() {
      return this.mKilledVengeance;
  }

  /**
   @param pKilledVengeance
   Ayarlamak için öldürülen Vengeance sayısı
   */
  public void setKilledVengeance(final long pKilledVengeance) {
      this.mKilledVengeance = pKilledVengeance;
  }
  
  /**
  @return Öldürülen Goliath sayısı
  */
  public void changeKilledVengeance(final long pDifferenceKilledVengeance) {
      this.mKilledVengeance += pDifferenceKilledVengeance;
  }
  
  public long getOwnKilled() {
      return this.mOwnKilled;
  }

  /**
   @param pKilledVengeance
   Ayarlamak için öldürülen Vengeance sayısı
   */
  public void setOwnKilled(final long pOwnKilled) {
      this.mOwnKilled = pOwnKilled;
  }
  
  /**
  @return Öldürülen Goliath sayısı
  */
  public void changeOwnKilled(final long pDifferenceOwnKilled) {
      this.mOwnKilled += pDifferenceOwnKilled;
  }
    
    public int getPremiumYesOrNO() {
    	return this.mPremiumYesOrNo;
    }
    
    public void setPremiumYesOrNo(final int pPremiumYesOrNo) {
    	this.mPremiumYesOrNo = pPremiumYesOrNo;
    }
    
    public short getRankId() {
        return this.mRankId;
    }
    
    public void convertPremium() {
    	if(this.isPremiumAccount() == true) {
    		this.setPremiumYesOrNo(1);
    	} else {
    		this.setPremiumYesOrNo(0);
    	}
    }
    
  /**
    Premium: Evet / Hayır
  */
 public String getPremiumYesOrNo() {
	 this.convertPremium();
     int premium = this.getPremiumYesOrNO();
     switch (premium) {
         case 0:  return "Hayır";
         case 1:  return "Evet";
         default: return "";
     }
 }
 
    /**
      Rütbe ismini çekme
    */
   public String getRankName() {
       int rankID = this.getRankId();
       switch (rankID) {
           case 1:  return "Acemi Uzay Pilotu";
           case 2:  return "Uzay Pilotu";
           case 3:  return "Acemi Pilot";
           case 4:  return "Acemi Çavuş";
           case 5:  return "Çavuş";
           case 6:  return "Uzman Çavuş";
           case 7:  return "Asteğmen";
           case 8:  return "Teğmen";
           case 9:  return "Üsteğmen";
           case 10: return "Acemi Yüzbaşı";
           case 11: return "Yüzbaşı";
           case 12: return "Uzman Yüzbaşı";
           case 13: return "Acemi Binbaşı";
           case 14: return "Binbaşı";
           case 15: return "Kurmay Binbaşı";
           case 16: return "Acemi Albay";
           case 17: return "Albay";
           case 18: return "Kurmay Albay";
           case 19: return "Tümgeneral";
           case 20: return "General";
           case 21: return "Admin";
           default: return "";
       }
   }

   
    /**
     @param pRankId
     rank ID to set
     */
    private void setRankId(final short pRankId) {
        this.mRankId = pRankId;
    }

    /**
     @return premium state for this account
     */
    public boolean isPremiumAccount() {
        return this.mPremiumAccount;
    }

    /**
     @param pPremiumAccount
     premium status to set
     */
    public void setPremiumAccount(final boolean pPremiumAccount) {
        this.mPremiumAccount = pPremiumAccount;
    }

    public int getIOil() {
    	return this.mIOil;
    }
    
    public void changeIOil(final int pIOil) {
        this.mIOil += pIOil;
    }
    
    public int getRocketLauncherAmount(final String pLootId) {
        switch (pLootId) {
            case AmmunitionManager.ECO_10:
                return this.getECO_10();
            case AmmunitionManager.SAR_02:
                return this.getSAR_02();
            default:
                return 0;
        }
    }
    
    public int getECO_10() {
    	return this.mECO_10;
    }
    
    public void changeECO_10(final int pECO_10) {
        this.mECO_10 += pECO_10;
    }
    
    public int getSAR_02() {
    	return this.mSAR_02;
    }
    
    public void changeSAR_02(final int pSAR_02) {
        this.mSAR_02 += pSAR_02;
    }
    
    /** 
     @return account title
     */
    public String getTitle() {
        return this.mTitle;
    }

    /**
     @param pTitle
     title to set
     */
    public void setTitle(final String pTitle) {
        this.mTitle = pTitle;
    }

    // ==============================================================

    /**
     @return account username
     */
    public String getUsername() {
        return this.mUsername;
    }
    
    /**
    @param pUsername
    username to set
    */
    public void setUsername(final String pUsername) {
       this.mUsername = pUsername;
    }  
   
    public String getShipUsername() {
        return this.mShipUsername;
    }
    
    public void setShipUsername(final String pShipUsername) {
        this.mShipUsername = pShipUsername;
    }
    
    public String getPetName() {
        return this.mPetName;
    }
    
    public void setPetName(final String pPetName) {
        this.mPetName = pPetName;
    }
    
    /**
     @return account MD5-encrypted password
     */
    public String getPasswordMD5() {
        return this.mPasswordMD5;
    }

    /**
     @param pPasswordMD5
     MD5-encrypted password to set
     */
    public void setPasswordMD5(final String pPasswordMD5) {
        this.mPasswordMD5 = pPasswordMD5;
    }

    /**
     @return account email
     */
    public String getEmail() {
        return this.mEmail;
    }

    /**
     @param pEmail
     email to set
     */
    public void setEmail(final String pEmail) {
        this.mEmail = pEmail;
    }

    /**
     @return user avatar web URL
     */
    public String getAvatarUrl() {
        return this.mAvatarUrl;
    }

    /**
     @param pAvatarUrl
     avatar URL to set
     */
    public void setAvatarUrl(String pAvatarUrl) {
        this.mAvatarUrl = pAvatarUrl;
    }

    // ==============================================================

    /**
     @param pClanID
     clan ID to set as user's Clan
     */
    public void setClan(int pClanID) {
    	
        final Clan clan = ClanStorage.getClan(pClanID);

        if (clan != null) {
            this.mClan = clan;
        }

    }   
    
    /**
     @return user's clan or null if user has no clan
     */
    public Clan getClan() {
        return this.mClan;
    }
    
    /**
     @return Clan ID or {@link #INVALID_ID} if has no clan
     */
    public int getClanId() {

        if (this.getClan() == null) {
            return INVALID_ID;
        } else {
            return this.getClan()
                       .getClanId();
        }

    }
    
    /**
     @return Clan tag or blank string if has no clan
     */
    public String getClanTag() {

        if (this.getClan() == null) {
            return DEFAULT_TEXT_VALUE;
        } else {
            return this.getClan()
                       .getClanTag();
        }
    }

    // ==============================================================

    /**
     @return Player object that corresponds this account(may be different from GameSession though)
     */
    public Player getPlayer() {
        return this.mPlayer;
    }

    // ==============================================================

    public PetMovement getMovement() {
        return this.mPetMovement;
    }
    
    /**
     @return AmmunitionManager for this account
     */
    public AmmunitionManager getAmmunitionManager() {
        return this.mAmmunitionManager;
    }

    /**
     @return TechsManager for this account
     */
    public TechsManager getTechsManager() {
        return this.mTechsManager;
    }

    /**
     @return ResourcesManager for this account
     */
    public ResourcesManager getResourcesManager() {
        return this.mResourcesManager;
    }

    /**
     @return ClientSettingsManager for this account
     */
    public ClientSettingsManager getClientSettingsManager() {
        return this.mClientSettingsManager;
    }

    /**
    @return ClientSettingsManager for this account
    */
    public BuyManager getBuyManager() {
        return this.mBuyManager;
    }
   
    /**
     @return EquipmentManager for this account
     */
    public EquipmentManager getEquipmentManager() {
        return this.mEquipmentManager;
    }

    /**
     @return DroneManager for this account
     */
    public DroneManager getDroneManager() {
        return this.mDroneManager;
    }
    
    public SkillsManager getSkillsManager() {
        return this.mSkillsManager;
    }
    
    public CpusManager getCpusManager() {
        return this.mCpusManager;
    }
    
    /**
     @return PetManager for this account
     */
    public PetManager getPetManager() {
        return this.mPetManager;
    }

    // ==============================================================

    /**
     @return true if user is admin
     */
    public boolean isAdmin() {
        return this.mIsAdmin == true;
    }
   
    public boolean isCm() {
        return this.mIsCm == true;
    }
    
    public boolean havePet() {
        return this.mHavePet == true;
    }
    
    public void setAdmin(final boolean pAdmin) {

        if (pAdmin) {
        	this.setRankId((short) 21);
            this.setPremiumAccount(true); // premium olarak ayarla
        } else {
            this.setRankId(getRankId());
        }
    }

    public int getExpansionStage() {
        return 3;
    }

}