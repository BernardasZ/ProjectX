using Api.Validators;

namespace Api.Models.Login;

public class UserResetPasswordModel : BaseValidatableObject, IBaseModel
{
	public string Token { get; set; }

	public string NewPassword { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateString(Token, nameof(Token))
		.ValidatePassword(NewPassword, nameof(NewPassword));
}