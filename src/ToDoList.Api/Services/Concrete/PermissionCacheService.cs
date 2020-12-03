using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete
{
	public class PermissionCacheService : ICacheService<List<PermissionView>>
	{
		private readonly object cacheLockOne = new object();

		private readonly IMemoryCache memoryCache;
		private readonly IRepository<PermissionView> permissionViewRepository;
		private readonly IOptionsMonitor<OptionManager> optionsManager;

		public PermissionCacheService(
			IMemoryCache memoryCache,
			IRepository<PermissionView> permissionViewRepository,
			IOptionsMonitor<OptionManager> optionsManager)
		{
			this.memoryCache = memoryCache;
			this.permissionViewRepository = permissionViewRepository;
			this.optionsManager = optionsManager;
		}

		public List<PermissionView> GetCache()
		{
			string key = optionsManager.CurrentValue.PermissionCacheSettings.Key;
			List<PermissionView> cacheItem = null;

			lock (cacheLockOne)
			{		
				memoryCache.TryGetValue<List<PermissionView>>(key, out cacheItem);

				if (cacheItem == null)
				{
					var expirationTime = TimeSpan.FromMinutes(optionsManager.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin);
					cacheItem = permissionViewRepository.FetchAll().ToList();			
					memoryCache.Set<List<PermissionView>>(key, cacheItem, expirationTime);
				}	
			}

			return cacheItem;
		}	
	}
}
