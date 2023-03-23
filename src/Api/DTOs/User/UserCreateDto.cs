using Api.Validators;

namespace Api.DTOs.User;

public class UserCreateDto : BaseValidatableObject, IDtoBase
{
	public string Name { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateString(Name, nameof(Name))
		.ValidateEmail(Email, nameof(Email))
		.ValidatePassword(Password, nameof(Password));
}