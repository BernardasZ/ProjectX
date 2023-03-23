using Api.Validators;

namespace Api.DTOs.Login;

public class InitPasswordResetDto : BaseValidatableObject, IDtoBase
{
	public string Email { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(Email, nameof(Email));
}