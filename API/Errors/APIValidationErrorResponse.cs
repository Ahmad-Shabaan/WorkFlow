namespace BookHavenAPI.Errors
{
    public class APIValidationErrorResponse : APIErrorResponse
    {
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public APIValidationErrorResponse() : base(400)
        {
        }
    }
}
