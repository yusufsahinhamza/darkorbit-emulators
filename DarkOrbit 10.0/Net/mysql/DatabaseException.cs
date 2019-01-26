namespace MySQLManager.Database.Database_Exceptions
{
    using System;

    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }
    }
}