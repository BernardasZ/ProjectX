using Api.Validators;
using Persistence.Enums;

namespace Api.Models.User;

public class UserModel : BaseValidatableObject, IBaseModel
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public UserRole Role { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateString(Name, nameof(Name))
		.ValidateEmail(Email, nameof(Email))
		.ValidatePassword(Password, nameof(Password));
}