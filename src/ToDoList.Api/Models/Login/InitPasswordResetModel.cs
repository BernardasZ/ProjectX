using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class InitPasswordResetModel : BaseValidatableObject
{
	public string UserEmail { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateEmail<UserValidator>(UserEmail, nameof(UserEmail));
}
