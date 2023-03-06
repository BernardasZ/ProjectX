namespace ToDoList.Api.Services;

public interface IUserSessionService
{
	bool IsValidUserSession();

	void CreateUserSession(string userId);

	void DeleteUserSession();

	void DeleteUserSession(string userId);

	void DeleteUserSession(string userIdentity, string ipAddress);
}
