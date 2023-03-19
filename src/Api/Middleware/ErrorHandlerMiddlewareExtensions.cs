using Microsoft.AspNetCore.Builder;

namespace Api.Middleware;

public static class ErrorHandlerMiddlewareExtensions
{
	public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder) =>
		builder.UseMiddleware<ErrorHandlerMiddleware>();
}