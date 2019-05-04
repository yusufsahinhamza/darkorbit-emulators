package simulator.users;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

import mysql.QueryManager;
import simulator.map_entities.movable.Player;
import simulator.netty.serverCommands.UpdateMenuItemCooldownGroupTimerCommand;

/**
 Created by LEJYONER on 26/05/2017.
 */

public class CpusManager
        extends AbstractAccountInternalManager {

    public static final String CLK_XL               = "equipment_extra_cpu_cl04k-xl";
    public static final String AUTO_ROCKET_CPU      = "equipment_extra_cpu_arol-x";
    public static final String AUTO_HELLSTROM_CPU   = "equipment_extra_cpu_rllb-x";
    public static final String ROCKET_LAUNCHER      = "equipment_weapon_rocketlauncher_hst-2";
    public static final String GALAXY_JUMP_CPU      = "equipment_extra_cpu_jp-02";

    public static final List<String> cpusCategory         = Arrays.asList(CLK_XL, AUTO_ROCKET_CPU, AUTO_HELLSTROM_CPU, GALAXY_JUMP_CPU);
    public final ArrayList<String> selectedCpus = new ArrayList<>();
    
    private static final int    PREMIUM_CLOAK_COOLDOWN_TIME  = 10000;
    private static final int    CLOAK_COOLDOWN_TIME          = 20000;
    
    private long mCloakCooldownEndTime   = 0L;
    private long mJumpCpuCooldownEndTime   = 0L;
    
    private boolean mAutoRocketCPU = true;
    private boolean mAutoRocketLauncherCPU = true;
    
    private String mSkillsJSON;
    
    public CpusManager(final Account pAccount) {
        super(pAccount);
        this.getSelectedCpus().add(AUTO_ROCKET_CPU);
        this.getSelectedCpus().add(AUTO_HELLSTROM_CPU);
    }
   
    public void sendJumpCpu() { 
    	final Player player = this.getAccount().getPlayer();
    	final long currentTime = System.currentTimeMillis();
    	final short mapID = 16;
    	Random randomGenerator = new Random();
		int posX = randomGenerator.nextInt(10000); 
		int posY = randomGenerator.nextInt(10000);
		
    	if(player.isInEquipZone()) {
    		if(!player.getLaserAttack().isAttackInProgress()) {
    			if ((currentTime - player.getLastDamagedTime()) >= 10000) {
    				if(player.getCurrentSpaceMapId() != mapID) {
    					player.jumpPortal(mapID, posX, posY);
    				}
    			} else {
                	final String errorMessage = "0|A|STD|Alınan son hasardan 10 saniye sonra atlama CPU kullanılabilir!";
                    player.sendPacketToBoundSessions(errorMessage);
    			}
    		} else {
            	final String errorMessage = "0|A|STD|Saldırı halindeyken atlama CPU kullanılamaz!";
                player.sendPacketToBoundSessions(errorMessage);
    		}
    	} else {
        	final String errorMessage = "0|A|STD|Atlama CPU yalnızca hangarda kullanılabilir!";
            player.sendPacketToBoundSessions(errorMessage);
    	}
    }
    
    public void sendAutoRocket()
    {
    	if(this.getAutoRocketCPU() == false) {
    	this.setAutoRocketCPU(true);
        final Player player = this.getAccount()
                .getPlayer();
        
        this.getSelectedCpus().add(AUTO_ROCKET_CPU);
        player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(AUTO_ROCKET_CPU));
    	}
    	else if (this.getAutoRocketCPU() == true) {
    		this.setAutoRocketCPU(false);
            final Player player = this.getAccount()
                    .getPlayer();
            
            this.getSelectedCpus().remove(AUTO_ROCKET_CPU);
            player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(AUTO_ROCKET_CPU));
    	}
    }
        
    public void sendAutoRocketLauncher()
    {
    	if(this.getAutoRocketLauncherCPU() == false) {
    	this.setAutoRocketLauncherCPU(true);
        final Player player = this.getAccount()
                .getPlayer();
        
        this.getSelectedCpus().add(AUTO_HELLSTROM_CPU);
        player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(AUTO_HELLSTROM_CPU));
    	}
    	else if (this.getAutoRocketLauncherCPU() == true) {
    		this.setAutoRocketLauncherCPU(false);
            final Player player = this.getAccount()
                    .getPlayer();
            
            this.getSelectedCpus().remove(AUTO_HELLSTROM_CPU);
            player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(AUTO_HELLSTROM_CPU));
    	}
    }
    
    public void sendCloak() // kamufle
    {
    	int uridium = -256;
    	final long currentTime = System.currentTimeMillis();
    	if (currentTime - this.getCloakCooldownEndTime() >= 0) {
    		if(this.getAccount().getPlayer().getCurrentSpaceMapId() != 42 && this.getAccount().getPlayer().getCurrentSpaceMapId() != 121) {
    		if(this.getAccount().getUridium() >= 256) {
    		if(!this.getAccount().isCloaked()) { //oyuncunun kamuflajı zaten aktifse kodu atlar
        final Player player = this.getAccount()
                .getPlayer();
        final String cloakPacket = "0|n|INV|" + player.getAccount().getUserId() + "|1";
        final String cloakPacketForPet = "0|n|INV|" + player.getAccount().getPetManager().getPetID() + "|1";
        player.getAccount().changeUridium(uridium);
        player.sendPacketToBoundSessions("0|LM|ST|URI|" + uridium + "|" + player.getAccount()
                .getUridium());
        player.getAccount().setCloaked(true);
        player.sendPacketToBoundSessions(cloakPacket);
        player.sendPacketToInRange(cloakPacket);
        player.sendPacketToBoundSessions(cloakPacketForPet);
        player.sendPacketToInRange(cloakPacketForPet);
        QueryManager.saveAccount(player.getAccount());
        if(player.getAccount().isPremiumAccount()) {
        	this.setCloakCooldownEndTime(currentTime + PREMIUM_CLOAK_COOLDOWN_TIME);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(CLK_XL),
            		player.getAccount().getAmmunitionManager().getItemTimerState(CLK_XL), PREMIUM_CLOAK_COOLDOWN_TIME,
            		PREMIUM_CLOAK_COOLDOWN_TIME));
            
            this.getSelectedCpus().add(CLK_XL);
            player.sendCommandToBoundSessions(player.getAccount().getAmmunitionManager().getCpuItemStatus(CLK_XL));
        }
        else {
        	this.setCloakCooldownEndTime(currentTime + CLOAK_COOLDOWN_TIME);
            player.sendCommandToBoundSessions(new UpdateMenuItemCooldownGroupTimerCommand(
            		player.getAccount().getAmmunitionManager().getCooldownType(CLK_XL),
            		player.getAccount().getAmmunitionManager().getItemTimerState(CLK_XL), CLOAK_COOLDOWN_TIME,
            		CLOAK_COOLDOWN_TIME));
        }
    		}
    		else {
                final Player player = this.getAccount()
                        .getPlayer();
            	final String cloakAlreadyActivePacket = "0|A|STD|Kamuflajın zaten aktif!";
                player.sendPacketToBoundSessions(cloakAlreadyActivePacket);
    		}
    		} else {
                final Player player = this.getAccount()
                        .getPlayer();
            	final String youDontHaveUridium = "0|A|STD|Yeterince uridiuma sahip değilsin!";
                player.sendPacketToBoundSessions(youDontHaveUridium);
    		}
    	 } else {
             final Player player = this.getAccount()
                     .getPlayer();
         	final String youCantUseCloak = "0|A|STD|Bu haritada kamuflaj kullanamazsın!";
             player.sendPacketToBoundSessions(youCantUseCloak); 
    	 }
      }
    }
    
    public ArrayList<String> getSelectedCpus() {
    	return this.selectedCpus;
    }
    
    public long getJumpCpuCooldownEndTime() {
        return mJumpCpuCooldownEndTime;
    }
    
    public void setJumpCpuCooldownEndTime(final long pJumpCpuCooldownEndTime) {
    	mJumpCpuCooldownEndTime = pJumpCpuCooldownEndTime;
    }
    
    public long getCloakCooldownEndTime() {
        return mCloakCooldownEndTime;
    }
    
    public void setCloakCooldownEndTime(final long pCloakCooldownEndTime) {
    	mCloakCooldownEndTime = pCloakCooldownEndTime;
    }
    
    public boolean getAutoRocketCPU() {
        return mAutoRocketCPU;
    }

    public void setAutoRocketCPU(final boolean pSetAutoRocketCPU) {
    	mAutoRocketCPU = pSetAutoRocketCPU;
    }
    
    public boolean getAutoRocketLauncherCPU() {
        return mAutoRocketLauncherCPU;
    }

    public void setAutoRocketLauncherCPU(final boolean pSetAutoRocketLauncherCPU) {
    	mAutoRocketLauncherCPU = pSetAutoRocketLauncherCPU;
    }
    
	@Override
	public void setFromJSON(final String pSkillsJSON) {
		this.mSkillsJSON = pSkillsJSON;
	}

	@Override
	public void setNewAccount() {
	}

	@Override
	public String packToJSON() {
		return this.mSkillsJSON;
	}
}
