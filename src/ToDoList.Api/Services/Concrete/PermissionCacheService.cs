using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoList.Api.Services.Concrete
{
	public class PermissionCacheService : ICacheService<List<PermissionView>>
	{
		private static readonly object cacheLockOne;

		private readonly IMemoryCache memoryCache;
		private readonly IServiceProvider serviceProvider;
		private readonly IOptionsMonitor<OptionManager> optionsManager;

		static PermissionCacheService()
		{
			cacheLockOne = new object();
		}

		public PermissionCacheService(
			IMemoryCache memoryCache,
			IServiceProvider serviceProvider,
			IOptionsMonitor<OptionManager> optionsManager)
		{
			this.memoryCache = memoryCache;
			this.serviceProvider = serviceProvider;
			this.optionsManager = optionsManager;
		}

		public List<PermissionView> GetCache()
		{
			var key = optionsManager.CurrentValue.PermissionCacheSettings.Key;

			List<PermissionView> cacheItem;

			lock (cacheLockOne)
			{		
				if (!memoryCache.TryGetValue<List<PermissionView>>(key, out cacheItem) || cacheItem == null)
				{
					var expirationTime = TimeSpan.FromMinutes(optionsManager.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin);
					var permissionViewRepository = serviceProvider.GetRequiredService<IRepository<PermissionView>>();
					cacheItem = permissionViewRepository.FetchAll().ToList();			
					memoryCache.Set<List<PermissionView>>(key, cacheItem, expirationTime);
				}	
			}

			return cacheItem;
		}	
	}
}
