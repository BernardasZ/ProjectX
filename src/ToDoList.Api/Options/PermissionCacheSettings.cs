namespace ToDoList.Api.Options;

public class PermissionCacheSettings
{
	public string Key { get; set; }
	public int ExpirationTimeInMin { get; set; }
}
