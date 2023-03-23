namespace Domain.Validators;

public interface IBaseValidator<Validator> : IValidator
	where Validator : IValidator
{
	Validator ValidateString(string value, string name);

	Validator ValidateId(int value, string name);

	Validator GetValidator();
}