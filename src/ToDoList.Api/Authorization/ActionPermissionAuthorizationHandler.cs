using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Api.Services;

namespace ToDoList.Api.Authorization
{
	internal class ActionPermissionAuthorizationHandler : AuthorizationHandler<ActionPermissionRequirement>
	{
		private readonly IUserPermissionService userPermissionService;
		private readonly IHttpContextAccessor contextAccessor;
		public ActionPermissionAuthorizationHandler(
			IUserPermissionService userPermissionService,
			IHttpContextAccessor contextAccessor)
		{
			this.userPermissionService = userPermissionService;
			this.contextAccessor = contextAccessor;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionPermissionRequirement requirement)
		{
			string userRole = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

			var endpoint = context.Resource as RouteEndpoint;
			var descriptor = endpoint?.Metadata?.SingleOrDefault(md => md is ControllerActionDescriptor) as ControllerActionDescriptor;

			if (!string.IsNullOrWhiteSpace(userRole) && descriptor != null && userPermissionService.ValidateUserPermissions(userRole, descriptor.ControllerName, descriptor.ActionName))
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
