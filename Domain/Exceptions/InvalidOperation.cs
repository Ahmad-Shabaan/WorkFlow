namespace Domain.Exceptions
{
    public class InvalidOperation(string message) : DomainException(message)
    {
    }
}
