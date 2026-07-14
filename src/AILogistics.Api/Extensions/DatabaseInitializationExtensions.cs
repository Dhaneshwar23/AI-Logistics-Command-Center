using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using AILogistics.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AILogistics.Api.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await dbContext.Database.MigrateAsync();

                await AdminUserSeeder.SeedAdminAsync(
                dbContext,
                passwordHasher,
                configuration);
            });
        }
    }
}
