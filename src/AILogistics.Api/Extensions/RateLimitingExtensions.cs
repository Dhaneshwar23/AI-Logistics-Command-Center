using AILogistics.Api.Configuration;
using Microsoft.Identity.Client;
using System.Reflection;
using System.Threading.RateLimiting;

namespace AILogistics.Api.Extensions
{
    public static class RateLimitingExtensions
    {
        public const string AuthenticationPolicy = "authentication";

        public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = configuration.GetSection("RateLimiting")
                .Get<RateLimitingSettings>()
                ?? throw new InvalidOperationException("RateLimiting configuration is missing. ");

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.GlobalLimiter =
                    PartitionedRateLimiter.Create<HttpContext, string>(
                        httpContext =>
                        {
                            var partitionKey =
                                httpContext.Connection.RemoteIpAddress?.ToString()
                                ?? "unknown";

                            return RateLimitPartition.GetFixedWindowLimiter(
                                partitionKey,
                                _ => new FixedWindowRateLimiterOptions
                                {
                                    PermitLimit = settings.General.PermitLimit,

                                    Window = TimeSpan.FromSeconds(
                                        settings.General.WindowSeconds),

                                    QueueLimit = settings.General.QueueLimit,

                                    QueueProcessingOrder =
                                        QueueProcessingOrder.OldestFirst,

                                    AutoReplenishment = true
                                });
                        });

                options.AddPolicy(
                    AuthenticationPolicy,
                    httpContext =>
                    {
                        var partitionKey =
                            httpContext.Connection.RemoteIpAddress?.ToString()
                                ?? "unknown";
                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey,
                            _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit =
                                    settings.Authentication.PermitLimit,

                                Window =
                                    TimeSpan.FromSeconds(settings.Authentication.WindowSeconds),

                                QueueLimit =
                                    settings.Authentication.QueueLimit,

                                QueueProcessingOrder =
                                    QueueProcessingOrder.OldestFirst,

                                AutoReplenishment = true
                            });
                    });

                options.OnRejected = async (
                    rejectionContext,
                    cancellationToken) =>
                {
                    rejectionContext.HttpContext.Response.StatusCode =
                        StatusCodes.Status429TooManyRequests;
                    if (rejectionContext.Lease.TryGetMetadata(
                        MetadataName.RetryAfter,
                        out var retryAfter
                        ))
                    {
                        rejectionContext.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString();
                    }
                    await rejectionContext.HttpContext.Response.WriteAsJsonAsync(
                        new
                        {
                            statusCode = StatusCodes.Status429TooManyRequests,
                            message = "Too many requests. Please try again later. "
                        }, cancellationToken);
                };
            });

            return services;
        }
    }
}
