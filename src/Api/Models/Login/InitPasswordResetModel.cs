using Api.Validators;

namespace Api.Models.Login;

public class InitPasswordResetModel : BaseValidatableObject, IBaseModel
{
	public string Email { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateEmail(Email, nameof(Email));
}