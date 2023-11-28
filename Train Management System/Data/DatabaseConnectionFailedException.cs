namespace Data
{
    public class DatabaseConnectionFailedException : Exception
    {
        public DatabaseConnectionFailedException()
        {
        }

        public DatabaseConnectionFailedException(string message) : base(message)
        {

        }
    }
}
