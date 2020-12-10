using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoList.Api.Enums;
using ToDoList.Api.Exeptions;

namespace ToDoList.Api.Middleware
{
	public class ErrorHandlerMiddleware
	{
        private readonly ILogger logger = Log.ForContext<ErrorHandlerMiddleware>();
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                

                string message = string.Empty;

                switch (error)
                {
                    case GenericException e:
                        message = e.ErrorCode;
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    default:
                        message = GenericErrorEnum.InternalSystemError.ToString();
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                logger.Error(error, "Handled error message: {message}", message);

                var result = JsonSerializer.Serialize(new { ErrorMessage = message });
                await response.WriteAsync(result);
            }
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
