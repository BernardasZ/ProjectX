using Application.Database.Exceptions;
using Application.Exceptions;
using Domain.Exeptions;
using Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Middleware;

public class ErrorHandlerMiddleware
{
    private const string _UnhandledException = "Unhandled service error occurred.";
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
            _logger.LogError(ex, _UnhandledException);

            await SendErrorResponse(context, ex);
        }
    }

    private static async Task SendErrorResponse(HttpContext context, Exception ex)
    {
        var response = context.Response;
        response.StatusCode = GetHttpStatusCode(ex);
        response.ContentType = "application/json";

        var errorMessage = GetErrorMessage(context, ex);

        await response.WriteAsync(JsonSerializer.Serialize(new { Error = errorMessage }));
    }

    private static int GetHttpStatusCode(Exception exception) => exception switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        RepositoryBaseException => StatusCodes.Status400BadRequest,
        AuthenticationException => StatusCodes.Status401Unauthorized,
        Exception => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status400BadRequest,
    };

    private static string GetErrorMessage(HttpContext context, Exception exception)
    {
        if (exception is IExceptionTranslationMapper exceptionBase)
        {
            var resourceManager = (IResourceManager)context.RequestServices.GetService(typeof(IResourceManager));

            return exceptionBase.GetErrorTranslation(resourceManager);
        }

        return _UnhandledException;
    }
}