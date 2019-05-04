package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
Created by LEJYONER on 08/01/2018
*/

public class WindowSettingsRequest
        extends ClientCommand {

    public static final int ID = 20592;
    
    public boolean tumPencereleriKapatma = false;    
    public String sagUstMenuPozisyonu = "";    
    public int miniHaritaBuyuklugu = 0;    
    public String normalSlotCubuguPozisyonu = "";    
    public String solUstMenuPozisyonu = "";    
    public String solUstMenuSiralamasi = "";    
    public String premiumSlotCubuguPozisyonu = "";    
    public String premiumSlotCubuguSiralamasi = "";    
    public String normalSlotCubuguSiralamasi = "";    
    public String kategoriCubuguPozisyonu = "";    
    public String bilmiyorum11 = "";    
    public String sagUstMenuSiralamasi = "";    
    public String gemiPenceresiAyarlari = "";

    /**
     Constructor
     */
    public WindowSettingsRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
        try {
            this.tumPencereleriKapatma = in.readBoolean();
            this.sagUstMenuPozisyonu = in.readUTF();
            this.miniHaritaBuyuklugu = in.readInt();
            this.miniHaritaBuyuklugu = this.miniHaritaBuyuklugu << 6 | this.miniHaritaBuyuklugu >>> 26;
            this.normalSlotCubuguPozisyonu = in.readUTF();
            this.solUstMenuPozisyonu = in.readUTF();
            this.solUstMenuSiralamasi = in.readUTF();
            this.premiumSlotCubuguPozisyonu = in.readUTF();
            this.premiumSlotCubuguSiralamasi = in.readUTF();
            this.normalSlotCubuguSiralamasi = in.readUTF();
            this.kategoriCubuguPozisyonu = in.readUTF();
            this.bilmiyorum11 = in.readUTF();
            this.sagUstMenuSiralamasi = in.readUTF();
            this.gemiPenceresiAyarlari = in.readUTF();
        } catch (IOException e) {

        }
    }
}