using Api.Validators;

namespace Api.DTOs.User;

public class UserUpdateDto : BaseValidatableObject, IDtoBase
{
	public int UserId { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
			.ValidateId(UserId, nameof(UserId))
			.ValidateEmail(Email, nameof(Email));
}