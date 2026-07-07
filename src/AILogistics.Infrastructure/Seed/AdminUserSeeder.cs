using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Seed
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAdminAsync(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration)
        {
            string adminEmail = configuration["AdminUser:Email"]
                ?? throw new InvalidOperationException("AdminUser:Email is missing");

            string adminPassword = configuration["AdminUser:Password"]
                ?? throw new InvalidOperationException("AdminUser:Password is missing");

            string adminFullName = configuration["AdminUser:FullName"]
                ?? "System Admin";

            bool adminExists = await context.Users.AnyAsync(u => u.Role == UserRole.Admin);

            if (adminExists)
            {
                return;
            }

            var adminUser = new User
            {
                FullName = adminFullName,
                Email = adminEmail.Trim().ToLower(),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
            };

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, adminPassword);

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
