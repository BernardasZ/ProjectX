using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class UserLoginModel : BaseValidatableObject, IBaseModel
{
	public string UserEmail { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(UserEmail, nameof(UserEmail))
		.ValidatePassword(Password, nameof(Password));
}