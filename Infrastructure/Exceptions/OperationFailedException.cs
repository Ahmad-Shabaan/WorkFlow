
namespace Infrastructure.Exceptions
{
    public class OperationFailedException(string message) : InfraException(message)
    {
    }
}
