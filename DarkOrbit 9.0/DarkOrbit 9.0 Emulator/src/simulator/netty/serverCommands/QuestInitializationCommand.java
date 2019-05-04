package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by LEJYONER on 13/01/2018.
 */

public class QuestInitializationCommand
        implements ServerCommand {

    public static int ID = 7727;

    public QuestDefinitionModule quest;

    public QuestInitializationCommand(QuestDefinitionModule param1) {
        this.quest = param1;
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
            this.quest.write(param1);
        } catch (Exception e) {
        }
    }
}
