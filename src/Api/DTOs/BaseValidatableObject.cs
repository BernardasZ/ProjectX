using Domain.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs;

public abstract class BaseValidatableObject : IValidatableObject
{
	protected abstract IValidator Validate();

	public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		return Validate().GetValidationResults();
	}
}