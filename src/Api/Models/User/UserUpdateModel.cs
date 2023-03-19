using Api.Validators;

namespace Api.Models.User;

public class UserUpdateModel : BaseValidatableObject, IBaseModel
{
	public int UserId { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
			.ValidateId(UserId, nameof(UserId))
			.ValidateEmail(Email, nameof(Email));
}