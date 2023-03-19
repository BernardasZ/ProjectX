using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Validators;

public interface IValidator
{
	IEnumerable<ValidationResult> GetValidationResults();
}