namespace GithubActions.Contract
{
    public class InternalServerErrorResponse
    {
        public string DateTimeOccurredUtc { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}