using Api.DTOs.User;

namespace Api.DTOs.Login;

public class UserLoginResponseDto : UserResponseDto
{
	public string JWT { get; set; }
}