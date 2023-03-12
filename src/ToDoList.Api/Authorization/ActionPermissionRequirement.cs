using Microsoft.AspNetCore.Authorization;

namespace ToDoList.Api.Authorization;

internal class ActionPermissionRequirement : IAuthorizationRequirement
{
	public ActionPermissionRequirement()
	{
	}
}