using Domain.Validators;

namespace Api.DTOs.User;

public class UserUpdateDto : BaseValidatableObject
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
			.ValidateId(Id, nameof(Id))
			.ValidateEmail(Email, nameof(Email));
}