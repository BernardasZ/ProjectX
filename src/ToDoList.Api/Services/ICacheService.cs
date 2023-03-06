namespace ToDoList.Api.Services;

public interface ICacheService<T> where T : class
{
	T GetCache();
}
