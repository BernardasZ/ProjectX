using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

internal class ActionPermissionRequirement : IAuthorizationRequirement
{
	public ActionPermissionRequirement()
	{
	}
}