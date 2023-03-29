using Application.Models.Login;
using Domain.Models;

namespace Application.Services.Interfaces;

public interface IUserRecoverService
{
	Task<UserModel> ChangePasswordAsync(UserChangePasswordModel model);

	Task<UserModel> ResetPasswordAsync(UserResetPasswordModel model);

	Task InitUserPasswordResetAsync(InitPasswordResetModel model);
}