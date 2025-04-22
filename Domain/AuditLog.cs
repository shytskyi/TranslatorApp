namespace Domain
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }      
        public string TableName { get; set; } = ""; 
        public string ActionType { get; set; } = ""; 
        public string KeyValues { get; set; } = ""; 
        public string? OldValues { get; set; }       
        public string? NewValues { get; set; }
    }
}
