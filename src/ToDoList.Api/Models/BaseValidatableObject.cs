using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models
{
	public abstract class BaseValidatableObject : IValidatableObject
	{
		protected abstract BaseValidator Validate();

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Validate().GetValidationResults();
		}
	}
}
