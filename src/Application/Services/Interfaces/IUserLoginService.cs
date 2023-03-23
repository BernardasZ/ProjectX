using Application.Models.Login;

namespace Application.Services.Interfaces;

public interface IUserLoginService
{
	UserLoginResponseModel Login(UserLoginModel model);

	void Logout();
}