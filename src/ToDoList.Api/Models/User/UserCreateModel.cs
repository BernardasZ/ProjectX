using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserCreateModel : BaseValidatableObject
{
    public string UserName { get; set; }

    public string UserEmail { get; set; }

    public string Password { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateString<UserValidator>(UserName, nameof(UserName))
		.ValidateEmail<UserValidator>(UserEmail, nameof(UserEmail))
		.ValidatePassword<UserValidator>(Password, nameof(Password));
}
