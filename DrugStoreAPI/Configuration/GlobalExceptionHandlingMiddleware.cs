using System.Net;
using System.Text.Json;
using DrugStoreAPI.Exceptions;

namespace DrugStoreAPI.Configuration
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public GlobalExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            HttpStatusCode statusCode;

            var stackTrace = string.Empty;
            var message = string.Empty;
            var exceptionType = e.GetType();

            if (exceptionType == typeof(BadRequestException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
                stackTrace = e.StackTrace;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            var exceptionResult = JsonSerializer.Serialize(new { error = message});

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
