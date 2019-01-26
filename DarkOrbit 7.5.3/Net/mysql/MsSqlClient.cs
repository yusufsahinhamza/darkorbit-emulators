using MySQLManager.Database.Session_Details.Interfaces;
using System.Data.SqlClient;

namespace MySQLManager.Database
{
    public class MsSQLClient : IDatabaseClient
    {
        private SqlConnection connection;
        private DatabaseManager dbManager;
        private IQueryAdapter info;

        public MsSQLClient(DatabaseManager dbManager, int id)
        {
            this.dbManager = dbManager;
            this.connection = new SqlConnection(dbManager.getConnectionString());
        }

        public void connect()
        {
            this.connection.Open();
        }

        public void disconnect()
        {
            try
            {
                this.connection.Close();
            }
            catch
            { }
        }

        public void Dispose()
        {
            this.info = null;
            disconnect();
            dbManager.FreeConnection(this);
        }

        internal SqlCommand getNewCommand()
        {
            return this.connection.CreateCommand();
        }

        public IQueryAdapter getQueryReactor()
        {
            return this.info;
        }

        public bool isAvailable()
        {
            return (this.info == null);
        }

        public void prepare()
        {
            this.info = new MsSqlQueryReactor(this);
        }

        public void reportDone()
        {
            this.Dispose();
        }
    }
}