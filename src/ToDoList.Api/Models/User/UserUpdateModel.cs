using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserUpdateModel : BaseValidatableObject, IBaseModel
{
	public int UserId { get; set; }

	public string UserName { get; set; }

	public string UserEmail { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
			.ValidateId(UserId, nameof(UserId))
			.ValidateEmail(UserEmail, nameof(UserEmail));
}