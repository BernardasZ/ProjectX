using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Validators;

namespace Api.DTOs;

public abstract class BaseValidatableObject : IValidatableObject
{
	protected abstract IValidator Validate();

	public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
		Validate().GetValidationResults();
}