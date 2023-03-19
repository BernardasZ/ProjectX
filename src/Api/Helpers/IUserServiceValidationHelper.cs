using Persistence.Entities.ProjectX;

namespace Api.Helpers;

public interface IUserServiceValidationHelper
{
	void CheckIfNotNull(User model);

	void CheckIfUserIdMatching(int userId);

	void CheckIfPasswordResetTokenValid(User model);

	bool CheckIfAdminRole();
}