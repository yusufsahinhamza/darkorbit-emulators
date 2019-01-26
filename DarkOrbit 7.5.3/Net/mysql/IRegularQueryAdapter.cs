namespace MySQLManager.Session_Details.Interfaces
{
    using System.Data;
    using MySQLManager.Database;

    public interface IRegularQueryAdapter
    {
        void addParameter(string name, object query);
        bool findsResult();
        int getInteger();
        DataRow getRow();
        string getString();
        DataTable getTable();
        object query(string query, string[] parametersKeys = null, string[] parametersValues = null);
        void setQuery(string query);
        DatabaseType dbType { get; }
    }
}