using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login;

public class InitPasswordResetModel : BaseValidatableObject, IBaseModel
{
	public string UserEmail { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(UserEmail, nameof(UserEmail));
}