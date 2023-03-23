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

	protected void AddDefaultMessage(string name) => AddMessage($"Invalid property \"{name}\".");

	protected void AddCustomMessage(string message) => AddMessage(message);

	private void AddMessage(string message) => ValidationResults.Add(new ValidationResult(message));
}