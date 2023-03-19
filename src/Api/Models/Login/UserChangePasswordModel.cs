using Api.Validators;

namespace Api.Models.Login;

public class UserChangePasswordModel : BaseValidatableObject, IBaseModel
{
	public string Email { get; set; }

	public string NewPassword { get; set; }

	public string OldPassword { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(Email, nameof(Email))
		.ValidatePassword(NewPassword, nameof(NewPassword))
		.ValidatePassword(OldPassword, nameof(OldPassword));
}