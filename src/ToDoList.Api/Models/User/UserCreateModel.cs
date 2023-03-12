using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserCreateModel : BaseValidatableObject, IBaseModel
{
	public string UserName { get; set; }

	public string UserEmail { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateString(UserName, nameof(UserName))
		.ValidateEmail(UserEmail, nameof(UserEmail))
		.ValidatePassword(Password, nameof(Password));
}