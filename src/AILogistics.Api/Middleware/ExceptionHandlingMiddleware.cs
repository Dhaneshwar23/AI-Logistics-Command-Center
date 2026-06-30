using AILogistics.Api.Models;
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
                _logger.LogError(ex, "An unexpected error occurred");

                ErrorResponse res = CreateErrorResponse(context, ex);
                
                await context.Response.WriteAsJsonAsync(res);
            }
        }

        private ErrorResponse CreateErrorResponse(HttpContext context, Exception ex)
        {
            string message = string.Empty;

            context.Response.ContentType = "application/json";
            switch (ex)
            {
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            ErrorResponse response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Path = context.Request.Path,
                CorrelationId = context.Items["X-Correlation-ID"]?.ToString(),
                TimeStamp = DateTime.UtcNow
            };

            return response;
        }
    }
}
