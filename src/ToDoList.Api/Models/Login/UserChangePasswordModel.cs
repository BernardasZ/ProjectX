using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class UserChangePasswordModel : BaseValidatableObject, IBaseModel
{
	public string UserEmail { get; set; }

	public string NewPassword { get; set; }

	public string OldPassword { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(UserEmail, nameof(UserEmail))
		.ValidatePassword(NewPassword, nameof(NewPassword))
		.ValidatePassword(OldPassword, nameof(OldPassword));
}