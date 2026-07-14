using AILogistics.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AILogistics.Api.Extensions
{
    public static class HealthCheckExtensions
    {
        public const string ReadyTag = "ready";

        public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddCheck(
                "self",
                    () => HealthCheckResult.Healthy(),
                    tags: new[] { "live" })
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "database",
                    tags: new[] { "ready" });

            return services;
        }
    }
}
