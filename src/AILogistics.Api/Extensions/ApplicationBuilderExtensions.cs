using AILogistics.Api.Middleware;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using AILogistics.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AILogistics.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseResponseCompression();

            return app;
        }
    }
}
