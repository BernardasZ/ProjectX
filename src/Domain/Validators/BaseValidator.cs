using System.ComponentModel.DataAnnotations;

namespace Domain.Validators;

public abstract class BaseValidator<Validator> : IBaseValidator<Validator>
	where Validator : IValidator
{
	protected readonly ICollection<ValidationResult> ValidationResults = new List<ValidationResult>();

	public Validator ValidateString(string value, string name)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			AddDefaultMessage(name);
		}

		return GetValidator();
	}

	public Validator ValidateId(int value, string name)
	{
		if (value <= 0)
		{
			AddDefaultMessage(name);
		}

		return GetValidator();
	}

	public Validator GetValidator() => (Validator)(IValidator)this;

	public IEnumerable<ValidationResult> GetValidationResults() => ValidationResults;

	protected void AddDefaultMessage(string name) => AddMessage("Field can not be empty.", name);

	protected void AddCustomMessage(string message, string name) => AddMessage(message, name);

	private void AddMessage(string message, string member) =>
		ValidationResults.Add(new ValidationResult(message, new List<string> { member }));
}