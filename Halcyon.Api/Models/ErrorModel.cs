namespace Halcyon.Api.Models
{
    public class ErrorModel
    {
        public string RequestId { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
