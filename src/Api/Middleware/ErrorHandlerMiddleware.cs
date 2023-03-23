using Domain.Exeptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Middleware;

public class ErrorHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ErrorHandlerMiddleware> _logger;

	public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled service error.");

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