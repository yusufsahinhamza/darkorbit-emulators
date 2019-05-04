package simulator.users;

import java.util.Arrays;
import java.util.List;
import java.util.Random;
import java.util.Vector;
import simulator.map_entities.movable.MovableMapEntity;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemStatusModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemTimerModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryItemTimerStateModule;
import simulator.netty.serverCommands.ClientUISlotBarCategoryModule;
import simulator.netty.serverCommands.ClientUITooltipModule;
import simulator.netty.serverCommands.ClientUITooltipsCommand;
import simulator.netty.serverCommands.CooldownTypeModule;
import simulator.netty.serverCommands.ShipDeselectionCommand;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;
import simulator.system.clans.Diplomacy;
import utils.MathUtils;

/**
 Created by LEJYONER on 04/05/2017.
 */
public class AmmunitionManager
        extends AbstractAccountInternalManager {

    public static final String LCB_10   = "ammunition_laser_lcb-10";
    public static final String MCB_25   = "ammunition_laser_mcb-25";
    public static final String MCB_50   = "ammunition_laser_mcb-50";
    public static final String UCB_100  = "ammunition_laser_ucb-100";
    public static final String SAB_50   = "ammunition_laser_sab-50";
    public static final String CBO_100  = "ammunition_laser_cbo-100";
    public static final String RSB_75   = "ammunition_laser_rsb-75";
    public static final String JOB_100  = "ammunition_laser_job-100";
    public static final String RB_214   = "ammunition_laser_rb-214";
    public static final String R_310    = "ammunition_rocket_r-310";
    public static final String PLT_2026 = "ammunition_rocket_plt-2026";
    public static final String PLT_2021 = "ammunition_rocket_plt-2021";
    public static final String PLT_3030 = "ammunition_rocket_plt-3030";
    public static final String PLD_8    = "ammunition_specialammo_pld-8";
    public static final String DCR_250  = "ammunition_specialammo_dcr-250";
    public static final String WIZ_X    = "ammunition_specialammo_wiz-x";
    public static final String BDR_1211 = "ammunition_rocket_bdr-1211";
    public static final String BDR_1212 = "ammunition_rocket_bdr-1212";
    public static final String R_IC3    = "ammunition_specialammo_r-ic3";
    public static final String HSTRM_01 = "ammunition_rocketlauncher_hstrm-01";
    public static final String UBR_100  = "ammunition_rocketlauncher_ubr-100";
    public static final String ECO_10   = "ammunition_rocketlauncher_eco-10";
    public static final String SAR_01   = "ammunition_rocketlauncher_sar-01";
    public static final String SAR_02   = "ammunition_rocketlauncher_sar-02";
    public static final String CBR      = "ammunition_rocketlauncher_cbr";
    public static final String BDR1212  = "ammunition_rocketlauncher_bdr1212";
    public static final String EMP_01   = "ammunition_specialammo_emp-01";
    public static final String FWX_COM  = "ammunition_firework_fwx-com";
    public static final String FWX_L    = "ammunition_firework_fwx-l";
    public static final String FWX_M    = "ammunition_firework_fwx-m";
    public static final String FWX_RZ   = "ammunition_firework_fwx-rz";
    public static final String FWX_S    = "ammunition_firework_fwx-s";
    public static final String ACM_01   = "ammunition_mine_acm-01";
    public static final String DDM_01   = "ammunition_mine_ddm-01";
    public static final String EMPM_01  = "ammunition_mine_empm-01";
    public static final String SABM_01  = "ammunition_mine_sabm-01";
    public static final String SLM_01   = "ammunition_mine_slm-01";
    public static final String SMB_01   = "ammunition_mine_smb-01";
    public static final String ISH_01   = "equipment_extra_cpu_ish-01";
    public static final String ROCKET_LAUNCHER = "equipment_weapon_rocketlauncher_hst-2";
    
    public static final String FIREWORK_IGNITE = "ammunition_firework_ignite";

    public static final List<String> laserCategory          =
            Arrays.asList(LCB_10, MCB_25, MCB_50, UCB_100, SAB_50, RSB_75/** JOB_100, RB_214, CBO_100*/);
    public static final List<String> rocketCategory         =
            Arrays.asList(R_310, PLT_2026, PLT_2021, PLT_3030 /**PLD_8, DCR_250, WIZ_X, BDR_1211, BDR_1212, R_IC3 */);
    public static final List<String> rocketLauncherCategory =
            Arrays.asList(ECO_10, SAR_02);
    public static final List<String> specialAmmoCategory    =
            Arrays.asList(SMB_01, ISH_01, EMP_01/**, FWX_COM, FWX_L, FWX_M, FWX_RZ, FWX_S */);
    public static final List<String> minesCategory          = Arrays.asList(/**ACM_01, SLM_01, SABM_01,EMPM_01, DDM_01*/);
    
    private static final int    ISH_COOLDOWN_TIME            = 30000;
    private static final int    SMB_COOLDOWN_TIME            = 30000;
    private static final int    EMP_COOLDOWN_TIME            = 40000;
    private static final int    ISH_EFFECT_DURATION          = 3000;
    private static final int    SMB_EFFECT_RANGE             = 700;
    private static final int    EMP_EFFECT_RANGE             = 700;
    private static final double SMB_DAMAGE_HITPOINTS         = 0.20;
    private static final int    EMP_EFFECT_DURATION          = 3000;
    
    private int mMineOwnerID;
    private int mMineX;
    private int mMineY;
    private int mMineID;
    private long   lastInsertedMine   = 0;
    private short  timeToEscape       = 3000; //mayın attıktan 3 saniye sonra patlama aktifleşir
    
    private long mIshCooldownEndTime = 0L;
    private long mSmbCooldownEndTime = 0L;
    private long mEmpCooldownEndTime = 0L;
    private long mIshEffectEndTime   = 0L;
    private long mEmpEffectEndTime   = 0L;
    private boolean mPlayerUsingISH   = false;

    public AmmunitionManager(final Account pAccount) {
        super(pAccount);
    }

    public void onTickCheckMethods() {
        this.checkSLMine();
    }
 
    private boolean mSLMineEffectActivated        = false;
    private long mSLMineEffectFinishTime            = 0L;
    private long mSLMineCooldownFinishTime     = 0L;
    private static final int    MINE_COOLDOWN          = 30000;
    
    public void sendSLMine() // yavaşlatıcı mayın
    {
    	final long currentTime = System.currentTimeMillis();
        final Player player = this.getAccount()
                                  .getPlayer();
        if(!player.getAccount().getPlayer().isInSecureZone()){
        if ((currentTime - this.getSLMineCooldownFinishTime()) >= 0) {
        Random randomMineID = new Random(); // rastgele mineID ıretir
		this.setMineID(randomMineID.nextInt(999999999)); // mineID değişkenine 1 ile 999999999 arasında bir sayı gönderir
		mMineOwnerID = player.getAccount().getUserId();
		this.setMineX(player.getCurrentPositionX());
		this.setMineY(player.getCurrentPositionY());
        this.setSLMineEffectActivated(true);
        this.setSLMineEffectFinishTime(currentTime + 7000);
        this.setSLMineCooldownFinishTime(
                currentTime + 30000);
        player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                player.getAccount().getAmmunitionManager().getCooldownType(SLM_01),
                player.getAccount().getAmmunitionManager().getItemTimerState(SLM_01), MINE_COOLDOWN,
                MINE_COOLDOWN));
	    lastInsertedMine = System.currentTimeMillis();
        final String minePacket = "0|L|" + this.getMineID() + "|7|" + this.getMineX() + "|" + this.getMineY() + "";
            player.sendPacketToBoundSessions(minePacket);
            player.sendPacketToInRange(minePacket);
        }
      }        
    }
    
    public long getSLMineCooldownFinishTime() {
        return mSLMineCooldownFinishTime;
    }

    public void setSLMineCooldownFinishTime(final long pSLMineCooldownEndTime) {
    	mSLMineCooldownFinishTime = pSLMineCooldownEndTime;
    }
    
    public long getSLMineEffectFinishTime() {
        return mSLMineEffectFinishTime;
    }

    public void setSLMineEffectFinishTime(final long pSLmineEffectFinishTime) {
    	mSLMineEffectFinishTime = pSLmineEffectFinishTime;
    }
         
    public boolean isSLMineEffectActivated() {
        return mSLMineEffectActivated;
    }

    public void setSLMineEffectActivated(final boolean pSLMineEffectActivated) {
    	mSLMineEffectActivated = pSLMineEffectActivated;
    }
   
    private boolean isSLMineActivated() {
        return (System.currentTimeMillis() - this.getSLMineEffectFinishTime()) < 0;
    }
    
    private void checkSLMine() {
        final Player player = this.getAccount()
                                  .getPlayer();
        if (this.isSLMineActivated()) {
            for (final MovableMapEntity thisMapEntity : player.getInRangeMovableMapEntities()) {
            	final Player otherPlayer = (Player) thisMapEntity;
                if (thisMapEntity.getCurrentPositionX() >= (getMineX() - 100) &&
                	thisMapEntity.getCurrentPositionX() <= (getMineX() + 100) &&
                	thisMapEntity.getCurrentPositionY() >= (getMineY() - 100) &&
                	thisMapEntity.getCurrentPositionY() <= (getMineY() + 100)) {
                        if (player.getCurrentPositionX() >= (getMineX() - 100) &&
                     		player.getCurrentPositionX() <= (getMineX() + 100) &&
                     		player.getCurrentPositionY() >= (getMineY() - 100) &&
                     		player.getCurrentPositionY() <= (getMineY() + 100)) {                	
                        	this.setMineX(-1);
                        	this.setMineY(-1);
                     	player.sendPacketToBoundSessions("0|n|MIN|" + getMineID());
                     	player.sendPacketToInRange("0|n|MIN|" + getMineID());
                       	player.getAccount().getEquipmentManager().setSpeedConfig1(-150);
                       	player.getAccount().getEquipmentManager().setSpeedConfig2(-150);
                       	player.sendCommandToBoundSessions(player.getSetSpeedCommand());
                        final String SaboteurEffectPacket = "0|n|fx|start|SABOTEUR_DEBUFF|" + otherPlayer.getMapEntityId() + "";
                        otherPlayer.sendPacketToBoundSessions(SaboteurEffectPacket);
                        otherPlayer.sendPacketToInRange(SaboteurEffectPacket);
                            }
                if(thisMapEntity instanceof Player)
                 {
                	this.setMineX(-1);
                	this.setMineY(-1);
             	player.sendPacketToBoundSessions("0|n|MIN|" + getMineID());
             	player.sendPacketToInRange("0|n|MIN|" + getMineID());
             	otherPlayer.sendPacketToBoundSessions("0|n|MIN|" + getMineID());
             	otherPlayer.sendPacketToInRange("0|n|MIN|" + getMineID());
             	otherPlayer.getAccount().getEquipmentManager().setSpeedConfig1(-150);
             	otherPlayer.getAccount().getEquipmentManager().setSpeedConfig2(-150);
             	otherPlayer.sendCommandToBoundSessions(otherPlayer.getSetSpeedCommand());
                final String SaboteurEffectPacket = "0|n|fx|start|SABOTEUR_DEBUFF|" + otherPlayer.getMapEntityId() + "";
                otherPlayer.sendPacketToBoundSessions(SaboteurEffectPacket);
                otherPlayer.sendPacketToInRange(SaboteurEffectPacket);
                 }
                }
            }
            if (getMineOwnerID() != player.getAccount().getUserId()|| (lastInsertedMine + timeToEscape) < System.currentTimeMillis()) {
                if (player.getCurrentPositionX() >= (getMineX() - 100) &&
             		player.getCurrentPositionX() <= (getMineX() + 100) &&
             		player.getCurrentPositionY() >= (getMineY() - 100) &&
             		player.getCurrentPositionY() <= (getMineY() + 100)) {                	
                	this.setMineX(-1);
                	this.setMineY(-1);
             	player.sendPacketToBoundSessions("0|n|MIN|" + getMineID());
             	player.sendPacketToInRange("0|n|MIN|" + getMineID());
               	player.getAccount().getEquipmentManager().setSpeedConfig1(-150);
               	player.getAccount().getEquipmentManager().setSpeedConfig2(-150);
               	player.sendCommandToBoundSessions(player.getSetSpeedCommand());
                final String SaboteurEffectPacket = "0|n|fx|start|SABOTEUR_DEBUFF|" + player.getMapEntityId() + "";
                player.sendPacketToBoundSessions(SaboteurEffectPacket);
                player.sendPacketToInRange(SaboteurEffectPacket);
                    }
                }
        } else if (this.isSLMineEffectActivated()) {
           	player.getAccount().getEquipmentManager().setSpeedConfig1(0);
           	player.getAccount().getEquipmentManager().setSpeedConfig2(0);
           	player.sendCommandToBoundSessions(player.getSetSpeedCommand());
           	final long currentTime = System.currentTimeMillis();
            if ((currentTime - this.getSLMineCooldownFinishTime()) >= 0) {
           	player.sendPacketToBoundSessions("0|n|MIN|" + getMineID());
           	player.sendPacketToInRange("0|n|MIN|" + getMineID());
           	}
            final String SaboteurEffectPacket = "0|n|fx|end|SABOTEUR_DEBUFF|" + player.getMapEntityId() + "";
            player.sendPacketToBoundSessions(SaboteurEffectPacket);
            player.sendPacketToInRange(SaboteurEffectPacket);
           	for (final MovableMapEntity thisMapEntity : player.getInRangeMovableMapEntities()) {
            	final Player otherPlayer = (Player) thisMapEntity;
         	otherPlayer.sendPacketToBoundSessions("0|n|MIN|" + getMineID());
         	otherPlayer.sendPacketToInRange("0|n|MIN|" + getMineID());
         	otherPlayer.getAccount().getEquipmentManager().setSpeedConfig1(0);
         	otherPlayer.getAccount().getEquipmentManager().setSpeedConfig2(0);
         	otherPlayer.sendCommandToBoundSessions(otherPlayer.getSetSpeedCommand());
            final String SaboteurEffectPacketForOtherPlayer = "0|n|fx|end|SABOTEUR_DEBUFF|" + player.getMapEntityId() + "";
            otherPlayer.sendPacketToBoundSessions(SaboteurEffectPacketForOtherPlayer);
            otherPlayer.sendPacketToInRange(SaboteurEffectPacketForOtherPlayer);
           	}
            this.setSLMineEffectActivated(false);
        }
    }
    
    public void sendDCR250Rocket() // yavaşlatıcı roket
    {
        final Player player = this.getAccount()
                .getPlayer();
        final String DCR250RocketPacket = "0|v|" + player.getAccount().getUserId() + "|" + player.getLockedTarget().getMapEntityId() + "|H|10|2|0";
        final String SaboteurEffectPacket = "0|n|fx|start|SABOTEUR_DEBUFF|" + player.getLockedTarget().getMapEntityId() + "";
        player.sendPacketToBoundSessions(DCR250RocketPacket);
        player.sendPacketToInRange(DCR250RocketPacket);
        player.sendPacketToBoundSessions(SaboteurEffectPacket);
        player.sendPacketToInRange(SaboteurEffectPacket);
    }
    
    public void sendISH() {
        final long currentTime = System.currentTimeMillis();
        if (currentTime - this.getIshCooldownEndTime() >= 0) {
            final Player player = this.getAccount()
                                      .getPlayer();
            final String ishPacket = "0|n|ISH|" + player.getMapEntityId();
            player.sendPacketToBoundSessions(ishPacket);
            player.sendPacketToInRange(ishPacket);
            this.setIshEffectEndTime(currentTime + ISH_EFFECT_DURATION);
            this.setIshCooldownEndTime(currentTime + ISH_COOLDOWN_TIME);
            this.setPlayerUsingISH(true);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    getCooldownType(AmmunitionManager.ISH_01),
                    getItemTimerState(AmmunitionManager.ISH_01), ISH_COOLDOWN_TIME,
                    ISH_COOLDOWN_TIME));
        }
    }

    public void sendSMB() {
        final long currentTime = System.currentTimeMillis();
        final Player player2 = this.getAccount()
                .getPlayer();
        if(!player2.getAccount().getPlayer().isInSecureZone()){
        if (currentTime - this.getSmbCooldownEndTime() >= 0) {
            final Player player = this.getAccount()
                                      .getPlayer();	
            final String smbPacket = "0|n|SMB|" + player.getMapEntityId();
            player.sendPacketToBoundSessions(smbPacket);
            player.sendPacketToInRange(smbPacket);
            this.setSmbCooldownEndTime(currentTime + SMB_COOLDOWN_TIME);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    getCooldownType(AmmunitionManager.SMB_01),
                    getItemTimerState(AmmunitionManager.SMB_01), SMB_COOLDOWN_TIME,
                    SMB_COOLDOWN_TIME));
            for (final MovableMapEntity movableMapEntity : player.getInRangeMovableMapEntities()) {
                if (MathUtils.hypotenuse(player.getCurrentPositionX() - movableMapEntity.getCurrentPositionX(),
                                         player.getCurrentPositionY() - movableMapEntity.getCurrentPositionY()) <=
                    SMB_EFFECT_RANGE) {
                	if(movableMapEntity instanceof Player) {
                	if(((Player) movableMapEntity).canBeShoot()) {												
        			final Player thisPlayer = (Player) player;
                    final Player otherPlayer = (Player) movableMapEntity;
                        boolean isWar;
                        if(thisPlayer.getAccount().getFactionId() == otherPlayer.getAccount().getFactionId()){
                            isWar = false;
                            for(Diplomacy dip : thisPlayer.getAccount().getClan().getDiplomacies()){
                                if(dip.relationType == 3 && (dip.clanID1 == otherPlayer.getAccount().getClanId() || dip.clanID2 == otherPlayer.getAccount().getClanId())){
                                    isWar = true;
                                }
                            }
                            
                            if(!isWar) {
                            	return;
                            }
                            
                        }    
                        
						final Player targetPlayer = (Player) movableMapEntity;
                        final int damage = (int) (SMB_DAMAGE_HITPOINTS * targetPlayer.getCurrentHitPoints());
                        targetPlayer.addHitPointsDamage(player, damage);						
                	}
                	}
                }
            }
            }        			
        }    
    }

    public void sendEMP() {
        final long currentTime = System.currentTimeMillis();
        if (currentTime - this.getEmpCooldownEndTime() >= 0) {
            final Player player = this.getAccount()
                                      .getPlayer();
            final String empPacket = "0|n|EMP|" + player.getMapEntityId();
            player.sendPacketToBoundSessions(empPacket);
            player.sendPacketToInRange(empPacket);
            this.setEmpEffectEndTime(currentTime + EMP_EFFECT_DURATION);
            this.setEmpCooldownEndTime(currentTime + EMP_COOLDOWN_TIME);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
                    getCooldownType(AmmunitionManager.EMP_01),
                    getItemTimerState(AmmunitionManager.EMP_01), EMP_COOLDOWN_TIME,
                    EMP_COOLDOWN_TIME));            
            for (final MovableMapEntity thisMapEntity : player.getInRangeMovableMapEntities()) {
            	if(thisMapEntity instanceof Player) {
            	if(((Player)thisMapEntity) != player) {
            		if(((Player)thisMapEntity).getLockedTarget() == player) {
            	final String empMessagePacket = "0|A|STM|msg_own_targeting_harmed";
            	((Player)thisMapEntity).sendPacketToBoundSessions(empMessagePacket);
            		}
            	  }
            	}
                if (thisMapEntity.getLockedTarget() == player) {
                    thisMapEntity.setLockedTarget(null);
                    if (thisMapEntity instanceof Player) {
                        ((Player) thisMapEntity).sendCommandToBoundSessions(new ShipDeselectionCommand());
                    }
                }
                if (MathUtils.hypotenuse(player.getCurrentPositionX() - thisMapEntity.getCurrentPositionX(),
                        player.getCurrentPositionY() - thisMapEntity.getCurrentPositionY()) <=
                        EMP_EFFECT_RANGE) {
                if(thisMapEntity instanceof Player) {
                	if (((Player) thisMapEntity).getAccount().getFactionId() != player.getAccount().getFactionId()) {
                    final String cloakPacket = "0|n|INV|" + ((Player) thisMapEntity).getAccount().getPlayer().getAccount().getUserId() + "|0";
                	((Player) thisMapEntity).getAccount().getPlayer().getAccount().setCloaked(false);
                	((Player) thisMapEntity).getAccount().getPlayer().sendPacketToBoundSessions(cloakPacket);
                	((Player) thisMapEntity).getAccount().getPlayer().sendPacketToInRange(cloakPacket);
                	((Player) thisMapEntity).getAccount().getCpusManager().getSelectedCpus().remove(CpusManager.CLK_XL);
                	((Player) thisMapEntity).sendCommandToBoundSessions(((Player) thisMapEntity).getAccount().getAmmunitionManager().getCpuItemStatus(CpusManager.CLK_XL));
                	}
                }
                }
            }
            final String deactivePacket = "0|SD|D|R|5|" + this.getAccount().getUserId() + "";
            player.sendPacketToInRange(deactivePacket);
            player.sendPacketToBoundSessions(deactivePacket);
            final String deactivePacket2 = "0|SD|D|R|2|" + this.getAccount().getUserId() + "";
            player.sendPacketToInRange(deactivePacket2);
            player.sendPacketToBoundSessions(deactivePacket2);
        }
    }

   // private static final String LASER_TOOLTIP_ID = "";

    public ClientUISlotBarCategoryModule getLasersCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> lasersItems = new Vector<>();
        for (final String itemLootId : laserCategory) {

            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            lasersItems.add(new ClientUISlotBarCategoryItemModule(1, this.getLaserItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("lasers", lasersItems);
    }

    public ClientUISlotBarCategoryModule getRocketsCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> rocketItems = new Vector<>();
        for (final String itemLootId : rocketCategory) {

            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            rocketItems.add(new ClientUISlotBarCategoryItemModule(1, this.getRocketItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("rockets", rocketItems);
    }
  
    public ClientUISlotBarCategoryModule getRocketLauncherCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> rocketLauncherItems = new Vector<>();
        for (final String itemLootId : rocketLauncherCategory) {

            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            rocketLauncherItems.add(new ClientUISlotBarCategoryItemModule(1, this.getRocketLauncherItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("rocket_launchers", rocketLauncherItems);
    }
    
    public ClientUISlotBarCategoryModule getSpecialAmmoCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> specialAmmoItems = new Vector<>();
        for (final String itemLootId : specialAmmoCategory) {

            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            specialAmmoItems.add(new ClientUISlotBarCategoryItemModule(1, this.getSpecialItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("special_items", specialAmmoItems);
    }
    
    public ClientUISlotBarCategoryModule getTechCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> techItems = new Vector<>();
        for (final String itemLootId : TechsManager.techsCategory) {

            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            techItems.add(new ClientUISlotBarCategoryItemModule(1, this.getTechItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("tech_items", techItems);
    }
   
    public ClientUISlotBarCategoryModule getCpusCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> cpuItems = new Vector<>();
        for (final String itemLootId : CpusManager.cpusCategory) {
            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            cpuItems.add(new ClientUISlotBarCategoryItemModule(1, this.getCpuItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("cpus", cpuItems);
    }
    
    public ClientUISlotBarCategoryModule getDroneFormationsCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> droneFormationItems = new Vector<>();
        for (final String itemLootId : DroneManager.droneCategory) {
            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            droneFormationItems.add(new ClientUISlotBarCategoryItemModule(1, this.getDroneItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("drone_formations", droneFormationItems);
    }
    
    public String getShipLootId(int pShipID) {
    	switch(pShipID) {
    		case 63:
    			return "ability_solace";
    		case 64:
    			return "ability_diminisher";
    		case 65:
    			return "ability_spectrum";
    		case 447:
    			return "ability_spectrum";
    		case 450:
    			return "ability_spectrum";
    		case 66:
    			return "ability_sentinel";
    		case 448:
    			return "ability_sentinel";
    		case 449:
    			return "ability_sentinel";
    		case 67:
    			return "ability_venom";
    		case 445:
    			return "ability_venom";
    		case 452:
    			return "ability_venom";
    		default: 
    			return "";
    	}
    }
    
    public ClientUISlotBarCategoryModule getAbilityCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> abilityItems = new Vector<>();
        
        final String itemLootId = this.getShipLootId(this.getAccount().getPlayer().getPlayerShip().getShipId());
        
            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            abilityItems.add(new ClientUISlotBarCategoryItemModule(1, this.getAbilityItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        return new ClientUISlotBarCategoryModule("ship_abilities", abilityItems);
    }
    
    public ClientUISlotBarCategoryModule getBuyCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> petItems = new Vector<>();
        for (final String itemLootId : BuyManager.buyCategory) {

            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            petItems.add(new ClientUISlotBarCategoryItemModule(1, this.getBuyItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NUMBER,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("buy_now", petItems);
    }
    
    public ClientUISlotBarCategoryModule getMinesCategory() {
        final Vector<ClientUISlotBarCategoryItemModule> minesItems = new Vector<>();
        for (final String itemLootId : AmmunitionManager.minesCategory) {
            ClientUISlotBarCategoryItemTimerModule categoryTimerModule =
                    new ClientUISlotBarCategoryItemTimerModule(this.getCooldownTime(itemLootId),
                                                               this.getItemTimerState(itemLootId), 90000000, itemLootId,
                                                               false);

            minesItems.add(new ClientUISlotBarCategoryItemModule(1, this.getMineItemStatus(itemLootId),
                                                                  ClientUISlotBarCategoryItemModule.SELECTION,
                                                                  ClientUISlotBarCategoryItemModule.NONE,
                                                                  this.getCooldownType(itemLootId),
                                                                  categoryTimerModule));
        }
        return new ClientUISlotBarCategoryModule("mines", minesItems);
    }
    
    public CooldownTypeModule getCooldownType(final String pItemId) {
        switch (pItemId) {
            case SLM_01:
                return new CooldownTypeModule(CooldownTypeModule.short_755);
            case RSB_75:
                return new CooldownTypeModule(CooldownTypeModule.RAPID_SALVO_BLAST);
            case CpusManager.GALAXY_JUMP_CPU:
                return new CooldownTypeModule(CooldownTypeModule.short_1736);
            case CpusManager.CLK_XL:
                return new CooldownTypeModule(CooldownTypeModule.short_2419);
            case TechsManager.TECH_ENERGY_LEECH:
                return new CooldownTypeModule(CooldownTypeModule.ENERGY_LEECH_ARRAY);
            case TechsManager.TECH_BATTLE_REPAIR_BOT:
                return new CooldownTypeModule(CooldownTypeModule.BATTLE_REPAIR_BOT);
            case TechsManager.TECH_BACKUP_SHIELDS:
                return new CooldownTypeModule(CooldownTypeModule.SHIELD_BACKUP);
            case TechsManager.TECH_PRECISION_TARGETER:
                return new CooldownTypeModule(CooldownTypeModule.ROCKET_PROBABILITY_MAXIMIZER);
            case SkillsManager.AEGIS_HP_REPAIR:
                return new CooldownTypeModule(CooldownTypeModule.short_2642);
            case SkillsManager.AEGIS_SHIELD_REPAIR:
                return new CooldownTypeModule(CooldownTypeModule.short_1789);
            case SkillsManager.SOLACE_ABILITY:
                return new CooldownTypeModule(CooldownTypeModule.short_1699);
            case SkillsManager.SPECTRUM_ABILITY:
                return new CooldownTypeModule(CooldownTypeModule.short_2204);
            case SkillsManager.VENOM_ABILITY:
                return new CooldownTypeModule(CooldownTypeModule.short_798);
            case SkillsManager.SENTINEL_ABILITY:
                return new CooldownTypeModule(CooldownTypeModule.short_1952);
            case SkillsManager.DIMINISHER_ABILITY:
                return new CooldownTypeModule(CooldownTypeModule.short_888);
            case DroneManager.ARROW_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.BARRAGE_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.BAT_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.CHEVRON_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.CRAB_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.DEFAULT_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.DIAMOND_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.DOUBLE_ARROW_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.HEART_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.LANCE_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.MOTH_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.PINCER_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.STAR_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case DroneManager.TURTLE_FORMATION:
                return new CooldownTypeModule(CooldownTypeModule.short_987);
            case ISH_01:
                return new CooldownTypeModule(CooldownTypeModule.MINE);
            case SMB_01:
                return new CooldownTypeModule(CooldownTypeModule.short_1048);
            case EMP_01:
                return new CooldownTypeModule(CooldownTypeModule.short_1085);
            case R_310:
                return new CooldownTypeModule(CooldownTypeModule.ROCKET);
            case PLT_2026:
                return new CooldownTypeModule(CooldownTypeModule.ROCKET);
            case PLT_2021:
                return new CooldownTypeModule(CooldownTypeModule.ROCKET);
            case PLT_3030:
                return new CooldownTypeModule(CooldownTypeModule.ROCKET);
            default:
                return new CooldownTypeModule(CooldownTypeModule.NONE);
        }
    }

    public ClientUISlotBarCategoryItemStatusModule getLaserItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, this.getAccount()
                                                                                            .getClientSettingsManager()
                                                                                            .getSelectedLaserItem()
                                                                                            .equalsIgnoreCase(pItemId),
                                                           0);
    }

    public ClientUISlotBarCategoryItemStatusModule getMineItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, this.getAccount()
                                                                                            .getClientSettingsManager()
                                                                                            .getSelectedLaserItem()
                                                                                            .equalsIgnoreCase(pItemId),
                                                           0);
    }
    
    public ClientUISlotBarCategoryItemStatusModule getRocketItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, this.getAccount()
                                                                                            .getClientSettingsManager()
                                                                                            .getSelectedRocketItem()
                                                                                            .equalsIgnoreCase(pItemId),
                                                           0);
    }
  
    public ClientUISlotBarCategoryItemStatusModule getRocketLauncherItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           this.getAccount().getRocketLauncherAmount(pItemId), false, true,
                                                           slotBarStatusTooltip, false, this.getAccount()
                                                                                            .getClientSettingsManager()
                                                                                            .getSelectedRocketLauncherItem()
                                                                                            .equalsIgnoreCase(pItemId),
                                                           0);
    }
    
    public ClientUISlotBarCategoryItemStatusModule getSpecialItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, false,
                                                           0);
    }
    
    public ClientUISlotBarCategoryItemStatusModule getTechItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, false,
                                                           0);
    }
    
    public ClientUISlotBarCategoryItemStatusModule getCpuItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, this.getAccount()
								                                                            .getCpusManager()
								                                                            .getSelectedCpus()
								                                                            .contains(pItemId),
                                                           0);
    }
   
    public ClientUISlotBarCategoryItemStatusModule getDroneItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           0, false, true,
                                                           slotBarStatusTooltip, false, false,
                                                           0);
    }
    
    public ClientUISlotBarCategoryItemStatusModule getAbilityItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                0, false, true,
                slotBarStatusTooltip, false, false,
                0);
    }
    
    public ClientUISlotBarCategoryItemStatusModule getBuyItemStatus(final String pItemId) {

        ClientUITooltipsCommand itemBarStatusTootip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());
        ClientUITooltipsCommand slotBarStatusTooltip = new ClientUITooltipsCommand(new Vector<ClientUITooltipModule>());

        return new ClientUISlotBarCategoryItemStatusModule(itemBarStatusTootip, true, pItemId, true,
                                                           ClientUISlotBarCategoryItemStatusModule.BLUE, pItemId,
                                                           100, false, true,
                                                           slotBarStatusTooltip, true, false,
                                                           0);
    }
    
    private long getCooldownTime(final String pItemId) {
        switch (pItemId) {
            case RSB_75:
                return this.getAccount()
                           .getPlayer()
                           .getLaserAttack()
                           .getRsbCooldownTime();
            case R_310:
            	return this.getAccount()
            			.getPlayer()
            			.getRocketAttack()
            			.getRocketCooldownTime();
            case PLT_2026:
            	return this.getAccount()
            			.getPlayer()
            			.getRocketAttack()
            			.getRocketCooldownTime();
            case PLT_2021:
            	return this.getAccount()
            			.getPlayer()
            			.getRocketAttack()
            			.getRocketCooldownTime();
            case PLT_3030:
            	return this.getAccount()
            			.getPlayer()
            			.getRocketAttack()
            			.getRocketCooldownTime();
            default:
                return 0;
        }
    }

    public ClientUISlotBarCategoryItemTimerStateModule getItemTimerState(final String pItemId) {
        switch (pItemId) {
            case RSB_75:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case R_310:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case PLT_2026:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case PLT_2021:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case PLT_3030:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case EMP_01:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case ISH_01:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case SMB_01:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case SkillsManager.SOLACE_ABILITY:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case SkillsManager.SENTINEL_ABILITY:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case SkillsManager.DIMINISHER_ABILITY:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case SkillsManager.VENOM_ABILITY:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case SkillsManager.SPECTRUM_ABILITY:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case TechsManager.TECH_BATTLE_REPAIR_BOT:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case TechsManager.TECH_BACKUP_SHIELDS:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case TechsManager.TECH_PRECISION_TARGETER:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case TechsManager.TECH_ENERGY_LEECH:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.ARROW_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.BARRAGE_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.BAT_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.CHEVRON_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.CRAB_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.DEFAULT_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.DIAMOND_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.DOUBLE_ARROW_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.HEART_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.LANCE_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.MOTH_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.PINCER_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.STAR_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case DroneManager.TURTLE_FORMATION:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case CpusManager.GALAXY_JUMP_CPU:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            case CpusManager.CLK_XL:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.short_2168);
            default:
                return new ClientUISlotBarCategoryItemTimerStateModule(
                        ClientUISlotBarCategoryItemTimerStateModule.ACTIVE);
        }
    }
      
    public long getIshCooldownEndTime() {
        return mIshCooldownEndTime;
    }

    public void setIshCooldownEndTime(final long pIshCooldownEndTime) {
        mIshCooldownEndTime = pIshCooldownEndTime;
    }

    public long getSmbCooldownEndTime() {
        return mSmbCooldownEndTime;
    }

    public void setSmbCooldownEndTime(final long pSmbCooldownEndTime) {
        mSmbCooldownEndTime = pSmbCooldownEndTime;
    }

    public long getEmpCooldownEndTime() {
        return mEmpCooldownEndTime;
    }

    public void setEmpCooldownEndTime(final long pEmpCooldownEndTime) {
        mEmpCooldownEndTime = pEmpCooldownEndTime;
    }

    public long getIshEffectEndTime() {
        return mIshEffectEndTime;
    }

    public void setIshEffectEndTime(final long pIshEffectEndTime) {
        mIshEffectEndTime = pIshEffectEndTime;
    }
    
    public void setPlayerUsingISH(final boolean pUsingISH) {
        mPlayerUsingISH = pUsingISH;
    }
    
    public boolean getPlayerUsingISH() {
        return mPlayerUsingISH;
    }
    
    public long getEmpEffectEndTime() {
        return mEmpEffectEndTime;
    }

    public void setEmpEffectEndTime(final long pEmpEffectEndTime) {
        mEmpEffectEndTime = pEmpEffectEndTime;
    }
    
    public int getMineOwnerID() {
    	return this.mMineOwnerID;
    }
    
    public void setMineOwnerID(final int pMineOwnerID) {
    	mMineOwnerID = pMineOwnerID;
    }
    
    public int getMineX() {
    	return this.mMineX;
    }
    
    public void setMineX(final int pMineX) {
    	mMineX = pMineX;
    }
    
    public int getMineY() {
    	return this.mMineY;
    }
    
    public void setMineY(final int pMineY) {
    	mMineY = pMineY;
    }
    
    public int getMineID() {
    	return this.mMineID;
    }
    
    public void setMineID(final int pMineID) {
    	mMineID = pMineID;
    }

	public void setFromJSON(String pJSON) {
		
	}

	public void setNewAccount() {
		
	}

	public String packToJSON() {
		return null;
	}
}
