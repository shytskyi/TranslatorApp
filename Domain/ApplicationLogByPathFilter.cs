namespace Domain
{
    public class ApplicationLogByPathFilter
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
