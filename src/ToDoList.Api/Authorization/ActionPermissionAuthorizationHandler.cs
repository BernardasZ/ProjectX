using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToDoList.Api.Authorization
{
	internal class ActionPermissionAuthorizationHandler : AuthorizationHandler<ActionPermissionRequirement>
	{
		public ActionPermissionAuthorizationHandler()
		{

		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionPermissionRequirement requirement)
		{
			string userRole = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
			string userId = context.User.Identity.Name;




			context.Succeed(requirement);




            return Task.CompletedTask;
        }
	}
}
