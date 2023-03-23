using Domain.Models;

namespace Application.Validations;

public interface IUserValidationService
{
	void CheckIfUserNotNull(UserModel model);

	void CheckIfUserIdMatchesSessionId(int userId);

	void CheckIfUserPasswordResetTokenIsValid(UserModel model);

	bool CheckIfUserIsAdmin();
}