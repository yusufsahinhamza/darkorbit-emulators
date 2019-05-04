package simulator.users;

/**
 Created by bpdev on 02/02/2015.
 */
public abstract class AbstractAccountInternalManager {

    protected final Account mAccount;

    public AbstractAccountInternalManager(final Account pAccount) {
        this.mAccount = pAccount;
    }

    public abstract void setFromJSON(final String pJSON);

    public abstract void setNewAccount();

    public abstract String packToJSON();

    public Account getAccount() {
        return this.mAccount;
    }

}
