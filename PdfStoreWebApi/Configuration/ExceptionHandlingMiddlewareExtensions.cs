using DocumentStore.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace DocumentStore.WebApi.Configuration
{
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}