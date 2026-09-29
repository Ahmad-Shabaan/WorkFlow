namespace Infrastructure.Exceptions
{
    public class UnauthorizedException(string message) : InfraException(message)
    {
    }
}
