using Microsoft.Extensions.Caching.Memory;
using System;

namespace Api.Services.Concrete;

public class CacheService<T> : ICacheService<T>
	where T : class
{
	private static readonly object cacheLock;

	private readonly IMemoryCache _memoryCache;

	static CacheService()
	{
		cacheLock = new object();
	}

	public CacheService(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}

	public T GetCache(string key) => _memoryCache.Get<T>(key);

	public T SetCache(T data, string key, TimeSpan expirationTime)
	{
		lock (cacheLock)
		{
			if (!_memoryCache.TryGetValue<T>(key, out var cacheItem) || cacheItem == null)
			{
				_memoryCache.Set(key, data, expirationTime);
			}

			return _memoryCache.Get<T>(key);
		}
	}
}