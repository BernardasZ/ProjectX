namespace ToDoList.Api.Helpers
{
	public class OptionManager
	{
		public AppSettings AppSettings { get; set; }
		public ConnectionStrings ConnectionStrings { get; set; }
		public PermissionCacheSettings PermissionCacheSettings { get; set; }
	}


	public class ConnectionStrings
	{
		public string ProjectXConnectionString { get; set; }
	}

	public class AppSettings
	{
		public string JWTSecret { get; set; }
		public int JWTExpirationInDay { get; set; }
		public string AesKey { get; set; }
		public string AlgorithmIV { get; set; }
	}

	public class PermissionCacheSettings
	{
		public string Key { get; set; }
		public int ExpirationTimeInMin { get; set; }
	}
}