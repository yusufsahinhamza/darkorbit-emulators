using Ow;
using MySQLManager.Session_Details.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using Ow.Utils;

namespace MySQLManager.Database.Session_Details
{
    internal class QueryAdapter : IRegularQueryAdapter
    {
        private static bool dbEnabled
        {
            get
            {
                return DatabaseManager.dbEnabled;
            }
        }
        protected MySqlClient client;
        protected MySqlCommand command;

        public DatabaseType dbType
        {
            get
            {
                return DatabaseType.MySql;
            }
        }

        internal QueryAdapter(MySqlClient client)
        {
            this.client = client;
        }

        public void addParameter(string name, byte[] data)
        {
            this.command.Parameters.Add(new MySqlParameter(name, MySqlDbType.Blob, data.Length));
        }

        public void addParameter(string parameterName, object val)
        {
            this.command.Parameters.AddWithValue(parameterName, val);
        }

        public bool findsResult()
        {
            if (!dbEnabled)
                return false;
            var now = DateTime.Now;
            var hasRows = false;
            try
            {
                using (var reader = this.command.ExecuteReader())
                {
                    hasRows = reader.HasRows;
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }
            return hasRows;
        }

        public int getInteger()
        {
            if (!dbEnabled)
                return 0;
            var now = DateTime.Now;
            var result = 0;
            try
            {
                var obj2 = this.command.ExecuteScalar();
                if (obj2 != null)
                {
                    int.TryParse(obj2.ToString(), out result);
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }
            return result;
        }

        public DataRow getRow()
        {
            if (!dbEnabled)
                return null;
            var now = DateTime.Now;
            DataRow row = null;
            try
            {
                var dataSet = new DataSet();
                using (var adapter = new MySqlDataAdapter(this.command))
                {
                    adapter.Fill(dataSet);
                }
                if ((dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count == 1))
                {
                    row = dataSet.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }
            return row;
        }

        public string getString()
        {
            if (!dbEnabled)
                return string.Empty;
            var now = DateTime.Now;
            var str = string.Empty;
            try
            {
                var obj2 = this.command.ExecuteScalar();
                if (obj2 != null)
                {
                    str = obj2.ToString();
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }
            return str;
        }

        public DataTable getTable()
        {
            var now = DateTime.Now;
            var dataTable = new DataTable();
            if (!dbEnabled)
                return dataTable;
            try
            {
                using (var adapter = new MySqlDataAdapter(this.command))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }
            return dataTable;
        }

        public long insertQuery()
        {
            if (!dbEnabled)
                return 0;
            var now = DateTime.Now;
            var lastInsertedId = 0L;
            try
            {
                this.command.ExecuteScalar();
                lastInsertedId = this.command.LastInsertedId;
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }

            return lastInsertedId;
        }

        public object query(string query, string[] parametersKeys = null, string[] parametersValues = null)
        {
            if (!dbEnabled)
                return 0;

            this.setQuery(query);
            if (parametersKeys != null)
            {
                uint count = 0;
                foreach (var Key in parametersKeys)
                {
                    this.addParameter(Key, parametersValues[count]);
                    ++count;
                }
            }

            if (query.StartsWith("INSERT"))
            {
                try
                {
                    return (long)command.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Out.WriteLine("Error [MySQL] " + e);
                    return 0;
                }
            }
            else if (query.StartsWith("SELECT"))
            {
                return getTable();
            }
            else
            {
                runQuery();
            }

            return 0;
        }

        public void runQuery()
        {
            if (!dbEnabled)
                return;
            var now = DateTime.Now;
            try
            {
                this.command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //Out.WriteLine("Error [MySQL] " + this.command.CommandText);
                Out.WriteLine("Error [MySQL] " + e);
            }
        }

        public void setQuery(string query)
        {
            this.command.Parameters.Clear();
            this.command.CommandText = query;
        }
    }
}