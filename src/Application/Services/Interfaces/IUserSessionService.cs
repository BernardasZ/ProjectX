using Domain.Models;

namespace Application.Services.Interfaces;

public interface IUserSessionService
{
	Task<bool> IsValidUserSessionAsync();

	Task<UserSessionModel> CreateUserSessionAsync(string userId);

	Task DeleteUserSessionsByIpAndUserIdAsync();

	Task DeleteUserSessionsByIpAndUserIdAsync(int userId);

	Task DeleteAllUserSessionsAsync(int userId);
}