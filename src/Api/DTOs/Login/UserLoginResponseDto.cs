using Api.DTOs.User;

namespace Api.DTOs.Login;

public class UserLoginResponseDto : UserResponseDto, IDtoBase
{
	public string JWT { get; set; }
}