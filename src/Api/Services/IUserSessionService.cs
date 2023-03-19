namespace Api.Services;

public interface IUserSessionService
{
	bool IsValidUserSession();

	void CreateUserSession(string id);

	void DeleteUserSession();

	void DeleteUserSession(string id);
}