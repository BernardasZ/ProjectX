using Domain.Validators;

namespace Api.DTOs.Login;

public class UserResetPasswordDto : BaseValidatableObject
{
	public string Token { get; set; }

	public string NewPassword { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateString(Token, nameof(Token))
		.ValidatePassword(NewPassword, nameof(NewPassword));
}