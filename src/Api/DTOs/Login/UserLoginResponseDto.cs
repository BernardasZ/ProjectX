using Api.DTOs.User;

namespace Api.DTOs.Login;

public class UserLoginResponseDto : UserDto
{
	public string JWT { get; set; }
}