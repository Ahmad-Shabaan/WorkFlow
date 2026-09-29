namespace Application.Common.Errors
{
    public sealed record ForbiddenError(string Code, string Message) : Error(Code, Message)
    {
    }
}
