namespace Application.Authentication;

public interface IJwtService
{
	string ConstructUserJwt(string role, string identifier);
}