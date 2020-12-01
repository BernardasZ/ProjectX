namespace ToDoList.Api.Helpers
{
	public class OptionsManager
	{
		public AppSettings AppSettings { get; set; }
		public ConnectionStrings ConnectionStrings { get; set; }
	}

	public class ConnectionStrings
	{
		public string ProjectXConnectionString { get; set; }
	}

	public class AppSettings
	{
		public string Secret { get; set; }
	}
}
