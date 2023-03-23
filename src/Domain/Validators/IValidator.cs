using System.ComponentModel.DataAnnotations;

namespace Domain.Validators;

public interface IValidator
{
	IEnumerable<ValidationResult> GetValidationResults();
}