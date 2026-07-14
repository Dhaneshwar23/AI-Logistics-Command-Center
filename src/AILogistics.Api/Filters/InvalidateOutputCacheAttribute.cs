using Microsoft.AspNetCore.Mvc;

using Microsoft.Identity.Client;

namespace AILogistics.Api.Filters
{
    [AttributeUsage(
        AttributeTargets.Method,
        AllowMultiple = true,
        Inherited = true)]
    public sealed class InvalidateOutputCacheAttribute : TypeFilterAttribute
    {
        public InvalidateOutputCacheAttribute(string tag) : 
            base(typeof(InvalidateOutputCacheFilter))
        {
            Arguments = new object[] { tag };
        }
    }
}
