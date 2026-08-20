using AILogistics.Application.AI.Abstractions;
using AILogistics.Application.AI.Tools;
using AILogistics.Application.Interface;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.AI;
using AILogistics.Infrastructure.AI.Gemini;
using AILogistics.Infrastructure.Authentication;
using AILogistics.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace AILogistics.Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<ITrackingEventService, TrackingEventService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<GeminiAgent>();
            services.AddScoped<IAiAgent, FallBackAiAgent>();
            services.AddScoped<IAiTool, GetShipmentTool>();
            services.AddScoped<IAiTool, GetShipmentTrackingTool>();
            services.AddScoped<IAiTool, GetDashboardSummaryTool>();
            services.AddScoped<IAiTool, GetShipmentsTool>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
