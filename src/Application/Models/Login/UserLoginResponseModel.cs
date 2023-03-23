using Domain.Models;

namespace Application.Models.Login;

public class UserLoginResponseModel : UserModel
{
	public string JWT { get; set; }
}