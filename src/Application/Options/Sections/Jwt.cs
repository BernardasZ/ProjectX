namespace Application.Options.Sections;

public class Jwt
{
	public string JWTSecret { get; set; }
	public int JWTExpirationInDay { get; set; }
}