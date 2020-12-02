using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Services
{
	public interface ICacheService
	{
		void SetCache<T>(T cacheItem, string key, TimeSpan cacheExpirationTime) where T : class;
		T GetCache<T>(string key) where T : class;
		void RemoveFromCache(string key);
	}
}
