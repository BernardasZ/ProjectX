namespace Api.Helpers;

public interface IClientContextScraper
{
	string GetClientClaimsIdentityName();

	string GetClientClaimsRole();

	string GetClientIpAddress();

	string GetControllerName();

	string GetActionrName();
}