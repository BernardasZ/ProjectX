﻿using Application.Authorization;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Models;

namespace Application.Services;

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

	public async Task<bool> ValidateUserPermissionsAsync()
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

		var permissions = (await GetPermissionsAsync())
			.Any(x => (x.Role.Name == userRole || (userRole == UserRole.Admin.ToString() && x.Role.Name == userRole))
					&& x.Controller.Name == controller
					&& (x.Action.Name == action || x.AllowAllActions));

		return permissions;
	}

	public async Task<List<PermissionMappingModel>> GetPermissionsAsync() => await _permissionCacheService.GetCacheAsync();
}