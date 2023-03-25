namespace Infrastructure.Databases.Options;

public class ConnectionStringSettings
{
	public const string SelectionName = nameof(ConnectionStringSettings);

	public string ProjectXConnectionString { get; set; }
}