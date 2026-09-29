namespace Infrastructure.Exceptions
{
    public class NotFoundException(string message) : InfraException(message)
    {
    }
}
