namespace Domain.Exceptions
{
    public class InvalidArgument(string message) : DomainException(message)
    {
    }
}
