using Api.Validators;

namespace Api.Models.User;

public class UserCreateModel : BaseValidatableObject, IBaseModel
{
	public string Name { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateString(Name, nameof(Name))
		.ValidateEmail(Email, nameof(Email))
		.ValidatePassword(Password, nameof(Password));
}