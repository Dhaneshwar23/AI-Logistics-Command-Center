using AILogistics.Api.Extensions;
using AILogistics.Api.Middleware;
using AILogistics.Application.Interface;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.AI.Gemini;
using AILogistics.Infrastructure.AI.Groq;
using AILogistics.Infrastructure.Authentication;
using AILogistics.Infrastructure.Persistence;
using AILogistics.Infrastructure.Seed;
using AILogistics.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddApiVersioningConfiguration();

builder.Services.AddRateLimitingConfiguration(builder.Configuration);

builder.Services.AddHealthCheckConfiguration();

builder.Services.AddCorsConfiguration(builder.Configuration);

builder.Services.Configure<GeminiOptions>(
    builder.Configuration.GetSection(GeminiOptions.SectionName));

builder.Services.Configure<GroqOptions>(
    builder.Configuration.GetSection(GroqOptions.SectionName));

builder.Services.AddHttpClient<GroqAgent>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

builder.Services.AddResponseCompressionConfiguration();

builder.Services.AddOutputCacheConfiguration();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.UseCustomMiddlewares();

// Configure the HTTP request pipeline.
if (builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwaggerDocumentation();
}

if(!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(CorsExtensions.PolicyName);

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.UseOutputCache();

app.MapHealthChecks(
    "/health/live",
    new HealthCheckOptions
    {
        Predicate = check =>
                        check.Tags.Contains("live"),
        ResponseWriter = (context, report) => HealthCheckResponseWriter.WriteResponseAsync(context,
                                                                                            report,
                                                                                            app.Environment.IsDevelopment())
    })
    .DisableRateLimiting()
    .AllowAnonymous();

app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions
    {
        Predicate = check =>
                        check.Tags.Contains("ready"),
        ResponseWriter = (context, report) => HealthCheckResponseWriter.WriteResponseAsync(context,
                                                                                            report,
                                                                                            app.Environment.IsDevelopment())
    })
    .DisableRateLimiting()
    .AllowAnonymous();

app.MapControllers();

app.Run();
