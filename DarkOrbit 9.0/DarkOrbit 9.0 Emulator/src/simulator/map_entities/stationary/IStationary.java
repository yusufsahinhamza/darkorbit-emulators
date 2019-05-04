package simulator.map_entities.stationary;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 22-03-2015.
 */
public interface IStationary {

    public String getAssetName();

    public short getAssetType();

    public ServerCommand getAssetCreateCommand();
}
