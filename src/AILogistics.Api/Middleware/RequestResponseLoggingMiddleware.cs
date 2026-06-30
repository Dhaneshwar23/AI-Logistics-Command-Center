using System.Diagnostics;

namespace AILogistics.Api.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Items["X-Correlation-ID"]?.ToString();

            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Incoming request: {Method} {Path} CorrelationId: {correlationId} ",
                context.Request.Method,
                context.Request.Path,
                correlationId);
            
            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation("Outgoing response: {Method} {Path} CorrelationId: {correlationId} responded {StatusCode} in {ElapsedMilliseconds} ms",
                context.Request.Method,
                context.Request.Path,
                correlationId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
            
        }
    }
}
