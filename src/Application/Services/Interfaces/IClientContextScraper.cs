namespace Application.Services.Interfaces;

public interface IClientContextScraper
{
	string GetClientClaimsIdentityName();

	string GetClientClaimsRole();

	string GetClientIpAddress();

	string GetControllerName();

	string GetActionrName();
}