
namespace BookHavenAPI.Errors
{
    public class APIErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public APIErrorResponse(int status, string? message = null)
        {
            this.StatusCode = status;
            this.Message = message ?? CreateDefaultMessageForStatusCode(status);
        }

        private static string? CreateDefaultMessageForStatusCode(int status)
        {
            return status switch
            {
                400 => "A Bad Request, You Have Made",
                401 => "Authorized, You Are Not",
                404 => "Resource was not found",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate . Hate leads to career change.",
                _ => null
            };
        }
    }
}
