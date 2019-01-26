using MySQLManager.Database.Database_Exceptions;

namespace MySQLManager.Managers.Database
{
    public class DatabaseServer
    {
        private readonly string databaseName;
        private readonly string host;
        private readonly string password;
        private readonly uint port;
        private readonly string user;

        public DatabaseServer(string host, uint port, string username, string password, string databaseName)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new DatabaseException("No host was given");
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new DatabaseException("No username was given");
            }
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new DatabaseException("No database name was given");
            }
            this.host = host;
            this.port = port;
            this.databaseName = databaseName;
            user = username;
            this.password = password ?? "";
        }

        public string GetDatabaseName()
        {
            return databaseName;
        }

        public string GetHost()
        {
            return host;
        }

        public string GetPassword()
        {
            return password;
        }

        public uint GetPort()
        {
            return port;
        }

        public string GetUsername()
        {
            return user;
        }

        public override string ToString()
        {
            return (user + "@" + host);
        }
    }
}