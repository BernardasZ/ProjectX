using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete;

public class PermissionCacheService : ICacheService<List<PermissionView>>
{
	private static readonly object cacheLockOne;

	private readonly IMemoryCache _memoryCache;
	private readonly IRepository<PermissionView> _permissionViewRepository;
	private readonly IOptionsMonitor<OptionManager> _optionsManager;

	static PermissionCacheService()
	{
		cacheLockOne = new object();
	}

	public PermissionCacheService(
		IMemoryCache memoryCache,
		IRepository<PermissionView> permissionViewRepositoryr,
		IOptionsMonitor<OptionManager> optionsManager)
	{
		_memoryCache = memoryCache;
		_permissionViewRepository = permissionViewRepositoryr;
		_optionsManager = optionsManager;
	}

	public List<PermissionView> GetCache()
	{
		var key = _optionsManager.CurrentValue.PermissionCacheSettings.Key;

		List<PermissionView> cacheItem;

		lock (cacheLockOne)
		{		
			if (!_memoryCache.TryGetValue(key, out cacheItem) || cacheItem == null)
			{
				var expirationTime = TimeSpan.FromMinutes(_optionsManager.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin);

				cacheItem = _permissionViewRepository.FetchAll().ToList();		
				
				_memoryCache.Set(key, cacheItem, expirationTime);
			}	
		}

		return cacheItem;
	}	
}
