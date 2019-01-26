using System;
using System.Data;
using System.Data.SqlClient;
using MySQLManager.Database;
using MySQLManager.Session_Details.Interfaces;
using Ow;
using Ow.Utils;

namespace Do.core.mysql
{
    class MssqlQueryAdapter : IRegularQueryAdapter
    {
        private static bool DbEnabled
        {
            get
            {
                return DatabaseManager.dbEnabled;
            }
        }

        protected MsSQLClient client;
        protected SqlCommand command;
        public DatabaseType dbType
        {
            get
            {
                return DatabaseType.Mssql;
            }
        }

        internal MssqlQueryAdapter(MsSQLClient client)
        {
            this.client = client;
        }

        public void addParameter(string name, byte[] data)
        {
            command.Parameters.Add(new SqlParameter(name, SqlDbType.Binary, data.Length));
        }

        public void addParameter(string parameterName, object val)
        {
            command.Parameters.AddWithValue(parameterName, val);
        }

        public bool findsResult()
        {
            if (!DbEnabled)
                return false;
            var hasRows = false;
            try
            {
                using (var reader = command.ExecuteReader())
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
            if (!DbEnabled)
                return 0;
            var now = DateTime.Now;
            var result = 0;
            try
            {
                var obj2 = command.ExecuteScalar();
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
            if (!DbEnabled)
                return null;
            var now = DateTime.Now;
            DataRow row = null;
            try
            {
                var dataSet = new DataSet();
                using (var adapter = new SqlDataAdapter(command))
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
            if (!DbEnabled)
                return string.Empty;
            var now = DateTime.Now;
            var str = string.Empty;
            try
            {
                var obj2 = command.ExecuteScalar();
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
            if (!DbEnabled)
                return dataTable;
            try
            {
                using (var adapter = new SqlDataAdapter(command))
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

        public object query(string query, string[] parametersKeys = null, string[] parametersValues = null)
        {
            if (!DbEnabled)
                return 0;

            setQuery(query);
            if (parametersKeys != null)
            {
                uint count = 0;
                foreach (var Key in parametersKeys)
                {
                    addParameter(Key, parametersValues[count]);
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
            if (!DbEnabled)
                return;
            var now = DateTime.Now;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Out.WriteLine("Error [MySQL] " + e);
            }
        }

        public void setQuery(string query)
        {
            command.Parameters.Clear();
            command.CommandText = query;
        }
    }
}