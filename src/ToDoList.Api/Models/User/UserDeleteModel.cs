using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserDeleteModel : BaseValidatableObject
{
	public int UserId { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateId<UserValidator>(UserId, nameof(UserId));
}
