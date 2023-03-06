using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ToDoList.Api.Services;

namespace ToDoList.Api.Authorization;

internal class ActionPermissionAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
	private readonly IUserPermissionService _userPermissionService;
	public ActionPermissionAuthorizationHandler(
		IUserPermissionService userPermissionService)
	{
		_userPermissionService = userPermissionService;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
	{
		if (_userPermissionService.ValidateUserPermissions())
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
