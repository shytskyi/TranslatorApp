﻿namespace Domain
{
    public class ApplicationLogByMethodAndStatus
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
