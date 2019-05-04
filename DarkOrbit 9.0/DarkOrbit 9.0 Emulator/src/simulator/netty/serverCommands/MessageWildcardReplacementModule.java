package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 30-03-2015.
 */
public class MessageWildcardReplacementModule
        implements ServerCommand {

    public static int ID = 1020;

    public ClientUITooltipTextFormatModule var_1507;
    public String                          wildcard;
    public String                          replacement;

    public MessageWildcardReplacementModule(String param1, String param2, ClientUITooltipTextFormatModule param3) {
        this.wildcard = param1;
        this.replacement = param2;
        this.var_1507 = param3;
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
            this.var_1507.write(param1);
            param1.writeUTF(this.wildcard);
            param1.writeUTF(this.replacement);
        } catch (IOException e) {
        }
    }
}
