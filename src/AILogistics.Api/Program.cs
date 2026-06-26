using AILogistics.Api.Middleware;
using AILogistics.Application.Interface;
using AILogistics.Application.Interfaces;
using AILogistics.Infrastructure.Persistence;
using AILogistics.Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ITrackingEventService, TrackingEventService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();