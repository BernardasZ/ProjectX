using DataModel.Entities.ProjectX;

namespace ToDoList.Api.Helpers;

public interface IUserServiceValidationHelper
{
	void ValidateUserData(UserData model);

	void ValidateUserId(int userId);

	void ValidateUserPasswordToken(UserData model);

	bool IsAdmin();
}
