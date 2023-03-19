using Api.Options;
using Microsoft.Extensions.Options;
using Persistence.Entities.ProjectX;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services.Concrete;

public class PermissionCacheService : IPermissionCacheService
{
	private readonly ICacheService<List<PermissionMapping>> _cacheService;
	private readonly IPermissionMappingRepository _repository;
	private readonly IOptionsMonitor<OptionManager> _optionsManager;

	public PermissionCacheService(
		ICacheService<List<PermissionMapping>> cacheService,
		IPermissionMappingRepository repository,
		IOptionsMonitor<OptionManager> optionsManager)
	{
		_cacheService = cacheService;
		_repository = repository;
		_optionsManager = optionsManager;
	}

	public List<PermissionMapping> GetCache() => GetPermissionsCache() ?? SetPermissionsCache();

	private List<PermissionMapping> GetPermissionsCache() => _cacheService.GetCache(GetKey());

	private List<PermissionMapping> SetPermissionsCache() =>
		_cacheService.SetCache(GetPermissionMappingData(), GetKey(), GetExpirationTime());

	private List<PermissionMapping> GetPermissionMappingData() => _repository.GetAllByFilter().ToList();

	private string GetKey() => _optionsManager.CurrentValue.PermissionCacheSettings.Key;

	private int GetExpirationTimeInMin() => _optionsManager.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin;

	private TimeSpan GetExpirationTime() => TimeSpan.FromMinutes(GetExpirationTimeInMin());
}