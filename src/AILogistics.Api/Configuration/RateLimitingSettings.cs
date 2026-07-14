namespace AILogistics.Api.Configuration
{
    public class RateLimitingSettings
    {
        public RateLimitPolicySettings General { get; set; } = new();
        public RateLimitPolicySettings Authentication { get; set; } = new();

        public class RateLimitPolicySettings()
        {
            public int PermitLimit { get; set; }
            public int WindowSeconds { get; set; }
            public int QueueLimit { get; set; }
        }
    }
}
