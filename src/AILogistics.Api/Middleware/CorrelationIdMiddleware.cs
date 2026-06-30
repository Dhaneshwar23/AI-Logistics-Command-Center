using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;

namespace AILogistics.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        private const string CorrelationIdHeaderName = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
             
            string correlationId = context.Request.Headers[CorrelationIdHeaderName];

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }
            context.Items[CorrelationIdHeaderName] = correlationId;
            context.Response.Headers[CorrelationIdHeaderName] = correlationId;

            await _next(context);
        }

    }
}
