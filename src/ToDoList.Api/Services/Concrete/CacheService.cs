using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Services.Concrete
{
	public class CacheService : ICacheService
	{
		private readonly object cacheLock = new object();
		private readonly IMemoryCache memoryCache;
		public CacheService(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}

		public void SetCache<T>(T cacheItem, string key, TimeSpan cacheExpirationTime) where T : class
		{
			lock (cacheLock)
			{
				if (GetCache<T>(key) == null)
				{
					memoryCache.Set<T>(key, cacheItem, cacheExpirationTime);
				}
			}
		}

		public T GetCache<T>(string key) where T : class
		{
			T cacheItem = null;
			memoryCache.TryGetValue<T>(key, out cacheItem);
			return cacheItem;
		}

		public void RemoveFromCache(string key)
		{
			memoryCache.Remove(key);
		}
	}
}
