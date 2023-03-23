using Application.Database.Repositories;
using Application.Options;
using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Authorization;

public class PermissionCacheService : IPermissionCacheService
{
	private readonly ICacheService<List<PermissionMappingModel>> _cacheService;
	private readonly IRepository<PermissionMappingModel> _repository;
	private readonly IOptionsMonitor<Configuration> _configuration;

	public PermissionCacheService(
		ICacheService<List<PermissionMappingModel>> cacheService,
		IRepository<PermissionMappingModel> repository,
		IOptionsMonitor<Configuration> configuration)
	{
		_cacheService = cacheService;
		_repository = repository;
		_configuration = configuration;
	}

	public List<PermissionMappingModel> GetCache() => GetPermissionsCache() ?? SetPermissionsCache();

	private List<PermissionMappingModel> GetPermissionsCache() => _cacheService.GetCache(GetKey());

	private List<PermissionMappingModel> SetPermissionsCache() =>
		_cacheService.SetCache(GetPermissionMappingData(), GetKey(), GetExpirationTime());

	private List<PermissionMappingModel> GetPermissionMappingData() => _repository.GetAll().ToList();

	private string GetKey() => _configuration.CurrentValue.PermissionCacheSettings.Key;

	private int GetExpirationTimeInMin() => _configuration.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin;

	private TimeSpan GetExpirationTime() => TimeSpan.FromMinutes(GetExpirationTimeInMin());
}