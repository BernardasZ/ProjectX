using Application.Authorization.Options;
using Application.Services.Interfaces;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Authorization;

public class PermissionCacheService : IPermissionCacheService
{
	private readonly ICacheService<List<PermissionMappingModel>> _cacheService;
	private readonly IRepositoryBase<PermissionMappingModel> _repository;
	private readonly IOptionsMonitor<PermissionCacheSettings> _permissionCacheSettings;

	public PermissionCacheService(
		ICacheService<List<PermissionMappingModel>> cacheService,
		IRepositoryBase<PermissionMappingModel> repository,
		IOptionsMonitor<PermissionCacheSettings> permissionCacheSettings)
	{
		_cacheService = cacheService;
		_repository = repository;
		_permissionCacheSettings = permissionCacheSettings;
	}

	public async Task<List<PermissionMappingModel>> GetCacheAsync() => GetPermissionsCache() ?? await SetPermissionsCacheAsync();

	private List<PermissionMappingModel> GetPermissionsCache() => _cacheService.GetCache(GetKey());

	private async Task<List<PermissionMappingModel>> SetPermissionsCacheAsync() =>
		_cacheService.SetCache(await GetPermissionMappingDataAsync(), GetKey(), GetExpirationTime());

	private async Task<List<PermissionMappingModel>> GetPermissionMappingDataAsync() => await _repository.GetAllAsync();

	private string GetKey() => _permissionCacheSettings.CurrentValue.Key;

	private int GetExpirationTimeInMin() => _permissionCacheSettings.CurrentValue.ExpirationTimeInMin;

	private TimeSpan GetExpirationTime() => TimeSpan.FromMinutes(GetExpirationTimeInMin());
}