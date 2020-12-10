using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Validators;
using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models.Login
{
	public class UserResetPasswordModel : IValidatableObject
    {
		public string Token { get; set; }
		public string NewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateToken(Token);
            validator.ValidateNewPassword(NewPassword);

            return validator.GetValidationResults();
        }
    }
}
