using Api.Validators;

namespace Api.DTOs.User;

public class UserDeleteDto : BaseValidatableObject, IDtoBase
{
	public int UserId { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateId(UserId, nameof(UserId));
}