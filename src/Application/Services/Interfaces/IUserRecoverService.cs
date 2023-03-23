using Application.Models.Login;
using Domain.Models;

namespace Application.Services.Interfaces;

public interface IUserRecoverService
{
	UserModel ChangePassword(UserChangePasswordModel model);

	UserModel ResetPassword(UserResetPasswordModel model);

	void InitUserPasswordReset(InitPasswordResetModel model);
}