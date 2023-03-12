using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Validators;

public interface IValidator
{
	IEnumerable<ValidationResult> GetValidationResults();
}