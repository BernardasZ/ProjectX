using DataModel.Enums;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserModel : BaseValidatableObject
{
    public int UserId { get; set; }

    public UserRoleEnum Role { get; set; }

    public string UserName { get; set; }

    public string UserEmail { get; set; }

    public string Password { get; set; }

	protected override BaseValidator Validate() => new UserValidator()
		.ValidateId<UserValidator>(UserId, nameof(UserId))
		.ValidateString<UserValidator>(UserName, nameof(UserName))
		.ValidateEmail<UserValidator>(UserEmail, nameof(UserEmail))
		.ValidatePassword<UserValidator>(Password, nameof(Password));
}
