package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

public class AssetCreateCommand
        implements simulator.netty.ServerCommand {

    public static int     ID             = 10339;
    public        boolean showBubble     = false;
    public        int     clanId         = 0;
    public        int     assetId        = 0;
    public        String  clanTag        = "";
    public        int     expansionStage = 0;
    public ClanRelationModule            clanRelation;
    public Vector<VisualModifierCommand> modifier;
    public boolean detectedByWarnRadar = false;
    public int     factionId           = 0;
    public boolean invisible           = false;
    public AssetTypeModule type;
    public int     posX               = 0;
    public int     posY               = 0;
    public String  userName           = "";
    public int     designId           = 0;
    public boolean visibleOnWarnRadar = false;

    public AssetCreateCommand(AssetTypeModule param1, String param2, int param3, String param4, int param5, int param6,
                              int param7, int param8, int param9, int param10, boolean param11, boolean param12,
                              boolean param13, boolean param14, ClanRelationModule param15,
                              Vector<VisualModifierCommand> param16) {
        this.type = param1;
        this.userName = param2;
        this.factionId = param3;
        this.clanTag = param4;
        this.assetId = param5;
        this.designId = param6;
        this.expansionStage = param7;
        this.posX = param8;
        this.posY = param9;
        this.clanId = param10;
        this.invisible = param11;
        this.visibleOnWarnRadar = param12;
        this.detectedByWarnRadar = param13;
        this.showBubble = param14;
        this.clanRelation = param15;
        this.modifier = param16;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.expansionStage << 10 | this.expansionStage >>> 22);
            param1.writeShort(18759);
            param1.writeInt(this.factionId << 13 | this.factionId >>> 19);
            param1.writeBoolean(this.detectedByWarnRadar);
            param1.writeUTF(this.userName);
            this.clanRelation.write(param1);
            param1.writeInt(this.assetId << 2 | this.assetId >>> 30);
            this.type.write(param1);
            param1.writeInt(this.posY >>> 16 | this.posY << 16);
            param1.writeInt(this.posX << 5 | this.posX >>> 27);
            param1.writeBoolean(this.showBubble);
            param1.writeInt(this.designId << 11 | this.designId >>> 21);
            param1.writeBoolean(this.invisible);
            param1.writeInt(this.clanId >>> 5 | this.clanId << 27);
            param1.writeInt(this.modifier.size());
            for (VisualModifierCommand modifier : this.modifier) {
                modifier.write(param1);
            }
            param1.writeBoolean(this.visibleOnWarnRadar);
            param1.writeUTF(this.clanTag);
        } catch (IOException e) {
        }
    }
}