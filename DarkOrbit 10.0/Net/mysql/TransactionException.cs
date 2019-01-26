namespace MySQLManager.Database.Database_Exceptions
{
    using System;

    public class TransactionException : Exception
    {
        public TransactionException(string message) : base(message)
        {
        }
    }
}