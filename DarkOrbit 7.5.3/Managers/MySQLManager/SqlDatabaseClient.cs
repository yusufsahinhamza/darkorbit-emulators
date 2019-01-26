using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Managers.MySQLManager
{
    class SqlDatabaseClient : IDisposable
    {
        public MySqlConnection mConnection;
        private MySqlCommand mCommand;

        public SqlDatabaseClient(MySqlConnection Connection)
        {
            mConnection = Connection;
            mCommand = new MySqlCommand();
            mCommand.Connection = mConnection;
        }

        public void Dispose()
        {
            if (mConnection != null)
            {
                mConnection.Close();
                mConnection = null;
            }
            if (mCommand != null)
            {
                mCommand.Dispose();
                mCommand = null;
            }
        }

        public void SetParameter(string Key, object Value)
        {
            mCommand.Parameters.Add(new MySqlParameter(Key, Value));
        }

        public void SetParameterByteArray(string Key, byte[] Value)
        {
            mCommand.Parameters.Add(new MySqlParameter(Key, MySqlDbType.LongBlob, Value.Length)).Value = Value;
        }

        public int ExecuteNonQuery(string CommandText)
        {
            mCommand.CommandText = CommandText;

            int Affected = mCommand.ExecuteNonQuery();

            return Affected;
        }

        public MySqlDataReader ExecuteQueryReader(string CommandText)
        {
            mCommand.CommandText = CommandText;
            mCommand.Connection = mConnection;

            MySqlDataAdapter adap = new MySqlDataAdapter(mCommand);
            DataSet set = new DataSet();

            return mCommand.ExecuteReader();
        }

        public DataSet ExecuteQuerySet(string CommandText)
        {
            DataSet DataSet = new DataSet();

            mCommand.CommandText = CommandText;

            using (MySqlDataAdapter Adapter = new MySqlDataAdapter(mCommand))
            {
                Adapter.Fill(DataSet);
            }

            return DataSet;
        }

        public DataTable ExecuteQueryTable(string CommandText)
        {
            DataSet DataSet = ExecuteQuerySet(CommandText);
            return DataSet.Tables.Count > 0 ? DataSet.Tables[0] : null;
        }

        public DataRow ExecuteQueryRow(string CommandText)
        {
            DataTable DataTable = ExecuteQueryTable(CommandText);
            return DataTable.Rows.Count > 0 ? DataTable.Rows[0] : null;
        }
    }
}