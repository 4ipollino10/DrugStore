using DrugStoreAPI.Exceptions;
using System.Net;
using System.Text.Json;

namespace DrugStoreAPI.src.Configuration
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

            string stackTrace = string.Empty;
            string? message;
            var exceptionType = e.GetType();

            if (exceptionType == typeof(DuplicateComponentException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(ComponentNotFoundException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(DrugNotFoundException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(DuplicateDrugException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(ClientNotFoundException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(OrderNotFoundException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(OrderBadRequestException))
            {
                message = e.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = e.Message;
                stackTrace = e.StackTrace;
            }

            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
