using DataModel.Enums;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User;

public class UserModel : BaseValidatableObject, IBaseModel
{
	public int UserId { get; set; }

	public UserRoleEnum Role { get; set; }

	public string UserName { get; set; }

	public string UserEmail { get; set; }

	public string Password { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(UserName, nameof(UserName))
		.ValidateEmail(UserEmail, nameof(UserEmail))
		.ValidatePassword(Password, nameof(Password));
}