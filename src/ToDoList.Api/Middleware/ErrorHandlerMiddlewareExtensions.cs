using Microsoft.AspNetCore.Builder;

namespace ToDoList.Api.Middleware;

public static class ErrorHandlerMiddlewareExtensions
{
	public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder) =>
		builder.UseMiddleware<ErrorHandlerMiddleware>();
}
