using Api.Services.Interfaces;
using Application.Authorization;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services;

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

		var permissions = GetPermissions()
			.Any(x => (x.Role.Name == userRole || userRole == UserRole.Admin.ToString() && x.Role.Name == userRole)
					&& x.Controller.Name == controller
					&& (x.Action.Name == action || x.AllowAllActions));

		return permissions;
	}

	public List<PermissionMappingModel> GetPermissions() => _permissionCacheService.GetCache();
}