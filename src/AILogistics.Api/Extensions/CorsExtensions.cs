namespace AILogistics.Api.Extensions
{
    public static class CorsExtensions
    {
        public const string PolicyName = "FrontendPolicy";

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigin = configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>()
                ?? throw new InvalidOperationException("Cors allowed configuration is missing");

            services.AddCors(options =>
            {
                options.AddPolicy(
                    PolicyName,
                    policy =>
                    {
                        policy
                            .WithOrigins(allowedOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
