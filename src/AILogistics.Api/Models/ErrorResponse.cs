namespace AILogistics.Api.Models
{
    public class ErrorResponse
    {
        public int StatusCode {  get; set; }
        public string Message { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? CorrelationId {  get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
