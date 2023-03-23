using Domain.Models;

namespace Application.Services.Interfaces;

public interface IUserSessionService
{
	bool IsValidUserSession();

	UserSessionModel CreateUserSession(string userId);

	void DeleteUserSessionsByIpAndUserId();

	void DeleteUserSessionsByIpAndUserId(int userId);

	void DeleteAllUserSessions(int userId);
}