using Api.Validators;

namespace Api.DTOs.Login;

public class UserResetPasswordDto : BaseValidatableObject, IDtoBase
{
	public string Token { get; set; }

	public string NewPassword { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateString(Token, nameof(Token))
		.ValidatePassword(NewPassword, nameof(NewPassword));
}