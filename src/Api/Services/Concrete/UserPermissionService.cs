using Api.Helpers;
using Persistence.Entities.ProjectX;
using Persistence.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services.Concrete;

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
			.Any(x => (x.Role.Name == userRole || x.Role.Value == UserRole.AllRoles)
					&& x.Controller.Name == controller
					&& (x.Action.Name == action || x.AllowAllActions));
	}

	public List<PermissionMapping> GetPermissions() => _permissionCacheService.GetCache();
}