using AILogistics.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace AILogistics.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string message;
                _logger.LogError(ex, "An unexpected error occurred");

                context.Response.ContentType = "application/json";

                if (ex is NotFoundException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = ex.Message;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = ex.Message; /*"An unexpected error occurred";*/
                }
                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message,
                    path = context.Request.Path,
                    timestamp = DateTime.UtcNow
                };

                //var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
