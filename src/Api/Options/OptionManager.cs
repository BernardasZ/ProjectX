namespace Api.Options;

public class OptionManager
{
	public AppSettings AppSettings { get; set; }
	public ConnectionStrings ConnectionStrings { get; set; }
	public PermissionCacheSettings PermissionCacheSettings { get; set; }
	public Jwt Jwt { get; set; }
	public SmtpSettings SmtpSettings { get; set; }
}