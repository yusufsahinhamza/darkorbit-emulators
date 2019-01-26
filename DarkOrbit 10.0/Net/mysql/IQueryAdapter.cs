namespace MySQLManager.Database.Session_Details.Interfaces
{
    using MySQLManager.Session_Details.Interfaces;
    using System;

    public interface IQueryAdapter : IRegularQueryAdapter, IDisposable
    {
        void doCommit();
        void doRollBack();
        void runQuery();
    }
}