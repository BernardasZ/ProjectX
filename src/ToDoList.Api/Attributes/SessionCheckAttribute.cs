using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Api.Services;

namespace ToDoList.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public SessionCheckAttribute()
        {
        }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

			if (!userService.IsValidUserSession())
			{
				context.Result = new UnauthorizedResult();
			}

			base.OnActionExecuting(context);
		}
	}
}
