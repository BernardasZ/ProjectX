using Api.Validators;

namespace Api.Models.Login;

public class UserLoginModel : BaseValidatableObject, IBaseModel
{
	public string Email { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(Email, nameof(Email))
		.ValidatePassword(Password, nameof(Password));
}