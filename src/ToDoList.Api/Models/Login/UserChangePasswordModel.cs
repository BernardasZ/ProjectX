using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class UserChangePasswordModel : BaseValidatableObject
{
	public string UserEmail { get; set; }

	public string NewPassword { get; set; }

    public string OldPassword { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateEmail<UserValidator>(UserEmail, nameof(UserEmail))
		.ValidatePassword<UserValidator>(NewPassword, nameof(NewPassword))
		.ValidatePassword<UserValidator>(OldPassword, nameof(OldPassword));
}
