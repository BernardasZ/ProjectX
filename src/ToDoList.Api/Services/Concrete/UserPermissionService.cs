using DataModel.Entities.ProjectX;
using DataModel.Enums;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete;

public class UserPermissionService : IUserPermissionService
{
	private readonly IPermissionCacheService _permissionCacheService;
	private readonly IClientContextScraper _clientContextScraper;

	public UserPermissionService(
		IPermissionCacheService permissionCacheService,
		IClientContextScraper clientContextScraper)
	{
		_permissionCacheService = permissionCacheService;
		_clientContextScraper = clientContextScraper;
	}

	public bool ValidateUserPermissions()
	{
		var userRole = _clientContextScraper.GetClientClaimsRole();
		var controller = _clientContextScraper.GetControllerName();
		var action = _clientContextScraper.GetActionrName();

		if (string.IsNullOrWhiteSpace(userRole)
			|| string.IsNullOrWhiteSpace(controller)
			|| string.IsNullOrWhiteSpace(action))
		{
			return false;
		}

		return GetPermissions()
			.Any(x => (x.RoleName == userRole || x.RoleName == UserRoleEnum.AllRoles.ToString())
					&& x.ControllerName == controller
					&& (x.ActionName == action || x.AllowAllActions));
	}

	public List<PermissionView> GetPermissions() => _permissionCacheService.GetCache();
}