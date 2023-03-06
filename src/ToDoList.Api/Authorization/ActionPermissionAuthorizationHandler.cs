using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;
using ToDoList.Api.Services;

namespace ToDoList.Api.Authorization
{
	internal class ActionPermissionAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
	{
		private readonly IUserPermissionService userPermissionService;
		public ActionPermissionAuthorizationHandler(
			IUserPermissionService userPermissionService)
		{
			this.userPermissionService = userPermissionService;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
		{
			if (userPermissionService.ValidateUserPermissions())
			{
				context.Succeed(requirement);
			}
			else
			{
				context.Fail();
			}

            return Task.CompletedTask;
        }
	}
}
