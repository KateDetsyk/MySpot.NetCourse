using Humanizer;
using Microsoft.AspNetCore.Http;
using MySpot.Core.Exceptions;

namespace MySpot.Infrastructure.Exceptions
{
    internal sealed class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }catch (Exception exception)
            {
                await HandleExcceptionAsync(exception, context);
            }
        }

        private async Task HandleExcceptionAsync(Exception exception, HttpContext context)
        {
            var (statusCode, error) = exception switch
            {
                CustomExcption => (StatusCodes.Status400BadRequest, 
                    new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty), exception.Message)),
                _ => (StatusCodes.Status500InternalServerError, new Error("error", "There was an error.")),
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private record Error(string Code, string Reason);
    }
}
