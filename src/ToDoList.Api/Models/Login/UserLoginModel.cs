using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class UserLoginModel : BaseValidatableObject
{
	public string UserEmail { get; set; }

	public string Password { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateEmail<UserValidator>(UserEmail, nameof(UserEmail))
		.ValidatePassword<UserValidator>(Password, nameof(Password));
}
