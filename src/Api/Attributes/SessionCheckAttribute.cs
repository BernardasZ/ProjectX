using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class SessionCheckAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var userService = context.HttpContext.RequestServices.GetRequiredService<IUserSessionService>();

		if (!userService.IsValidUserSession())
		{
			context.Result = new UnauthorizedResult();
		}

		base.OnActionExecuting(context);
	}
}