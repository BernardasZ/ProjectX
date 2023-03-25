namespace Api.Options;

public class JwtSettings
{
	public const string SelectionName = nameof(JwtSettings);

	public string JWTSecret { get; set; }

	public int JWTExpirationInDay { get; set; }
}