using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Exceptions.Enums;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class SessionCheckAttribute : ActionFilterAttribute
{
	public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var userService = context?.HttpContext.RequestServices.GetRequiredService<IUserSessionService>();

		if (userService != null && !(await userService.IsValidUserSessionAsync()))
		{
			throw new ValidationException(ValidationErrorCodes.UserSessionExpired, StatusCodes.Status401Unauthorized);
		}

		await base.OnActionExecutionAsync(context, next);
	}
}