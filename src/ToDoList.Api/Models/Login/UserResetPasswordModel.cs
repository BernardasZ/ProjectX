using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class UserResetPasswordModel : BaseValidatableObject
{
	public string Token { get; set; }

	public string NewPassword { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateString<UserValidator>(Token, nameof(Token))
		.ValidatePassword<UserValidator>(NewPassword, nameof(NewPassword));
}
