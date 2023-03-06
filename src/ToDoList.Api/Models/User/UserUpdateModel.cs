using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserUpdateModel : BaseValidatableObject
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string UserEmail { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
			.ValidateId<UserValidator>(UserId, nameof(UserId))
			.ValidateEmail<UserValidator>(UserEmail, nameof(UserEmail));
}
