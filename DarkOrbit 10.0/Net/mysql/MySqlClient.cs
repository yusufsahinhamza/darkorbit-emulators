using System;
using MySQLManager.Database.Session_Details;
using MySQLManager.Database.Session_Details.Interfaces;
using MySql.Data.MySqlClient;
using Ow.Utils;

namespace MySQLManager.Database
{
    public class MySqlClient : IDatabaseClient
    {
        private MySqlConnection connection;
        private DatabaseManager dbManager;
        private IQueryAdapter info;

        public MySqlClient(DatabaseManager dbManager, int id)
        {
            this.dbManager = dbManager;
            this.connection = new MySqlConnection(dbManager.getConnectionString());
        }

        public void connect()
        {
            try
            {
                this.connection.Open();
            }
            catch (MySqlException mysqlex)
            {
                Out.WriteLine("Can't connect to MySql");
                System.IO.File.WriteAllText("errorlog.txt", "Error Log (" + DateTime.Now + "):" + Environment.NewLine + mysqlex.StackTrace);
                Console.ReadKey();
                Environment.Exit(-1);
            }
            catch (Exception e)
            {
                Out.WriteLine(e.StackTrace);
            }
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

        internal MySqlCommand getNewCommand()
        {
            return this.connection.CreateCommand();
        }

        public IQueryAdapter getQueryReactor()
        {
            return this.info;
        }

        internal MySqlTransaction getTransaction()
        {
            return this.connection.BeginTransaction();
        }

        public bool isAvailable()
        {
            return (this.info == null);
        }

        public void prepare()
        {
            this.info = new NormalQueryReactor(this);
        }

        public void reportDone()
        {
            this.Dispose();
        }
    }
}