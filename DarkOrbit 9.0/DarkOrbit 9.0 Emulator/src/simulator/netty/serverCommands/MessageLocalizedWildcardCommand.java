package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 30-03-2015.
 */
public class MessageLocalizedWildcardCommand
        implements ServerCommand {

    public static int ID = 17438;

    public Vector<MessageWildcardReplacementModule> mWildCardReplacements;
    public String                                   mBaseKey;
    public ClientUITooltipTextFormatModule          var_3317;

    public MessageLocalizedWildcardCommand(String pBaseKey, ClientUITooltipTextFormatModule param2,
                                           Vector<MessageWildcardReplacementModule> pWildCardReplacements) {
        this.mBaseKey = pBaseKey;
        this.var_3317 = param2;
        this.mWildCardReplacements = pWildCardReplacements;
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
            param1.writeInt(this.mWildCardReplacements.size());
            for (MessageWildcardReplacementModule c : this.mWildCardReplacements) {
                c.write(param1);
            }
            param1.writeUTF(this.mBaseKey);
            this.var_3317.write(param1);
            param1.writeShort(-30581);
        } catch (IOException e) {
        }
    }
}
