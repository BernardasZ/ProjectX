using Domain.Enums;

namespace Api.DTOs.User;

public class UserResponseDto : IDtoBase
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public UserRole Role { get; set; }
}