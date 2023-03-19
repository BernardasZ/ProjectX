using Api.Models.Login;
using Api.Models.User;

namespace Api.Services;

public interface IUserRecoverService
{
	UserModel ChangePassword(UserChangePasswordModel model);

	UserModel ResetPassword(UserResetPasswordModel model);

	bool InitUserPasswordReset(InitPasswordResetModel model);
}