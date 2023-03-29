using System.Threading.Tasks;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

internal class ActionPermissionAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
	private readonly IUserPermissionService _userPermissionService;

	public ActionPermissionAuthorizationHandler(
		IUserPermissionService userPermissionService) =>
		_userPermissionService = userPermissionService;

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
	{
		if (await _userPermissionService.ValidateUserPermissionsAsync())
		{
			context.Succeed(requirement);
		}
		else
		{
			context.Fail();
		}

		await Task.CompletedTask;
	}
}