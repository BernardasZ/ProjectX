using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using ToDoList.Api.Exeptions;

namespace ToDoList.Api.Middleware;

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
        catch (Exception ex)
		{
			await SendErrorResponse(context, ex);
		}
	}

	private async Task SendErrorResponse(HttpContext context, Exception ex)
	{
		var response = context.Response;
		response.StatusCode = GetHttpStatusCode(ex);
		response.ContentType = "application/json";

		logger.Error(ex, "Unhandled service error.");

		await response.WriteAsync(ex.Message);
	}

	private static int GetHttpStatusCode(Exception exception) => exception switch
	{
		GenericException => StatusCodes.Status400BadRequest,
		AuthenticationException => StatusCodes.Status401Unauthorized,
		_ => StatusCodes.Status400BadRequest,
	};
}
