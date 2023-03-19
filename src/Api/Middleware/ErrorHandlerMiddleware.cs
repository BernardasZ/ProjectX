using Api.Exeptions;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Middleware;

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
			logger.Error(ex, "Unhandled service error.");

			await SendErrorResponse(context, ex);
		}
	}

	private async Task SendErrorResponse(HttpContext context, Exception ex)
	{
		var response = context.Response;
		response.StatusCode = GetHttpStatusCode(ex);
		response.ContentType = "application/json";

		await response.WriteAsync(JsonSerializer.Serialize(ex.Message));
	}

	private static int GetHttpStatusCode(Exception exception) => exception switch
	{
		GenericException => StatusCodes.Status400BadRequest,
		AuthenticationException => StatusCodes.Status401Unauthorized,
		_ => StatusCodes.Status400BadRequest,
	};
}