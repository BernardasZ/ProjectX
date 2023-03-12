using System;

namespace ToDoList.Api.Services;

public interface ICacheService<T>
	where T : class
{
	T GetCache(string key);

	T SetCache(T data, string key, TimeSpan expirationTime);
}