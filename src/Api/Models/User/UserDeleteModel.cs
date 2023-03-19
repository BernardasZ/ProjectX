using Api.Validators;

namespace Api.Models.User;

public class UserDeleteModel : BaseValidatableObject, IBaseModel
{
	public int UserId { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateId(UserId, nameof(UserId));
}