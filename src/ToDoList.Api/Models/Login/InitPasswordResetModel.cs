using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login
{
	public class InitPasswordResetModel : IValidatableObject
	{
		public string UserEmail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateEmail(UserEmail);

            return validator.GetValidationResults();
        }
    }
}
