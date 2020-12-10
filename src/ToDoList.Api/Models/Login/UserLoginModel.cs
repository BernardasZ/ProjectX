using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login
{
	public class UserLoginModel : IValidatableObject
	{
		public string UserEmail { get; set; }
		public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateEmail(UserEmail);
            validator.ValidatePassword(Password);

            return validator.GetValidationResults();
        }
    }
}
