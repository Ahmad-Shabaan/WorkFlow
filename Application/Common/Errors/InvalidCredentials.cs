namespace Application.Common.Errors
{
    public sealed record InvalidCredentials(string Code, string Message) : Error(Code, Message);

}
