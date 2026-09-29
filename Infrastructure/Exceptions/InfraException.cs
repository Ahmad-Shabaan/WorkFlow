namespace Infrastructure.Exceptions
{
    public class InfraException : Exception
    {
        protected InfraException(string message) : base(message)
        {
        }
    }
}
