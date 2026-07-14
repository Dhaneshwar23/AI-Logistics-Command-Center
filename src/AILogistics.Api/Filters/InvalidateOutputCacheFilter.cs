using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OutputCaching;

namespace AILogistics.Api.Filters
{
    public class InvalidateOutputCacheFilter : IAsyncActionFilter
    {
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly string _tag;

        public InvalidateOutputCacheFilter(IOutputCacheStore outputCacheStore, string tag)
        {
            _outputCacheStore = outputCacheStore;
            _tag = tag;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if(executedContext.Exception is not null && 
                !executedContext.ExceptionHandled)
            {
                return;
            }

            var statusCode = GetStatusCode(executedContext.Result);

            if(statusCode is >= 200 and < 300)
            {
                await _outputCacheStore.EvictByTagAsync(
                    _tag,
                    context.HttpContext.RequestAborted);
            }
        }

        public static int GetStatusCode(IActionResult? result)
        {
            return result switch
            {
                ObjectResult objectResult =>
                    objectResult.StatusCode ?? StatusCodes.Status200OK,

                StatusCodeResult statusCodeResult =>
                    statusCodeResult.StatusCode,

                EmptyResult =>
                    StatusCodes.Status200OK,

                null =>
                    StatusCodes.Status200OK,

                _ => StatusCodes.Status200OK
            };
        }
    }
}
