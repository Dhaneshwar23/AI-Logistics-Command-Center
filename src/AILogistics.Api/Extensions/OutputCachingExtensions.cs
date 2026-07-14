namespace AILogistics.Api.Extensions
{
    public static class OutputCachingExtensions
    {
        public const string GeneralPolicy = "GeneralOutputCache";

        public const string CustomersTag = "customers";

        public const string ShipmentTag = "shipments";

        public const string TrackingEventsTag = "tracking-events";

        public static IServiceCollection AddOutputCacheConfiguration(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.AddPolicy(
                    GeneralPolicy,
                    policy =>
                    {
                        policy
                        .Expire(TimeSpan.FromSeconds(30))
                        .Tag(CustomersTag, ShipmentTag, TrackingEventsTag);
                        
                    });
            });

            return services;
        }
    }
}
