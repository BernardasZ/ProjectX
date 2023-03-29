using Application.Models.Login;

namespace Application.Services.Interfaces;

public interface IUserLoginService
{
	Task<UserLoginResponseModel> LoginAsync(UserLoginModel model);

	Task LogoutAsync();
}