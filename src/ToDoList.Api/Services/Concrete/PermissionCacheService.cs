using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Options;

namespace ToDoList.Api.Services.Concrete;

public class PermissionCacheService : IPermissionCacheService
{
	private readonly ICacheService<List<PermissionView>> _cacheService;
	private readonly IRepository<PermissionView> _permissionViewRepository;
	private readonly IOptionsMonitor<OptionManager> _optionsManager;

	public PermissionCacheService(
		ICacheService<List<PermissionView>> cacheService,
		IRepository<PermissionView> permissionViewRepositoryr,
		IOptionsMonitor<OptionManager> optionsManager)
	{
		_cacheService = cacheService;
		_permissionViewRepository = permissionViewRepositoryr;
		_optionsManager = optionsManager;
	}

	public List<PermissionView> GetCache() => GetPermissionsCache() ?? SetPermissionsCache();

	private List<PermissionView> GetPermissionsCache() => _cacheService.GetCache(GetKey());

	private List<PermissionView> SetPermissionsCache() =>
		_cacheService.SetCache(GetPermissionViewData(), GetKey(), GetExpirationTime());

	private List<PermissionView> GetPermissionViewData() => _permissionViewRepository.GetAllByFilter().ToList();

	private string GetKey() => _optionsManager.CurrentValue.PermissionCacheSettings.Key;

	private int GetExpirationTimeInMin() => _optionsManager.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin;

	private TimeSpan GetExpirationTime() => TimeSpan.FromMinutes(GetExpirationTimeInMin());
}