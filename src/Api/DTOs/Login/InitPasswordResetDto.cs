using Domain.Validators;

namespace Api.DTOs.Login;

public class InitPasswordResetDto : BaseValidatableObject
{
	public string Email { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(Email, nameof(Email));
}