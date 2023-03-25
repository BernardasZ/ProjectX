using System;
using Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Services;

public class CacheService<T> : ICacheService<T>
	where T : class
{
	private static readonly object _cacheLock;

	private readonly IMemoryCache _memoryCache;

	static CacheService() => _cacheLock = new object();

	public CacheService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

	public T GetCache(string key) => _memoryCache.Get<T>(key);

	public T SetCache(T data, string key, TimeSpan expirationTime)
	{
		lock (_cacheLock)
		{
			if (!_memoryCache.TryGetValue<T>(key, out var cacheItem) || cacheItem == null)
			{
				_memoryCache.Set(key, data, expirationTime);
			}

			return _memoryCache.Get<T>(key);
		}
	}
}