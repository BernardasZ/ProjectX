using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Application.Database.Exceptions;
using Application.Exceptions;
using Domain.Exeptions;
using Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Middleware;

public class ErrorHandlerMiddleware
{
	private const string _unhandledException = "Service error occurred.";
	private const string _errorHandlerException = "Error occurred while handling service exception.";
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
			await HandleErrorResponse(context, ex);
		}
	}

	private async Task HandleErrorResponse(HttpContext context, Exception ex)
	{
		try
		{
			var response = context.Response;

			response.ContentType = "application/json";
			response.StatusCode = GetHttpStatusCode(ex);

			var errorCode = GetErrorCode(ex);
			var errorMessage = GetErrorMessage(context, ex);

			await response.WriteAsync(GetMessage(errorCode, errorMessage));

			_logger.LogError(ex, "{title} ErrorCode: {errorCode}, Message: {message}", _unhandledException, errorCode, errorMessage);
		}
		catch (Exception handlerException)
		{
			handlerException.Data.Add("HandlerException", ex);
			_logger.LogError(handlerException, _errorHandlerException);
		}
	}

	private static string GetMessage(string code, string message) =>
		$"{{ \"errors\": {{ \"{code}\": [\"{message}\"] }} }}";

	private static int GetHttpStatusCode(Exception exception) => exception switch
	{
		ValidationException => ((ValidationException)exception).GetStatusCode(),
		RepositoryBaseException => ((RepositoryBaseException)exception).GetStatusCode(),
		AuthenticationException => StatusCodes.Status401Unauthorized,
		Exception => StatusCodes.Status404NotFound,
		_ => StatusCodes.Status400BadRequest,
	};

	private static string GetErrorCode(Exception exception) => exception switch
	{
		ValidationException => ((ValidationException)exception).GetErrorCode(),
		RepositoryBaseException => ((RepositoryBaseException)exception).GetErrorCode(),
		AuthenticationException => StatusCodes.Status401Unauthorized.ToString(),
		Exception => StatusCodes.Status404NotFound.ToString(),
		_ => StatusCodes.Status400BadRequest.ToString(),
	};

	private static string GetErrorMessage(HttpContext context, Exception exception)
	{
		if (exception is IExceptionTranslationMapper exceptionBase)
		{
			var resourceManager = (IResourceManager)context.RequestServices.GetService(typeof(IResourceManager));

			return exceptionBase.GetErrorTranslation(resourceManager);
		}

		return _unhandledException;
	}
}