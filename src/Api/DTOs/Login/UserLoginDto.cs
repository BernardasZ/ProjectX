using Domain.Validators;

namespace Api.DTOs.Login;

public class UserLoginDto : BaseValidatableObject
{
	public string Email { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(Email, nameof(Email))
		.ValidatePassword(Password, nameof(Password));
}