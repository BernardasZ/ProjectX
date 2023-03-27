using Domain.Validators;

namespace Api.DTOs.User;

public class UserDeleteDto : BaseValidatableObject
{
	public int Id { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateId(Id, nameof(Id));
}