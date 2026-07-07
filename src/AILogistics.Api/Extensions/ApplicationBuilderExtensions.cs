using AILogistics.Api.Middleware;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using AILogistics.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;

namespace AILogistics.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }

        public static async Task SeedAdminUserAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            await AdminUserSeeder.SeedAdminAsync(
                dbContext,
                passwordHasher,
                configuration);
        }
    }
}
