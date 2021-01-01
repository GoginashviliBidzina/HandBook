using Microsoft.AspNetCore.Builder;

namespace HandBook.Api.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionLoggingMiddleware>();
    }
}