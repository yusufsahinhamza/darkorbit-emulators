package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ClientUITextReplacementModule
        implements ServerCommand {

    public static int ID = 1020;

    public String mReplacement = "";

    public String mWildcard = "";

    public ClientUITooltipTextFormatModule mTooltipTextFormat;

    public ClientUITextReplacementModule(String pWildCard, ClientUITooltipTextFormatModule pTooltipTextFormat,
                                         String pReplacement) {
        this.mWildcard = pWildCard;
        this.mReplacement = pReplacement;
        this.mTooltipTextFormat = pTooltipTextFormat;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 4;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream out) {
        try {
            this.mTooltipTextFormat.write(out);
            out.writeUTF(this.mWildcard);
            out.writeUTF(this.mReplacement);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}