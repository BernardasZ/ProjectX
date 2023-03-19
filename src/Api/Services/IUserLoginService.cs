using Api.Models.Login;

namespace Api.Services;

public interface IUserLoginService
{
	UserLoginResponseModel Login(UserLoginModel model);

	void Logout();
}