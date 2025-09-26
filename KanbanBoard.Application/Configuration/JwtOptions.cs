namespace KanbanBoard.Application.Configuration
{
    public class JwtOptions
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public double ExpireMinutes { get; set; } = 30;
    }
}