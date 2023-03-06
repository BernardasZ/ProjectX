using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Validators;

public class BaseValidator
{
	protected readonly ICollection<ValidationResult> _results = new List<ValidationResult>();

	public T ValidateString<T>(string value, string name) where T : BaseValidator
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			AddDefaultMessage(name);
		}

		return this as T;
	}

	public T ValidateId<T>(int value, string name) where T : BaseValidator
	{
		if (value <= 0)
		{
			AddDefaultMessage(name);
		}

		return this as T;
	}

	protected void AddDefaultMessage(string name) => AddMessage($"Invalid property \"{name}\".");

	protected void AddCustomMessage(string message) => AddMessage(message);

	private void AddMessage(string message) => _results.Add(new ValidationResult(message));

	public IEnumerable<ValidationResult> GetValidationResults() => _results;
}
